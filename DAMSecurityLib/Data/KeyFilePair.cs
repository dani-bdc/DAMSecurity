using DAMSecurityLib.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityLib.Data
{
    /// <summary>
    /// Class representing pair data with Key and File Byte array
    /// Is used to share and serialize this data
    /// </summary>
    public class KeyFilePair
    {
        /// <summary>
        /// Byte array associated with the Key
        /// </summary>
        public byte[]? Key { get; set; }

        /// <summary>
        /// Byte array associated with the File
        /// </summary>
        public byte[]? File { get; set; }

        /// <summary>
        /// Serialize current object to Json String
        /// </summary>
        /// <returns>Json string corresponding to current object</returns>
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }


        /// <summary>
        /// Deserialize json string and convert it to KeyFilePairObject
        /// </summary>
        /// <param name="json">String to deserialize</param>
        /// <returns>KeyFilePairObject corresponding to deserialized string</returns>
        /// <exception cref="IncorrectParametersException"></exception>
        public static KeyFilePair Deserialize(string json)
        {
            var obj = JsonConvert.DeserializeObject<KeyFilePair>(json);
            if (obj == null)
            {
                throw new IncorrectParametersException("Deserialize json incorrect");
            }
            return obj;
        }

    }
}
