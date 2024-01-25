﻿using DAMSecurityLib.Crypto;
using DAMSecurityLib.Data;
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
    public class Client
    {
        private int port;
        private string address;

        /// <summary>
        /// Construct socket client with default values
        /// </summary>
        public Client()
        {
            port = 1234;
            address = "localhost";
        }

        /// <summary>
        /// Construct socket client with some values
        /// </summary>
        /// <param name="address">Socket's address</param>
        /// <param name="port">Socket's port</param>
        public Client(string address, int port)
        {
            this.address = address;
            this.port = port;
        }

        public byte[] Process(string reportName, X509Certificate2 certificate)
        {
            TcpClient tcpClient = new TcpClient(address, port);

            ObjectPair objectPair = new ObjectPair();
            var cPk = certificate.GetRSAPublicKey();
            var publicKey = cPk?.ExportParameters(false);
            objectPair.Obj1 = reportName;
            objectPair.Obj2 = publicKey;
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
            byte[] finalBytes = Hybrid.Decrypt(certificate, response);

            stream.Close();
            tcpClient.Close();

            return finalBytes;
        }
    }
}
