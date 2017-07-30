using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using Laplace.Framework.Log;

namespace Laplace.Framework.Helper
{
    public class NamedPipeClient : IDisposable
    {
        string _serverName;
        string _pipeName;
        readonly NamedPipeClientStream _pipeClient;
        public delegate void HandleMessage(string str);//委托接受消息处理方法
        private const int PipeInBufferSize = 1024 * 10;

        private const int PipeOutBufferSize = 1024 * 10;
        private Encoding encoding = Encoding.UTF8;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverName">服务器地址</param>
        /// <param name="pipName">管道名称</param>
        public NamedPipeClient(string serverName, string pipeName)
        {
            _serverName = serverName;
            _pipeName = pipeName;

            _pipeClient = new NamedPipeClientStream(serverName, pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);

        }

        public void Stop()
        {
            try
            {
                _pipeClient.WaitForPipeDrain();
            }
            finally
            {
                _pipeClient.Close();
                _pipeClient.Dispose();
            }
        }
     
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool SendMessage(string msg)
        {
            if (!_pipeClient.IsConnected)
            {
                return false;
            }
            byte[] data = encoding.GetBytes(msg);

            _pipeClient.BeginWrite(data, 0, data.Length, PipeWriteCallback, _pipeClient);
            return true;
            //using (var sw = new StreamWriter(_pipeClient))
            //{
            //    sw.WriteLine(msg);
            //    sw.Flush();
            //}
            //return true;

            //StreamReader sr = new StreamReader(_pipeClient);
            //string temp;
            //string returnVal = "";
            //while ((temp = sr.ReadLine()) != null)
            //{
            //    returnVal = temp;
            //    //nothing
            //}
            //return returnVal;
        }
        private void PipeWriteCallback(IAsyncResult ar)
        {
            try
            {
                var pipe = (NamedPipeClientStream)ar.AsyncState;
                pipe.EndWrite(ar);
                pipe.Flush();
                pipe.WaitForPipeDrain();
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
            }
            
        }

        public void StartReceiveMessage(HandleMessage printMessage)
        {
            if (_startReceiveMessage)
            {
                return;
            }
            _startReceiveMessage = true;
            //StreamReader sr = new StreamReader(_pipeClient);
            //Connect();

            //_pipeClient.BeginWrite(data, 0, data.Length, PipeWriteCallback, _pipe);
            //_pipeClient.BeginRead()
            ThreadPool.QueueUserWorkItem(delegate
            {
                while (_startReceiveMessage)
                {
                    
                    if (_pipeClient.IsConnected)
                    {
                        var data = new byte[PipeInBufferSize];

                        var count = _pipeClient.Read(data, 0, data.Length);

                        if (count > 0)
                        {
                            string message = encoding.GetString(data, 0, count);
                            printMessage(message);
                        }
                        Thread.Sleep(100);
                        //if (_pipeClient.CanRead)
                        //{
                        //    printMessage(sr.ReadLine());
                        //}

                    }
                    else
                    {
                        Thread.Sleep(1000);
                        Connect();
                    }
                    
                    //StreamString ss = new StreamString(pipeClient);
                    //printMessage(ss.ReadString());

                }
            });

        }
        private void PipeReadCallback(IAsyncResult ar)
        {
            var pipe = (NamedPipeClientStream)ar.AsyncState;

            pipe.EndWrite(ar);
            pipe.Flush();
            pipe.WaitForPipeDrain();

          
        }
        private void Connect()
        {
            if (!_pipeClient.IsConnected)
            {
                try
                {

                    _pipeClient.Connect(1000*2);
                    _pipeClient.ReadMode = PipeTransmissionMode.Message;
                }
                catch (Exception ex)
                {
                    Logger.LogError4Exception(ex);
                }
                
            }
        }

        public void StopReceiveMessage()
        {
            _startReceiveMessage = false;
        }
        private bool _startReceiveMessage = false;
        #region IDisposable 成员

        bool _disposed = false;
        public void Dispose()
        {
            StopReceiveMessage();
            if (!_disposed && _pipeClient != null)
            {
                _pipeClient.Dispose();
                _disposed = true;
            }
        }

        #endregion
    }
}
