using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParserCombinators;
using System.Linq;
namespace ParserCombinatorsTest {
    [TestClass]
    public class UnitTest1 {

        [TestMethod]
        public void AnyTest() {
            var p = Parsers.Any<char>();
            var r = p.Parse("hello world".ToCharArray());
            Assert.AreEqual<char>('h', r);
        }
        [TestMethod]
        public void ManyTest() {
            var p = Parsers.Any<char>();
            var ps = p.Many();
            var r = ps.Parse("hello world".ToCharArray());
            Assert.AreEqual("hello world", new string(r.value.ToArray()));
        }
    }
}
