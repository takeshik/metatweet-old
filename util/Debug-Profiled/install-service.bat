@echo off
cd %~DP0

C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe ..\..\bin\Debug-Profiled\MetaTweetHostService.exe || (
    echo Press ENTER key to exit.
    pause > nul
)