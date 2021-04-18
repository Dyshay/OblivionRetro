using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oblivion.Utils.Encoding
{
    public class Base64
    {
        private static char[] CHARSET = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
        't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
        'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};

        private Base64()
        {

        }

        public static int ord(char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return c - 'a';
            }

            if (c >= 'A' && c <= 'Z')
            {
                return c - 'A' + 26;
            }

            if (c >= '0' && c <= '9')
            {
                return c - '0' + 52;
            }

            switch (c)
            {
                case '-':
                    return 62;
                case '_':
                    return 63;
                default:
                    throw new ArgumentException("Invalid char value");
            }
        }

        public static char chr(int value)
        {
            return CHARSET[value];
        }

        public static char chrMod(int value)
        {
            return CHARSET[value % CHARSET.Length];
        }

        public static string encode(int value, int length)
        {
            if(length < 1 || length > 6)
            {
                throw new ArgumentException("Invalid length parameter : it must be in range [1-6]");
            }

            char[] encoded = new char[length];

            for (int i = length - 1; i >= 0; --i)
            {
                encoded[i] = CHARSET[value & 63];
                value >>= 6;
            }

            return new string(encoded);
        }

        public static string encode(byte[] data)
        {
            char[] encoded = new char[data.Length];

            for (int i = 0; i < data.Length; ++i)
            {
                encoded[i] = chr(data[i]);
            }

            return new string(encoded);
        }

        public static int decode(string encoded)
        {
            if (encoded.Length > 6)
                throw new ArgumentException("Invalid encoded string : too long");

            int value = 0;

            for (int i = 0; i < encoded.Length; ++i)
            {
                value <<= 6;
                value += ord(encoded.ToCharArray()[i]);
            }

            return value;
        }

        public static byte[] toBytes(string encoded)
        {
            byte[] decoded = new byte[encoded.Length];

            for (int i = 0; i < encoded.Length; i++)
            {
                decoded[i] = (byte)Base64.ord(encoded.ToCharArray()[i]);
            }

            return decoded;
        }
    }
}
