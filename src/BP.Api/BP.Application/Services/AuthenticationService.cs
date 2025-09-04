using BP.Application.Interfaces.Services;
using BP.Domain.Repository;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BP.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private const int Iterations = 100_000;
    private const int SaltSize = 16; // 128-bit
    private const int KeySize = 32;  // 256-bit

    private readonly IUserRepository _userRepository;
    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<bool> Authenticate(string username, string password)
    {
        var user = await _userRepository.GetUser(username);
        if (user == null)
            return false;

        return VerifyPassword(password, user.Password);
    }

    public string HashPassword(string password)
    {
        // Generate a salt
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        // Derive a key (hash)
        byte[] hash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: Iterations,
            numBytesRequested: KeySize);

        // Store: {iterations}.{salt}.{hash}
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split('.');
        if (parts.Length != 3)
            return false;

        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] storedHash = Convert.FromBase64String(parts[2]);

        // Recompute hash
        byte[] computedHash = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: iterations,
            numBytesRequested: storedHash.Length);

        // Compare in constant time
        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }
}
