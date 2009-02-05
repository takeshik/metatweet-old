@echo off

InstallUtil ..\..\bin\Debug\MetaTweetHostService.exe || (
	echo Press ENTER key to exit.
	pause > nul
)