#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;
namespace CineStarBD.Presentacion;
// Diseno del formulario principal de boleteria.
// Equivale a FormPedido.Designer.cs en el proyecto Pizzeria.
partial class FormCine
{
    private System.ComponentModel.IContainer? components = null;
    private GroupBox grpCliente = null!;
    private Label lblCliente = null!;
    private TextBox txtCliente = null!;
    private Label lblRut = null!;
    private TextBox txtRut = null!;
    private CheckBox chkSocio = null!;
    private GroupBox grpFuncion = null!;
    private Label lblPelicula = null!;
    private ComboBox cboPelicula = null!;
    private Label lblCombo = null!;
    private ComboBox cboCombo = null!;
    private Label lblCantidad = null!;
    private TextBox txtCantidad = null!;
    private GroupBox grpResumen = null!;
    private Label lblSubtotalNombre = null!;
    private Label lblSubtotalValor = null!;
    private Label lblSnacksNombre = null!;
    private Label lblSnacksValor = null!;
    private Label lblDescuentoNombre = null!;
    private Label lblDescuentoValor = null!;
    private Label lblNetoNombre = null!;
    private Label lblNetoValor = null!;
    private Label lblIvaNombre = null!;
    private Label lblIvaValor = null!;
    private Label lblTotalNombre = null!;
    private Label lblTotalValor = null!;
    private GroupBox grpMetricas = null!;
    private Label lblVentasMetric = null!;
    private Label lblRecaudadoMetric = null!;
    private Label lblPromedioMetric = null!;
    private Button btnRegistrar = null!;
    private Button btnActualizar = null!;
    private Button btnEliminar = null!;
    private Button btnTodos = null!;
    private Button btnProbarConexion = null!;
    private Button btnSalir = null!;
    private Label lblBuscar = null!;
    private TextBox txtBuscar = null!;
    private Label lblId = null!;
    private TextBox txtId = null!;
    private DataGridView dgvVentas = null!;
    private Label lblAyuda = null!;
    protected override void Dispose(bool disposing)
    {
        if (Program.UltimoArchivo != "FormCine.Designer.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.Designer.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.Designer.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: Dispose(bool disposing:{disposing})");
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        if (Program.UltimoArchivo != "FormCine.Designer.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.Designer.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.Designer.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + "FUNCION: InitializeComponent()");
        components = new System.ComponentModel.Container();
        this.grpCliente = new GroupBox();
        this.lblCliente = new Label();
        this.txtCliente = new TextBox();
        this.lblRut = new Label();
        this.txtRut = new TextBox();
        this.chkSocio = new CheckBox();
        this.grpFuncion = new GroupBox();
        this.lblPelicula = new Label();
        this.cboPelicula = new ComboBox();
        this.lblCombo = new Label();
        this.cboCombo = new ComboBox();
        this.lblCantidad = new Label();
        this.txtCantidad = new TextBox();
        this.grpResumen = new GroupBox();
        this.lblSubtotalNombre = new Label();
        this.lblSubtotalValor = new Label();
        this.lblSnacksNombre = new Label();
        this.lblSnacksValor = new Label();
        this.lblDescuentoNombre = new Label();
        this.lblDescuentoValor = new Label();
        this.lblNetoNombre = new Label();
        this.lblNetoValor = new Label();
        this.lblIvaNombre = new Label();
        this.lblIvaValor = new Label();
        this.lblTotalNombre = new Label();
        this.lblTotalValor = new Label();
        this.grpMetricas = new GroupBox();
        this.lblVentasMetric = new Label();
        this.lblRecaudadoMetric = new Label();
        this.lblPromedioMetric = new Label();
        this.btnRegistrar = new Button();
        this.btnActualizar = new Button();
        this.btnEliminar = new Button();
        this.btnTodos = new Button();
        this.btnProbarConexion = new Button();
        this.btnSalir = new Button();
        this.lblBuscar = new Label();
        this.txtBuscar = new TextBox();
        this.lblId = new Label();
        this.txtId = new TextBox();
        this.dgvVentas = new DataGridView();
        this.lblAyuda = new Label();
        this.grpCliente.SuspendLayout();
        this.grpFuncion.SuspendLayout();
        this.grpResumen.SuspendLayout();
        this.grpMetricas.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).BeginInit();
        this.SuspendLayout();
        // grpCliente
        this.grpCliente.Text = "Datos del Cliente";
        this.grpCliente.Location = new Point(10, 10);
        this.grpCliente.Size = new Size(470, 110);
        this.grpCliente.Controls.Add(this.lblCliente);
        this.grpCliente.Controls.Add(this.txtCliente);
        this.grpCliente.Controls.Add(this.lblRut);
        this.grpCliente.Controls.Add(this.txtRut);
        this.grpCliente.Controls.Add(this.chkSocio);
        this.lblCliente.Text = "Nombre:";
        this.lblCliente.Location = new Point(15, 25);
        this.lblCliente.Size = new Size(90, 20);
        this.txtCliente.Location = new Point(110, 22);
        this.txtCliente.Size = new Size(340, 23);
        this.lblRut.Text = "RUT:";
        this.lblRut.Location = new Point(15, 60);
        this.lblRut.Size = new Size(90, 20);
        this.txtRut.Location = new Point(110, 57);
        this.txtRut.Size = new Size(170, 23);
        this.chkSocio.Text = "Socio CineClub (15% dcto)";
        this.chkSocio.Location = new Point(300, 57);
        this.chkSocio.Size = new Size(155, 24);
        // grpFuncion
        this.grpFuncion.Text = "Funcion y Confiteria";
        this.grpFuncion.Location = new Point(500, 10);
        this.grpFuncion.Size = new Size(470, 110);
        this.grpFuncion.Controls.Add(this.lblPelicula);
        this.grpFuncion.Controls.Add(this.cboPelicula);
        this.grpFuncion.Controls.Add(this.lblCombo);
        this.grpFuncion.Controls.Add(this.cboCombo);
        this.grpFuncion.Controls.Add(this.lblCantidad);
        this.grpFuncion.Controls.Add(this.txtCantidad);
        this.lblPelicula.Text = "Pelicula:";
        this.lblPelicula.Location = new Point(15, 25);
        this.lblPelicula.Size = new Size(90, 20);
        this.cboPelicula.Location = new Point(110, 22);
        this.cboPelicula.Size = new Size(340, 23);
        this.cboPelicula.DropDownStyle = ComboBoxStyle.DropDownList;
        this.lblCombo.Text = "Combo:";
        this.lblCombo.Location = new Point(15, 60);
        this.lblCombo.Size = new Size(90, 20);
        this.cboCombo.Location = new Point(110, 57);
        this.cboCombo.Size = new Size(200, 23);
        this.cboCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        this.lblCantidad.Text = "Entradas:";
        this.lblCantidad.Location = new Point(325, 60);
        this.lblCantidad.Size = new Size(70, 20);
        this.txtCantidad.Location = new Point(400, 57);
        this.txtCantidad.Size = new Size(50, 23);
        this.txtCantidad.Text = "1";
        // grpResumen
        this.grpResumen.Text = "Detalle de Pago";
        this.grpResumen.Location = new Point(10, 130);
        this.grpResumen.Size = new Size(470, 150);
        this.grpResumen.Controls.Add(this.lblSubtotalNombre);
        this.grpResumen.Controls.Add(this.lblSubtotalValor);
        this.grpResumen.Controls.Add(this.lblSnacksNombre);
        this.grpResumen.Controls.Add(this.lblSnacksValor);
        this.grpResumen.Controls.Add(this.lblDescuentoNombre);
        this.grpResumen.Controls.Add(this.lblDescuentoValor);
        this.grpResumen.Controls.Add(this.lblNetoNombre);
        this.grpResumen.Controls.Add(this.lblNetoValor);
        this.grpResumen.Controls.Add(this.lblIvaNombre);
        this.grpResumen.Controls.Add(this.lblIvaValor);
        this.grpResumen.Controls.Add(this.lblTotalNombre);
        this.grpResumen.Controls.Add(this.lblTotalValor);
        this.lblSubtotalNombre.Text = "Subtotal entradas:";
        this.lblSnacksNombre.Text = "Combo:";
        this.lblDescuentoNombre.Text = "Descuento socio (15%):";
        this.lblNetoNombre.Text = "Neto afecto:";
        this.lblIvaNombre.Text = "IVA chileno (19%):";
        this.lblTotalNombre.Text = "TOTAL A PAGAR:";
        this.lblSubtotalValor.Text = "$0 CLP";
        this.lblSnacksValor.Text = "$0 CLP";
        this.lblDescuentoValor.Text = "- $0 CLP";
        this.lblNetoValor.Text = "$0 CLP";
        this.lblIvaValor.Text = "+ $0 CLP";
        this.lblTotalValor.Text = "$0 CLP";
        this.lblSubtotalNombre.Location = new Point(15, 25);
        this.lblSnacksNombre.Location = new Point(15, 47);
        this.lblDescuentoNombre.Location = new Point(15, 69);
        this.lblNetoNombre.Location = new Point(15, 91);
        this.lblIvaNombre.Location = new Point(15, 113);
        // CORRECCION de diseno: el TOTAL estaba en x=250, detras de la columna de
        // valores (x=200, ancho 180), por lo que nunca se veia. Se mueve a la derecha.
        this.lblTotalNombre.Location = new Point(330, 91);
        this.lblSubtotalValor.Location = new Point(200, 25);
        this.lblSnacksValor.Location = new Point(200, 47);
        this.lblDescuentoValor.Location = new Point(200, 69);
        this.lblNetoValor.Location = new Point(200, 91);
        this.lblIvaValor.Location = new Point(200, 113);
        this.lblTotalValor.Location = new Point(330, 113);
        this.lblTotalValor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        this.lblTotalValor.ForeColor = Color.Goldenrod;
        // Se estrecha la columna de valores para que no tape la columna del TOTAL.
        foreach (Control c in new Control[] { this.lblSubtotalValor, this.lblSnacksValor,
                    this.lblDescuentoValor, this.lblNetoValor, this.lblIvaValor })
            c.Size = new Size(120, 20);
        this.lblTotalValor.Size = new Size(130, 20);
        // grpMetricas
        this.grpMetricas.Text = "Metricas Globales en Tiempo Real";
        this.grpMetricas.Location = new Point(500, 130);
        this.grpMetricas.Size = new Size(470, 150);
        this.grpMetricas.Controls.Add(this.lblVentasMetric);
        this.grpMetricas.Controls.Add(this.lblRecaudadoMetric);
        this.grpMetricas.Controls.Add(this.lblPromedioMetric);
        this.lblVentasMetric.Text = "Ventas registradas: 0";
        this.lblRecaudadoMetric.Text = "Total recaudado: $0 CLP";
        this.lblPromedioMetric.Text = "Promedio por venta: $0 CLP";
        this.lblVentasMetric.Location = new Point(20, 35);
        this.lblRecaudadoMetric.Location = new Point(20, 70);
        this.lblPromedioMetric.Location = new Point(20, 105);
        foreach (Control c in new Control[] { this.lblVentasMetric,
                    this.lblRecaudadoMetric, this.lblPromedioMetric })
            c.Size = new Size(420, 25);
        // Botones
        this.btnRegistrar.Text = "Registrar Venta";
        this.btnRegistrar.Location = new Point(10, 295);
        this.btnRegistrar.Size = new Size(150, 35);
        this.btnActualizar.Text = "Actualizar Venta";
        this.btnActualizar.Location = new Point(170, 295);
        this.btnActualizar.Size = new Size(150, 35);
        this.btnEliminar.Text = "Anular Registro";
        this.btnEliminar.Location = new Point(330, 295);
        this.btnEliminar.Size = new Size(150, 35);
        this.btnTodos.Text = "Todas las Ventas";
        this.btnTodos.Location = new Point(490, 295);
        this.btnTodos.Size = new Size(150, 35);
        this.btnProbarConexion.Text = "Probar Conexion";
        this.btnProbarConexion.Location = new Point(650, 295);
        this.btnProbarConexion.Size = new Size(160, 35);
        this.btnSalir.Text = "Salir";
        this.btnSalir.Location = new Point(830, 295);
        this.btnSalir.Size = new Size(140, 35);
        // Busqueda e Id
        this.lblBuscar.Text = "Buscar cliente:";
        this.lblBuscar.Location = new Point(10, 345);
        this.lblBuscar.Size = new Size(100, 20);
        this.txtBuscar.Location = new Point(110, 342);
        this.txtBuscar.Size = new Size(300, 23);
        this.lblId.Text = "ID:";
        this.lblId.Location = new Point(430, 345);
        this.lblId.Size = new Size(30, 20);
        this.txtId.Location = new Point(465, 342);
        this.txtId.Size = new Size(70, 23);
        this.txtId.ReadOnly = true;
        // Grilla
        this.dgvVentas.Location = new Point(10, 375);
        this.dgvVentas.Size = new Size(960, 265);
        this.dgvVentas.ReadOnly = true;
        this.dgvVentas.AllowUserToAddRows = false;
        this.dgvVentas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.dgvVentas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvVentas.BackgroundColor = Color.White;
        // Estado
        this.lblAyuda.Text = "Pase el mouse sobre un boton para ver ayuda.";
        this.lblAyuda.Location = new Point(10, 650);
        this.lblAyuda.Size = new Size(960, 25);
        // Formulario
        this.Text = "CineStar Concepcion - POS Boleteria";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.ClientSize = new Size(980, 685);
        this.BackColor = SystemColors.Control;
        this.Controls.Add(this.grpCliente);
        this.Controls.Add(this.grpFuncion);
        this.Controls.Add(this.grpResumen);
        this.Controls.Add(this.grpMetricas);
        this.Controls.Add(this.btnRegistrar);
        this.Controls.Add(this.btnActualizar);
        this.Controls.Add(this.btnEliminar);
        this.Controls.Add(this.btnTodos);
        this.Controls.Add(this.btnProbarConexion);
        this.Controls.Add(this.btnSalir);
        this.Controls.Add(this.lblBuscar);
        this.Controls.Add(this.txtBuscar);
        this.Controls.Add(this.lblId);
        this.Controls.Add(this.txtId);
        this.Controls.Add(this.dgvVentas);
        this.Controls.Add(this.lblAyuda);
        // Eventos: AQUI se cablean los 13 eventos. Por eso en FormCine.cs no
        // hay que suscribir nada a mano. Si haces F11 desde Program.Main,
        this.Load += FormCine_Load;
        this.btnRegistrar.Click += BtnRegistrar_Click;
        this.btnActualizar.Click += btnActualizar_Click;
        this.btnEliminar.Click += btnEliminar_Click;
        this.btnTodos.Click += btnTodos_Click;
        this.btnProbarConexion.Click += btnProbarConexion_Click;
        this.btnSalir.Click += btnSalir_Click;
        this.txtBuscar.TextChanged += txtBuscar_TextChanged;
        this.dgvVentas.CellClick += dgvVentas_CellClick;
        this.cboPelicula.SelectedIndexChanged += Calculo_Changed;
        this.cboCombo.SelectedIndexChanged += Calculo_Changed;
        this.txtCantidad.TextChanged += Calculo_Changed;
        this.chkSocio.CheckedChanged += Calculo_Changed;
        this.grpCliente.ResumeLayout();
        this.grpFuncion.ResumeLayout();
        this.grpResumen.ResumeLayout();
        this.grpMetricas.ResumeLayout();
        ((System.ComponentModel.ISupportInitialize)(this.dgvVentas)).EndInit();
        this.ResumeLayout();
        // Tema oscuro: se aplica al final para no modificar posiciones, nombres ni eventos.
        AplicarTemaOscuro();
    }
    // TEMA OSCURO EN TONOS DE GRISES
    // Paleta: fondo #1E1E1E, campos #262626, botones/encabezados #373737,
    private void AplicarTemaOscuro()
    {
        if (Program.UltimoArchivo != "FormCine.Designer.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.Designer.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.Designer.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: AplicarTemaOscuro(controlesDelForm:{this.Controls.Count})");
        Color fondo = Color.FromArgb(30, 30, 30);       // #1E1E1E
        Color entrada = Color.FromArgb(38, 38, 38);     // #262626
        Color superficie = Color.FromArgb(55, 55, 55);  // #373737
        Color borde = Color.FromArgb(85, 85, 85);       // #555555
        Color texto = Color.FromArgb(224, 224, 224);    // #E0E0E0
        Color textoSuave = Color.FromArgb(170, 170, 170); // #AAAAAA
        this.BackColor = fondo;
        this.ForeColor = texto;
        foreach (Control control in this.Controls)
            PintarControl(control, fondo, entrada, superficie, borde, texto, textoSuave);
    }
    // Aplica color segun el tipo de control y baja recursivamente a sus hijos.
    private void PintarControl(Control c, Color fondo, Color entrada, Color superficie,
                               Color borde, Color texto, Color textoSuave)
    {
        if (Program.UltimoArchivo != "FormCine.Designer.cs") { Console.WriteLine(); Program.UltimoArchivo = "FormCine.Designer.cs"; }
        Console.WriteLine($"{++Program.Secuencia,4}. " + "ARCHIVO: FormCine.Designer.cs");
        Console.WriteLine($"{++Program.Secuencia,4}. " + $"FUNCION: PintarControl(Control c:{c.GetType().Name} | texto:{c.Text})");
        // Las etiquetas NO llevan BackColor propio: si se pinta su fondo,
        // tapan a los controles que estan detras en el z-order (p. ej. el TOTAL).
        if (c is not Label) c.BackColor = fondo;
        c.ForeColor = texto;
        switch (c)
        {
            case TextBox t:
                // txtId es de solo lectura: se distingue con un gris mas oscuro.
                t.BackColor = t.ReadOnly ? fondo : entrada;
                t.ForeColor = t.ReadOnly ? textoSuave : texto;
                t.BorderStyle = BorderStyle.FixedSingle;
                break;
            case ComboBox cb:
                cb.BackColor = entrada;
                cb.ForeColor = texto;
                cb.FlatStyle = FlatStyle.Flat;
                break;
            case Button b:
                b.BackColor = superficie;
                b.ForeColor = texto;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderColor = borde;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(75, 75, 75);
                b.FlatAppearance.MouseDownBackColor = Color.FromArgb(95, 95, 95);
                break;
            case GroupBox:
            case CheckBox:
                c.BackColor = fondo;
                c.ForeColor = texto;
                break;
            case Label l:
                // El total se resalta en blanco; el resto queda gris claro.
                l.ForeColor = l == this.lblTotalValor ? Color.White : texto;
                break;
            case DataGridView g:
                g.BackgroundColor = fondo;
                g.GridColor = borde;
                g.DefaultCellStyle.BackColor = entrada;
                g.DefaultCellStyle.ForeColor = texto;
                g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(90, 90, 90);
                g.DefaultCellStyle.SelectionForeColor = Color.White;
                g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
                g.ColumnHeadersDefaultCellStyle.BackColor = superficie;
                g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                g.RowHeadersDefaultCellStyle.BackColor = superficie;
                g.RowHeadersDefaultCellStyle.ForeColor = texto;
                // Necesario para que Windows no repinte los encabezados con el tema claro.
                g.EnableHeadersVisualStyles = false;
                break;
        }
        foreach (Control hijo in c.Controls)
            PintarControl(hijo, fondo, entrada, superficie, borde, texto, textoSuave);
    }
}
