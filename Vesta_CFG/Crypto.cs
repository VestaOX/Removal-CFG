using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Vesta_CFG
{
	// Token: 0x0200000B RID: 11
	public class Crypto
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00004594 File Offset: 0x00002794
		public static string GetKey()
		{
			return Crypto.string_0;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000045AC File Offset: 0x000027AC
		public static byte[] MD5Hash(string value)
		{
			return Crypto.md5CryptoServiceProvider_0.ComputeHash(Encoding.ASCII.GetBytes(value));
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000045D4 File Offset: 0x000027D4
		public static string Encrypt(string stringToEncrypt)
		{
			Crypto.tripleDESCryptoServiceProvider_0.Key = Crypto.MD5Hash(Crypto.string_0);
			Crypto.tripleDESCryptoServiceProvider_0.Mode = CipherMode.ECB;
			byte[] bytes = Encoding.ASCII.GetBytes(stringToEncrypt);
			return Convert.ToBase64String(Crypto.tripleDESCryptoServiceProvider_0.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000462C File Offset: 0x0000282C
		public static string Decrypt(string encryptedString)
		{
			string result;
			try
			{
				Crypto.tripleDESCryptoServiceProvider_0.Key = Crypto.MD5Hash(Crypto.string_0);
				Crypto.tripleDESCryptoServiceProvider_0.Mode = CipherMode.ECB;
				byte[] array = Convert.FromBase64String(encryptedString);
				result = Encoding.ASCII.GetString(Crypto.tripleDESCryptoServiceProvider_0.CreateDecryptor().TransformFinalBlock(array, 0, array.Length));
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to get data please update your software or contact your software administrator!", "Decryption Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				result = null;
			}
			return result;
		}

		// Token: 0x04000012 RID: 18
		private static TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider_0 = new TripleDESCryptoServiceProvider();

		// Token: 0x04000013 RID: 19
		private static MD5CryptoServiceProvider md5CryptoServiceProvider_0 = new MD5CryptoServiceProvider();

		// Token: 0x04000014 RID: 20
		private static string string_0 = "i34yP@$$3r2.82o20R@NzH!3o7";
	}
}
