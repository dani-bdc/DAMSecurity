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
    /// Class representing Socket client request
    /// </summary>
    public  class ClientRequest
    {
        /// <summary>
        /// Request type
        /// </summary>
        public RequestType RequestType { get; set; } = RequestType.None;

        /// <summary>
        /// Content data associated to RequestType
        /// Each content is associated to RequestType
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
        /// Deserialize json string and convert it to ClientRequest
        /// </summary>
        /// <param name="json">String to deserialize</param>
        /// <returns>ClientRequest corresponding to serialized string</returns>
        /// <exception cref="IncorrectParametersException"></exception>
        public static ClientRequest Deserialize(string json)
        {
            var obj = JsonConvert.DeserializeObject<ClientRequest>(json);
            if (obj == null)
            {
                throw new IncorrectParametersException("Deserialize json incorrect");
            }
            return obj;
        }

    }

}
