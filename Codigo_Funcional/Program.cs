using System;
using System.Windows.Forms;
using CineStarBD.Presentacion;
namespace CineStarBD;
// PUNTO DE ENTRADA: el sistema operativo empieza a ejecutar el programa aqui.
// Este archivo no tiene reglas de negocio: solo arranca la interfaz.
static class Program
{
    // Recuerda el ultimo archivo impreso para separar solo al cambiar de archivo.
    internal static string? UltimoArchivo;
    internal static int Secuencia;
    // La app es WinForms (WinExe) y no trae consola. Estas APIs de Windows
    // abren una y la dejan SIEMPRE VISIBLE encima del IDE, en paralelo al form.
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern bool AllocConsole();
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr insertAfter,
                                            int x, int y, int cx, int cy, uint flags);
    // [STAThread] es obligatorio en WinForms: los controles de Windows viven en un
    // unico hilo de apartamento (Single-Threaded Apartment).
    [STAThread]
    static void Main()
    {
        // 1) Configura tema visual, fuentes y escala DPI ANTES de crear la ventana.
        ApplicationConfiguration.Initialize();
        // 2) ABRIR LA CONSOLA antes de escribir cualquier mensaje (si no, se pierde).
        AllocConsole();
        try
        {
            Console.SetWindowSize(115, 35);
            Console.SetBufferSize(115, 500);
            IntPtr hwnd = GetConsoleWindow();
            ShowWindow(hwnd, 9);                       // SW_RESTORE
            // HWND_TOPMOST (-1) + SWP_SHOWWINDOW (0x40): queda encima de Visual Studio.
            SetWindowPos(hwnd, new IntPtr(-1), 820, 60, 540, 720, 0x0040);
        }
        catch { /* si la salida esta redirigida, se ignora */ }
        if (Program.UltimoArchivo != "Program.cs") { Console.WriteLine(); Program.UltimoArchivo = "Program.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: Program.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + "FUNCION: Main()");
        // 3) Crea FormCine. Aqui se ejecuta su constructor y, mas adelante,
        //    el evento Load (combos, grilla y metricas).
        Application.Run(new FormCine());
    }
}
