

namespace Shop.Web.Data
{
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DataContext context) : base(context)
        {
            Context = context;
        }

        public DataContext Context { get; }

        public IQueryable GetAllWithUsers()
        {
            return this.Context.Products.Include(p => p.User);
        }
    }

}
