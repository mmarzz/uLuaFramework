using System;
using System.Text;
using System.Security.Cryptography;

namespace SimpleFramework.Utils {

	public class MD5Utils {

        public static string GetMD5(string input) {
			byte[] data = Encoding.UTF8.GetBytes(input);
			return GetMD5(data);
		}

        public static string GetMD5(byte[] input) {
			MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(input);

            StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++) {
				sBuilder.Append(data[i].ToString("x2"));
			}

			return sBuilder.ToString();
		}

        public static bool VerifyMD5(string input, string hash) {
			byte[] data = Encoding.UTF8.GetBytes(input);
			return VerifyMD5(data, hash);
		}

        public static bool VerifyMD5(byte[] input, string hash) {
			string hashOfInput = GetMD5(input);
			StringComparer comparer = StringComparer.OrdinalIgnoreCase;

			if (0 == comparer.Compare(hashOfInput, hash)) {
				return true;
			} else {
				return false;
			}
		}
    }
}