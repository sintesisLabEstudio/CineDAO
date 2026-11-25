@echo off
setlocal EnableExtensions
set "BASE_DIR=%~dp0"
set "SQL_FILE=%BASE_DIR%..\scriptsSql\01_Estructura_CineStarDB.sql"
if not exist "%SQL_FILE%" set "SQL_FILE=%BASE_DIR%..\..\scriptsSql\01_Estructura_CineStarDB.sql"

where sqlcmd >nul 2>&1
if errorlevel 1 (
    echo ERROR: no se encontro sqlcmd en el PATH.
    echo Instale SQL Server Command Line Utilities y vuelva a intentar.
    goto :fin
)

if not exist "%SQL_FILE%" (
    echo ERROR: no se encontro el script 01_Estructura_CineStarDB.sql.
    echo Ruta esperada: %SQL_FILE%
    goto :fin
)

echo Creando CineStarDB y la tabla dbo.Ventas si no existen...
sqlcmd -S ".\SQLEXPRESS" -E -b -i "%SQL_FILE%"
if errorlevel 1 (
    echo Error al crear la base o la tabla.
) else (
    echo Base y tabla listas.
)

:fin
endlocal
pause
