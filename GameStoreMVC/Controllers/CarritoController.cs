using GameStoreMVC.Data;
using GameStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Image;
using iText.Layout.Borders;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace GameStoreMVC.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ProductoRepository _productoRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly PedidoRepository _pedidoRepo;

        public CarritoController(ProductoRepository productoRepo,
                                 IWebHostEnvironment webHostEnvironment,
                                 PedidoRepository pedidoRepo)
        {
            _productoRepo = productoRepo;
            _webHostEnvironment = webHostEnvironment;
            _pedidoRepo = pedidoRepo;
        }

        public async Task<IActionResult> Agregar(int idProducto)
        {
            var producto = await _productoRepo.ObtenerProductoPorIdAsync(idProducto);
            if (producto == null) return NotFound();

            var carrito = ObtenerCarrito();
            var itemExistente = carrito.FirstOrDefault(c => c.IdProducto == idProducto);

            if (itemExistente != null) itemExistente.Cantidad++;
            else
            {
                carrito.Add(new CarritoItem
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    ImagenUrl = producto.ImagenUrl,
                    Cantidad = 1
                });
            }
            GuardarCarrito(carrito);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Checkout()
        {
            var carrito = ObtenerCarrito();
            if (carrito.Count == 0) return RedirectToAction("Index", "Home");

            ViewBag.Total = carrito.Sum(x => x.Total);

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Correo = User.Identity.Name;
                ViewBag.Nombre = "Juan Antonio";
                ViewBag.Apellido = "Espinoza";
                ViewBag.Telefono = "999999999";
                ViewBag.IsLoggedIn = true;
            }
            else ViewBag.IsLoggedIn = false;

            return View(carrito);
        }

        [HttpPost]
        public IActionResult ProcesarPedido(string correo, string nombre, string tarjetaNumero)
        {
            var carrito = ObtenerCarrito();
            if (carrito.Count == 0) return RedirectToAction("Index", "Home");

            if (string.IsNullOrEmpty(tarjetaNumero) || tarjetaNumero.Length < 16)
            {
                TempData["Error"] = "Tarjeta inválida. Ingrese 16 dígitos.";
                return RedirectToAction("Checkout");
            }

            decimal total = carrito.Sum(x => x.Total);
            bool guardado = _pedidoRepo.RegistrarPedido(nombre, correo, total, carrito);

            if (guardado)
            {
                TempData["NombreCliente"] = nombre;
                TempData["CorreoCliente"] = correo;

                return RedirectToAction("Confirmacion");
            }
            else
            {
                TempData["Error"] = "Error al guardar el pedido. Intente nuevamente.";
                return RedirectToAction("Checkout");
            }
        }

        public IActionResult Confirmacion()
        {
            return View();
        }

        public IActionResult DescargarBoleta()
        {
            var carrito = ObtenerCarrito();
            if (carrito == null || carrito.Count == 0) return RedirectToAction("Index", "Home");

            string nombre = TempData.Peek("NombreCliente") as string ?? "Cliente General";
            string correo = TempData.Peek("CorreoCliente") as string ?? "Sin Correo";
            decimal total = carrito.Sum(x => x.Total);
            string fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                PdfFont fuenteNegrita = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont fuenteRegular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                string rutaImagen = Path.Combine(_webHostEnvironment.WebRootPath, "images", "gamestore.png");
                if (System.IO.File.Exists(rutaImagen))
                {
                    Image img = new Image(ImageDataFactory.Create(rutaImagen));
                    img.ScaleToFit(120, 60);
                    document.Add(img);
                }

                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("BOLETA DE VENTA ELECTRÓNICA")
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(18).SetFont(fuenteNegrita).SetFontColor(ColorConstants.ORANGE));

                document.Add(new Paragraph("N° Pedido: " + new Random().Next(10000, 99999))
                    .SetTextAlignment(TextAlignment.CENTER).SetFontSize(10).SetFont(fuenteRegular));

                document.Add(new Paragraph("\nDATOS DEL CLIENTE").SetFont(fuenteNegrita).SetFontSize(12));
                document.Add(new Paragraph($"Cliente: {nombre}\nCorreo: {correo}\nFecha de Emisión: {fecha}").SetFontSize(10).SetFont(fuenteRegular));
                document.Add(new Paragraph("\n"));

                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 1.5f, 2, 2.5f }));
                table.SetWidth(UnitValue.CreatePercentValue(100));

                table.AddHeaderCell(new Cell().Add(new Paragraph("PRODUCTO").SetFont(fuenteNegrita).SetFontColor(ColorConstants.WHITE)).SetBackgroundColor(ColorConstants.DARK_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("CANT.").SetFont(fuenteNegrita).SetFontColor(ColorConstants.WHITE)).SetBackgroundColor(ColorConstants.DARK_GRAY).SetTextAlignment(TextAlignment.CENTER));
                table.AddHeaderCell(new Cell().Add(new Paragraph("P. UNIT").SetFont(fuenteNegrita).SetFontColor(ColorConstants.WHITE)).SetBackgroundColor(ColorConstants.DARK_GRAY).SetTextAlignment(TextAlignment.RIGHT));
                table.AddHeaderCell(new Cell().Add(new Paragraph("TOTAL").SetFont(fuenteNegrita).SetFontColor(ColorConstants.WHITE)).SetBackgroundColor(ColorConstants.DARK_GRAY).SetTextAlignment(TextAlignment.RIGHT));

                foreach (var item in carrito)
                {
                    table.AddCell(new Cell().Add(new Paragraph(item.Nombre).SetFontSize(10).SetFont(fuenteRegular)));
                    table.AddCell(new Cell().Add(new Paragraph(item.Cantidad.ToString()).SetFontSize(10).SetFont(fuenteRegular)).SetTextAlignment(TextAlignment.CENTER));
                    table.AddCell(new Cell().Add(new Paragraph("S/ " + item.Precio.ToString("0.00")).SetFontSize(10).SetFont(fuenteRegular)).SetTextAlignment(TextAlignment.RIGHT));
                    table.AddCell(new Cell().Add(new Paragraph("S/ " + item.Total.ToString("0.00")).SetFontSize(10).SetFont(fuenteRegular)).SetTextAlignment(TextAlignment.RIGHT));
                }
                document.Add(table);

                document.Add(new Paragraph("\n"));
                Table totalTable = new Table(2).SetHorizontalAlignment(HorizontalAlignment.RIGHT);
                totalTable.AddCell(new Cell().Add(new Paragraph("TOTAL A PAGAR:").SetFont(fuenteNegrita)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
                totalTable.AddCell(new Cell().Add(new Paragraph("S/ " + total.ToString("0.00")).SetFont(fuenteNegrita).SetFontSize(14).SetFontColor(ColorConstants.RED)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
                document.Add(totalTable);

                document.Add(new Paragraph("\n\nGracias por su compra en GameStore.\nVisítenos pronto.").SetTextAlignment(TextAlignment.CENTER).SetFontSize(8).SetFont(fuenteRegular).SetFontColor(ColorConstants.GRAY));
                document.Close();

                return File(stream.ToArray(), "application/pdf", $"Boleta_GameStore_{DateTime.Now.Ticks}.pdf");
            }
        }

        private List<CarritoItem> ObtenerCarrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            if (string.IsNullOrEmpty(carritoJson)) return new List<CarritoItem>();
            return JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);
        }

        private void GuardarCarrito(List<CarritoItem> carrito)
        {
            var carritoJson = JsonSerializer.Serialize(carrito);
            HttpContext.Session.SetString("Carrito", carritoJson);
        }

        public IActionResult Eliminar(int idProducto)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(c => c.IdProducto == idProducto);
            if (item != null)
            {
                carrito.Remove(item);
                GuardarCarrito(carrito);
            }
            return RedirectToAction("Checkout");
        }
    }
}