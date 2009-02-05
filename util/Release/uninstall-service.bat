@echo off

InstallUtil /u ..\..\bin\Release\MetaTweetHostService.exe || (
	echo Press ENTER key to exit.
	pause > nul
)