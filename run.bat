@ECHO OFF

IF "%CONFIGURATION%"=="" SET CONFIGURATION=Debug

star %* --resourcedir="%~dp0src\ShoppingCart\wwwroot" "%~dp0src/ShoppingCart/bin/%CONFIGURATION%/ShoppingCart.exe"