@echo off
setlocal EnableExtensions
set "BASE_DIR=%~dp0"
set "SQL_FILE=%BASE_DIR%..\scriptsSql\02_DatosDemo_CineStarDB.sql"
if not exist "%SQL_FILE%" set "SQL_FILE=%BASE_DIR%..\..\scriptsSql\02_DatosDemo_CineStarDB.sql"

where sqlcmd >nul 2>&1
if errorlevel 1 (
    echo ERROR: no se encontro sqlcmd en el PATH.
    goto :fin
)

if not exist "%SQL_FILE%" (
    echo ERROR: no se encontro el script 02_DatosDemo_CineStarDB.sql.
    goto :fin
)

echo Asegurando modo MULTI_USER...
sqlcmd -S ".\SQLEXPRESS" -E -d master -b -Q "ALTER DATABASE [CineStarDB] SET MULTI_USER WITH ROLLBACK IMMEDIATE;"
if errorlevel 1 goto :fin

echo Cargando datos demo (solo si la tabla esta vacia)...
sqlcmd -S ".\SQLEXPRESS" -E -d CineStarDB -b -i "%SQL_FILE%"
if errorlevel 1 (
    echo Error al cargar los datos. Ejecute antes 01_creaBaseDatosCine-01.bat.
) else (
    echo Datos demo cargados.
)

:fin
endlocal
pause
