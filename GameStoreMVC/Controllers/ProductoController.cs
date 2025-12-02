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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await productoRepo.ObtenerProductoPorIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductoModel productoModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productoModel);
            }
            await productoRepo.ActualizarProductoAsync(productoModel);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await productoRepo.ObtenerProductoPorIdAsync(id);

            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await productoRepo.EliminarProductoAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}