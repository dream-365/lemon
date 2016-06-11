using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Demo001
{
    static internal class Utils
    {
        public static string ComputeStringMD5Hash(string text, Encoding encode)
        {
            var md5 = MD5.Create();

            byte[] bytes = encode.GetBytes(text);

            return BytesToHexString(md5.ComputeHash(bytes));
        }


        public static string ComputeStringMD5Hash(string text)
        {
            return ComputeStringMD5Hash(text, Encoding.UTF8);
        }

        public static string ComputeStringPairMD5Hash(string left, string right)
        {
            var temp = new List<string> { left, right };

            temp.Sort();

            var text = string.Join("|", temp);


            return ComputeStringMD5Hash(text, Encoding.UTF8);
        }

        private static string BytesToHexString(byte[] bytes)
        {
            if (bytes == null)
                return null;

            StringBuilder sb = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
                sb.AppendFormat("{0:x2}", bytes[i]);

            return sb.ToString();
        }
    }
}
