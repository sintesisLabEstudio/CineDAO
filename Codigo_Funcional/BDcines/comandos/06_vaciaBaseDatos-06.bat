@echo off
setlocal EnableExtensions

where sqlcmd >nul 2>&1
if errorlevel 1 (
    echo ERROR: no se encontro sqlcmd en el PATH.
    goto :fin
)

choice /c SN /m "Esto BORRA todas las ventas. Desea continuar"
if errorlevel 2 (
    echo Operacion cancelada.
    goto :fin
)

echo Vaciando dbo.Ventas y reiniciando el contador IDENTITY...
sqlcmd -S ".\SQLEXPRESS" -E -d CineStarDB -b -Q "SET XACT_ABORT ON; BEGIN TRANSACTION; TRUNCATE TABLE dbo.Ventas; DBCC CHECKIDENT ('dbo.Ventas', RESEED, 0); COMMIT TRANSACTION;"
if errorlevel 1 (
    echo Error al vaciar la base. No se aplico ningun cambio.
) else (
    echo Base vaciada y contador reiniciado.
)

:fin
endlocal
pause
