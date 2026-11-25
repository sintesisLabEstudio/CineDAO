# Mapa de flujo y llamadas — CineStarBD

> Cómo se ocupan unas a otras las funciones, tanto **dentro de un mismo archivo** como **entre archivos**.
> Los diagramas están en Mermaid: se renderizan automáticamente en GitHub, VS Code (extensión Mermaid) o cualquier visor Markdown.
> Referencias: `Program.cs`, `Dominio\*.cs`, `Presentacion\FormCine.cs`, `Presentacion\FormCine.Designer.cs`.

---

## 1. Mapa de dependencias entre archivos (quién conoce a quién)

```mermaid
%%{init: {'flowchart': {'nodeSpacing': 80, 'rankSpacing': 110, 'curve': 'basis'}}}%%
flowchart TD
    P["Program.cs<br/>Main()"]
    DS["FormCine.Designer.cs<br/>InitializeComponent()"]
    F["FormCine.cs<br/>logica de pantalla"]

    subgraph CAL["Dominio — calculo"]
        direction TB
        CA["CatalogoCine.cs<br/>catalogo y reglas"]
        VC["VentaCine.cs<br/>entidad + calculos puros"]
    end

    subgraph DAT["Dominio — datos"]
        direction TB
        VD["VentaDAO.cs<br/>CRUD + metricas"]
        CB["ConexionBD.cs<br/>cadena + SqlConnection"]
    end

    DB[("CineStarDB.dbo.Ventas<br/>SQL Express .SQLEXPRESS")]
    SQL["BDcines/scriptsSql<br/>01 estructura / 02 demo"]
    BAT["BDcines/comandos<br/>7 .bat"]

    P -->|"new FormCine()"| F
    DS -->|"InitializeComponent()"| F
    DS -.->|"cablea 13 eventos"| F
    F -->|"DatosVenta / Calcular"| CA
    CA -->|"new VentaCine(...)"| VC
    F -->|"7 metodos publicos"| VD
    VD -->|"ObtenerConexion()"| CB
    F -->|"ProbarConexion()"| CB
    CB -->|"Open()"| DB
    VD -->|"SELECT / INSERT / UPDATE / DELETE"| DB
    SQL -->|"DDL + demo"| DB
    BAT -->|"operaciones"| DB

    classDef ui fill:#e3f2fd,stroke:#1565c0,color:#0d47a1;
    classDef dom fill:#e8f5e9,stroke:#2e7d32,color:#1b5e20;
    classDef data fill:#fff3e0,stroke:#ef6c00,color:#e65100;
    class P,F,DS ui;
    class CA,VC,VD,CB dom;
    class SQL,BAT,DB data;
```

**Regla de oro:** `FormCine` nunca abre una conexión. Todo pasa por `VentaDAO` → `ConexionBD`. `CatalogoCine` y `VentaCine` no conocen ni WinForms ni SQL.

---

## 2. Grafo de llamadas dentro de `FormCine.cs` (mismo archivo)

Para que las flechas no se apelotonen, el grafo se divide en tres momentos. Todos usan `nodeSpacing`/`rankSpacing` amplios y curva `basis`, que abre ángulo entre las líneas.

### 2.1 Carga inicial

```mermaid
%%{init: {'flowchart': {'nodeSpacing': 90, 'rankSpacing': 110, 'curve': 'basis'}}}%%
flowchart TD
    CTOR["FormCine() ctor"]
    LOAD["FormCine_Load"]
    RESET["ReiniciarResumen()"]
    HELP["ConfigurarAyuda()"]
    BIND["Vincular() x6"]
    GRID["CargarVentas()"]
    COLS["ConfigurarColumnasGrilla()"]
    TITLE["Titulo() x8"]
    METR["ActualizarMetricas()"]
    STATUS["MostrarEstado()"]

    CTOR --> LOAD
    LOAD --> RESET
    LOAD --> HELP
    HELP --> BIND
    BIND --> STATUS
    LOAD --> GRID
    GRID --> COLS
    COLS --> TITLE
    LOAD --> METR
    LOAD --> STATUS
    GRID --> STATUS
    METR --> STATUS
```

### 2.2 Cálculo en vivo

```mermaid
%%{init: {'flowchart': {'nodeSpacing': 100, 'rankSpacing': 120, 'curve': 'basis'}}}%%
flowchart TD
    CHANGED["Calculo_Changed<br/>4 eventos del Designer"]
    BUILD["ConstruirVenta()"]
    SHOW["MostrarResumen()"]
    EXT["Va a Dominio:<br/>CatalogoCine.Calcular → VentaCine"]

    CHANGED --> BUILD
    CHANGED --> SHOW
    BUILD --> EXT
```

> El salto a Dominio se detalla en el apartado 3.1.

### 2.3 Acciones del usuario (botones y grilla)

> Diagrama **horizontal y ampliado**. En el visor Markdown se recorre con la barra de desplazamiento lateral; en el PDF ocupa una página apaisada para que se lea grande.

```mermaid
%%{init: {'flowchart': {'nodeSpacing': 140, 'rankSpacing': 180, 'curve': 'basis'}, 'themeVariables': {'fontSize': '16px'}}}%%
flowchart LR
    subgraph ESCRITURA["Escritura en BD"]
        direction TB
        REG["BtnRegistrar_Click"]
        UPD["btnActualizar_Click"]
        DEL["btnEliminar_Click"]
    end
    subgraph LECTURA["Lectura / UI"]
        direction TB
        ALL["btnTodos_Click"]
        TEST["btnProbarConexion_Click"]
        SEARCH["txtBuscar_TextChanged"]
        CELL["dgvVentas_CellClick"]
    end

    VALID["DatosValidos()"]
    BUILD["ConstruirVenta()"]
    SHOW["MostrarResumen()"]
    CLEAR["LimpiarFormulario()"]
    RESET["ReiniciarResumen()"]
    GRID["CargarVentas()"]
    METR["ActualizarMetricas()"]
    COLS["ConfigurarColumnasGrilla()"]
    FILL["LlenaForm()"]
    STATUS["MostrarEstado()"]

    REG --> VALID
    UPD --> VALID
    REG --> BUILD
    UPD --> BUILD
    REG --> SHOW
    REG --> CLEAR
    CLEAR --> RESET
    DEL --> CLEAR
    REG --> GRID
    UPD --> GRID
    DEL --> GRID
    REG --> METR
    UPD --> METR
    DEL --> METR
    ALL --> GRID
    SEARCH --> GRID
    SEARCH --> COLS
    CELL --> FILL
    FILL --> SHOW
    FILL --> STATUS
    TEST --> STATUS
    GRID --> STATUS
    METR --> STATUS
```

**Observación:** `MostrarEstado` sigue siendo el nodo sumidero, pero al quedar a la derecha y con `rankSpacing` de 180, las flechas llegan desde ángulos muy distintos en vez de apilarse. `CargarVentas` y `ActualizarMetricas` se repiten tras registrar / actualizar / eliminar.

---

## 3. Grafo de llamadas hacia `Dominio` (entre archivos)

Igual que en el apartado 2: dos grafos verticales en vez de uno horizontal, para que cada rama tenga su propio carril.

### 3.1 Dominio — cálculo puro (sin SQL)

```mermaid
%%{init: {'flowchart': {'nodeSpacing': 90, 'rankSpacing': 110, 'curve': 'basis'}}}%%
flowchart TD
    CH["FormCine.ConstruirVenta()"]
    CALC["CatalogoCine.Calcular(datos)"]
    GP["CatalogoCine.ObtenerPeliculas()"]
    GC["CatalogoCine.ObtenerCombos()"]
    NEW["new VentaCine(...)"]
    TOT["VentaCine.CalcularTotales()"]

    subgraph PURAS["Metodos puros — se llaman entre si"]
        direction TB
        SE["CalcularSubtotalEntradas()"]
        SS["CalcularSubtotalSnacks()"]
        DE["CalcularMontoDescuento()"]
        NE["CalcularNetoAfecto()"]
        IV["CalcularMontoIVA()"]
        TO["CalcularTotal()"]
    end

    CH --> CALC
    CALC --> NEW
    NEW --> TOT
    TOT --> SE
    TOT --> SS
    TOT --> DE
    TOT --> NE
    TOT --> IV
    TOT --> TO
    DE --> SE
    DE --> SS
    DE --> NE
    NE -.->|"lee"| SE
    NE -.->|"lee"| SS
    NE --> IV
    IV --> TO
    TO -.->|"lee"| NE
    TO -.->|"lee"| IV

    GP -.->|"solo carga el combo box"| CH
    GC -.->|"solo carga el combo box"| CH

    classDef pure fill:#e8f5e9,stroke:#2e7d32,color:#1b5e20;
    class SE,SS,DE,NE,IV,TO pure;
```

> Las líneas discontinuas son **lecturas** de valores ya calculados, no llamadas de control: por eso se dibujan aparte y no cruzan el árbol principal.

### 3.2 Dominio — persistencia (DAO → conexión → BD)

```mermaid
%%{init: {'flowchart': {'nodeSpacing': 95, 'rankSpacing': 115, 'curve': 'basis'}}}%%
flowchart TD
    F["FormCine<br/>botones y grilla"]
    PC["btnProbarConexion_Click"]

    subgraph DAOPS["VentaDAO — metodos publicos"]
        direction TB
        INS["InsertarVenta(v)"]
        LIS["ObtenerVentasDesconectado()"]
        FIND["BuscarVentasPorCliente(f)"]
        BYID["ObtenerVentaPorId(id)"]
        UPT["ActualizarVenta(v)"]
        ERA["EliminarVenta(id)"]
        MET["ObtenerMetricas()"]
    end

    subgraph AYUDA["Apoyo interno del DAO"]
        direction TB
        PAR["CargarParametros()<br/>AddWithValue x14"]
        ROLL["Revertir()<br/>Rollback tolerante"]
        READ["LeerVenta(reader)"]
    end

    PROB["ConexionBD.ProbarConexion()"]
    CON["ConexionBD.ObtenerConexion()"]
    DB[("CineStarDB.dbo.Ventas")]

    F --> INS
    F --> LIS
    F --> FIND
    F --> BYID
    F --> UPT
    F --> ERA
    F --> MET
    INS --> PAR
    UPT --> PAR
    INS --> ROLL
    UPT --> ROLL
    BYID --> READ
    INS --> CON
    UPT --> CON
    ERA --> CON
    LIS --> CON
    FIND --> CON
    BYID --> CON
    MET --> CON
    PC --> PROB
    PROB --> CON
    CON --> DB

    classDef data fill:#fff3e0,stroke:#ef6c00,color:#e65100;
    class INS,LIS,FIND,BYID,UPT,ERA,MET,PAR,ROLL,READ,PROB,CON,DB data;
```

---

## 4. Secuencias de los flujos principales

### 4.1 Arranque de la pantalla (`FormCine_Load`)

```mermaid
%%{init: {'sequence': {'diagramMarginX': 50, 'diagramMarginY': 25, 'actorMargin': 90, 'messageMargin': 45}}}%%
sequenceDiagram
    autonumber
    participant Program as Program.cs
    participant F as FormCine.cs
    participant CA as CatalogoCine.cs
    participant DAO as VentaDAO.cs
    participant CB as ConexionBD.cs
    participant DB as CineStarDB

    Program->>F: new FormCine()
    F->>F: InitializeComponent() (Designer)
    F->>F: FormCine_Load
    F->>CA: ObtenerPeliculas()
    CA-->>F: string[] ordenado
    F->>CA: ObtenerCombos()
    CA-->>F: string[] ordenado
    F->>F: ReiniciarResumen() -> labels en $ 0
    F->>F: ConfigurarAyuda() -> Vincular() x6 -> MostrarEstado()
    F->>DAO: ObtenerVentasDesconectado()
    DAO->>CB: ObtenerConexion()
    CB->>DB: Open()
    DAO->>DB: SELECT Id, Cliente, ... ORDER BY Id DESC
    DB-->>DAO: DataTable
    DAO-->>F: DataTable
    F->>F: ConfigurarColumnasGrilla() -> Titulo() x8
    F->>DAO: ObtenerMetricas()
    DAO->>CB: ObtenerConexion()
    DAO->>DB: SELECT COUNT / SUM / AVG
    DB-->>DAO: DataTable (1 fila)
    DAO-->>F: DataTable
    F->>F: ActualizarMetricas() -> 3 labels
    F->>F: MostrarEstado("Listo...")
```

### 4.2 Cálculo en vivo (`Calculo_Changed`)

```mermaid
%%{init: {'sequence': {'diagramMarginX': 50, 'diagramMarginY': 25, 'actorMargin': 90, 'messageMargin': 45}}}%%
sequenceDiagram
    autonumber
    participant U as Usuario
    participant DS as Designer.cs
    participant F as FormCine.cs
    participant CA as CatalogoCine.cs
    participant V as VentaCine.cs

    U->>DS: cambia cboPelicula / cboCombo / txtCantidad / chkSocio
    DS->>F: Calculo_Changed (4 eventos)
    F->>F: if cargandoFormulario -> return
    F->>F: ConstruirVenta() -> new DatosVenta
    F->>CA: Calcular(datos)
    CA->>CA: valida Pelicula / Combo / Cantidad (defaults)
    CA->>V: new VentaCine(cliente, rut, socio, pelicula, precio, cantidad, combo, precioCombo)
    V->>V: CalcularTotales()
    V->>V: SubE, SubS, Desc, Neto, IVA, Total (6 metodos puros)
    V-->>CA: VentaCine con montos
    CA-->>F: VentaCine con montos
    F->>F: MostrarResumen(v) -> 6 labels
```

### 4.3 Registrar una venta (`BtnRegistrar_Click`)

```mermaid
%%{init: {'sequence': {'diagramMarginX': 50, 'diagramMarginY': 25, 'actorMargin': 90, 'messageMargin': 45}}}%%
sequenceDiagram
    autonumber
    participant U as Usuario
    participant F as FormCine.cs
    participant CA as CatalogoCine.cs
    participant V as VentaCine.cs
    participant DAO as VentaDAO.cs
    participant CB as ConexionBD.cs
    participant DB as CineStarDB

    U->>F: click btnRegistrar
    F->>F: DatosValidos() -> nombre, RUT, cantidad > 0
    F->>F: ConstruirVenta() -> CatalogoCine.Calcular -> VentaCine.CalcularTotales
    F->>F: MostrarResumen(venta)
    F->>U: MessageBox "¿Registrar la venta? Total: $..."
    U-->>F: Yes
    F->>DAO: InsertarVenta(venta)
    DAO->>CB: ObtenerConexion()
    DAO->>DB: BEGIN TRANSACTION
    DAO->>DAO: CargarParametros(cmd, v) -> AddWithValue x14
    DAO->>DB: INSERT INTO dbo.Ventas (...) + SCOPE_IDENTITY()
    DB-->>DAO: Id generado
    DAO->>DAO: v.Id = Convert.ToInt32(resultado)
    DAO->>DB: COMMIT
    DAO-->>F: true
    F->>U: MostrarEstado + MessageBox "Venta registrada"
    F->>F: LimpiarFormulario() -> ReiniciarResumen()
    F->>DAO: ObtenerVentasDesconectado() -> CargarVentas()
    F->>DAO: ObtenerMetricas() -> ActualizarMetricas()
```

> Si falla: `Revertir(tran)` → `Rollback()` tolerante y la excepción se propaga al `catch` del botón.

### 4.4 Actualizar / Eliminar / Buscar

```mermaid
%%{init: {'sequence': {'diagramMarginX': 50, 'diagramMarginY': 25, 'actorMargin': 90, 'messageMargin': 45}}}%%
sequenceDiagram
    autonumber
    participant F as FormCine.cs
    participant DAO as VentaDAO.cs
    participant CB as ConexionBD.cs
    participant DB as CineStarDB

    Note over F,DB: ACTUALIZAR
    F->>F: btnActualizar_Click -> lee txtId -> DatosValidos -> ConstruirVenta
    F->>DAO: ActualizarVenta(venta con Id)
    DAO->>CB: ObtenerConexion()
    DAO->>DB: BEGIN TRAN -> UPDATE ... WHERE Id=@Id -> COMMIT
    DAO-->>F: filas afectadas > 0
    F->>DAO: ObtenerVentasDesconectado() + ObtenerMetricas()

    Note over F,DB: ELIMINAR
    F->>F: btnEliminar_Click -> confirma -> lee txtId
    F->>DAO: EliminarVenta(id)
    DAO->>CB: ObtenerConexion()
    DAO->>DB: DELETE FROM dbo.Ventas WHERE Id=@Id
    DAO-->>F: filas afectadas > 0
    F->>F: LimpiarFormulario() + recarga grilla y metricas

    Note over F,DB: BUSCAR (cada tecla)
    F->>F: txtBuscar_TextChanged
    alt filtro vacio
        F->>DAO: ObtenerVentasDesconectado()
    else con texto
        F->>DAO: BuscarVentasPorCliente(filtro)
        DAO->>DB: SELECT ... WHERE Cliente LIKE @Filtro
    end
    DAO-->>F: DataTable
    F->>F: ConfigurarColumnasGrilla() -> Titulo() x8

    Note over F,DB: CARGAR DETALLE AL HACER CLIC EN FILA
    F->>F: dgvVentas_CellClick -> lee columna Id
    F->>DAO: ObtenerVentaPorId(id)
    DAO->>DB: SELECT 15 columnas WHERE Id=@Id
    DAO->>DAO: LeerVenta(reader) -> object initializer de VentaCine
    DAO-->>F: VentaCine ( SIN recalcular )
    F->>F: LlenaForm(v) -> MostrarResumen(v) -> MostrarEstado(...)
```

### 4.5 Prueba de conexión

```mermaid
%%{init: {'sequence': {'diagramMarginX': 50, 'diagramMarginY': 25, 'actorMargin': 90, 'messageMargin': 45}}}%%
sequenceDiagram
    participant U as Usuario
    participant F as FormCine.cs
    participant CB as ConexionBD.cs
    participant DB as SQL Server

    U->>F: click btnProbarConexion
    F->>CB: ProbarConexion()
    CB->>CB: ObtenerConexion() -> new SqlConnection + Open()
    CB->>DB: Open()
    alt exito
        DB-->>CB: State = Open
        CB-->>F: true
    else SqlException
        CB->>CB: Console.WriteLine(error) (invisible en WinForms)
        CB-->>F: false
    end
    F->>CB: CadenaConexion (property)
    F->>U: MessageBox con cadena + estado
```

---

## 5. Tabla de trazabilidad (función → llamada desde → llama a)

| Función | Archivo | Llamada desde | Llama a |
|---|---|---|---|
| `Main()` | `Program.cs` | Windows | `Application.Run(new FormCine())` |
| `FormCine()` | `FormCine.cs` | `Program.Main` | `InitializeComponent()` |
| `FormCine_Load` | `FormCine.cs` | Designer (`Load`) | `ObtenerPeliculas`, `ObtenerCombos`, `ReiniciarResumen`, `ConfigurarAyuda`, `CargarVentas`, `ActualizarMetricas`, `MostrarEstado` |
| `ConstruirVenta` | `FormCine.cs` | `Calculo_Changed`, `BtnRegistrar`, `btnActualizar` | `CatalogoCine.DatosVenta`, `CatalogoCine.Calcular` |
| `Calculo_Changed` | `FormCine.cs` | Designer (4 eventos) | `ConstruirVenta`, `MostrarResumen` |
| `MostrarResumen` | `FormCine.cs` | `Calculo_Changed`, `BtnRegistrar`, `LlenaForm` | 6 labels |
| `DatosValidos` | `FormCine.cs` | `BtnRegistrar`, `btnActualizar` | `MessageBox`, `Focus()` |
| `BtnRegistrar_Click` | `FormCine.cs` | Designer (`Click`) | `DatosValidos`, `ConstruirVenta`, `MostrarResumen`, `VentaDAO.InsertarVenta`, `LimpiarFormulario`, `CargarVentas`, `ActualizarMetricas`, `MostrarEstado` |
| `btnActualizar_Click` | `FormCine.cs` | Designer (`Click`) | `DatosValidos`, `ConstruirVenta`, `VentaDAO.ActualizarVenta`, `CargarVentas`, `ActualizarMetricas` |
| `btnEliminar_Click` | `FormCine.cs` | Designer (`Click`) | `VentaDAO.EliminarVenta`, `LimpiarFormulario`, `CargarVentas`, `ActualizarMetricas` |
| `btnTodos_Click` | `FormCine.cs` | Designer (`Click`) | `CargarVentas`, `MostrarEstado` |
| `btnProbarConexion_Click` | `FormCine.cs` | Designer (`Click`) | `ConexionBD.ProbarConexion`, `CadenaConexion`, `MostrarEstado` |
| `btnSalir_Click` | `FormCine.cs` | Designer (`Click`) | `Close()` |
| `txtBuscar_TextChanged` | `FormCine.cs` | Designer (`TextChanged`) | `ObtenerVentasDesconectado` o `BuscarVentasPorCliente`, `ConfigurarColumnasGrilla` |
| `CargarVentas` | `FormCine.cs` | 5 lugares | `VentaDAO.ObtenerVentasDesconectado`, `ConfigurarColumnasGrilla`, `MostrarEstado` |
| `ConfigurarColumnasGrilla` | `FormCine.cs` | `CargarVentas`, `txtBuscar_TextChanged` | `Titulo` ×8 |
| `dgvVentas_CellClick` | `FormCine.cs` | Designer (`CellClick`) | `VentaDAO.ObtenerVentaPorId`, `LlenaForm` |
| `LlenaForm` | `FormCine.cs` | `dgvVentas_CellClick` | `MostrarResumen`, `MostrarEstado` |
| `LimpiarFormulario` | `FormCine.cs` | `BtnRegistrar`, `btnEliminar` | `ReiniciarResumen` |
| `ActualizarMetricas` | `FormCine.cs` | 4 lugares | `VentaDAO.ObtenerMetricas`, `MostrarEstado` |
| `ConfigurarAyuda` | `FormCine.cs` | `FormCine_Load` | `Vincular` ×6 |
| `Vincular` | `FormCine.cs` | `ConfigurarAyuda` | `MostrarEstado` (lambdas MouseEnter/Leave) |
| `MostrarEstado` | `FormCine.cs` | casi todos | actualiza `lblAyuda` |
| `CatalogoCine.Calcular` | `CatalogoCine.cs` | `ConstruirVenta` | valida diccionarios → `new VentaCine(...)` |
| `ObtenerPeliculas` / `ObtenerCombos` | `CatalogoCine.cs` | `FormCine_Load` | `Keys.OrderBy().ToArray()` |
| `VentaCine` ctor | `VentaCine.cs` | `CatalogoCine.Calcular`, `VentaDAO.LeerVenta` | `CalcularTotales()` (solo el ctor sobrecargado) |
| `CalcularTotales` | `VentaCine.cs` | ctor sobrecargado | los 6 métodos puros |
| `CalcularMontoDescuento` | `VentaCine.cs` | `CalcularTotales` | `CalcularSubtotalEntradas`, `CalcularSubtotalSnacks` |
| `CalcularNetoAfecto` | `VentaCine.cs` | `CalcularTotales` | `SubE`, `SubS`, `Descuento` |
| `CalcularMontoIVA` | `VentaCine.cs` | `CalcularTotales` | `CalcularNetoAfecto` |
| `CalcularTotal` | `VentaCine.cs` | `CalcularTotales` | `Neto`, `IVA` |
| `InsertarVenta` | `VentaDAO.cs` | `BtnRegistrar_Click` | `ObtenerConexion`, `CargarParametros`, `Revertir`, `Commit` |
| `ActualizarVenta` | `VentaDAO.cs` | `btnActualizar_Click` | `ObtenerConexion`, `CargarParametros`, `Revertir`, `Commit` |
| `EliminarVenta` | `VentaDAO.cs` | `btnEliminar_Click` | `ObtenerConexion` |
| `ObtenerVentasDesconectado` | `VentaDAO.cs` | `CargarVentas`, `txtBuscar` | `ObtenerConexion`, `SqlDataAdapter.Fill` |
| `BuscarVentasPorCliente` | `VentaDAO.cs` | `txtBuscar_TextChanged` | `ObtenerConexion`, `LIKE @Filtro` |
| `ObtenerVentaPorId` | `VentaDAO.cs` | `dgvVentas_CellClick` | `ObtenerConexion`, `LeerVenta` |
| `ObtenerMetricas` | `VentaDAO.cs` | `ActualizarMetricas` | `ObtenerConexion`, `COUNT/SUM/AVG` |
| `CargarParametros` | `VentaDAO.cs` | `InsertarVenta`, `ActualizarVenta` | `AddWithValue` ×14 |
| `LeerVenta` | `VentaDAO.cs` | `ObtenerVentaPorId` | `new VentaCine { ... }` (object initializer) |
| `Revertir` | `VentaDAO.cs` | `InsertarVenta`, `ActualizarVenta` | `tran.Rollback()` |
| `ObtenerConexion` | `ConexionBD.cs` | los 7 métodos del DAO + `ProbarConexion` | `SqlConnection.Open()` |
| `ProbarConexion` | `ConexionBD.cs` | `btnProbarConexion_Click` | `ObtenerConexion`, `Console.WriteLine` si falla |
| `CadenaConexion` | `ConexionBD.cs` | `btnProbarConexion_Click` | devuelve `cadenaConexion` |
| `InitializeComponent` | `FormCine.Designer.cs` | ctor de `FormCine` | crea controles y cablea 13 eventos |

---

## 6. Resumen en una frase

> `Program` llama a `FormCine`; el Designer le inyecta 13 eventos; `FormCine` calcula con `CatalogoCine` + `VentaCine` y persiste vía `VentaDAO`; el DAO abre conexiones con `ConexionBD` y habla con `CineStarDB`; después de cada escritura, `FormCine` vuelve a llamar a `CargarVentas` y `ActualizarMetricas`, y termina siempre en `MostrarEstado`.
