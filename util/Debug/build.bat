@echo off

MSBuild /target:Build /property:Configuration=Debug /nodeReuse:False ..\..\MetaTweet.sln || (
    echo Press ENTER key to exit.
    pause > nul
)