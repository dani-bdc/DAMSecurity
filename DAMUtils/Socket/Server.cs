using DAMSecurityLib.Crypto;
using DAMSecurityLib.Data;
using DAMUtils.PDF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.Socket
{
    public class Server
    {
        private int port;
        private IPAddress address;
        private TcpListener? listener;

        /// <summary>
        /// Construct socket server with default values
        /// </summary>
        public Server()
        {
            port = 1234;
            address = IPAddress.Any;
        }

        /// <summary>
        /// Construct socket server with some values
        /// </summary>
        /// <param name="address">Socket's address</param>
        /// <param name="port">Socket's port</param>
        public Server(IPAddress address, int port)
        {
            this.address = address;
            this.port = port;
        }

        /// <summary>
        /// Starts listening on socket server
        /// </summary>
        public void Start()
        {
            listener=new TcpListener(address, port);
            listener.Start();

            var client = listener.AcceptTcpClient();
            NetworkStream stream= client.GetStream();
            byte[] buffer = new byte[4096];
            int bytesRead;
            StringBuilder receivedData = new StringBuilder();

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                receivedData.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            ObjectPair pair = ObjectPair.Deserialize(receivedData.ToString());
            KeyFilePair kfp = new KeyFilePair();
            var json = kfp.Serialize();
            var sendBytes = Encoding.UTF8.GetBytes(json);
            stream.Write(sendBytes, 0, sendBytes.Length);
        }

        /// <summary>
        /// Stops listening on socket server
        /// </summary>
        public void Stop()
        {
            listener?.Stop();
        }

    }
}
