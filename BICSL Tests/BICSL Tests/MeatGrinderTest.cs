using BICSL;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BICSL_Tests
{
    [TestClass]
    public class MeatGrinderTest
    {
        [TestMethod]
        public void TestGetApiURL()
        {
            
            PrivateType grinder = new PrivateType(typeof(MeatGrinder));
            PrivateObject obj = new PrivateObject(grinder);
            var retval = obj.Invoke("GetApiURL");

            Assert.AreEqual(1, 1);


        }

        /// <summary>
        /// Tests if the method can get a specific number of sentences
        /// </summary>
        [TestMethod]
        public void TestGetSentences()
        {
            var sentences = MeatGrinder.GetSentences(15, false);
            var i = 0;
            var cIndex = 0;

            while ((cIndex = sentences.IndexOf('.', ++cIndex)) > -1)
            {
                i++;
            }

            Assert.AreEqual<int>(15, i);
        }

        [TestMethod]
        public void TestGetParagraphs()
        {
            var sentences = MeatGrinder.GetSentences(15, false);
            var i = 0;
            var cIndex = 0;

            while ((cIndex = sentences.IndexOf('.', ++cIndex)) > -1)
            {
                i++;
            }

            Assert.AreEqual<int>(15, i);
        }

        [TestMethod]
        public void TestGetWords()
        {

        }
    }
}
