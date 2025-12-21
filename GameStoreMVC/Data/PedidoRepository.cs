using GameStoreMVC.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GameStoreMVC.Data
{
    public class PedidoRepository
    {
        private readonly string _cadenaConexion;

        public PedidoRepository(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("CadenaSQL");
        }

        public bool RegistrarPedido(string nombreCliente, string correo, decimal total, List<CarritoItem> detalle)
        {
            bool exito = true;

            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                conexion.Open();
                SqlTransaction transaccion = conexion.BeginTransaction();

                try
                {
                    string queryCabecera = "INSERT INTO tb_pedido (id_usuario, fecha_pedido, monto_total, estado) VALUES (1, GETDATE(), @total, 'Pendiente'); SELECT SCOPE_IDENTITY();";

                    int idPedido = 0;
                    using (SqlCommand cmd = new SqlCommand(queryCabecera, conexion, transaccion))
                    {
                        cmd.Parameters.AddWithValue("@total", total);
                        idPedido = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    foreach (var item in detalle)
                    {
                        string queryDetalle = "INSERT INTO tb_detalle_pedido (id_pedido, id_producto, cantidad, precio_unitario) VALUES (@idPedido, @idProducto, @cantidad, @precio)";
                        using (SqlCommand cmd = new SqlCommand(queryDetalle, conexion, transaccion))
                        {
                            cmd.Parameters.AddWithValue("@idPedido", idPedido);
                            cmd.Parameters.AddWithValue("@idProducto", item.IdProducto);
                            cmd.Parameters.AddWithValue("@cantidad", item.Cantidad);
                            cmd.Parameters.AddWithValue("@precio", item.Precio);
                            cmd.ExecuteNonQuery();
                        }

                        string queryStock = "UPDATE tb_producto SET stock = stock - @cantidad WHERE id_producto = @idProducto";
                        using (SqlCommand cmdStock = new SqlCommand(queryStock, conexion, transaccion))
                        {
                            cmdStock.Parameters.AddWithValue("@cantidad", item.Cantidad);
                            cmdStock.Parameters.AddWithValue("@idProducto", item.IdProducto);
                            cmdStock.ExecuteNonQuery();
                        }
                    }

                    transaccion.Commit();
                }
                catch
                {
                    transaccion.Rollback();
                    exito = false;
                }
            }
            return exito;
        }
    }
}