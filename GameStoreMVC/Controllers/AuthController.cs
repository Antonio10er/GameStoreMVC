using Microsoft.AspNetCore.Mvc;
using GameStoreMVC.Data;
using GameStoreMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace GameStoreMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public AuthController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string password)
        {
            UsuarioModel usuarioEncontrado = _usuarioRepo.ValidarUsuario(correo, password);

            if (usuarioEncontrado != null)
            {
                string rol = usuarioEncontrado.EsAdmin ? "Administrador" : "Cliente";

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarioEncontrado.NombreCompleto),
                    new Claim(ClaimTypes.Email, usuarioEncontrado.Correo),
                    new Claim(ClaimTypes.Role, rol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Correo o contraseña incorrectos.";
                return View();
            }
        }

        [HttpPost]
        public IActionResult Registro(string nombre, string correo, string password, string telefono)
        {
            UsuarioModel nuevoUsuario = new UsuarioModel()
            {
                NombreCompleto = nombre,
                Correo = correo,
                Clave = password,
                Telefono = telefono
            };

            bool creado = _usuarioRepo.RegistrarUsuario(nuevoUsuario);

            if (creado)
            {
                TempData["MensajeExito"] = "¡Tu cuenta ha sido creada exitosamente! Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewData["Mensaje"] = "No se pudo crear el usuario. El correo podría estar en uso.";
                return View("Login");
            }
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}