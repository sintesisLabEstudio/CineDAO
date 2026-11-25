@echo off
setlocal EnableExtensions
set "RESPALDO_DIR=%~dp0respaldo"
set "BACKUP_FILE=%RESPALDO_DIR%\CineStarDB_Backup.bak"

where sqlcmd >nul 2>&1
if errorlevel 1 (
    echo ERROR: no se encontro sqlcmd en el PATH.
    goto :fin
)

if not exist "%RESPALDO_DIR%" mkdir "%RESPALDO_DIR%"
if exist "%BACKUP_FILE%" (
    choice /c SN /m "Ya existe un respaldo. Desea sobrescribirlo"
    if errorlevel 2 (
        echo Respaldo cancelado.
        goto :fin
    )
)

echo Generando respaldo en %BACKUP_FILE% ...
sqlcmd -S ".\SQLEXPRESS" -E -b -Q "BACKUP DATABASE [CineStarDB] TO DISK = N'%BACKUP_FILE%' WITH INIT;"
if errorlevel 1 (
    echo Error en el respaldo.
) else (
    echo Respaldo realizado correctamente.
)

:fin
endlocal
pause
