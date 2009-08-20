@echo off
cd %~DP0

%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe /target:Rebuild /property:Configuration=ReleaseNoSign /consoleloggerparameters:NoSummary;ShowTimestamp /nodeReuse:False ..\..\MetaTweet.sln || (
    echo Press ENTER key to exit.
    pause > nul
)