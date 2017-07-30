using Laplace.Framework.Log;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laplace.Framework.Redis
{
  

    public interface IRedisDal<T> where T : class
    {       
        //string GetKeyName(T model);
        bool Insert(string key,T model,int dbIndex);
        bool Insert(string key, T model, int dbIndex, int timeOut);
        void Delete(string key, int dbIndex);
        T GetModel(string key, int dbIndex);

        List<T> GetList(string tableName, int dbIndex);

    }

    public class BaseRedisDal<T> : IRedisDal<T> where T : class //BaseRedis
    {
        //protected IServer _redisServer;

        public BaseRedisDal()
        {
            //_redisServer = RedisHost.GetRedisServer();
        }

        private IDatabase GetDatabase(int dbIndex)
        {
            return RedisHost.GetDataBase(dbIndex);
        }

        
        //public string GetKeyName(T model)
        //{
        //    return model.KeyName;
        //}
        /// <summary>
        /// 插入Model记录    
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public bool Insert(string key,T model,int dbIndex)
        {
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return false;
                }
                return GetDatabase(dbIndex).StringSet(key, JsonConvert.SerializeObject(model));

            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return false;
        }
        /// <summary>
        /// 添加Model记录，并设置记录有效期
        /// </summary>
        /// <param name="model"></param>
        /// <param name="timeOut">记录有效期，单位秒</param>
        /// <returns></returns>
        public bool Insert(string key, T model, int dbIndex, int timeOut)
        {
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return false;
                }
              
                return GetDatabase(dbIndex).StringSet(key, JsonConvert.SerializeObject(model), TimeSpan.FromSeconds(timeOut));
        
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex,new string[] {"AppLogger", "RedisLogger" } );
                ReConnectRedis();
            }
            return false;
            
        }
       
        public void Delete(string key,int dbIndex)
        {
            try
            {

                if (!IsConnected && !ReConnectRedis())
                {
                    return;
                }
                GetDatabase(dbIndex).KeyDelete(key);

                //GetDatabase(dbIndex).KeyMove(key, 0);
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            
        }
        /// <summary>
        /// 产生递增数
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public long CreateIdentity(string keyName, int dbIndex)
        {
            long ret = -1;
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return ret;
                }
                ret = GetDatabase(dbIndex).StringIncrement(keyName, 1); 

            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return ret;
        }
        private bool IsConnected
        {
            get { return RedisHost.IsConnected; }
        }

        private bool ReConnectRedis()
        {
            if (RedisHost.ReConnectRedis())
            {
                //_redisServer = RedisHost.GetRedisServer();
                return true;
            }
            else
            {
                return false;
            }
        }
        //public T GetModel(string key, T model,int dbIndex)
        //{
        //    try
        //    {
        //        return GetModel(GetKeyName(model),dbIndex);
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
        //        RedisHost.ReConnectRedis();
        //    }
        //    return null;
        //}
        public T GetModel(string keyName,int dbIndex)
        {
            string errMsg;
            return GetModel(keyName,dbIndex,out errMsg);

        }
        public T GetModel(string keyName, int dbIndex,out string errMsg)
        {
            T ret = null;
            errMsg = string.Empty;
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    errMsg = "Redis断开";
                    return ret;
                }
                var json = GetDatabase(dbIndex).StringGet(keyName);
                if (!string.IsNullOrEmpty(json))
                {
                    try
                    {
                        ret = JsonConvert.DeserializeObject<T>(json);
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex.Message;
                        Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                    }
                    
                }

            }
            catch (RedisConnectionException ex)
            {
                errMsg = ex.Message;
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }

            return ret;

        }
        public List<T> GetList(string tableName,int dbIndex)
        {
            List<T> datas = new List<T>();
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return new List<T>();
                }
                
                
                //System.Console.WriteLine("{0} Start At {1}", tableName, DateTime.Now.ToString("mm:ss fff"));
                var keys = RedisHost.GetRedisServer().Keys(dbIndex, tableName + "*");

                foreach (var key in keys)
                {
                    string str = GetDatabase(dbIndex).StringGet(key);
                    try
                    {
                        if(string.IsNullOrEmpty(str))
                        {
                            Logger.LogError(string.Format("_redisDb.StringGet({0}) 返回值为空!" , key));
                        }
                        else
                        {
                            datas.Add(JsonConvert.DeserializeObject<T>(str));
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                        ReConnectRedis();
                    }
                }
                //System.Console.WriteLine("{0} End At {1}", keys.ToList().Count, DateTime.Now.ToString("mm:ss fff"));
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }

            return datas;
        }
        #region--List表操作--
        public bool ListInsert(string key, T model, int dbIndex)
        {
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return false;
                }
                GetDatabase(dbIndex).ListRightPush(key, JsonConvert.SerializeObject(model));
                return true;
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return false;
        }
        public bool ListInsert(string key, string value, int dbIndex)
        {
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return false;
                }
                GetDatabase(dbIndex).ListRightPush(key, value);
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return false;
        }
        public List<string> ListRange(string key, int dbIndex)
        {
            var ret = new List<string>();
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return ret;
                }
                foreach (var value in GetDatabase(dbIndex).ListRange(key))
                {
                    ret.Add(value);
                }
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return ret;
        }
        public bool ListExists(string key,string value,int dbIndex)
        {
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return false;
                }
                foreach (var listValue in GetDatabase(dbIndex).ListRange(key))
                {
                    if(listValue == value)
                    {
                        return true;
                    }
                }
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return false;
        }
        #endregion-List表操作-
        #region--Hash表操作--
        /// <summary>
        /// 向Hash表里添加字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public bool HashInsert(string key, string field, string value, int dbIndex)
        {
            var ret = false;
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return false;
                }
                GetDatabase(dbIndex).HashSet(key, field, value);
                ret = true;
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return ret;
        }

        /// <summary>
        /// 向Hash表里添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="model"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public bool HashInsert(string key, string field, T model, int dbIndex)
        {
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return false;
                }
                return HashInsert(key, field, JsonConvert.SerializeObject(model), dbIndex);
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return false;
        }

        /// <summary>
        /// 向Hash表里批量添加值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="listField"></param>
        /// <param name="listModel"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public bool HashInsert(string key, List<string> listField, List<T> listModel, int dbIndex)
        {
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return false;
                }
                var count = Math.Min(listField.Count, listModel.Count);
                var save = new HashEntry[count];
                for(var i=0;i<count;i++)
                {
                    save[i] = new HashEntry(listField[i], JsonConvert.SerializeObject(listModel[i]));
                }
                return HashInsert(key, save,dbIndex, CommandFlags.FireAndForget);
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return false;
        }
        /// <summary>
        /// 向Hash表里批量添加数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        private bool HashInsert(string key, HashEntry[] hashFields, int dbIndex, CommandFlags flags = CommandFlags.None)
        {
            var ret = false;
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return false;
                }
                GetDatabase(dbIndex).HashSet(key, hashFields, flags);
                ret = true;
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return ret;
        }
   
        public bool HashExists(string key, string field, int dbIndex)
        {
            if (!IsConnected && !ReConnectRedis())
            {
                return false;
            }
            return GetDatabase(dbIndex).HashExists(key, field);
        }
        public bool HashDelete(string key, string field, int dbIndex)
        {
            if (!IsConnected && !ReConnectRedis())
            {
                return false;
            }
            return GetDatabase(dbIndex).HashDelete(key, field);
        }
        /// <summary>
        /// 从hash表里返回对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>

        public T HashGetModel(string key,string field,int dbIndex)
        {
            T ret = null;
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return null;
                }
                var value = GetDatabase(dbIndex).HashGet(key, field);
                if(value.HasValue)
                {
                    ret = JsonConvert.DeserializeObject<T>(value.ToString());
                }
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return ret;
        }
        /// <summary>
        /// 从Hash表里返回字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public string HashGetString(string key, string field, int dbIndex)
        {
            string  ret = null;
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return ret;
                }
                var value = GetDatabase(dbIndex).HashGet(key, field);
                if (value.HasValue)
                {
                    ret = value.ToString();
                }
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            return ret;
        }
        /// <summary>
        /// 返回hash列表
        /// </summary>
        /// <param name="key">要返回的Hash列表Key</param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public List<T> HashGetList(string key, int dbIndex)
        {
            List<T> datas = new List<T>();
            var db = GetDatabase(dbIndex);
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return datas;
                }
                foreach (var value in db.HashGetAll(key))
                {
                    try
                    {
                        if (string.IsNullOrEmpty(value.Value))
                        {
                            Logger.LogError(string.Format("db.HashGet({0},{1}) 返回值为空!", key, value.Name));
                        }
                        else
                        {
                            datas.Add(JsonConvert.DeserializeObject<T>(value.Value));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                        ReConnectRedis();
                    }
                    
                }
                //foreach (var field in db.HashKeys(key))
                //{
                //    try
                //    {
                //        var value = db.HashGet(key, field);
                //        if (string.IsNullOrEmpty(value))
                //        {
                //            Logger.LogError(string.Format("db.HashGet({0},{1}) 返回值为空!", key, field));
                //        }
                //        else
                //        {
                //            datas.Add(JsonConvert.DeserializeObject<T>(value));
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                //        ReConnectRedis();
                //    }
                   
                //}
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }

            return datas;
        }
        /// <summary>
        /// 返回指定hash表的Key列表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dbIndex"></param>
        /// <returns></returns>
        public List<string> HashGetKeyList(string key, int dbIndex)
        {
            List<string> datas = new List<string>();
            
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return datas;
                }
                var db = GetDatabase(dbIndex);
                foreach (var value in db.HashGetAll(key))
                {
                    try
                    {
                        if (string.IsNullOrEmpty(value.Name))
                        {
                            Logger.LogError(string.Format("db.HashGet({0},{1}) 返回值为空!", key, value.Name));
                        }
                        else
                        {
                            datas.Add(value.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                        ReConnectRedis();
                    }

                }
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }

            return datas;
        }
        public List<string> HashGetStringList(string key, int dbIndex)
        {
            List<string> datas = new List<string>();
            
            try
            {
                if (!IsConnected && !ReConnectRedis())
                {
                    return datas;
                }
                var db = GetDatabase(dbIndex);
                foreach (var value in db.HashGetAll(key))
                {
                    try
                    {
                        if (string.IsNullOrEmpty(value.Value))
                        {
                            Logger.LogError(string.Format("db.HashGet({0},{1}) 返回值为空!", key, value.Name));
                        }
                        else
                        {
                            datas.Add(value.Value);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                        ReConnectRedis();
                    }

                }
            }
            catch (RedisConnectionException ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex, new string[] { "AppLogger", "RedisLogger" });
                ReConnectRedis();
            }

            return datas;
        }
        #endregion-Hash表操作-
    }


}
