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
    /// Class representing pair of objects
    /// </summary>
    public class ObjectPair
    {
        /// <summary>
        /// Class first object
        /// </summary>
        public object? Obj1 { get; set; }

        /// <summary>
        /// Class second object
        /// </summary>
        public object? Obj2 { get; set; }

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
        public static ObjectPair Deserialize(string json)
        {
            var obj = JsonConvert.DeserializeObject<ObjectPair>(json);
            if (obj == null)
            {
                throw new IncorrectParametersException("Deserialize json incorrect");
            }
            return obj;
        }
    }
}
