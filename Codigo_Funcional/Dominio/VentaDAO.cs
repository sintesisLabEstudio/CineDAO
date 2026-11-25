using System;
using System.Data;
using Microsoft.Data.SqlClient;
namespace CineStarBD.Dominio;
// DAO (Data Access Object) de dbo.Ventas.
// Su trabajo es traducir operaciones de C# a SQL. La pantalla nunca ve una
public class VentaDAO
{
    // INSERTAR: guarda la venta y su desglose en UNA SOLA transaccion.
    // Secuencia para seguir con F10/F11:
    public bool InsertarVenta(VentaCine v)
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: InsertarVenta(cliente:{v.Cliente} | rut:{v.Rut} | pelicula:{v.PeliculaTexto} | entradas:{v.CantidadTickets} | combo:{v.ComboNombre} | total:{v.TotalPagar})");
        //1/9 QUERY
        const string query = @"
            INSERT INTO dbo.Ventas
                (Cliente, Rut, EsSocio, PeliculaTexto, PrecioPelicula, CantidadTickets,
                 ComboNombre, PrecioCombo, SubtotalEntradas, SubtotalSnacks,
                 MontoDescuento, NetoAfecto, MontoIva, TotalPagar)
            VALUES
                (@Cliente, @Rut, @EsSocio, @PeliculaTexto, @PrecioPelicula, @CantidadTickets,
                 @ComboNombre, @PrecioCombo, @SubtotalEntradas, @SubtotalSnacks,
                 @MontoDescuento, @NetoAfecto, @MontoIva, @TotalPagar);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";
        //2/9 CONX
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        //3/9 TRANS
        using (SqlTransaction tran = conexion.BeginTransaction())
        {
            try
            {
                //4/9 CMD
                using (SqlCommand cmd = new SqlCommand(query, conexion, tran))
                {
                    //5/9 PARAM
                    Console.WriteLine($"{++Program.Secuencia,4}. " + $"INSERT preparado -> Cliente:{v.Cliente} | RUT:{v.Rut} | Pelicula:{v.PeliculaTexto} | Entradas:{v.CantidadTickets} | Combo:{v.ComboNombre} | Subtotal:{v.SubtotalEntradas} | Descuento:{v.MontoDescuento} | Neto:{v.NetoAfecto} | IVA:{v.MontoIva} | Total:{v.TotalPagar}");
                    CargarParametros(cmd, v);
                    Console.WriteLine($"{++Program.Secuencia,4}. " + "Cada columna recibe su parametro" + " | " + "Parametros" + " = " + cmd.Parameters.Count);;
                    //6/9 EXEC
                    // ExecuteScalar toma la PRIMERA columna de la PRIMERA fila.
                    object? resultado = cmd.ExecuteScalar();
                    Console.WriteLine($"{++Program.Secuencia,4}. " + $"INSERT ejecutado en dbo.Ventas -> Id generado:{(resultado ?? "(null)")} | Cliente:{v.Cliente} | Total:{v.TotalPagar}");
                    //7/9 RESP
                    bool exito = resultado != null && resultado != DBNull.Value;
                    if (exito)
                    {
                        //8/9 ID
                        // Asigna el Id generado por la base de datos a la entidad VentaCine.
                        v.Id = Convert.ToInt32(resultado);
                        // Confirma la transaccion si la insercion fue exitosa.
                        //9/9 COMMIT
                        tran.Commit();
                    }
                    else
                    {
                        // Revertir la transaccion si la insercion fallo.
                        Revertir(tran);
                    }
                    return exito;
                }
            }
            catch
            {
                Revertir(tran);
                throw;
            }
        }
    }
    // LISTADO: llena un DataTable y CIERRA la conexion (gracias al using).
    // Al DataGridView le basta el DataTable: no necesita estar conectado.
    public DataTable ObtenerVentasDesconectado()
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + "FUNCION: ObtenerVentasDesconectado(tabla:dbo.Ventas | orden:Id DESC)");
        //1/6 DT
        DataTable dt = new DataTable();
        //2/6 QUERY
        const string query = @"SELECT Id, Cliente, Rut, EsSocio, PeliculaTexto,
                                      CantidadTickets, ComboNombre, TotalPagar
                               FROM dbo.Ventas ORDER BY Id DESC;";
        //3/6    CONX
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        //4/6 ADAPT ya que es lectura
        using (SqlDataAdapter adapter = new SqlDataAdapter(query, conexion))
        {
            //5/6 LLENA ADAP
            adapter.Fill(dt);
            Console.WriteLine($"{++Program.Secuencia,4}. " + "SQL devuelve el historial y se llena el DataTable" + " | " + "Filas leidas" + " = " + dt.Rows.Count);;
            if (dt.Rows.Count > 0)
                Console.WriteLine($"{++Program.Secuencia,4}. " + "Primera fila del historial" + " | " + "Fila 1" + " = " + $"Id={dt.Rows[0]["Id"]}, {dt.Rows[0]["Cliente"]}, Total={dt.Rows[0]["TotalPagar"]}");;
        }
        //6/6 RETORNA DT
        return dt;
    }
    // BUSQUEDA: se dispara en CADA tecla de txtBuscar (TextChanged).
    // El comodin '%' se agrega al VALOR del parametro, no al SQL:
    public DataTable BuscarVentasPorCliente(string filtroCliente)
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: BuscarVentasPorCliente(string filtroCliente:{filtroCliente})");
        //1 DT
        DataTable dt = new DataTable();
        //2 Q
        const string query = @"SELECT Id, Cliente, Rut, EsSocio, PeliculaTexto,
                                      CantidadTickets, ComboNombre, TotalPagar
                               FROM dbo.Ventas WHERE Cliente LIKE @Filtro ORDER BY Id DESC;";
        //3 CONX
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        //4 CMD
        using (SqlCommand cmd = new SqlCommand(query, conexion))
        {
            //5 PARAM FILTRO
            Console.WriteLine($"{++Program.Secuencia,4}. " + "El texto tecleado se convierte en patron LIKE" + " | " + "Filtro" + " = " + filtroCliente);;
            cmd.Parameters.AddWithValue("@Filtro", "%" + filtroCliente + "%");
            Console.WriteLine($"{++Program.Secuencia,4}. " + "El valor parametrizado que llega a SQL" + " | " + "@Filtro" + " = " + cmd.Parameters["@Filtro"].Value);;
            //6 ADAP LLENO
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(dt);
            }
            Console.WriteLine($"{++Program.Secuencia,4}. " + "La BD devuelve solo los clientes que coinciden" + " | " + "Coincidencias" + " = " + dt.Rows.Count);;
        }
        //7 RETORNA DT
        return dt;
    }
    // DETALLE POR ID: lo llama dgvVentas_CellClick al hacer clic en una fila.
    // Devuelve null si no hay fila con ese Id (el formulario lo comprueba).
    public VentaCine? ObtenerVentaPorId(int id)
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ObtenerVentaPorId(int id:{id})");
        //1 Q
        const string query = @"SELECT Id, Cliente, Rut, EsSocio, PeliculaTexto, PrecioPelicula,
                                      CantidadTickets, ComboNombre, PrecioCombo, SubtotalEntradas,
                                      SubtotalSnacks, MontoDescuento, NetoAfecto, MontoIva, TotalPagar
                               FROM dbo.Ventas WHERE Id = @Id;";
        //2 CNX
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        //3 CMD
        using (SqlCommand cmd = new SqlCommand(query, conexion))
        {
            //4 PARAM ID
            Console.WriteLine($"{++Program.Secuencia,4}. " + "Se pide a la BD el detalle de la fila clickeada" + " | " + "Id solicitado" + " = " + id);;
            cmd.Parameters.AddWithValue("@Id", id);
            //5 READER EXEC
            using (SqlDataReader r = cmd.ExecuteReader())
            {
                //6 RETORNA 
                bool encontrada = r.Read();
                return encontrada ? LeerVenta(r) : null;
            }
        }
    }
    // ACTUALIZAR: misma idea que Insertar, pero con WHERE Id=@Id.
    // ExecuteNonQuery devuelve cuantas filas se modificaron:
    public bool ActualizarVenta(VentaCine v)
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ActualizarVenta(VentaCine v:Id={v.Id} / {v.Cliente} / Total={v.TotalPagar})");
        const string query = @"UPDATE dbo.Ventas SET
                                 Cliente=@Cliente, Rut=@Rut, EsSocio=@EsSocio,
                                 PeliculaTexto=@PeliculaTexto, PrecioPelicula=@PrecioPelicula,
                                 CantidadTickets=@CantidadTickets, ComboNombre=@ComboNombre,
                                 PrecioCombo=@PrecioCombo, SubtotalEntradas=@SubtotalEntradas,
                                 SubtotalSnacks=@SubtotalSnacks, MontoDescuento=@MontoDescuento,
                                 NetoAfecto=@NetoAfecto, MontoIva=@MontoIva, TotalPagar=@TotalPagar
                               WHERE Id=@Id;";
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlTransaction tran = conexion.BeginTransaction())
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conexion, tran))
                {
                    cmd.Parameters.AddWithValue("@Id", v.Id);
                    CargarParametros(cmd, v);
                    bool exito = cmd.ExecuteNonQuery() > 0;
                    Console.WriteLine($"{++Program.Secuencia,4}. " + $"UPDATE ejecutado en dbo.Ventas -> Id:{v.Id} | Cliente:{v.Cliente} | RUT:{v.Rut} | Total grabado:{v.TotalPagar} | Filas afectadas:{(exito ? 1 : 0)}");
                    if (exito) tran.Commit();
                    else Revertir(tran);
                    return exito;
                }
            }
            catch
            {
                Revertir(tran);
                throw;
            }
        }
    }
    // ELIMINAR: borrado FISICO (no hay columna "anulada").
    // La confirmacion la hace el formulario antes de llegar aqui.
    public bool EliminarVenta(int id)
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: EliminarVenta(int id:{id})");
        const string query = "DELETE FROM dbo.Ventas WHERE Id=@Id;";
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlCommand cmd = new SqlCommand(query, conexion))
        {
            cmd.Parameters.AddWithValue("@Id", id);
            bool borrado = cmd.ExecuteNonQuery() > 0;
            Console.WriteLine($"{++Program.Secuencia,4}. " + $"DELETE ejecutado en dbo.Ventas -> Id:{id} | Registro eliminado:{(borrado ? "SI" : "NO")}");
            return borrado;
        }
    }
    // METRICAS: agrega COUNT, SUM y AVG en UNA sola consulta.
    // ISNULL evita que SUM/AVG devuelvan NULL cuando la tabla esta vacia.
    public DataTable ObtenerMetricas()
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + "FUNCION: ObtenerMetricas(tabla:dbo.Ventas | campos:COUNT/SUM/AVG(TotalPagar))");
        DataTable dt = new DataTable();
        const string query = @"SELECT COUNT(*) AS TotalVentas,
                                      ISNULL(SUM(TotalPagar), 0) AS TotalRecaudado,
                                      ISNULL(AVG(TotalPagar), 0) AS PromedioVenta
                               FROM dbo.Ventas;";
        using (SqlConnection conexion = ConexionBD.ObtenerConexion())
        using (SqlDataAdapter adapter = new SqlDataAdapter(query, conexion))
        {
            adapter.Fill(dt);
            Console.WriteLine($"{++Program.Secuencia,4}. " + "La consulta agregada ObtenerMetricas() ya tiene resultado | Metricas = " +
                (dt.Rows.Count > 0 ? $"Ventas={dt.Rows[0]["TotalVentas"]}, Recaudado={dt.Rows[0]["TotalRecaudado"]}" : "(sin filas)"));
        }
        return dt;
    }
    // LEE UNA FILA del SqlDataReader y la convierte en VentaCine.
    // Punto clave para entender el flujo: usa el CONSTRUCTOR POR DEFECTO con
    private static VentaCine LeerVenta(SqlDataReader r)
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: LeerVenta(SqlDataReader r:Id={r["Id"]})");
        return new VentaCine
        {
            Id = Convert.ToInt32(r["Id"]),
            Cliente = r["Cliente"]?.ToString() ?? "",
            Rut = r["Rut"]?.ToString() ?? "",
            EsSocio = Convert.ToBoolean(r["EsSocio"]),
            PeliculaTexto = r["PeliculaTexto"]?.ToString() ?? "",
            PrecioPelicula = Convert.ToInt32(r["PrecioPelicula"]),
            CantidadTickets = Convert.ToInt32(r["CantidadTickets"]),
            ComboNombre = r["ComboNombre"]?.ToString() ?? "Sin combo",
            PrecioCombo = Convert.ToInt32(r["PrecioCombo"]),
            SubtotalEntradas = Convert.ToInt32(r["SubtotalEntradas"]),
            SubtotalSnacks = Convert.ToInt32(r["SubtotalSnacks"]),
            MontoDescuento = Convert.ToInt32(r["MontoDescuento"]),
            NetoAfecto = Convert.ToInt32(r["NetoAfecto"]),
            MontoIva = Convert.ToInt32(r["MontoIva"]),
            TotalPagar = Convert.ToInt32(r["TotalPagar"])
        };
    }
    // CARGA LOS 14 PARAMETROS del INSERT / UPDATE, uno por columna que se escribe.
    // AddWithValue infiere el tipo desde el valor de C#: string->NVARCHAR,
    private static void CargarParametros(SqlCommand cmd, VentaCine v)
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: CargarParametros(cliente:{v.Cliente} | rut:{v.Rut} | id:{v.Id} | total:{v.TotalPagar} | parametros:14)");
        cmd.Parameters.AddWithValue("@Cliente", v.Cliente);
        cmd.Parameters.AddWithValue("@Rut", v.Rut);
        cmd.Parameters.AddWithValue("@EsSocio", v.EsSocio);
        cmd.Parameters.AddWithValue("@PeliculaTexto", v.PeliculaTexto);
        cmd.Parameters.AddWithValue("@PrecioPelicula", v.PrecioPelicula);
        cmd.Parameters.AddWithValue("@CantidadTickets", v.CantidadTickets);
        cmd.Parameters.AddWithValue("@ComboNombre", v.ComboNombre);
        cmd.Parameters.AddWithValue("@PrecioCombo", v.PrecioCombo);
        cmd.Parameters.AddWithValue("@SubtotalEntradas", v.SubtotalEntradas);
        cmd.Parameters.AddWithValue("@SubtotalSnacks", v.SubtotalSnacks);
        cmd.Parameters.AddWithValue("@MontoDescuento", v.MontoDescuento);
        cmd.Parameters.AddWithValue("@NetoAfecto", v.NetoAfecto);
        cmd.Parameters.AddWithValue("@MontoIva", v.MontoIva);
        cmd.Parameters.AddWithValue("@TotalPagar", v.TotalPagar);
    }
    // ROLLBACK TOLERANTE: si la transaccion ya se cerro (por ejemplo porque el
    // Commit fallo), Rollback lanzaria otra excepcion y taparia el error real.
    private static void Revertir(SqlTransaction tran)
    {
        if (Program.UltimoArchivo != "VentaDAO.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaDAO.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaDAO.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: Revertir(SqlTransaction tran:{tran.Connection?.Database})");
        try { tran.Rollback(); }
        catch (Exception) { /* la transaccion ya no esta activa */ }
    }
}
