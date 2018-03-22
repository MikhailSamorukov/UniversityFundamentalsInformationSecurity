using Encryption;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityFundamentalsInformationSecurity
{
    class Program
    {
        static void Main(string[] args)
        {
            var encryptionByPleyfer = new EncryptionByPleyfer();
            var encryptionBySimpleSubstitution = new EncryptionBySimpleSubstitution();

            FileSystemWorker fw = new FileSystemWorker(encryptionByPleyfer, encryptionBySimpleSubstitution);
            var clearDataPath = $@"{Directory.GetCurrentDirectory()}\ClearData.txt";
            Console.WriteLine("Press y for fill key, press other to generate key automaticly");
            var encryptKey = Console.ReadLine() == "y" ? GenerateKeyManualy() : encryptionBySimpleSubstitution.GenerateRandomKey(FileSystemWorker.ValidateFileAndGetValues(clearDataPath));
            fw.EncryptTwoEncryptors(clearDataPath, encryptKey);
            fw.DecryptTwoEncryptors($@"{Directory.GetCurrentDirectory()}\EnctyptedData.txt", encryptKey);
        }

        public static List<int> GenerateKeyManualy() {
            Console.WriteLine("Fill key please");
            var data = Console.ReadLine();
            if (String.IsNullOrWhiteSpace(data))
                throw new Exception("data can't be null");

            var result = new List<int>();
            var array = data.ToCharArray();

            foreach (var item in array)
            {
                try
                {
                    result.Add((int)char.GetNumericValue(item));
                }
                catch (Exception ex) {
                    throw new Exception($"problem with parce data, details: {ex.Message}");
                }
            }
            return result;
        }
    }
}
