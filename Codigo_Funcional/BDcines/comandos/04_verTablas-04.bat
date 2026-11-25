@echo off
setlocal EnableExtensions

where sqlcmd >nul 2>&1
if errorlevel 1 (
    echo ERROR: no se encontro sqlcmd en el PATH.
    goto :fin
)

echo Estructura de dbo.Ventas:
sqlcmd -S ".\SQLEXPRESS" -E -d CineStarDB -b -W -Q "SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME='Ventas' ORDER BY ORDINAL_POSITION;"
if errorlevel 1 goto :error

echo Contenido de dbo.Ventas:
sqlcmd -S ".\SQLEXPRESS" -E -d CineStarDB -b -W -Q "SELECT Id, Cliente, Rut, EsSocio, PeliculaTexto, CantidadTickets, ComboNombre, TotalPagar FROM dbo.Ventas ORDER BY Id DESC;"
if errorlevel 1 goto :error
goto :fin

:error
echo Error al leer la base. Verifique con 03_verEstadoConexion-03.bat.

:fin
endlocal
pause
