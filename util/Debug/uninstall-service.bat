@echo off

InstallUtil /u ..\..\bin\Debug\MetaTweetHostService.exe || (
	echo Press ENTER key to exit.
	pause > nul
)