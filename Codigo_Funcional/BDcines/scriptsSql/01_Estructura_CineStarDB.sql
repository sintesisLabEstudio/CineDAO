-- Estructura_CineStarDB.sql
-- Estructura de la base de datos CineStarDB

-- creaa BD CineStarDB si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CineStarDB')
    CREATE DATABASE CineStarDB;
GO
USE CineStarDB;
GO
-- crea tabla ventas dbo.Ventas
IF OBJECT_ID('', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Ventas (
    -- cliente
        Id INT IDENTITY(1,1) NOT NULL, -- pk
        Cliente NVARCHAR(100) NOT NULL,
        Rut NVARCHAR(20) NOT NULL,
        EsSocio BIT NOT NULL,
    -- pedidoPelicula
        PeliculaTexto NVARCHAR(150) NOT NULL,
        PrecioPelicula INT NOT NULL,
    -- tikets
        CantidadTickets INT NOT NULL,
    -- combo
        ComboNombre NVARCHAR(50) NOT NULL,
        PrecioCombo INT NOT NULL,
    -- subTotales
        SubtotalEntradas INT NOT NULL,
        SubtotalSnacks INT NOT NULL,
    -- dcto
        MontoDescuento INT NOT NULL,
    -- neto afecto, iva y total
        NetoAfecto INT NOT NULL,
        MontoIva INT NOT NULL,
        TotalPagar INT NOT NULL,
    -- CLAVE PRIMARIA
        CONSTRAINT PK_Ventas PRIMARY KEY CLUSTERED (Id ASC) -- ascendente
    );
END
GO
