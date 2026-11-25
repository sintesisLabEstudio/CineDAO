# CineStarBD - POS de Boletería (Certamen 2)

Aplicación WinForms en **C# (.NET 10)** con **SQL Server Express (`.\SQLEXPRESS`)**, desarrollada con el mismo estilo de programación del proyecto Pizzeria Bella Italia: entidades POO en `Dominio`, reglas de negocio estáticas, helper estático de conexión, DAO con consultas parametrizadas y transacciones, y formularios con `partial class` + `.Designer.cs`.

## Estructura

```
Codigo_Funcional\
  Program.cs                       Punto de entrada (FormCine)
  CineStarBD.csproj                net10.0-windows + Microsoft.Data.SqlClient 7.1.2
  Dominio\
    VentaCine.cs                   Entidad con constructores sobrecargados y métodos puros de cálculo
    CatalogoCine.cs                Cartelera, combos y reglas (equivalente a CalculadoraPedido)
    ConexionBD.cs                  Cadena única y fábrica de SqlConnection
    VentaDAO.cs                    CRUD parametrizado + métricas
  Presentacion\
    FormCine.cs                    Lógica de la pantalla principal
    FormCine.Designer.cs           Controles, eventos y diseño
  BDcines\
    scriptsSql\                   01_Estructura_CineStarDB.sql / 02_DatosDemo_CineStarDB.sql
    comandos\                     7 .bat operativos (crear, cargar, diagnóstico, listar, respaldo, vaciar, desconectar)
```

## Puesta en marcha

1. Ejecutar `BDcines\comandos\01_creaBaseDatosCine-01.bat` (crea `CineStarDB` y `dbo.Ventas`).
2. Ejecutar `BDcines\comandos\02_cargaBaseDatos-02.bat` (carga 4 ventas de ejemplo si la tabla está vacía).
3. Compilar y ejecutar: `dotnet run --project CineStarBD.csproj`.

Si la instancia no fuera `.\SQLEXPRESS`, cambiar la cadena en `Dominio\ConexionBD.cs`.

## Funcionalidad

- Registro de ventas con cliente, RUT, película, cantidad de entradas y combo opcional.
- Descuento de **15%** para socios CineClub (`chkSocio`).
- Cálculo de **IVA 19%** sobre el neto y desglose en vivo (`Subtotal entradas`, `Combo`, `Descuento`, `Neto`, `IVA`, `Total`).
- Historial en `DataGridView` con filtro dinámico por cliente (`LIKE`, `txtBuscar.TextChanged`).
- Carga de una venta en modo edición al hacer clic en una fila (`CellClick`).
- Actualización y anulación (borrado físico con confirmación).
- Métricas globales acumuladas en tiempo real: total de ventas, recaudado y promedio.
- Prueba de conexión y mensajes de ayuda contextual.

## Decisiones de implementación

- El PDF de referencia no especificaba precios, por lo que la cartelera y los combos se definieron en `CatalogoCine` y son **fácilmente modificables** allí (al igual que `CalculadoraPedido.Tamanos` en la pizzería).
- La tabla documentada tenía 8 columnas; se conservan todas y se agregan 7 columnas más (`ComboNombre`, `PrecioCombo`, `SubtotalEntradas`, `SubtotalSnacks`, `MontoDescuento`, `NetoAfecto`, `MontoIva`) para poder **persistir y volver a mostrar el desglose** de cada venta, tal como pide el enunciado.
- Se usa `int` para los montos (mismo criterio que el caso de estudio original).
