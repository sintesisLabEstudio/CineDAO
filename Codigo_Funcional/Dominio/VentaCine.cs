using System;
namespace CineStarBD.Dominio;
// ENTIDAD DE DOMINIO: representa UNA venta de boleteria.
// No conoce WinForms ni SQL: solo guarda datos y sabe calcular sus montos.
public class VentaCine
{
    // Propiedades de la entidad: corresponden a las columnas de la tabla dbo.Ventas.
    public int Id { get; set; }                       // PK identity de dbo.Ventas.
    public string Cliente { get; set; } = "";         // Nombre del cliente.
    public string Rut { get; set; } = "";             // RUT del cliente.
    public bool EsSocio { get; set; }                 // Socio CineClub: 15% de descuento.
    public string PeliculaTexto { get; set; } = "";   // Pelicula elegida en cartelera.
    public int PrecioPelicula { get; set; }           // Precio unitario de la entrada.
    public int CantidadTickets { get; set; }          // Cantidad de entradas.
    public string ComboNombre { get; set; } = "Sin combo"; // Combo de confiteria opcional.
    public int PrecioCombo { get; set; }              // Precio del combo.
    public int SubtotalEntradas { get; set; }         // PrecioPelicula * CantidadTickets.
    public int SubtotalSnacks { get; set; }           // Precio del combo.
    public int MontoDescuento { get; set; }           // 15% del subtotal si es socio.
    public int NetoAfecto { get; set; }               // Subtotal - descuento.
    public int MontoIva { get; set; }                 // 19% del neto.
    public int TotalPagar { get; set; }               // Neto + IVA.
    // CONSTRUCTOR 1: por defecto.
    // Lo usa VentaDAO.LeerVenta con un "object initializer": crea el objeto y
    public VentaCine()
    {
        if (Program.UltimoArchivo != "VentaCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + "FUNCION: VentaCine()");
    }
    // CONSTRUCTOR 2: sobrecargado, con los datos crudos de la boleteria.
    // Lo llama CatalogoCine.Calcular(datos). Fijate en la ultima linea:
    public VentaCine(string cliente, string rut, bool esSocio, string pelicula,
                     int precioPelicula, int cantidadTickets, string comboNombre, int precioCombo)
    {
        if (Program.UltimoArchivo != "VentaCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: VentaCine(string cliente:{cliente}, string rut:{rut}, bool esSocio:{esSocio}, string pelicula:{pelicula}, int precioPelicula:{precioPelicula}, int cantidadTickets:{cantidadTickets}, string comboNombre:{comboNombre}, int precioCombo:{precioCombo})");
        Cliente = cliente;
        Rut = rut;
        EsSocio = esSocio;
        PeliculaTexto = pelicula;
        PrecioPelicula = precioPelicula;
        CantidadTickets = cantidadTickets;
        ComboNombre = comboNombre;
        PrecioCombo = precioCombo;
        CalcularTotales(); // <-- Aqui empieza la "receta" de calculo.
    }
    // METODOS PUROS: solo leen propiedades y devuelven un valor.
    // No cambian nada ni acceden a la BD. Son la receta del negocio.
    public int CalcularSubtotalEntradas()
    {
        if (Program.UltimoArchivo != "VentaCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: CalcularSubtotalEntradas() = {PrecioPelicula} x {CantidadTickets}");
        return PrecioPelicula * CantidadTickets;
    }
    // CORRECCION 24/11/2026: se restaura este metodo, que habia quedado eliminado.
    // Su ausencia producia CS0103 en CalcularMontoDescuento, CalcularNetoAfecto y
    public int CalcularSubtotalSnacks()
    {
        if (Program.UltimoArchivo != "VentaCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: CalcularSubtotalSnacks() = {PrecioCombo}");
        return PrecioCombo;
    }
    // 15% SOLO si es socio, y se aplica a entradas + combo.
    // Detalle: el cast a (int) TRUNCA (no redondea): 2745.5 queda en 2745.
    public int CalcularMontoDescuento()
    {
        if (Program.UltimoArchivo != "VentaCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaCine.cs");
        int resultado = EsSocio ? (int)((CalcularSubtotalEntradas() + CalcularSubtotalSnacks()) * 0.15) : 0;
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: CalcularMontoDescuento(bool esSocio:{EsSocio}) = {resultado}");
        return resultado;
    }
    // Vuelve a llamar a los metodos anteriores en vez de leer las propiedades ya
    // calculadas: asi la receta siempre es consistente, sin importar el orden.
    public int CalcularNetoAfecto()
    {
        if (Program.UltimoArchivo != "VentaCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaCine.cs");
        int resultado = CalcularSubtotalEntradas() + CalcularSubtotalSnacks() - CalcularMontoDescuento();
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: CalcularNetoAfecto() = {resultado}");
        return resultado;
    }
    // IVA chileno del 19% sobre el NETO (no sobre el subtotal bruto).
    public int CalcularMontoIVA()
    {
        if (Program.UltimoArchivo != "VentaCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaCine.cs");
        int resultado = (int)(CalcularNetoAfecto() * 0.19);
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: CalcularMontoIVA() = {resultado}");
        return resultado;
    }
    public int CalcularTotal()
    {
        if (Program.UltimoArchivo != "VentaCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaCine.cs");
        int resultado = CalcularNetoAfecto() + CalcularMontoIVA();
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: CalcularTotal() = {resultado}");
        return resultado;
    }
    // Materializa la receta en las propiedades de la entidad.
    // Es lo que luego se muestra en pantalla y se graba en dbo.Ventas.
    public void CalcularTotales()
    {
        if (Program.UltimoArchivo != "VentaCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "VentaCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: VentaCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: CalcularTotales(precioPelicula:{PrecioPelicula} | entradas:{CantidadTickets} | precioCombo:{PrecioCombo} | socio:{EsSocio})");
        Console.WriteLine($"{++Program.Secuencia,4}. " + "Dominio: se ejecuta la receta de calculo en 6 pasos" + " | " + "Cliente" + " = " + Cliente);;
        SubtotalEntradas = CalcularSubtotalEntradas();
        Console.WriteLine($"{++Program.Secuencia,4}. " + "1) precio de la pelicula por cantidad de entradas" + " | " + "Subtotal entradas" + " = " + SubtotalEntradas);;
        SubtotalSnacks = CalcularSubtotalSnacks();
        Console.WriteLine($"{++Program.Secuencia,4}. " + "2) recargo del combo elegido" + " | " + "Subtotal snacks" + " = " + SubtotalSnacks);;
        MontoDescuento = CalcularMontoDescuento();
        Console.WriteLine($"{++Program.Secuencia,4}. " + "3) descuento del 15% solo si es socio" + " | " + "Descuento" + " = " + $"socio={EsSocio} -> {MontoDescuento}");;
        NetoAfecto = CalcularNetoAfecto();
        Console.WriteLine($"{++Program.Secuencia,4}. " + "4) subtotal menos descuento" + " | " + "Neto afecto" + " = " + NetoAfecto);;
        MontoIva = CalcularMontoIVA();
        Console.WriteLine($"{++Program.Secuencia,4}. " + "5) IVA del 19% calculado sobre el neto" + " | " + "Monto IVA" + " = " + MontoIva);;
        TotalPagar = CalcularTotal();
        Console.WriteLine($"{++Program.Secuencia,4}. " + "6) neto mas IVA: este valor se muestra y se graba" + " | " + "TOTAL A PAGAR" + " = " + TotalPagar);;
    }
    //plantea un caso de uso comentado de esta clase VentaCine
    // Caso de uso: un cliente compra 3 entradas para la pelicula "Ciencia Ficcion 3D" con un combo "Combo Grande" y es socio del cine.
}
