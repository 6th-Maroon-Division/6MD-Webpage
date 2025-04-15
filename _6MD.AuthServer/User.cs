using _6MD.AuthServer.DB;
using _6MD.AuthServer.utils;
using _6MD.AuthServer.utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using System.Security.Claims;
using System.Text.Json;

namespace _6MD.AuthServer
{
    public class User
    {
        public string Username { get; }
        private PasswordHash PasswordHash { get; }

        private DB.User? _User;

        private UserDB _context;

        public User(UserDB context, string Username, string Password)
        {
            _context = context;
            this.Username = Username;
            _User = context.Users
                .Include(u => u.Ranks)
                .Include(u => u.Groups)
                    .ThenInclude(g => g.Premissions)
                .Include(u => u.Premissions)
                .FirstOrDefault(u => u.Name == Username);
            if (_User != null)
            {
                if (_User.PasswordHash is null || _User.PasswordSalt is null)
                {
                    throw new PasswordNotSetException(Username);
                }
                this.PasswordHash = new PasswordHash(Password, _User.PasswordHash, _User.PasswordSalt);
            }
            else
            {
                throw new UserNotFoundException(Username);
            }
        }

        public string GUID { get => _User?.Guid.ToString()??""; }
        public string Email { get => _User?.Email ?? ""; }

        public Dictionary<string, int> getPermissions()
        {
            Dictionary<string,int> permissions = [];
            if (_User is null)
                throw new UserNotFoundException(Username);
            foreach (var item in _User.Groups)
            {
                foreach (var item1 in item.Premissions)
                {
                    if (!permissions.ContainsKey(item1.Key))
                    {
                        permissions.Add(item1.Key, item1.Power);
                    }
                    else
                    {
                        if (item1.Power > permissions[item1.Key])
                        {
                            permissions[item1.Key] = item1.Power;
                        }
                    }
                }
            }
            foreach (var item in _User.Premissions)
            {
                if (!permissions.ContainsKey(item.Key))
                {
                    permissions.Add(item.Key, item.Power);
                }
                else
                {
                    if (item.Power > permissions[item.Key])
                    {
                        permissions[item.Key] = item.Power;
                    }
                }
            }
            return permissions;
        }

        public bool login()
        {
            if (_User is null)
                throw new UserNotFoundException(Username);
            return PasswordHash.Verify();
        }
    }
}
