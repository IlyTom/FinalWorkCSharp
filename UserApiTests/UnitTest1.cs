using ModelsLibrary.UserModels.Entity;
using System.Security.Cryptography;
using System.Text;
using UserApi.Utility;

namespace UserApiTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void AuthentificateNullUserTest()
        {
            UserEntity user = null;

            Assert.IsNull(user);
        }

        [Test]
        public void CheckIncorrectPassword()
        {
            Assert.IsTrue(!CheckPassword.Check("3545455"));
        }

        [Test]
        public void CheckCorrectPassword()
        {
            Assert.IsTrue(CheckPassword.Check("Qwerty12"));
        }

        [Test]
        public void CheckIncorrectEmail()
        {
            Assert.IsTrue(!CheckEmail.Check("yandex.ru"));
        }

        [Test]
        public void CheckCorrectEmail()
        {
            Assert.IsTrue(CheckEmail.Check("yandex@yandex.ru"));
        }


        [Test]
        public void AuthentificateCorrectPasswordTest()
        {
            string pass = "password";
            byte[] pw = Encoding.UTF8.GetBytes(pass);
            byte[] salt = Encoding.UTF8.GetBytes("salt");
            UserEntity userEntity = new UserEntity()
            {
                Id = new Guid(),
                Email = "test@mail.ru",
                Password = pw,
                Salt = salt,
                Name = "Test",
                Surname = "Test"
            };

            var expected = pw.Concat(salt).ToArray();
            SHA512 shaM = new SHA512Managed();
            var expPass = shaM.ComputeHash(expected);

            var data = userEntity.Password.Concat(salt).ToArray();
            var password = shaM.ComputeHash(data);
            CollectionAssert.AreEqual(expPass, password);
        }

        [Test]
        public void AuthentificateIncorrectPasswordTest()
        {
            string pass = "password";
            byte[] pw = Encoding.UTF8.GetBytes(pass);
            byte[] salt = Encoding.UTF8.GetBytes("salt");
            UserEntity userEntity = new UserEntity()
            {
                Id = new Guid(),
                Email = "test@mail.ru",
                Password = pw,
                Salt = salt,
                Name = "Test",
                Surname = "Test"
            };

            SHA512 shaM = new SHA512Managed();
            var data = userEntity.Password.Concat(salt).ToArray();
            var password = shaM.ComputeHash(data);
            CollectionAssert.AreNotEqual(Encoding.UTF8.GetBytes("hash"), password);
        }
    }
}