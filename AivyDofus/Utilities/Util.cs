﻿using System;
using System.Linq;
using System.Collections.Generic;
using Codebreak.Framework.Utils;
using System.Text;
using System.Web;
using System.Globalization;

namespace Codebreak.Service.World
{
    /// <summary>
    /// 
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        public static List<char> HASH = new List<char>() {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
                't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};

        /// <summary>
        /// 
        /// </summary>
        private static char[] CHAR_LIST = new char[] 
                                         {
                                             '0', '1','2','3','4','5','6','7','8','9',
                                             'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
                                         };
        
        /// <summary>
        /// 
        /// </summary>
        private static FastRandom Random = new FastRandom();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            return Random.Next(min, max);
        }        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int NextJet(int min, int max)
        {
            max++;
            if (max <= min)
                return min;
            return Next(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String EncodeBase36(long input)
        {
            var result = new Stack<char>();
            bool negative = input < 0;
            input = Math.Abs(input);

            while (input != 0)
            {
                result.Push(CHAR_LIST[input % 36]);
                input /= 36;
            }

            if (negative) result.Push('-');

            return new string(result.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellId"></param>
        /// <returns></returns>
        public static string CellToChar(int cellId)
        {
            return HASH[cellId / 64].ToString() + HASH[cellId % 64];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellCode"></param>
        /// <returns></returns>
        public static int CharToCell(string cellCode)
        {
            char char1 = cellCode[0], char2 = cellCode[1];
            int code1 = 0, code2 = 0, a = 0;
            while (a < HASH.Count)
            {
                if (HASH[a] == char1)
                {
                    code1 = a * 64;
                }
                if (HASH[a] == char2)
                {
                    code2 = a;
                }
                a++;
            }
            return (code1 + code2);
        }     
    }
}
