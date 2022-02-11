using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGUI.CoreLib.Utils;

namespace TGUI.CoreLib.UnitTests
{
    [TestClass]
    public class CommandParserTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            string command = "/command";
            Assert.IsTrue(SupportFunctions.TryParseCommand(command, out (string command, string content) res));
            Assert.IsTrue(res.command.Equals(command));
            Assert.IsTrue(string.IsNullOrEmpty(res.content));
        }

        [TestMethod]
        public void TestMethod2()
        {
            string command = "/command";
            string content = "adaadadsa";
            string fullCommand = command + " " + content;
            Assert.IsTrue(SupportFunctions.TryParseCommand(fullCommand, out (string command, string content) res));
            Assert.IsTrue(res.command.Equals(command));
            Assert.IsTrue(res.content.Equals(res.content));
        }


        [TestMethod]
        public void TestMethod3()
        {
            string command = "/command";
            string content = "adaadadsa  sdsds ˚‚Ù‚˚‚Ù.";
            string fullCommand = command + " " + content;
            Assert.IsTrue(SupportFunctions.TryParseCommand(fullCommand, out (string command, string content) res));
            Assert.IsTrue(res.command.Equals(command));
            Assert.IsTrue(res.content.Equals(res.content));
        }

        [TestMethod]
        public void TestMethod4()
        {
            string command = "/command";
            string content = "adaadadsa -3%762338?*??:?:?π˝\\sdsds ˚‚Ù‚˚‚Ù.";
            string fullCommand = command + " " + content;
            Assert.IsTrue(SupportFunctions.TryParseCommand(fullCommand, out (string command, string content) res));
            Assert.IsTrue(res.command.Equals(command));
            Assert.IsTrue(res.content.Equals(res.content));
        }
    }
}
