

namespace Shop.Web.Controllers
{
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;


    public class Products1Controller : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly IUserHelper userHelper;

        public Products1Controller(IProductRepository productRepository, IUserHelper userHelper)
        {
            this.productRepository = productRepository;
            this.userHelper = userHelper;
        }

        // GET: Products1
        public IActionResult Index()
        {
            return View(productRepository.GetAll().OrderBy(p => p.Name));
        }

        // GET: Products1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel view)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        view.ImageFile.FileName);

                    using (FileStream stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Products/{view.ImageFile.FileName}";
                }

               var product = this.ToProduct(view, path);

                // TODO: Pending to change to: this.User.Identity.Name
                product.User = await userHelper.GetUserByEmailAsync("pablolsperoni@gmail.com");
                await productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        private Product ToProduct(ProductViewModel view, string path)
        {
            return new Product

            {
                Id = view.Id,
                ImageUrl = path,
                IsAvailable = view.IsAvailable,
                LastPurchase = view.LastPurchase,
                LastSale = view.LastSale,
                Name = view.Name,
                Price = view.Price,
                Stock = view.Stock,
                User = view.User
            };

        }

        // GET: Products1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var view = this.ToProductViewModel(product);
            return View(view);
        }

        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };
        }

    
        // POST: Products1/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel view)
        {
            if (ModelState.IsValid)
            {
                try
                {

                var path = view.ImageUrl;

                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Products",
                        view.ImageFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Products/{view.ImageFile.FileName}";
                }

               

                // TODO: Pending to change to: this.User.Identity.Name
               view.User = await this.userHelper.GetUserByEmailAsync("pablolsperoni@gmail.com");
                var product = this.ToProduct(view, path);
                await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await productRepository.ExistAsync(view.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }




        // GET: Products1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // POST: Products1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product product = await productRepository.GetByIdAsync(id);
            await productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }


    }
}
