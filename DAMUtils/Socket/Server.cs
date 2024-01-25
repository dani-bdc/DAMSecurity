using DAMSecurityLib.Crypto;
using DAMSecurityLib.Data;
using DAMUtils.PDF;
using DAMUtils.PDF.Fake;
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
        public PDF.IPDFGenerator PdfGenerator { get; set; } = new FakePDFGenerator();

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

            while (true)
            {
                var client = listener.AcceptTcpClient();

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[4096];
                int bytesRead;
                StringBuilder receivedData = new StringBuilder();

                // Receive data from client
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    receivedData.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                }
                // pair.Obj1 contains report name to generate
                // pair.Obj2 contains client Public Key
                ObjectPair pair = ObjectPair.Deserialize(receivedData.ToString());

                // Generate pdf 
                byte[] pdfBytes;
                if (pair != null && pair.Obj1 != null)
                    pdfBytes = PdfGenerator.Generate(pair.Obj1);
                else
                    pdfBytes = new byte[0];

                // Encrypt pdf and key
                KeyFilePair kfp;
                if (pair != null && pair.Obj2 != null)
                    kfp = Hybrid.Crypt((RSAParameters)pair.Obj2, pdfBytes);
                else
                    kfp = new KeyFilePair();

                // Send data to the client
                var json = kfp.Serialize();
                var sendBytes = Encoding.UTF8.GetBytes(json);
                stream.Write(sendBytes, 0, sendBytes.Length);
            }
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
