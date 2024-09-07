using System.Text;

namespace stocks.Services.Helpers;

public static class EncryptionService
{
    public static string GetEncrypted(string toEncrypt)
    {
        var bytes = Encoding.ASCII.GetBytes(toEncrypt);
        return Convert.ToBase64String(bytes);
    }

    public static string GetDecrypted(string toDecrypt)
    {
        var bytes = Convert.FromBase64String(toDecrypt);
        return Encoding.ASCII.GetString(bytes);
    }
}