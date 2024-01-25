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

        [Test]
        public void TestKeyIVPairSerialize()
        {
            KeyIVPair pair = new KeyIVPair();
            AESCrypt aES = new AESCrypt();

            pair.Key = aES.Key;
            pair.IV = aES.IV;
            var str = pair.Serialize();

            KeyIVPair newPair = KeyIVPair.Deserialize(str);
            if (pair.Key == null || newPair.Key == null || pair.IV == null || newPair.IV == null)
            {
                Assert.IsTrue(false);
                return;
            }

            Assert.That(pair.Key.SequenceEqual(newPair.Key));
            Assert.That(pair.IV.SequenceEqual(newPair.IV));
        }

        [Test]
        public void TestKeyIVPairEquals()
        {
            KeyIVPair pair = new KeyIVPair();
            AESCrypt aES = new AESCrypt();

            pair.Key = aES.Key;
            pair.IV = aES.IV;
            var str = pair.Serialize();

            KeyIVPair newPair = KeyIVPair.Deserialize(str);
            Assert.IsTrue(pair.Equals(newPair));
        }

        [Test]
        public void TestObjectPairSerialize1()
        {
            ObjectPair objPair = new ObjectPair();
            objPair.Obj1 = 1;
            objPair.Obj2 = "this is a string";

            var str=objPair.Serialize();

            ObjectPair objPair2 = ObjectPair.Deserialize(str);
            Assert.IsTrue(Convert.ToInt32( objPair.Obj1) == Convert.ToInt32( objPair2.Obj1) && objPair.Obj2.Equals(objPair2.Obj2));
        }
    }
}
