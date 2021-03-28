using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using PacketStudio.Core;

namespace PacketStudio.Tests
{
    [TestClass]
    public class SharksFinderTests
    {
        private string valid_dir;

        [TestInitialize]
        public void InitWiresharkDir()
        {
            var tempPath = new string(Path.GetTempFileName().TakeWhile(c=>c!='.').ToArray());
            var dirInfo = Directory.CreateDirectory(tempPath);
            foreach (string fileName in new string[] {"wireshark.exe", "tshark.exe", "capinfos.exe"})
            {
                string filePath = Path.Combine(tempPath, fileName);
                File.Create(filePath).Close();
            }

            valid_dir = tempPath;
        }



        [TestMethod]
        public void TryGetByPath__DirExists__ReturnsDir()
        {
            // Arrange 

            // Act
            SharksFinder.TryGetByPath(valid_dir, out WiresharkDirectory wd);

            // Assert
            Assert.IsNotNull(wd);
        }
    }
}
