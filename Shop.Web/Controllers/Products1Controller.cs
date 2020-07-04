

namespace Shop.Web.Controllers
{
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;


    public class Products1Controller : Controller
    {
        private readonly IRepository repository;
        private readonly IUserHelper userHelper;

        public Products1Controller(IRepository repository, IUserHelper userHelper)
        {
            this.repository = repository;
            this.userHelper = userHelper;
        }

        // GET: Products1
        public IActionResult Index()
        {
            return View(repository.GetProducts());
        }

        // GET: Products1/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = repository.GetProduct(id.Value);
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
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                //TODO: Cambiar por el usuario logueado
                product.User = await userHelper.GetUserByEmailAsync("pablolsperoni@gmail.com");
                repository.AddProduct(product);
                await repository.SaveAllAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products1/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = repository.GetProduct(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products1/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    repository.UpdateProduct(product);
                    await repository.SaveAllAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!repository.ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products1/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = repository.GetProduct(id.Value);

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
            Product product = repository.GetProduct(id);
            repository.RemoveProduct(product);
            await repository.SaveAllAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
