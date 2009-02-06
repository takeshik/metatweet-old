@echo off

C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe /target:Build /property:Configuration=Debug /nodeReuse:False ..\..\MetaTweet.sln || (
    echo Press ENTER key to exit.
    pause > nul
)