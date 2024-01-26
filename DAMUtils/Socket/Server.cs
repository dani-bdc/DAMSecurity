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
                var str = Utils.ReadToString(stream);
                
                ObjectPair pair = ObjectPair.Deserialize(str);

                // Generate pdf 
                byte[] pdfBytes = PdfGenerator.Generate(pair.Obj1);

                // Encrypt pdf and key
                KeyFilePair kfp = Hybrid.Crypt((RSAParameters)pair.Obj2, pdfBytes);
                
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
