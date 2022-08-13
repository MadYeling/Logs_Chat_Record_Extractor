using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Logs_chat_record_extractor
{
    public class TextChecker
    {

        public static string Encrypt(string line, int lineNub)
        {
            var text = Encoding.UTF8.GetBytes($"{line}|{lineNub}");
            var sha256 = new SHA256Managed();
            var textHash = sha256.ComputeHash(text);
            var res = u_49152(textHash);
            return res;
        }

        private static string u_49152(IReadOnlyList<byte> byteStr)
        {
            var lookup = u_49151();
            var res = new List<char>();
            for (var i = 0; i < 16; i++)
            {
                res.Add('\0');
            }

            for (var i = 0; i < 8; i++)
            {
                var num = lookup[byteStr[i]];
                res[2 * i] = (char)(num % 128);
                res[2 * i + 1] = (char)((num >> 16) % 128);
            }

            return string.Join("", res);
        }

        private static List<int> u_49151()
        {
            var numArr = new List<int>();
            for (var i = 0; i < 256; i++)
            {
                var temp = $"{i:x02}";
                numArr.Add(temp[0] + (temp[1] << 16));
            }

            return numArr;
        }
    }
}