@echo off

MSBuild /target:Build /property:Configuration=Release /nodeReuse:False ..\..\MetaTweet.sln || (
    echo Press ENTER key to exit.
    pause > nul
)