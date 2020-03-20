using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FileSystemVisitor;

namespace FileSystemVisitorTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void toStopTest()
        {
            // arrange
            var dirInfo = new Mock<IDirectory>();
            var visitor = new Visitor(dirInfo.Object);
            var expected = true;
            // act
            visitor.toStop = true;

            // assert
            Assert.AreEqual(expected, visitor.toStop);
        }

        [TestMethod]
        public void toExcludeTest()
        {
            // arrange
            var dirInfo = new Mock<IDirectory>();
            dirInfo.Setup(d => d.GetDirectories(It.IsAny<string>())).Returns(new string[] { "string","string2" });
            var visitor = new Visitor(dirInfo.Object);
            var expected = true;
            
            // act
            //visitor.toExclude = true;
            var tmp = visitor.GetDirs("aa", x => x.Length == 6);


            // assert
            Assert.AreEqual(expected, visitor.toExclude);
            
        }

    }
}
