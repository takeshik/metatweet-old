@echo off
cd %~DP0

"C:\Program Files\EWSoftware\Sandcastle Help File Builder\SandcastleBuilderConsole.exe" ..\..\MetaTweet.shfb || (
	echo Press ENTER key to exit.
	pause > nul
)