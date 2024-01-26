using DAMSecurityLib.Crypto;
using DAMSecurityLib.Data;
using DAMSecurityLib.Exceptions;
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
    /// <summary>
    /// Client socket
    /// </summary>
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

        /// <summary>
        /// Sends data to the server and returns decrypted pdf
        /// </summary>
        /// <param name="reportName">Report to generate</param>
        /// <param name="certificate">Certificate used to encrypt/decrupt</param>
        /// <returns>byte[] corresponding to decrypted pdf</returns>
        public byte[] Process(string reportName, X509Certificate2 certificate)
        {
            byte[] finalBytes;

            using (TcpClient tcpClient = new TcpClient(address, port))
            {
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    var publicKey = certificate.GetRSAPublicKey()?.ExportParameters(false);
                    if (publicKey == null)
                        throw new IncorrectKeyException("Public key is invalid");

                    var objectBytes = new ObjectPair(reportName, publicKey).ToBytes();

                    // Send parameters to server
                    stream.Write(objectBytes, 0, objectBytes.Length);

                    // Wait for server response
                    var str = Utils.ReadToString(stream);

                    // Deserialize response and decrypt it
                    KeyFilePair response = KeyFilePair.Deserialize(str);
                    finalBytes = Hybrid.Decrypt(certificate, response);
                }
            }
            
            // Return decrypted file
            return finalBytes;
        }
    }
}
