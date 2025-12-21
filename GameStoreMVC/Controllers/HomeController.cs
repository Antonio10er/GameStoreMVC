using GameStoreMVC.Data;
using GameStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace GameStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductoRepository _productoRepo;

        public HomeController(ILogger<HomeController> logger, ProductoRepository productoRepo)
        {
            _logger = logger;
            _productoRepo = productoRepo;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoRepo.ObtenerProductosAsync();

            var model = new HomeViewModel();

            model.JuegosPS5 = productos.Where(p => p.IdCategoria == 1).Take(4).ToList();
            model.JuegosSwitch = productos.Where(p => p.IdCategoria == 2).Take(4).ToList();
            model.Consolas = productos.Where(p => p.IdCategoria == 3).Take(4).ToList();
            model.Accesorios = productos.Where(p => p.IdCategoria == 4).Take(4).ToList();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int idProducto)
        {
            ProductoModel producto = await _productoRepo.ObtenerProductoPorIdAsync(idProducto);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Categoria(int idCategoria)
        {
            var productos = await _productoRepo.ObtenerProductosAsync();

            var productosFiltrados = productos.Where(p => p.IdCategoria == idCategoria).ToList();

            string nombreCategoria = "Productos";
            switch (idCategoria)
            {
                case 1: nombreCategoria = "Juegos PlayStation 5"; break;
                case 2: nombreCategoria = "Juegos Nintendo Switch"; break;
                case 3: nombreCategoria = "Consolas"; break;
                case 4: nombreCategoria = "Accesorios"; break;
                default: nombreCategoria = "Catálogo"; break;
            }

            ViewBag.NombreCategoria = nombreCategoria;

            return View(productosFiltrados);
        }
    }
}