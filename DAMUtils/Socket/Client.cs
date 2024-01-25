﻿using DAMSecurityLib.Data;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.Socket
{
    public class Client
    {
        private int port;
        private IPAddress address;

        /// <summary>
        /// Construct socket client with default values
        /// </summary>
        public Client()
        {
            port = 1234;
            address = IPAddress.Any;
        }

        /// <summary>
        /// Construct socket client with some values
        /// </summary>
        /// <param name="address">Socket's address</param>
        /// <param name="port">Socket's port</param>
        public Client(IPAddress address, int port)
        {
            this.address = address;
            this.port = port;
        }

        public void Process()
        {
            TcpClient tcpClient = new TcpClient(address, port);

            ObjectPair objectPair = new ObjectPair();
            var objectStr = objectPair.Serialize();
            var objectBytes = Encoding.UTF8.GetBytes(objectStr);

            NetworkStream stream = tcpClient.GetStream();
            stream.Write(objectBytes, 0, objectBytes.Length);

            byte[] buffer = new byte[4096];
            StringBuilder responseData = new StringBuilder();
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                responseData.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            KeyFilePair response = KeyFilePair.Deserialize(responseData.ToString());
            
            stream.Close();
            tcpClient.Close();

        }
    }
}
