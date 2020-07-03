namespace Shop.Web.Data

{

    using Shop.Web.Data.Entities;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class SeedDb
    {
        private readonly DataContext context;
        private readonly Random random;

        public SeedDb(DataContext context)
        {
            this.context = context;
            random = new Random();
        }

        public async Task SeedAsync()
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.Products.Any())
            {
                AddProduct("iPhone x");
                AddProduct("Magic Mouse");
                AddProduct("iwath Series 4");
                await context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name)
        {
            context.Products.Add(new Product
            {
                Name = name,
                Price = random.Next(1000),
                IsAvailable = true,
                Stock = random.Next(100)
            });
        }
    }
}

