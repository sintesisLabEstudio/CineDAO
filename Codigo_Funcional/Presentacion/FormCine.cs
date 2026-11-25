using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CineStarBD.Dominio;
namespace CineStarBD.Presentacion;
// FORMULARIO PRINCIPAL (Capa de Presentacion).
// Es un "controlador de eventos": reacciona a clics y cambios de controles.
public partial class FormCine : Form
{
    // Unico acceso a datos que tiene la pantalla. Se crea una sola vez.
    private readonly VentaDAO ventaDao = new VentaDAO();
    // BANDERA ANTI-RECURSION: al cargar datos en los controles se disparan los
    // eventos SelectedIndexChanged / TextChanged, que recalcularian a mitad de
    private bool cargandoFormulario = false;
    public FormCine()
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + "FUNCION: FormCine()");
        // InitializeComponent vive en el Designer: crea los controles, aplica el
        // tema oscuro y cablea los 13 eventos. F11 aqui salta a ese archivo.
        InitializeComponent();
    }
    // Evento Load: NADIE lo llama a mano; Windows lo dispara automaticamente
    // apenas se muestra el formulario (desde Application.Run en Program.cs).
    private void FormCine_Load(object? sender, EventArgs e)
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: FormCine_Load(object? sender:{sender?.GetType().Name ?? "null"}, EventArgs e:{e})");
        cargandoFormulario = true; //bandera  flag
        // ComboBo peliculas 
        cboPelicula.Items.Clear();
        cboPelicula.Items.AddRange(CatalogoCine.ObtenerPeliculas());
        cboPelicula.SelectedItem = "Aventura 2D";
        //combo items snacks
        cboCombo.Items.Clear();
        cboCombo.Items.AddRange(CatalogoCine.ObtenerCombos());
        cboCombo.SelectedItem = "Sin combo";
        Console.WriteLine($"{++Program.Secuencia,4}. " + "Load llena los combos desde CatalogoCine" + " | " + "Opciones" + " = " + $"{cboPelicula.Items.Count} peliculas / {cboCombo.Items.Count} combos");;
        //cantidad de entradas 
        txtCantidad.Text = "1";
        //limpiar resumen
        ReiniciarResumen();
        //cargar ventas
        cargandoFormulario = false;
        CargarVentas();
        ConfigurarAyuda();
        //cargar metricas
        ActualizarMetricas();
        //estado inicial
        MostrarEstado("Listo. Ingrese los datos de la venta.");
    }
    // PUENTE HACIA DOMINIO: lee los controles -> arma un DatosVenta ->
    // llama a CatalogoCine.Calcular -> devuelve una VentaCine YA calculada.
    private VentaCine ConstruirVenta()
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ConstruirVenta(cliente:{txtCliente.Text} | rut:{txtRut.Text} | pelicula:{cboPelicula.Text} | combo:{cboCombo.Text} | cantidad:{txtCantidad.Text} | socio:{chkSocio.Checked})");
        // Crear un objeto DatosVenta con los datos del formulario
        var datosVenta = new CatalogoCine.DatosVenta //en base a la clase DatosVenta de CatalogoCine
        {
            Cliente = txtCliente.Text.Trim(),
            Rut = txtRut.Text.Trim(),
            EsSocio = chkSocio.Checked,
            Pelicula = cboPelicula.Text,
            Combo = cboCombo.Text,
            CantidadTickets = int.TryParse(txtCantidad.Text.Trim(), out int n) ? n : 1
        };
        Console.WriteLine($"{++Program.Secuencia,4}. " + "La pantalla lee los controles y arma el DTO de entrada" + " | " + "DatosVenta" + " = " + $"{datosVenta.Cliente} | {datosVenta.Pelicula} | {datosVenta.Combo} | cant={datosVenta.CantidadTickets} | socio={datosVenta.EsSocio}");;
        // Llamar a CatalogoCine.Calcular() para obtener un objeto VentaCine con los totales calculados
        return CatalogoCine.Calcular(datosVenta);
    }
    // CALCULO EN VIVO: 4 eventos distintos apuntan AQUI (ver Designer):
    //   cboPelicula.SelectedIndexChanged, cboCombo.SelectedIndexChanged,
    private void Calculo_Changed(object? sender, EventArgs e)
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: Calculo_Changed(object? sender:{sender?.GetType().Name ?? "null"}, EventArgs e:{e})");
        Console.WriteLine($"{++Program.Secuencia,4}. " + "Un control cambio y dispara el recalculo en vivo" + " | " + "Evento" + " = " + sender?.GetType().Name ?? "?");;
        // Evita recalcular mientras se cargan o limpian los controles.
        if (cargandoFormulario) return;
        // Recalcula y muestra el resumen de la venta
        MostrarResumen(ConstruirVenta()); //en base a la construccion de los datos
    }
    // Solo pinta los 6 labels del panel "Detalle de Pago". No graba nada en la BD.
    // :N0 da formato de miles sin decimales (por eso se truncaron los montos).
    private void MostrarResumen(VentaCine v)
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: MostrarResumen(VentaCine v:{v.Cliente} / {v.PeliculaTexto} / Total={v.TotalPagar})");
        lblSnacksValor.Text = $"$ {v.SubtotalSnacks:N0} CLP";
        lblSubtotalValor.Text = $"$ {v.SubtotalEntradas:N0} CLP";
        lblDescuentoValor.Text = $"- $ {v.MontoDescuento:N0} CLP";
        lblNetoValor.Text = $"$ {v.NetoAfecto:N0} CLP";
        lblIvaValor.Text = $"+ $ {v.MontoIva:N0} CLP";
        lblTotalValor.Text = $"$ {v.TotalPagar:N0} CLP";
        Console.WriteLine($"{++Program.Secuencia,4}. " + "Los montos calculados llegan a los labels del formulario" + " | " + "Desglose" + " = " + $"Sub={v.SubtotalEntradas} | Combo={v.SubtotalSnacks} | Dcto={v.MontoDescuento} | Neto={v.NetoAfecto} | IVA={v.MontoIva} | TOTAL={v.TotalPagar}");;
    }
    // Reinicia los labels del resumen a cero.
    private void ReiniciarResumen()
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ReiniciarResumen(totalActual:{lblTotalValor.Text} -> $ 0 CLP)");
        lblSubtotalValor.Text = "$ 0 CLP";
        lblSnacksValor.Text = "$ 0 CLP";
        lblDescuentoValor.Text = "- $ 0 CLP";
        lblNetoValor.Text = "$ 0 CLP";
        lblIvaValor.Text = "+ $ 0 CLP";
        lblTotalValor.Text = "$ 0 CLP";
    }
    // VALIDACION DE UI: se ejecuta ANTES de tocar la BD.
    // Comprueba nombre, RUT y que la cantidad sea entero > 0.
    private bool DatosValidos()
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: DatosValidos(cliente:{txtCliente.Text} | rut:{txtRut.Text} | cantidad:{txtCantidad.Text})");
        // Valida que el nombre del cliente no esté vacío.
        if (string.IsNullOrWhiteSpace(txtCliente.Text))
        {
            MessageBox.Show("Ingrese el nombre del cliente.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCliente.Focus();
            return false;
        }
        // Valida que el RUT del cliente no esté vacío.
        if (string.IsNullOrWhiteSpace(txtRut.Text))
        {
            MessageBox.Show("Ingrese el RUT del cliente.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtRut.Focus();
            return false;
        }
        // Valida que la cantidad de entradas sea un número entero mayor que cero.
        if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad) || cantidad <= 0)
        {
            MessageBox.Show("Ingrese una cantidad de entradas valida (numero entero mayor que cero).",
                            "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtCantidad.Focus();
            return false;
        }
        return true;
    }
    // FLUJO COMPLETO DE REGISTRO (el mas importante para seguir con F11):
    //   1) DatosValidos
    private void BtnRegistrar_Click(object? sender, EventArgs e)
    {
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"EL USUARIO HIZO CLICK EN EL BOTON BTNREGISTRAR | CLIENTE:{txtCliente.Text} | RUT:{txtRut.Text}".ToUpperInvariant());
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: BtnRegistrar_Click(object? sender:{sender?.GetType().Name ?? "null"}, EventArgs e:{e})");
        // Valida los datos del formulario antes de registrar la venta.
        if (!DatosValidos()) return;
        // Construye un objeto VentaCine a partir de los datos del formulario y muestra un resumen de la venta.
        VentaCine venta = ConstruirVenta();
        MostrarResumen(venta);
        var respuesta = MessageBox.Show($"¿Registrar la venta?\n\nTotal: $ {venta.TotalPagar:N0} CLP",
            "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (respuesta != DialogResult.Yes) return;
        try
        {
            if (ventaDao.InsertarVenta(venta))
            {
                Console.WriteLine($"{++Program.Secuencia,4}. " + $"REGISTRO INSERTADO -> Id:{venta.Id} | Cliente:{venta.Cliente} | RUT:{venta.Rut} | Pelicula:{venta.PeliculaTexto} | Entradas:{venta.CantidadTickets} | Combo:{venta.ComboNombre} | Total:{venta.TotalPagar}");
                MostrarEstado("Venta registrada correctamente.", true);
                MessageBox.Show("Venta registrada correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarVentas();
                ActualizarMetricas();
            }
            else
            {
                MostrarEstado("No se pudo registrar la venta.", false);
                MessageBox.Show("No se pudo registrar la venta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MostrarEstado("Error al registrar la venta.", false);
            MessageBox.Show("Error al grabar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    // ACTUALIZAR: el Id NO se escribe a mano, viene de txtId, que se llena
    // automaticamente al hacer clic en una fila de la grilla (CellClick).
    private void btnActualizar_Click(object? sender, EventArgs e)
    {
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"EL USUARIO HIZO CLICK EN EL BOTON BTNACTUALIZAR | ID:{txtId.Text} | CLIENTE:{txtCliente.Text}".ToUpperInvariant());
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: btnActualizar_Click(object? sender:{sender?.GetType().Name ?? "null"}, EventArgs e:{e})");
        if (!int.TryParse(txtId.Text.Trim(), out int id) || id <= 0)
        {
            MessageBox.Show("Seleccione una venta de la tabla para actualizar.", "Validacion",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!DatosValidos()) return;
        try
        {
            VentaCine venta = ConstruirVenta();
            venta.Id = id;
            Console.WriteLine($"{++Program.Secuencia,4}. " + $"REGISTRO A ACTUALIZAR -> Id:{id} | Cliente:{venta.Cliente} | RUT:{venta.Rut} | Total nuevo:{venta.TotalPagar}");
            if (ventaDao.ActualizarVenta(venta))
            {
                Console.WriteLine($"{++Program.Secuencia,4}. " + $"REGISTRO ACTUALIZADO -> Id:{venta.Id} | Cliente:{venta.Cliente} | RUT:{venta.Rut} | Total:{venta.TotalPagar}");
                MostrarEstado("Venta actualizada correctamente.", true);
                MessageBox.Show("Venta actualizada correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarVentas();
                ActualizarMetricas();
            }
            else
            {
                MessageBox.Show("No se pudo actualizar la venta.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    // ELIMINAR: pide confirmacion y llama al DAO con el Id de txtId.
    // Es borrado fisico (DELETE), no una anulacion logica.
    private void btnEliminar_Click(object? sender, EventArgs e)
    {
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"EL USUARIO HIZO CLICK EN EL BOTON BTNELIMINAR | ID:{txtId.Text} | CLIENTE:{txtCliente.Text}".ToUpperInvariant());
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: btnEliminar_Click(object? sender:{sender?.GetType().Name ?? "null"}, EventArgs e:{e})");
        if (!int.TryParse(txtId.Text.Trim(), out int id) || id <= 0)
        {
            MessageBox.Show("Seleccione una venta de la tabla para anular.", "Validacion",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"REGISTRO A ELIMINAR -> Id:{id} | Cliente:{txtCliente.Text} | RUT:{txtRut.Text}");
        var respuesta = MessageBox.Show("¿Está seguro de anular (borrar) este registro?", "Confirmar",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (respuesta != DialogResult.Yes) return;
        try
        {
            if (ventaDao.EliminarVenta(id))
            {
                Console.WriteLine($"{++Program.Secuencia,4}. " + $"REGISTRO ELIMINADO -> Id:{id} | Cliente:{txtCliente.Text} | RUT:{txtRut.Text}");
                MostrarEstado("Registro anulado correctamente.", true);
                MessageBox.Show("Registro anulado.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarVentas();
                ActualizarMetricas();
            }
            else
            {
                MessageBox.Show("No se pudo anular el registro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al anular: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void btnTodos_Click(object? sender, EventArgs e)
    {
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"EL USUARIO HIZO CLICK EN EL BOTON BTNTODOS".ToUpperInvariant());
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: btnTodos_Click(object? sender:{sender?.GetType().Name ?? "null"}, EventArgs e:{e})");
        txtBuscar.Clear();
        CargarVentas();
        MostrarEstado("Historial completo cargado.", true);
    }
    private void btnProbarConexion_Click(object? sender, EventArgs e)
    {
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"EL USUARIO HIZO CLICK EN EL BOTON BTNPROBARCONEXION".ToUpperInvariant());
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: btnProbarConexion_Click(object? sender:{sender?.GetType().Name ?? "null"}, EventArgs e:{e})");
        bool ok = ConexionBD.ProbarConexion();
        MostrarEstado(ok ? "Conexion exitosa." : "No se pudo conectar a la base de datos.", ok);
        string detalle = "Cadena de conexion:\n" + ConexionBD.CadenaConexion +
                         "\n\nEstado: " + (ok ? "Conexion exitosa." : "No se pudo conectar.");
        MessageBox.Show(detalle, "Probar Conexion", MessageBoxButtons.OK,
                        ok ? MessageBoxIcon.Information : MessageBoxIcon.Error);
    }
    private void btnSalir_Click(object? sender, EventArgs e)
    {
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"EL USUARIO HIZO CLICK EN EL BOTON BTNSALIR".ToUpperInvariant());
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: btnSalir_Click(object? sender:{sender?.GetType().Name ?? "null"}, EventArgs e:{e})");
        if (MessageBox.Show("¿Desea salir?", "Confirmar", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes) Close();
    }
    // FILTRO EN VIVO: se dispara en CADA tecla que escribes en txtBuscar.
    // Si el campo esta vacio -> ObtenerVentasDesconectado (todo).
    private void txtBuscar_TextChanged(object? sender, EventArgs e)
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: txtBuscar_TextChanged(object? sender:{sender?.GetType().Name ?? "null"}, EventArgs e:{e}, texto:{txtBuscar.Text})");
        try
        {
            string filtro = txtBuscar.Text.Trim();
            dgvVentas.DataSource = string.IsNullOrEmpty(filtro)
                ? ventaDao.ObtenerVentasDesconectado()
                : ventaDao.BuscarVentasPorCliente(filtro);
            ConfigurarColumnasGrilla();
        }
        catch (Exception ex)
        {
            MostrarEstado("No se pudo filtrar el historial.", false);
            MessageBox.Show("No se pudo buscar.\n" + ex.Message, "Base de datos",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
    // Recarga el historial completo desde la BD y se lo asigna al DataGridView.
    // Se llama desde 5 lugares: Load, Registrar, Actualizar, Eliminar y btnTodos.
    private void CargarVentas()
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: CargarVentas(filasActualesEnGrilla:{dgvVentas.Rows.Count})");
        try
        {
            //llena datagridview de ventas
            dgvVentas.DataSource = ventaDao.ObtenerVentasDesconectado();
            Console.WriteLine($"{++Program.Secuencia,4}. " + "El DataTable de la BD ya está en la grilla" + " | " + "Filas visibles" + " = " + dgvVentas.Rows.Count);;
            ConfigurarColumnasGrilla();
            Console.WriteLine($"{++Program.Secuencia,4}. " + "El DataTable de la BD ya está en la grilla" + " | " + "columnasvisibles" + " = " + dgvVentas.ColumnCount); ;
        }
        catch (Exception ex)
        {
            MostrarEstado("No se pudieron cargar las ventas.", false);
            MessageBox.Show("No se pudieron cargar las ventas.\n" + ex.Message, "Base de datos",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
    // RESPUESTA a tu pregunta "¿que hace este metodo?":
    // Solo MAQUILLA la grilla despues de asignar el DataSource.
    private void ConfigurarColumnasGrilla()
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ConfigurarColumnasGrilla(columnas:{dgvVentas.ColumnCount} | filas:{dgvVentas.Rows.Count} | primeraFilaId:{(dgvVentas.Rows.Count > 0 ? dgvVentas.Rows[0].Cells["Id"].Value?.ToString() : "(sin filas)")})");
        Titulo("Id", "ID");
        Titulo("Cliente", "Cliente");
        Titulo("Rut", "RUT");
        Titulo("EsSocio", "Socio");
        Titulo("PeliculaTexto", "Pelicula");
        Titulo("CantidadTickets", "Entradas");
        Titulo("ComboNombre", "Combo");
        Titulo("TotalPagar", "Total a Pagar");
        if (dgvVentas.Columns["TotalPagar"] != null)
            dgvVentas.Columns["TotalPagar"]!.DefaultCellStyle.Format = "$#,##0";
    }
    private void Titulo(string columna, string texto)
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        string valorCelda = (dgvVentas.Rows.Count > 0 && dgvVentas.Columns[columna] != null)
            ? dgvVentas.Rows[0].Cells[columna].Value?.ToString() ?? "(null)"
            : "(sin filas)";
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: Titulo(string columna:{columna} , string texto: {texto} , valorFila0:{valorCelda})");
        var col = dgvVentas.Columns[columna];
        if (col != null) col.HeaderText = texto;
    }
    // CLIC EN UNA FILA: toma el Id de la celda clickeada, pide el detalle al DAO
    // y carga el formulario en modo edicion con LlenaForm(v).
    private void dgvVentas_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: dgvVentas_CellClick(object? sender:{sender?.GetType().Name ?? "null"}, DataGridViewCellEventArgs e fila:{e.RowIndex})");
        Console.WriteLine($"{++Program.Secuencia,4}. " + "El usuario clickeó una fila del historial" + " | " + "Indice de fila" + " = " + e.RowIndex);;
        if (e.RowIndex < 0) return;
        var cell = dgvVentas.Rows[e.RowIndex].Cells["Id"];
        if (cell?.Value == null) return;
        if (!int.TryParse(cell.Value.ToString(), out int id)) return;
        try
        {
            VentaCine? v = ventaDao.ObtenerVentaPorId(id);
            if (v != null) LlenaForm(v);
        }
        catch (Exception ex)
        {
            MostrarEstado("No se pudo cargar el detalle de la venta.", false);
            MessageBox.Show("No se pudo cargar la venta.\n" + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    // MODO EDICION: copia todos los valores de la venta a los controles.
    // OJO con la bandera: se pone en true ANTES de asignar para que los eventos
    private void LlenaForm(VentaCine v)
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: LlenaForm(VentaCine v:Id={v.Id} / {v.Cliente} / Total={v.TotalPagar})");
        cargandoFormulario = true;
        txtId.Text = v.Id.ToString();
        txtCliente.Text = v.Cliente;
        txtRut.Text = v.Rut;
        chkSocio.Checked = v.EsSocio;
        cboPelicula.SelectedItem = v.PeliculaTexto;
        if (cboPelicula.SelectedIndex < 0) cboPelicula.Text = v.PeliculaTexto;
        cboCombo.SelectedItem = v.ComboNombre;
        if (cboCombo.SelectedIndex < 0) cboCombo.Text = v.ComboNombre;
        txtCantidad.Text = v.CantidadTickets.ToString();
        cargandoFormulario = false;
        Console.WriteLine($"{++Program.Secuencia,4}. " + "La venta leida de la BD se carga en modo edicion" + " | " + "Venta" + " = " + $"Id={v.Id} | {v.Cliente} | {v.PeliculaTexto} | Total={v.TotalPagar}");;
        MostrarResumen(v);
        MostrarEstado($"Venta N° {v.Id} cargada en modo edicion.", true);
    }
    // Vuelve al estado inicial tras registrar o eliminar.
    // Tambien usa la bandera cargandoFormulario para no disparar calculos
    private void LimpiarFormulario()
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: LimpiarFormulario(idAnterior:{txtId.Text} | clienteAnterior:{txtCliente.Text} | totalAnterior:{lblTotalValor.Text})");
        cargandoFormulario = true;
        txtId.Clear();
        txtCliente.Clear();
        txtRut.Clear();
        chkSocio.Checked = false;
        cboPelicula.SelectedItem = "Aventura 2D";
        cboCombo.SelectedItem = "Sin combo";
        txtCantidad.Text = "1";
        ReiniciarResumen();
        cargandoFormulario = false;
        txtCliente.Focus();
    }
    // Pide al DAO una consulta agregada (COUNT/SUM/AVG) y actualiza los 3
    // labels superiores. Se llama al cargar y despues de cada escritura.
    private void ActualizarMetricas()
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ActualizarMetricas(ventas:{lblVentasMetric.Text} | recaudado:{lblRecaudadoMetric.Text} | promedio:{lblPromedioMetric.Text})");
        try
        {
            DataTable dt = ventaDao.ObtenerMetricas();
            if (dt.Rows.Count == 0) return;
            DataRow r = dt.Rows[0];
            lblVentasMetric.Text = "Ventas registradas: " + Convert.ToInt32(r["TotalVentas"]);
            lblRecaudadoMetric.Text = "Total recaudado: $ " + Convert.ToDecimal(r["TotalRecaudado"]).ToString("N0") + " CLP";
            lblPromedioMetric.Text = "Promedio por venta: $ " + Convert.ToDecimal(r["PromedioVenta"]).ToString("N0") + " CLP";
            Console.WriteLine($"{++Program.Secuencia,4}. " + "La consulta agregada llega a los labels de metricas" + " | " + "Metricas" + " = " + $"{lblVentasMetric.Text} | {lblRecaudadoMetric.Text} | {lblPromedioMetric.Text}");;
        }
        catch (Exception)
        {
            MostrarEstado("No se pudieron actualizar las metricas.", false);
        }
    }
    // ConfigurarAyuda asocia un texto a cada boton; Vincular suscribe dos
    // eventos de mouse (MouseEnter/MouseLeave) con expresiones lambda.
    private void ConfigurarAyuda()
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: ConfigurarAyuda(botonesRegistrados:6 | estadoActual:{lblAyuda.Text})");
        Vincular(btnRegistrar, "Calcula el total y registra la venta en la base de datos.");
        Vincular(btnActualizar, "Actualiza la venta seleccionada en la tabla.");
        Vincular(btnEliminar, "Anula (borra) la venta seleccionada previa confirmacion.");
        Vincular(btnTodos, "Muestra el historial completo de ventas.");
        Vincular(btnProbarConexion, "Prueba la conexion con CineStarDB en SQLEXPRESS.");
        Vincular(btnSalir, "Cierra la aplicacion.");
    }
    private void Vincular(Button b, string ayuda)
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: Vincular(Button b:{b.Text}, string ayuda:{ayuda})");
        b.MouseEnter += (_, _) => MostrarEstado(ayuda);
        b.MouseLeave += (_, _) => MostrarEstado("Pase el mouse sobre un boton para ver ayuda.");
    }
    // SUMIDERO de casi todos los flujos: cualquier accion termina escribiendo
    // aqui. exito = true -> gris verdoso, false -> gris rosado, null -> gris.
    private void MostrarEstado(string texto, bool? exito = null)
    {
        if (Program.UltimoArchivo != "FormCine.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: MostrarEstado(string texto:{texto}, bool? exito: {exito})");
        lblAyuda.Text = texto;
        // Colores suaves y casi grises para no romper el tema oscuro del formulario.
        lblAyuda.ForeColor = exito == true ? System.Drawing.Color.FromArgb(165, 195, 165)  // exito: gris verdoso
            : exito == false ? System.Drawing.Color.FromArgb(205, 165, 165)                // error: gris rosado
            : System.Drawing.Color.FromArgb(176, 176, 176);                                // normal: gris
    }
}
