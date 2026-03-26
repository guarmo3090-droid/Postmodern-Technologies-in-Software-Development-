using NUnit.Framework;
using System.IO;
using MyTail;

namespace MyTail.Tests
{
    [TestFixture]
    public class TailAppTests
    {
        [Test]
        public void Run_ValidStdinWithN_Returns0AndCorrectLines()
        {
            // Arrange
            var input = new StringReader("line1\nline2\nline3\nline4\nline5\n");
            var output = new StringWriter();
            var error = new StringWriter();

            // Act
            int exitCode = TailApp.Run(new[] { "-n", "2" }, input, output, error);

            // Assert
            Assert.That(exitCode, Is.EqualTo(0));
            Assert.That(output.ToString().Replace("\r", ""), Is.EqualTo("line4\nline5\n"));
            Assert.That(error.ToString(), Is.Empty);
        }

        [Test]
        public void Run_InvalidOptionFormat_Returns2AndError()
        {
            // Arrange
            var input = new StringReader("test\n");
            var output = new StringWriter();
            var error = new StringWriter();

            // Act
            int exitCode = TailApp.Run(new[] { "-n", "invalid" }, input, output, error);

            // Assert
            Assert.That(exitCode, Is.EqualTo(2));
            Assert.That(output.ToString(), Is.Empty);
            Assert.That(error.ToString(), Does.Contain("valid positive numeric argument").IgnoreCase);
        }

        [Test]
        public void Run_FileNotFound_Returns1AndError()
        {
            // Arrange
            var input = new StringReader("");
            var output = new StringWriter();
            var error = new StringWriter();

            // Act
            int exitCode = TailApp.Run(new[] { "nonexistent_file.txt" }, input, output, error);

            // Assert
            Assert.That(exitCode, Is.EqualTo(1));
            Assert.That(output.ToString(), Is.Empty);
            Assert.That(error.ToString(), Does.Contain("not found").IgnoreCase);
        }
    }
}