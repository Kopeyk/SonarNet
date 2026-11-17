using System.Security.Cryptography;
using System.Text;

namespace SonarNet
{
    /// <summary>Базова аутентифікація: реєстрація, вхід і блокування після 3 невдалих спроб.</summary>
    public class UserAuth
    {
        private const int MaxFailedAttempts = 3;
        private readonly Dictionary<string, string> _passwordHashes = new Dictionary<string, string>();
        private readonly Dictionary<string, int> _failedAttempts = new Dictionary<string, int>();

        public bool Register(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return false;
            }
            if (_passwordHashes.ContainsKey(login))
            {
                return false;
            }
            _passwordHashes[login] = Hash(password);
            _failedAttempts[login] = 0;
            return true;
        }

        public bool Login(string login, string password)
        {
            if (login == null || !_passwordHashes.TryGetValue(login, out var hash) || IsLocked(login))
            {
                return false;
            }
            if (string.Equals(hash, Hash(password ?? string.Empty), StringComparison.Ordinal))
            {
                _failedAttempts[login] = 0;
                return true;
            }
            _failedAttempts[login]++;
            return false;
        }

        public bool IsLocked(string login) =>
            _failedAttempts.TryGetValue(login, out var failed) && failed >= MaxFailedAttempts;

        // Пароль нормалізується перед хешуванням
        private static string Hash(string password) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password.ToLowerInvariant())));
    }
}
