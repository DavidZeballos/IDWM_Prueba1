using UserApi.src.Models;
using Microsoft.Extensions.DependencyInjection;
using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using UserApi.src.Data;

namespace UserApi.src.Data
{
    public static class UserSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;
                }
                
                var genders = new List<string> { "masculino", "femenino", "otro", "prefiero no decirlo" };

                var faker = new Faker<User>()
                .RuleFor(u => u.Id, f => f.IndexFaker + 1)
                .RuleFor(u => u.RUT, f => f.Random.Replace("########-#"))
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Gender, f => f.PickRandom(genders))
                .RuleFor(u => u.BirthDate, f => f.Date.Past(100, DateTime.Now.AddYears(-1)));

                var users = faker.Generate(10); 

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}