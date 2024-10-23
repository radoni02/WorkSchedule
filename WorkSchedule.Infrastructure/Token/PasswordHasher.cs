using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WorkSchedule.Application.Interfaces;

namespace WorkSchedule.Infrastructure.Token;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool Verify(string password, string? passwordFromDb)
    {
        var parts = passwordFromDb.Split('-');
        var salt = Convert.FromHexString(parts[1]);
        var hash = Convert.FromHexString(parts[0]);

        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
        //return hash.SequenceEqual(inputHash);
        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}
