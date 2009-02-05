@echo off

SandcastleBuilderConsole ..\MetaTweet.shfb || (
	echo Press ENTER key to exit.
	pause > nul
)