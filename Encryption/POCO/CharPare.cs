using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryption.POCO
{
    class CharPare
    {
        public char FirstLetter { get; set; }
        public char LastLetter { get; set; }

        public override string ToString()
        {
            return new string(new char[] { FirstLetter, LastLetter });
        }
    }
}
