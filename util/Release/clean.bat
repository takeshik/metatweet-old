@echo off

C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe /target:Clean /property:Configuration=Release ..\..\MetaTweet.sln || (
	echo Press ENTER key to exit.
	pause > nul
)