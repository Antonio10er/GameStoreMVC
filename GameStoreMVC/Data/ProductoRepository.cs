using GameStoreMVC.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GameStoreMVC.Data
{
    public class ProductoRepository
    {
        private readonly string _cadenaSQL;

        public ProductoRepository(IConfiguration configuration)
        {
            _cadenaSQL = configuration.GetConnectionString("CadenaSQL");
        }

        public async Task<List<ProductoModel>> ObtenerProductosAsync()
        {
            List<ProductoModel> lista = new List<ProductoModel>();

            using (SqlConnection conexion = new SqlConnection(_cadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM tb_producto", conexion);
                cmd.CommandType = CommandType.Text;

                await conexion.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        lista.Add(new ProductoModel
                        {
                            IdProducto = Convert.ToInt32(dr["id_producto"]),
                            Nombre = dr["nombre"].ToString(),
                            Descripcion = dr["descripcion"].ToString(),
                            Precio = Convert.ToDecimal(dr["precio"]),
                            Stock = Convert.ToInt32(dr["stock"]),
                            ImagenUrl = dr["imagen_url"].ToString(),
                            IdCategoria = Convert.ToInt32(dr["id_categoria"])
                        });
                    }
                }
            }
            return lista;
        }

        public async Task AgregarProductoAsync(ProductoModel p)
        {
            string query = "INSERT INTO tb_producto (nombre, descripcion, precio, stock, imagen_url, id_categoria) VALUES (@nom, @des, @pre, @sto, @img, @cat)";

            using (SqlConnection conexion = new SqlConnection(_cadenaSQL))
            {
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nom", p.Nombre);
                cmd.Parameters.AddWithValue("@des", p.Descripcion);
                cmd.Parameters.AddWithValue("@pre", p.Precio);
                cmd.Parameters.AddWithValue("@sto", p.Stock);
                cmd.Parameters.AddWithValue("@img", p.ImagenUrl ?? "");
                cmd.Parameters.AddWithValue("@cat", p.IdCategoria);

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<ProductoModel> ObtenerProductoPorIdAsync(int id)
        {
            ProductoModel producto = null;

            using (SqlConnection conexion = new SqlConnection(_cadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM tb_producto WHERE id_producto = @id", conexion);
                cmd.Parameters.AddWithValue("@id", id);
                await conexion.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        producto = new ProductoModel
                        {
                            IdProducto = Convert.ToInt32(dr["id_producto"]),
                            Nombre = dr["nombre"].ToString(),
                            Descripcion = dr["descripcion"].ToString(),
                            Precio = Convert.ToDecimal(dr["precio"]),
                            Stock = Convert.ToInt32(dr["stock"]),
                            ImagenUrl = dr["imagen_url"].ToString(),
                            IdCategoria = Convert.ToInt32(dr["id_categoria"])
                        };
                    }
                }
            }
            return producto;
        }

        public async Task ActualizarProductoAsync(ProductoModel p)
        {
            string query = "UPDATE tb_producto SET nombre=@nom, descripcion=@des, precio=@pre, stock=@sto, imagen_url=@img, id_categoria=@cat WHERE id_producto=@id";

            using (SqlConnection conexion = new SqlConnection(_cadenaSQL))
            {
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@nom", p.Nombre);
                cmd.Parameters.AddWithValue("@des", p.Descripcion);
                cmd.Parameters.AddWithValue("@pre", p.Precio);
                cmd.Parameters.AddWithValue("@sto", p.Stock);
                cmd.Parameters.AddWithValue("@img", p.ImagenUrl ?? "");
                cmd.Parameters.AddWithValue("@cat", p.IdCategoria);
                cmd.Parameters.AddWithValue("@id", p.IdProducto);

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task EliminarProductoAsync(int id)
        {
            using (SqlConnection conexion = new SqlConnection(_cadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM tb_producto WHERE id_producto = @id", conexion);
                cmd.Parameters.AddWithValue("@id", id);

                await conexion.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}