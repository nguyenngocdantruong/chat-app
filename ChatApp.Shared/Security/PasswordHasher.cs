
using Generator =  Org.BouncyCastle.Crypto.Generators;

namespace ChatApp.Shared.Security;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty", nameof(password));
        }
        // Generate a salt manually since BCrypt does not have a GenerateSalt method
        var salt = new byte[16];
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        var passwordBytes = Generator.BCrypt.PasswordToByteArray(password.ToCharArray());
        var hashedBytes = Generator.BCrypt.Generate(passwordBytes, salt, 12);
        return Convert.ToBase64String(salt.Concat(hashedBytes).ToArray());
    }

    public static bool Verify(string password, string hash)
    {
        var hashedBytes = Convert.FromBase64String(hash);
        var salt = new byte[16]; // Extract the salt from the hash
        Array.Copy(hashedBytes, 0, salt, 0, 16);

        var passwordBytes = Generator.BCrypt.PasswordToByteArray(password.ToCharArray());
        var generatedHash = Generator.BCrypt.Generate(passwordBytes, salt, 12);

        // Compare the generated hash with the stored hash (excluding the salt part)
        return hashedBytes.Skip(16).SequenceEqual(generatedHash);
    }
}