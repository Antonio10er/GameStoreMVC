using Microsoft.AspNetCore.Mvc;
using GameStoreMVC.Data;
using GameStoreMVC.Models;

namespace GameStoreMVC.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoRepository productoRepo;

        public ProductoController(ProductoRepository productoRepo)
        {
            this.productoRepo = productoRepo;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await productoRepo.ObtenerProductosAsync();
            return View(productos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoModel productoModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productoModel);
            }
            await productoRepo.AgregarProductoAsync(productoModel);
            return RedirectToAction(nameof(Index));
        }
    }
}