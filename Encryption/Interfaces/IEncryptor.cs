using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryption.Interfaces
{
    public interface IEncryptor
    {
        string Encrypt(string text);
        string Decrypt(string text);
    }
}
