@echo off

C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe ..\..\bin\Debug\MetaTweetHostService.exe || (
    echo Press ENTER key to exit.
    pause > nul
)