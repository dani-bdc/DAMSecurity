using DAMSecurityLib.Crypto;
using DAMSecurityLib.Data;
using DAMUtils.Socket.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.Socket
{
    /// <summary>
    /// Class representing Server Extended
    /// This server allow to list available reports and pass parameters to report generation
    /// </summary>
    public class ServerExtended : Server
    {
        /// <summary>
        /// Construct socket server with port
        /// </summary>
        /// <param name="port">Socket's port</param>
        public ServerExtended(int port) : base(port) { }

        /// <summary>
        /// Construct socket server with some values
        /// </summary>
        /// <param name="address">Socket's address</param>
        /// <param name="port">Socket's port</param>
        public ServerExtended(IPAddress address, int port) : base(address, port) { }

        /// <summary>
        /// Process Client function
        /// </summary>
        protected override void ProcessClient()
        {
            if (listener == null)
                return;

            var client = listener.AcceptTcpClient();

            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            StringBuilder receivedData = new StringBuilder();

            // Receive data from client
            BinaryReader reader = new BinaryReader(stream);
            var size = reader.ReadInt64();
            while (bytesRead<size)
            {
                bytesRead = bytesRead + stream.Read(buffer, 0, buffer.Length);
                receivedData.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }

            var str= receivedData.ToString();

            ;

            ClientRequest clientRequest= ClientRequest.Deserialize(str);
            ServerResponse serverResponse = new ServerResponse();

            switch (clientRequest.RequestType)
            {
                case RequestType.ListReports:
                    serverResponse.StatusCode = StatusCode.Success;
                    serverResponse.Data = PdfGenerator.AvailableReports();
                    break;
                case RequestType.GetReport:
                    if (clientRequest.Data == null)
                        serverResponse.StatusCode = StatusCode.Indeterminate;
                    else
                    {
                        serverResponse.StatusCode = StatusCode.Success;
                        ObjectPair pair = (ObjectPair)clientRequest.Data;
                        byte[] pdfBytes = PdfGenerator.Generate(pair.Obj1);
                        // Encrypt pdf and key
                        KeyFilePair kfp = Hybrid.Crypt((RSAParameters)pair.Obj2, pdfBytes);
                        serverResponse.Data = kfp;
                    }
                    break;
                default:
                    serverResponse.StatusCode = StatusCode.InvalidType;
                    break ;
            }

            // Send data to the client
            var json = serverResponse.Serialize();
            var sendBytes = Encoding.UTF8.GetBytes(json);

            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(sendBytes.LongLength);
            stream.Write(sendBytes, 0, sendBytes.Length);

        }

    }
}
