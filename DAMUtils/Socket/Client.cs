using DAMSecurityLib.Crypto;
using DAMSecurityLib.Data;
using DAMSecurityLib.Exceptions;
using DAMUtils.PDF;
using DAMUtils.Socket.Data;
using Newtonsoft.Json;
using Org.BouncyCastle.Tls;
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

        /// <summary>
        /// Asks the server for available reports
        /// </summary>
        /// <returns>List of available reports</returns>
        /// <exception cref="Exception">If occurrs any error or data is incorrect</exception>
        public List<PDF.ReportInfo> GetAvailableReports()
        {
            using (TcpClient tcpClient = new TcpClient(address, port))
            {
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    ClientRequest clientRequest = new ClientRequest();
                    clientRequest.RequestType = RequestType.ListReports;
                    var objectBytes = clientRequest.ToBytes();

                    // Send parameters to server
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(objectBytes.LongLength);
                    stream.Write(objectBytes, 0, objectBytes.Length);

                    // Wait for server response
                    BinaryReader reader = new BinaryReader(stream);
                    var size = reader.ReadInt64();
                    int bytesRead = 0;
                    byte[] buffer = new byte[1024];
                    StringBuilder receivedData = new StringBuilder();
                    while (bytesRead < size)
                    {
                        bytesRead = bytesRead + stream.Read(buffer, 0, buffer.Length);
                        receivedData.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                    }
                    var str = receivedData.ToString();
                    //var str = Utils.ReadToString(stream);

                    // Deserialize response
                    ServerResponse serverResponse = ServerResponse.Deserialize(str);
                    if (serverResponse == null || serverResponse.Data == null)
                        throw new Exception();

                    var str2 = serverResponse.Data.ToString();
                    if (str2 == null)
                        throw new Exception();
                    var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PDF.ReportInfo>>(str2);
                    if (list == null)
                        throw new Exception();
                    return list;
                }
            }
        }

        /// <summary>
        /// Sends data to the server and returns decrypted pdf
        /// </summary>
        /// <param name="reportInfo">Report and required info to generate</param>
        /// <param name="certificate">Certificate used to encrypt/decrupt</param>
        /// <returns>byte[] corresponding to decrypted pdf</returns>
        public byte[] GetReportData(ReportInfo reportInfo, X509Certificate2 certificate)
        {
            byte[] finalBytes;

            using (TcpClient tcpClient = new TcpClient(address, port))
            {
                using (NetworkStream stream = tcpClient.GetStream())
                {
                    var publicKey = certificate.GetRSAPublicKey()?.ExportParameters(false);
                    if (publicKey == null)
                        throw new IncorrectKeyException("Public key is invalid");

                    ClientRequest clientRequest = new ClientRequest();
                    clientRequest.RequestType = RequestType.GetReport;
                    clientRequest.Data = new ObjectPair(reportInfo, publicKey);
                    var objectBytes = clientRequest.ToBytes();

                    // Send parameters to server
                    stream.Write(objectBytes, 0, objectBytes.Length);

                    // Wait for server response
                    var str = Utils.ReadToString(stream);

                    // Deserialize response and decrypt it
                    ServerResponse serverResponse = ServerResponse.Deserialize(str);
                    if (serverResponse == null || serverResponse.Data == null)
                        throw new Exception();

                    KeyFilePair responseData = (KeyFilePair)serverResponse.Data;
                    finalBytes = Hybrid.Decrypt(certificate, responseData);

                    return finalBytes;
                }
            }

        }
    }
}
