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
    class EncryptionBySimpleSubstitutionTests
    {
        private IEncryptorByKey<List<int>> _encryptionBySimpleSubstitution;

        [SetUp]
        public void Init()
        {
            _encryptionBySimpleSubstitution = new EncryptionBySimpleSubstitution();
        }

        [Test]
        public void TestValidation_Should_Right_Encrypt()
        {
            //Arrange
            //Act
            var result = _encryptionBySimpleSubstitution.Encrypt("НА ПЕРВОМ КУРСЕ ТЯЖЕЛО УЧИТЬСЯ ТОЛЬКО ПЕРВЫЕ ЧЕТЫРЕ ГОДА ДЕКАНАТ", new List<int> { 2, 4, 7, 1, 8, 5, 3, 6 });
            //Assert
            Assert.That(result == "А_ЯИЛВРДПУЕЬКЕ_КВЕ__ПЕДАНМТЧОРЫ_О_УТЕТАТЕРЛСО_ГА_КЖТЬЫЕЕРСОЯ_ЧОН");
        }

        [Test]
        public void TestValidation_Should_Right_Decrypt()
        {
            //Arrange
            //Act
            var result = _encryptionBySimpleSubstitution.Decrypt("А_ЯИЛВРДПУЕЬКЕ_КВЕ__ПЕДАНМТЧОРЫ_О_УТЕТАТЕРЛСО_ГА_КЖТЬЫЕЕРСОЯ_ЧОН", new List<int> { 2, 4, 7, 1, 8, 5, 3, 6 });
            //Assert
            Assert.That(result == "НА ПЕРВОМ КУРСЕ ТЯЖЕЛО УЧИТЬСЯ ТОЛЬКО ПЕРВЫЕ ЧЕТЫРЕ ГОДА ДЕКАНАТ");
        }

        [Test]
        public void TestValidation_Should_Throw_Exception_By_Key_Length()
        {
            //Arrange
            //Act
            var exception =
                Assert.Throws<ArgumentException>(() => _encryptionBySimpleSubstitution.Encrypt("НА ПЕРВОМ КУРСЕ ТЯЖЕЛО УЧИТЬСЯ ТОЛЬКО ПЕРВЫЕ ЧЕТЫРЕ ГОДА ДЕКАНАТ", new List<int> { 4, 8, 1, 2, 7, 6, 5 }));
            //Assert
            Assert.That(exception.Message, Is.EqualTo("The key length should be: 8"));
        }

        [Test]
        public void TestValidation_Should_Throw_Exception_By_Repetable_Key()
        {
            //Arrange
            //Act
            var exception =
                Assert.Throws<ArgumentException>(() => _encryptionBySimpleSubstitution.Encrypt("НА ПЕРВОМ КУРСЕ ТЯЖЕЛО УЧИТЬСЯ ТОЛЬКО ПЕРВЫЕ ЧЕТЫРЕ ГОДА ДЕКАНАТ", new List<int> { 4, 8, 1, 2, 7, 6, 5, 5 }));
            //Assert
            Assert.That(exception.Message, Is.EqualTo("The key doesn't should contain repeatable value"));
        }

        [Test]
        public void TestValidation_Should_Throw_Exception_Element_Cant_Be_Null()
        {
            //Arrange
            //Act
            var exception = Assert.Throws<ArgumentException>(() => _encryptionBySimpleSubstitution.Encrypt(" ", new List<int>()));
            //Assert
            Assert.That(exception.Message, Is.EqualTo("Text can't be null"));
        }

        [Test]
        public void TestValidation_Should_Throw_Exception_Key_Cant_Be_Null()
        {
            //Arrange
            //Act
            var exception = Assert.Throws<ArgumentException>(() => _encryptionBySimpleSubstitution.Encrypt("test", null));
            //Assert
            Assert.That(exception.Message, Is.EqualTo("Key can't be null"));
        }
    }
}
