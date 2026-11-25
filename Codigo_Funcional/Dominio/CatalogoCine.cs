using System.Collections.Generic;
using System.Linq;
namespace CineStarBD.Dominio;
// REGLAS DE NEGOCIO Y CATALOGOS (sin dependencia de WinForms ni de SQL).
// Equivale a CalculadoraPedido en el proyecto Pizzeria.
public static class CatalogoCine
{
    private const string PeliculaPorDefecto = "Aventura 2D";
    private const string ComboPorDefecto = "Sin combo";
    // Cartelera: pelicula -> precio unitario de la entrada (CLP).
    public static readonly IReadOnlyDictionary<string, int> Peliculas = new Dictionary<string, int>
    {
        { "Aventura 2D", 5000 },
        { "Ciencia Ficcion 3D", 7000 },
        { "Drama Premium", 6000 },
        { "Estreno VIP", 9000 }
    };
    // Confiteria: combo -> recargo (CLP).
    public static readonly IReadOnlyDictionary<string, int> Combos = new Dictionary<string, int>
    {
        { "Sin combo", 0 },
        { "Combo Clasico", 3000 },
        { "Combo Grande", 4500 },
        { "Combo Familiar", 6000 }
    };
    // DTO de ENTRADA: es lo que la pantalla le pasa a Calcular().
    // No calcula nada; solo transporta lo que escribio/tildo el usuario.
    public sealed class DatosVenta
    {
        public string Cliente { get; set; } = "";
        public string Rut { get; set; } = "";
        public bool EsSocio { get; set; }
        public string Pelicula { get; set; } = PeliculaPorDefecto;
        public string Combo { get; set; } = ComboPorDefecto;
        public int CantidadTickets { get; set; } = 1;
    }
    // CORAZON DEL CALCULO. Lo llama FormCine.ConstruirVenta().
    // FLUJO:
    public static VentaCine Calcular(DatosVenta d)
    {
        if (Program.UltimoArchivo != "CatalogoCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "CatalogoCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: CatalogoCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: Calcular(DatosVenta d:{d.Cliente} / {d.Pelicula} / cant={d.CantidadTickets})");
        //calcula valor de pelicula si no existe se asigna el valor por defecto
        string pelicula = Peliculas.ContainsKey(d.Pelicula) ? d.Pelicula : PeliculaPorDefecto; //del diccionario Peliculas,
        //calcula valor de combo si no existe se asigna el valor por defecto
        string combo = Combos.ContainsKey(d.Combo) ? d.Combo : ComboPorDefecto; //del diccionario Combos,
        //calcula cantidad de tickets 
        int cantidad = d.CantidadTickets > 0 ? d.CantidadTickets : 1; //si es mayoro igual a 0 se asigna 1
        Console.WriteLine($"{++Program.Secuencia,4}. " + "Dominio: se validan los datos y se buscan los precios" + " | " + "Pelicula" + " = " + $"{pelicula} = ${Peliculas[pelicula]}");;
        Console.WriteLine($"{++Program.Secuencia,4}. " + "El combo tambien se busca en el diccionario" + " | " + "Combo" + " = " + $"{combo} = ${Combos[combo]}");;
        Console.WriteLine($"{++Program.Secuencia,4}. " + "Cantidad corregida si era menor o igual a cero" + " | " + "Entradas" + " = " + cantidad);;
        //retorna una nueva venta de cine con los datos calculados
        return new VentaCine(d.Cliente, d.Rut, d.EsSocio, pelicula,
                             Peliculas[pelicula],  //Peliculas[pelicula] es el precio de la pelicula
                             cantidad,              //cantidad de tickets
                             combo,                 //nombre del combo
                             Combos[combo]);        // Combos[combo] es el precio del combo
    }
    // Utilidades para la grilla: FormCine_Load las usa para llenar los ComboBox.
    // OrderBy(x => x) deja la lista alfabetica.
    public static string[] ObtenerPeliculas()
    {
        if (Program.UltimoArchivo != "CatalogoCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "CatalogoCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: CatalogoCine.cs");
        string[] resultado = Peliculas.Keys.OrderBy(x => x).ToArray();
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ObtenerPeliculas() = {resultado.Length} peliculas");
        return resultado;
    }
    // Utilidad para la grilla: nombres de combos ordenados.
    //es un metodo que devuelve un arreglo de strings con los nombres de los combos ordenados alfabeticamente
    public static string[] ObtenerCombos()
    {
        if (Program.UltimoArchivo != "CatalogoCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "CatalogoCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: CatalogoCine.cs");
        string[] resultado = Combos.Keys.OrderBy(x => x).ToArray();
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ObtenerCombos() = {resultado.Length} combos");
        return resultado;
    }
    //CASO DE USO PARA ESTA CLASE
    // Caso de uso: calcular el total a pagar para un cliente que
}
