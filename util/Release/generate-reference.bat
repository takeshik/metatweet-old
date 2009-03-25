@echo off
cd %~DP0

C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe  ..\..\MetaTweetReference\MetaTweetReference.shfbproj || (
	echo Press ENTER key to exit.
	pause > nul
)