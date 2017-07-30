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
    public class NamedPipeServer
    {
        private NamedPipeServerStream _pipeServer = null;
        private string _pipeName = "Laplace";
        private HandleMessage _handleMessage;

        private const int PipeInBufferSize = 1024*10;

        private const int PipeOutBufferSize = 1024 * 10;

        private Encoding encoding = Encoding.UTF8;

        private bool _running = false;
        public NamedPipeServer(string pipeName)
        {
            _pipeName = pipeName;
            
        }

        public void Start( HandleMessage handleMessage)
        {
            _handleMessage = handleMessage;
            CreateNamedPipeServerStream();
            _running = true;
        
            _pipeServer.BeginWaitForConnection(WaitForConnectionCallback, _pipeServer);
        }

        private void CreateNamedPipeServerStream()
        {
            _pipeServer = new NamedPipeServerStream
           (
               _pipeName,
               PipeDirection.InOut,
               1,
               PipeTransmissionMode.Message,
               PipeOptions.Asynchronous | PipeOptions.WriteThrough,
               PipeInBufferSize,
               PipeOutBufferSize
            );
        }

        private void DisposeNamedPipeServerStream()
        {
            try
            {
                if (_pipeServer != null)
                {
                    _pipeServer.Close();
                    
                }
            }
            catch (Exception ex)
            {
                Logger.LogError4Exception(ex);
            }
            finally
            {
                _pipeServer = null;
            }
           
        }
        public bool SendMessage(string msg)
        {
            var ret = false;
            if (_pipeServer == null)
            {
                return ret;
            }
            if (_pipeServer.IsConnected)
            {
                try
                {

                    byte[] data = encoding.GetBytes(msg);
                    _pipeServer.Write(data, 0, data.Length);
                    _pipeServer.Flush();
                    _pipeServer.WaitForPipeDrain();
                    ret = true;
                }
                catch (Exception ex)
                {
                    Logger.LogError4Exception(ex);
                }
            }

            return ret;
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _running = false;
            _handleMessage = null;
            DisposeNamedPipeServerStream();
        }
        private void WaitForConnectionCallback(IAsyncResult ar)
        {
            var pipeServer = (NamedPipeServerStream)ar.AsyncState;

            pipeServer.EndWaitForConnection(ar);

            var data = new byte[PipeInBufferSize];
            //pipeServer.BeginRead(data,0, PipeInBufferSize, PipeReadCallback,)
            var count = pipeServer.Read(data, 0, PipeInBufferSize);

            if (count > 0)
            {

                
                if (_handleMessage != null)
                {
                    string message = encoding.GetString(data, 0, count);
                    _handleMessage(message);
                }
                
            }
            //else
            {
                // Kill original sever and create new wait server
                DisposeNamedPipeServerStream();
                CreateNamedPipeServerStream();
                // Recursively wait for the connection again and again....
                _pipeServer.BeginWaitForConnection(new AsyncCallback(WaitForConnectionCallback), _pipeServer);

            }
        }
        private void PipeReadCallback(IAsyncResult ar)
        {
            var pipe = (NamedPipeClientStream)ar.AsyncState;

            pipe.EndWrite(ar);
            pipe.Flush();
            pipe.WaitForPipeDrain();

            while (_running && !_pipeServer.IsConnected)
            {

            }
        }



        public delegate void HandleMessage(string str);//委托接受消息处理方法


      
    }
}
