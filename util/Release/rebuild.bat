@echo off

MSBuild /target:Rebuild /property:Configuration=Release /nodeReuse:False ..\..\MetaTweet.sln || (
    echo Press ENTER key to exit.
    pause > nul
)