using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
namespace _6MD.AuthServer.utils
{

    public class PasswordHash
    {
        private Argon2id Password { get; }
        private byte[] _Hash { get; set; }
        private byte[] _Salt { get; set; }

        public PasswordHash(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            this.Password = new(bytes);
            this._Salt = GenerateSalt(16);
            this._Hash = Hash();
        }

        public PasswordHash(string password, byte[] salt, byte[] Hash)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            this.Password = new(bytes);
            this._Salt = salt;
            this._Hash = Hash;
        }
        public PasswordHash(string password, string salt, string Hash)
        {
            this._Hash = Convert.FromBase64String(Hash);
            this._Salt = Convert.FromBase64String(salt);
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            this.Password = new(bytes);
        }

        public bool Verify() => _Hash.SequenceEqual(Hash());

        private byte[] Hash()
        {
            Password.DegreeOfParallelism = 1;
            Password.MemorySize = 8192;
            Password.Iterations = 5;
            Password.Salt = this._Salt;
            Password.KnownSecret = Encoding.UTF8.GetBytes("6MD");
            return Password.GetBytes(32);
        }

        public byte[] GetHash() => this._Hash;
        public string GetHashString() => Convert.ToBase64String(this._Hash);

        public byte[] GetSalt() => this._Salt;
        public string GetSaltString() => Convert.ToBase64String(this._Salt);

        private byte[] GenerateSalt(int size)
        {
            byte[] salt = new byte[size];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }

        ~PasswordHash()
        {
            Password.Dispose();
        }
    }
}
