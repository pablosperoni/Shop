namespace Shop.Web.Data

{
    using Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private readonly Random random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            random = new Random();
        }

        public async Task SeedAsync()
        {
            await context.Database.EnsureCreatedAsync();

            User user = await userHelper.GetUserByEmailAsync("pablolsperoni@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Pablo",
                    LastName = "Speroni",
                    Email = "pablolsperoni@gmail.com",
                    UserName = "pablolsperoni@gmail.com",
                    PhoneNumber = "47553093"

                };

                //TODO: Ver si el arreglo de código está bien
                IdentityResult result = await userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }



            if (!context.Products.Any())
            {
                AddProduct("iPhone x", user);
                AddProduct("Magic Mouse", user);
                AddProduct("iwath Series 4", user);
                await context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            context.Products.Add(new Product
            {
                Name = name,
                Price = random.Next(1000),
                IsAvailable = true,
                Stock = random.Next(100),
                User = user
            });
        }
    }
}

