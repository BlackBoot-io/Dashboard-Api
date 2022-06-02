using System.Security.Cryptography;
using System.Text;

namespace BlackBoot.Shared.Extentions;

public static class HashGenerator
{


    public static string Hash(string key)
    {
        string s = "Bl@ckb00t";
        byte[] bytes = Encoding.Unicode.GetBytes(key);
        byte[] bytes2 = Encoding.Unicode.GetBytes(s);
        byte[] array = new byte[bytes2.Length + bytes.Length];
        Buffer.BlockCopy(bytes2, 0, array, 0, bytes2.Length);
        Buffer.BlockCopy(bytes, 0, array, bytes2.Length, bytes.Length);
        return Convert.ToBase64String(HashAlgorithm.Create("SHA256")!.ComputeHash(array));
    }
}
