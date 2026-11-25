USE CineStarDB;
GO
IF (SELECT COUNT(*) FROM dbo.Ventas) = 0
BEGIN
    INSERT INTO dbo.Ventas
        (Cliente, Rut, EsSocio, PeliculaTexto, PrecioPelicula, CantidadTickets,
         ComboNombre, PrecioCombo, SubtotalEntradas, SubtotalSnacks,
         MontoDescuento, NetoAfecto, MontoIva, TotalPagar)
    VALUES
        (N'Maria Gonzalez', N'12.345.678-9', 1, N'Ciencia Ficcion 3D', 7000, 2,
         N'Combo Clasico', 3000, 14000, 3000, 2550, 14450, 2745, 17195),
        (N'Carlos Munoz', N'9.876.543-2', 0, N'Aventura 2D', 5000, 3,
         N'Sin combo', 0, 15000, 0, 0, 15000, 2850, 17850),
        (N'Ana Torres', N'7.654.321-0', 1, N'Estreno VIP', 9000, 1,
         N'Combo Familiar', 6000, 9000, 6000, 2250, 12750, 2422, 16172),
        (N'Luis Rojas', N'11.222.333-4', 0, N'Drama Premium', 6000, 2,
         N'Combo Grande', 4500, 12000, 4500, 0, 16500, 3135, 19635);
END
GO
SELECT COUNT(*) AS VentasDemo FROM dbo.Ventas;
GO
