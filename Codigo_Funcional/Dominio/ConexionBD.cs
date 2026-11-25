using System;
using System.Data;
using Microsoft.Data.SqlClient;
namespace CineStarBD.Dominio;
// HELPER DE CONEXION: la UNICA clase que conoce la cadena de acceso a la BD.
// La pantalla (FormCine) nunca ve un SqlConnection: todo pasa por VentaDAO.
public static class ConexionBD
{
    //CADENA
    // SQL Server Express segun la documentacion tecnica del caso CineStar.
    private static readonly string cadenaConexion =
        @"Server=.\SQLEXPRESS;Database=CineStarDB;Integrated Security=True;TrustServerCertificate=True;";
    // Propiedad de solo lectura: la usa el boton "Probar Conexion" para mostrarla.
    public static string CadenaConexion
    {
        get
        {
            if (Program.UltimoArchivo != "ConexionBD.cs") { Console.WriteLine(); Program.UltimoArchivo = "ConexionBD.cs"; }
            Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: ConexionBD.cs");
            Console.WriteLine($"{++Program.Secuencia,4}. " + "FUNCION: get_CadenaConexion()");
            return cadenaConexion;
        }
    }
    //ABRE
    // FABRICA DE CONEXIONES: crea una SqlConnection y YA LA ABRE.
    public static SqlConnection ObtenerConexion()
    {
        if (Program.UltimoArchivo != "ConexionBD.cs") { Console.WriteLine(); Program.UltimoArchivo = "ConexionBD.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: ConexionBD.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ObtenerConexion(cadena:{cadenaConexion})");
        SqlConnection conexion = new SqlConnection(cadenaConexion);
        try
        {
            //RETORNA
            conexion.Open();
            Console.WriteLine($"{++Program.Secuencia,4}. " + "Se abre la conexion contra SQL Server Express" + " | " + "Servidor / BD" + " = " + $"{conexion.DataSource} / {conexion.Database}");;
            Console.WriteLine($"{++Program.Secuencia,4}. " + "Estado de la conexion recien abierta" + " | " + "State" + " = " + conexion.State);;
            return conexion;
        }
        catch
        {
            conexion.Dispose();
            throw;
        }
    }
    // PRUEBA RAPIDA: envuelve ObtenerConexion en try/catch.
    // Solo captura SqlException; cualquier otro error se propaga.
    public static bool ProbarConexion()
    {
        if (Program.UltimoArchivo != "ConexionBD.cs") { Console.WriteLine(); Program.UltimoArchivo = "ConexionBD.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: ConexionBD.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ProbarConexion(cadena:{cadenaConexion})");
        try
        {
            //CNX
            using (SqlConnection conexion = ObtenerConexion())
                return conexion.State == ConnectionState.Open; //RETORNA
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"{++Program.Secuencia,4}. " + $"[Error de Conexion SQL]: {ex.Message}");
            return false;
        }
        //caso de uso para esta clase 
        // Caso de uso: Probar la conexión a la base de datos CineStarDB.
    }
}
