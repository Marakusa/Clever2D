using System.Security.Cryptography;
using System.Text;

namespace Clever2D.Engine
{
	/// <summary>
	/// Cryptography has hashing methods.
	/// </summary>
	public static class Cryptography
	{
		/// <summary>
		/// Returns a SHA1 hash of a string.
		/// </summary>
		/// <param name="input">String to hash.</param>
		public static string HashSHA1(string input)
		{
			using (SHA1Managed sha1 = new SHA1Managed())
			{
				var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
				var sb = new StringBuilder(hash.Length * 2);

				foreach (byte b in hash)
				{
					sb.Append(b.ToString("x2"));
				}

				return sb.ToString();
			}
		}
	}
}
