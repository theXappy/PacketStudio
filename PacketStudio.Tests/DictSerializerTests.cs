using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PacketStudio.DataAccess.Json;

namespace PacketStudio.Tests
{
    [TestClass]
    public class DictSerializerTests
    {
        [TestMethod]
        public void SerializeTest()
        {
            // Arrange
            var saveData = new Dictionary<string, object>();
            saveData["EtherType"] = "Test Str";
            DictJsonSerializer s = new DictJsonSerializer();
            
            // Act
            s.Serialize(saveData);

            // Assert - No excep
        }
    
        [TestMethod]
        public void SerializeDeserialize_Test()
        {
            // Arrange
            string testStr = "Test Str";
            string field = "EtherType";
            var saveData = new Dictionary<string, object>();
            saveData[field] = testStr;
            DictJsonSerializer s = new DictJsonSerializer();

            // Act
            string res = s.Serialize(saveData);
            dynamic des = s.Deserialize(res);

            // Assert
            Assert.AreEqual(testStr, des[field]);
        }
    }
}
