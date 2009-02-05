@echo off

MSBuild /target:Rebuild /property:Configuration=Debug /nodeReuse:False ..\..\MetaTweet.sln || (
    echo Press ENTER key to exit.
    pause > nul
)