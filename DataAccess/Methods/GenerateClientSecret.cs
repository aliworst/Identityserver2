using System.Security.Cryptography;
using Duende.IdentityServer.Models;

namespace DataAccess.Methods;

public class GenerateClientSecret
{
    public static byte[] GetRandomBytes(int length = 32)
    {
        using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        var randomBytes = new byte[length];
        rngCryptoServiceProvider.GetBytes(randomBytes);
        return randomBytes;
    }
    
    public static string GetRandomClientSecret(int byteLength = 32)
    {
        var randomBytes = GetRandomBytes(byteLength);
        var base64Secret = Convert.ToBase64String(randomBytes);
    
        // Make it URL-safe
        return base64Secret.TrimEnd('=').Replace('+', '-').Replace('/', '_').Sha256();
    }
}