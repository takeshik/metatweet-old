@echo off
cd %~DP0

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe ..\..\sbin\MetaTweetHostService.exe || (
    echo Press ENTER key to exit.
    pause > nul
)