@echo off

C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe /target:Build /property:Configuration=Release /nodeReuse:False ..\..\MetaTweet.sln || (
    echo Press ENTER key to exit.
    pause > nul
)