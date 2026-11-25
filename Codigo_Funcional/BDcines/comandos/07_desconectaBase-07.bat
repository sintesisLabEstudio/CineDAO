@echo off
setlocal EnableExtensions

where sqlcmd >nul 2>&1
if errorlevel 1 (
    echo ERROR: no se encontro sqlcmd en el PATH.
    goto :fin
)

echo Desconectando CineStarDB de la instancia SQLEXPRESS...
sqlcmd -S ".\SQLEXPRESS" -E -d master -b -Q "ALTER DATABASE [CineStarDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; EXEC sp_detach_db 'CineStarDB';"
if errorlevel 1 (
    echo Error al desconectar la base.
) else (
    echo Base desconectada. Los archivos .mdf y .ldf se conservan.
)

:fin
endlocal
pause
