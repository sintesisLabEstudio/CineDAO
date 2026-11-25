@echo off
setlocal EnableExtensions

where sqlcmd >nul 2>&1
if errorlevel 1 (
    echo ERROR: no se encontro sqlcmd en el PATH.
    goto :fin
)

echo Comprobando estado de CineStarDB...
sqlcmd -S ".\SQLEXPRESS" -E -d master -b -W -Q "SELECT name AS BaseDatos, state_desc AS Estado, user_access_desc AS Acceso FROM sys.databases WHERE name = 'CineStarDB';"

echo Cantidad de registros:
sqlcmd -S ".\SQLEXPRESS" -E -d CineStarDB -b -W -Q "SELECT COUNT(*) AS Ventas FROM dbo.Ventas;"

:fin
endlocal
pause
