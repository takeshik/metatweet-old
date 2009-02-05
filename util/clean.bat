@echo off

MSBuild /target:Clean ..\MetaTweet.sln || (
	echo Press ENTER key to exit.
	pause > nul
)