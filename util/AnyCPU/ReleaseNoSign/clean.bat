@echo off
cd %~DP0

%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe /target:Clean /property:Configuration=ReleaseNoSign;Platform=AnyCPU /consoleloggerparameters:NoSummary;ShowTimestamp ..\..\..\MetaTweet.sln || (
	echo Press ENTER key to exit.
	pause > nul
)