using DAMSecurityLib.Crypto;
using DAMSecurityLib.Data;
using DAMUtils.Socket.Data;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Process Client function
        /// </summary>
        protected override void ProcessClient()
        {
            if (listener == null)
                return;

            var client = listener.AcceptTcpClient();

            NetworkStream stream = client.GetStream();
            var str = Utils.ReadToString(stream);

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

            stream.Write(sendBytes, 0, sendBytes.Length);

        }

    }
}
