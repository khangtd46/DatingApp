using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DatingApp.Data
{
    public class Seed
    {
        public static async Task SeedUsers(AppDBContext context)
        {
            if (await context.Users.AnyAsync()) return;
            var data = await File.ReadAllTextAsync("Assets/UserSeedData.json");
            var option = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            var dataJson = JsonSerializer.Deserialize<List<SystemUser>>(data, option);
            foreach (var user in dataJson)
            {
               using var hmac = new HMACSHA512();
               user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("p4$$w0rd"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);
            }
            await context.SaveChangesAsync();
        }
    }
}
