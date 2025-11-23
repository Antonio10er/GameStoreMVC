using Microsoft.AspNetCore.Mvc;
using GameStoreMVC.Data;
using GameStoreMVC.Models;

namespace GameStoreMVC.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductoRepository _repo;

        public ProductosController(IConfiguration config)
        {
            _repo = new ProductoRepository(config);
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _repo.ListarProductosAsync();
            return View(productos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoModel producto)
        {
            if (ModelState.IsValid)
            {
                await _repo.AgregarProductoAsync(producto);
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }
    }
}