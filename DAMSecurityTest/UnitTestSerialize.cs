using DAMSecurityLib.Crypto;
using DAMSecurityLib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAMSecurityTest
{
    public class UnitTestSerialize
    {
        [Test]
        public void TestKeyFilePairSerialize()
        {
            KeyFilePair pair = new KeyFilePair();
            AESCrypt aES = new AESCrypt();

            pair.Key = aES.Key;
            pair.File = aES.IV;
            var str = pair.Serialize();

            KeyFilePair newPair = KeyFilePair.Deserialize(str);
            if (pair.Key == null || newPair.Key == null || pair.File == null || newPair.File == null)
            {
                Assert.IsTrue(false);
                return;
            }

            Assert.That(pair.Key.SequenceEqual(newPair.Key));
            Assert.That(pair.File.SequenceEqual(newPair.File));
        }

        [Test]
        public void TestKeyFilePairEquals()
        {
            KeyFilePair pair = new KeyFilePair();
            AESCrypt aES = new AESCrypt();

            pair.Key = aES.Key;
            pair.File = aES.IV;
            var str = pair.Serialize();

            KeyFilePair newPair = KeyFilePair.Deserialize(str);
            Assert.IsTrue(pair.Equals(newPair));
        }
    }
}
