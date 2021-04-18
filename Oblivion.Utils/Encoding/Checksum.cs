using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.Utils.Encoding
{
    public class Checksum
    {
        private Checksum()
        {

        }

        public static int integer(string value)
        {
            int checksum = 0;

            for (int i = 0; i < value.Length; i++)
            {
                checksum += value.ToCharArray()[i] % 16;
            }

            return checksum % 16;
        }

        public static string hexadecimal(string value)
        {
            return integer(value).ToString("X4").ToUpper();
        }

        public static bool verify(string input, int expectedChecksum)
        {
            return integer(input) == expectedChecksum;
        }

        public static bool verify(string input, string expectedChecksum)
        {
            return integer(input) == int.Parse(expectedChecksum);
        }
    }
}
