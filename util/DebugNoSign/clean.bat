@echo off
cd %~DP0

C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe /target:Clean /property:Configuration=DebugNoSign /consoleloggerparameters:NoSummary;ShowTimestamp ..\..\MetaTweet.sln || (
	echo Press ENTER key to exit.
	pause > nul
)