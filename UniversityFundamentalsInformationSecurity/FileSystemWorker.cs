using Encryption.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityFundamentalsInformationSecurity
{
    class FileSystemWorker
    {
        IEncryptor _firstEncryptor;
        IEncryptorByKey<List<int>> _secondEncryptor;

        public FileSystemWorker(IEncryptor firstEncryptor, IEncryptorByKey<List<int>> secondEncryptor)
        {
            _firstEncryptor = firstEncryptor;
            _secondEncryptor = secondEncryptor;
        }

        public void EncryptTwoEncryptors(string path, List<int> key)
        {
            var inputData = ValidateFileAndGetValues(path, Encoding.GetEncoding(1251));
            var encryptData = _firstEncryptor.Encrypt(inputData);
            encryptData = _secondEncryptor.Encrypt(encryptData, key);
            var pathToWrite = $@"{Directory.GetCurrentDirectory()}\EnctyptedData.txt";

            WrtiteToFile(encryptData, pathToWrite);
        }

        public void DecryptTwoEncryptors(string path, List<int> key)
        {
            var inputData = ValidateFileAndGetValues(path);
            var decryptData = _secondEncryptor.Decrypt(inputData, key);
            decryptData = _firstEncryptor.Decrypt(decryptData);
            var pathToWrite = $@"{Directory.GetCurrentDirectory()}\DecryptedData.txt";

            WrtiteToFile(decryptData, pathToWrite);
        }

        public static string ValidateFileAndGetValues(string path, Encoding encode = null)
        {
            if (!File.Exists(path))
                throw new Exception("File doesn't exist");

            var inputData = encode == null ? String.Concat(File.ReadAllLines(path)) : String.Concat(File.ReadAllLines(path, encode));

            if (string.IsNullOrWhiteSpace(inputData))
                throw new Exception("File is empty");
            return inputData;
        }

        private static void WrtiteToFile(string data, string path)
        {
            if (!File.Exists(path))
                File.Create(path);

            File.WriteAllText(path, data);
        }
    }
}
