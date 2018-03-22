using Encryption;
using Encryption.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTests
{
    [TestFixture]
    public class EncryptionByPleyferTests
    {
        private IEncryptor _encryptionByPleyfer;

        [SetUp]
        public void Init() {
            _encryptionByPleyfer = new EncryptionByPleyfer();
        }

        [Test]
        public void TestValidation_Should_Throw_Exception_Element_Cant_Be_Null() {
            //Arrange
            //Act
            var exception = Assert.Throws<ArgumentException>(() => _encryptionByPleyfer.Encrypt(" "));
            //Assert
            Assert.That(exception.Message, Is.EqualTo("Text can't be null"));
        }

        [Test]
        public void TestValidation_Should_Throw_Exception_By_Length()
        {
            //Arrange
            //Act
            var exception = Assert.Throws<ArgumentException>(() => _encryptionByPleyfer.Encrypt("абс"));
            //Assert
            Assert.That(exception.Message, Is.EqualTo("The length of the text should be divided into two"));
        }

        [Test]
        public void TestValidation_Should_Throw_Exception_Unsupported_Symbol()
        {
            //Arrange
            //Act
            var exception = Assert.Throws<ArgumentException>(() => _encryptionByPleyfer.Encrypt("zw"));
            //Assert
            Assert.That(exception.Message, Is.EqualTo("Unsupported symbol"));
        }

        [Test]
        public void TestValidation_Should_Throw_Exception_Found_Equal_Elements()
        {
            //Arrange
            //Act
            var exception = Assert.Throws<ArgumentException>(() => _encryptionByPleyfer.Encrypt("ккриптографияя"));
            //Assert
            Assert.That(exception.Message, Is.EqualTo("Found equal elements in pare"));
        }
        [Test]
        public void TestValidation_Should_Right_Encrypt()
        {
            //Arrange
            //Act
            var result = _encryptionByPleyfer.Encrypt("криптография");
            //Assert
            Assert.That(result == "итйицкаудрпш");
        }
        [Test]
        public void TestValidation_Should_Right_Decrypt()
        {
            //Arrange
            //Act
            var result = _encryptionByPleyfer.Decrypt("итйицкаудрпш");
            //Assert
            Assert.That(result == "криптография");
        }
    }
}
