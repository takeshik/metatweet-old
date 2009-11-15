@echo off
cd %~DP0

%WINDIR%\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe ..\..\..\dist\x86\ReleaseNoSign\sbin\MetaTweetHostService.exe || (
	echo Press ENTER key to exit.
	pause > nul
)