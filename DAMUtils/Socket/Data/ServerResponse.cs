using DAMSecurityLib.Data;
using DAMSecurityLib.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMUtils.Socket.Data
{
    /// <summary>
    /// Class representing Socket server response
    /// </summary>
    public class ServerResponse
    {
        /// <summary>
        /// Response status code
        /// </summary>
        public StatusCode StatusCode { get; set; } = StatusCode.Indeterminate;
        
        /// <summary>
        /// Response Data returned by the server
        /// </summary>
        public object? Data { get; set; } = null;

        /// <summary>
        /// Serialize current object to Json String
        /// </summary>
        /// <returns>Json string corresponding to current object</returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }


        /// <summary>
        /// Converts current object to byte[]
        /// </summary>
        /// <returns>Byte[] corresponding to current object</returns>
        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(Serialize());
        }

        /// <summary>
        /// Deserialize json string and convert it to ServerResponse
        /// </summary>
        /// <param name="json">String to deserialize</param>
        /// <returns>ServerResponse corresponding to serialized string</returns>
        /// <exception cref="IncorrectParametersException"></exception>
        public static ServerResponse Deserialize(string json)
        {
            var obj = JsonConvert.DeserializeObject<ServerResponse>(json);
            if (obj == null)
            {
                throw new IncorrectParametersException("Deserialize json incorrect");
            }
            return obj;
        }
    }
}
