using GameStoreMVC.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GameStoreMVC.Data
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaSQL");
        }

        public UsuarioModel ValidarUsuario(string correo, string clave)
        {
            UsuarioModel usuario = null;

            using (SqlConnection _conexion = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM tb_usuario WHERE correo = @correo AND clave = @clave";
                SqlCommand cmd = new SqlCommand(query, _conexion);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@clave", clave);

                _conexion.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        usuario = new UsuarioModel
                        {
                            IdUsuario = Convert.ToInt32(dr["id_usuario"]),
                            NombreCompleto = dr["nombre_completo"].ToString(),
                            Correo = dr["correo"].ToString(),
                            Clave = dr["clave"].ToString(),
                            EsAdmin = Convert.ToBoolean(dr["es_admin"]),
                            Telefono = dr["telefono"] != DBNull.Value ? dr["telefono"].ToString() : ""
                        };
                    }
                }
            }
            return usuario;
        }

        public bool RegistrarUsuario(UsuarioModel oUsuario)
        {
            bool respuesta = false;

            using (SqlConnection _conexion = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO tb_usuario (nombre_completo, correo, clave, telefono, es_admin) VALUES (@nombre, @correo, @clave, @telefono, 0)";
                SqlCommand cmd = new SqlCommand(query, _conexion);
                cmd.Parameters.AddWithValue("@nombre", oUsuario.NombreCompleto);
                cmd.Parameters.AddWithValue("@correo", oUsuario.Correo);
                cmd.Parameters.AddWithValue("@clave", oUsuario.Clave);
                cmd.Parameters.AddWithValue("@telefono", oUsuario.Telefono ?? "");

                try
                {
                    _conexion.Open();
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas > 0) respuesta = true;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}