using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Oblivion.Utils.Encoding
{
    public class Key
    {
        private static Random provider;
        private string key;
        private XorCipher Cipher;

        public Key(string key)
        {
            this.key = key;
        }

        public XorCipher cipher()
        {
            if (Cipher == null)
                Cipher = new XorCipher(key);

            return Cipher;
        }

        public string encode()
        {
            string raw;
            try
            {
                raw = HttpUtility.UrlEncode(key);
            }
            catch (Exception e)
            {

                throw new ArgumentException("Invalid UTF-8 key", e);
            }

            StringBuilder encrypted = new StringBuilder(raw.Length * 2);

            for (int i = 0; i < raw.Length; i++)
            {
                char c = raw.ToCharArray()[i];

                if (c < 16)
                    encrypted.Append('0');

                encrypted.Append(Convert.ToByte(c).ToString("X4"));
            }

            return encrypted.ToString();
        }

        public static Key parse(string input)
        {
            if (input.Length % 2 != 0)
                throw new ArgumentException("Invalid Key");

            char[] keyArr = new char[input.Length / 2];

            for (int i = 0; i < input.Length; i += 2)
            {
                keyArr[i / 2] = (char)int.Parse(input.Substring(i, i + 2));
            }

            try
            {
                return new Key(HttpUtility.UrlDecode(new string(keyArr)));
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid UTF-8 character");
            }
        }

        public static Key generate(int size)
        {
            if (provider == null)
                provider = new Random();

            return generate(size, provider);
        }

        public static Key generate(int size, Random random)
        {
            char[] keyArr = new char[size];

            for (int i = 0; i < size; i++)
            {
                keyArr[i] = (char)(random.Next(127 - 32) + 32);
            }

            return new Key(new string(keyArr));
        }
    }
}
