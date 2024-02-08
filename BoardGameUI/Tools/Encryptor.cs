using System.Security.Cryptography;
using System.Text;

namespace BoardGameUI.Tools
{
	public class Encryptor
	{
		public static string Encrypt(string password)
		{
			var provider = MD5.Create();
			string salt = "B00r4d6a33me7a1a2";
			byte[] bytes = provider.ComputeHash(Encoding.UTF32.GetBytes(salt + password));
			return BitConverter.ToString(bytes).Replace("-", "").ToLower();
		}
	}
}
