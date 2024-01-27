using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.Socket
{
    public class Utils
    {
        /// <summary>
        /// Reads contents from NetworkStream and converts it to string
        /// </summary>
        /// <param name="stream">Networkstream to read data</param>
        /// <returns>Content's string representation</returns>
        public static string ReadToString(NetworkStream stream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            StringBuilder receivedData = new StringBuilder();

            // Receive data from client
            
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                receivedData.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }

            return receivedData.ToString();
        }

    }
}
