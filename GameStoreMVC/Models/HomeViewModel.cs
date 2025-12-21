using System.Collections.Generic;

namespace GameStoreMVC.Models
{
    public class HomeViewModel
    {
        public List<ProductoModel> JuegosPS5 { get; set; }
        public List<ProductoModel> JuegosSwitch { get; set; }
        public List<ProductoModel> Consolas { get; set; }
        public List<ProductoModel> Accesorios { get; set; }
    }
}