using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EncodingS = System.Text.Encoding;
using System.Threading.Tasks;
using System.Web;

namespace Oblivion.Utils.Encoding
{
    public class XorCipher
    {
        private string key;

        public XorCipher(string key)
        {
            this.key = key;
        }

        public string Key()
        {
            return key;
        }

        public string encrypt(string value, int keyOffset)
        {
            value = escape(value);
            StringBuilder encrypted = new StringBuilder(value.Length * 2);
            for (int i = 0; i < value.Length; i++)
            {
                char c = value.ToCharArray()[i];
                char k = key.ToCharArray()[(i + keyOffset) % key.Length];

                char e = (char)(c ^ k);

                if(e < 16)
                {
                    encrypted.Append('0');
                }

                encrypted.Append(Convert.ToByte(e).ToString("X"));
            }

            return encrypted.ToString().ToUpper();
        }

        public string decrypt(string value, int keyOffset)
        {
            if(value.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid encrypted value");
            }

            char[] decrypted = new char[value.Length / 2];

            for (int i = 0; i < value.Length ; i += 2)
            {
                char k = key.ToCharArray()[((i / 2 + keyOffset) % key.Length)];
                char c = (char)int.Parse(value.Substring(i, i + 2));

                decrypted[i / 2] = (char)(c ^ k);
            }

            try
            {
                return HttpUtility.UrlDecode(new string(decrypted));
            }
            catch (Exception e)
            {
                throw new ArgumentException("Invalid UTF-8 character", e);
            }
        }


        private static string escape(string value)
        {
            StringBuilder escaped = new StringBuilder(value.Length);

            try
            {
                for (int i = 0; i < value.Length; i++)
                {
                    char c = value.ToCharArray()[i];

                    if (c < 32 || c > 127 || c == '%' || c == '+')
                    {
                        escaped.Append(HttpUtility.UrlEncode(Convert.ToString(c), EncodingS.UTF8));
                    }
                    else
                    {
                        escaped.Append(c);
                    }
                }
            }
            catch (Exception e)
            {

                throw new ArgumentException("Invalid UTF-8 character", e);
            }


            return escaped.ToString();
        }
    }
}
