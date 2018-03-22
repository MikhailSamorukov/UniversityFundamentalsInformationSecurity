using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryption.Interfaces
{
    public interface IEncryptorByKey<KeyType>
    {
        string Encrypt(string text, KeyType key);
        string Decrypt(string text, KeyType key);
        KeyType GenerateRandomKey(string text);
    }
}
