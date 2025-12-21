namespace GameStoreMVC.Models
{
    public class UsuarioModel
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public string Telefono { get; set; }
        public bool EsAdmin { get; set; }
    }
}
