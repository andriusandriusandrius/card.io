using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));
        return $"{Convert.ToBase64String(salt)}:{hashedPassword}";
    }
    public static bool Verify(string password, string hashedPassword)
    {
        string[] parts = hashedPassword.Split(":");
        byte[] salt = Convert.FromBase64String(parts[0]);
        string hash = parts[1];
        string hashOfInput = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8
        ));
        return hash == hashOfInput;

    }
}