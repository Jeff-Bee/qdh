using Laplace.Framework.Log;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laplace.Framework.Redis
{
    public class RedisHost
    {
        private static string _host = "127.0.0.1";
        private static int _port = 8000;
        private static string _password;
        //private static ConnectionMultiplexer _redisConnection;
        private static readonly IDatabase[] _dbs = new IDatabase[16];
        private static IServer _server;
        private static ConfigurationOptions _config;
        private static readonly Object MultiplexerLock = new Object();

        private static Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_config));
        //private static ConnectionMultiplexer _connectionMultiplexer = null;
        public static ConnectionMultiplexer Connection
        {
            get
            {
                try
                {
                    //if (_connectionMultiplexer == null)
                    //{
                    //    if (_lazyConnection != null)
                    //    {
                    //        _connectionMultiplexer = _lazyConnection.Value;
                    //    }
                    //}
                    //if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
                    //{
                    //    ConnectRedis();
                    //}
                    //if (/*_lazyConnection.Value == null ||*/ !_lazyConnection.Value.IsConnected)
                    //{
                    //    ConnectRedis();
                    //}
                    return _lazyConnection.Value;
                }
                catch (Exception ex)
                {
                    Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                }
                return null;

            }
        }
        public static IDatabase GetDataBase()
        {
            return GetDataBase(0);
        }
        public static IDatabase GetDataBase(int index)
        {
            if(index>=_dbs.Length || index<0)
            {
                return null;
            }
            try
            {
                if (_dbs[index] != null)
                {
                    return _dbs[index];
                }
                return _dbs[index] = Connection.GetDatabase(index);
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
            }
            return null;
        }

        public static IServer GetRedisServer()
        {
            if (_server == null && Connection!=null)
            {
                _server = Connection.GetServer(_host, _port);
            }
            return _server;
        }

        public static void Config(string host, int port,string password)
        {
            _host = host;
            _port = port;
            _password = password;
            _config = new ConfigurationOptions
            {
                EndPoints =
                    {
                        { _host, _port }
                    },
                KeepAlive = 180,    //Time (seconds) at which to send a message to help keep sockets alive
                Password = _password,
                AbortOnConnectFail = false,

                ConnectTimeout = 15000,
                SyncTimeout = 15000,

                //DefaultVersion = new Version("2.8.5"),
                // Needed for cache clear
                //AllowAdmin = true
            };
        }

        public static bool IsConnected
        {
            get
            {
                return Connection!=null && Connection.IsConnected;
            }
        }
        /// <summary>
        /// 重连Redis
        /// </summary>
        public static bool ReConnectRedis()
        {
            if (_redisConnecting || (DateTime.Now - _redisLastConnectTime).TotalMinutes < 1)
            {
                return false;
            }
            return ConnectRedis();

        }

        private static bool _redisConnecting = false;                           //标记Redis正在连接中
        private static DateTime _redisLastConnectTime=DateTime.MinValue;        //记录上次Redis连接时间
        /// <summary>
        /// 连接Redis
        /// </summary>
        public static bool ConnectRedis()
        {
            bool ret = false;
            try
            {
                _redisConnecting = true;
                Logger.LogWarning("Redis开始重连", new string[] { "AppLogger", "RedisLogger" });
                _redisLastConnectTime =DateTime.Now;
                lock (MultiplexerLock)
                {
                    if (Connection == null || !Connection.IsConnected)
                    {
                        DisposeConnection();
                        _lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_config));
                        _server = Connection.GetServer(_host, _port);
                    }
                    else
                    {
                        Logger.LogFatal("Redis连接正常，不需要重连", new string[] { "AppLogger", "RedisLogger" });
                    }
                    ret = IsConnected;
                }
                Logger.LogWarning(String.Format("Redis重连结束：{0}",ret?"成功":"失败"), new string[] { "AppLogger", "RedisLogger" });
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
            }
            _redisConnecting = false;
            return ret;
        }
        private static void DisposeConnection()
        {
            try
            {
                if (_lazyConnection!=null && _lazyConnection.Value != null)
                {
                    _lazyConnection.Value.Close();
                    _lazyConnection.Value.Dispose();
                    _lazyConnection = null;
                }

                //if (_connectionMultiplexer != null)
                //{
                //    _connectionMultiplexer.Close();
                //    _connectionMultiplexer.Dispose();
                //}
                for (int i = 0; i < _dbs.Length; i++)
                {
                    _dbs[i] = null;
                }
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] {"AppLogger", "RedisLogger"});
            }
            finally
            {
                //_lazyConnection = null;
            }
        }
    }
}
