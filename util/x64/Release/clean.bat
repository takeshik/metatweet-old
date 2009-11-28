@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean Release x64 ..\..\..\MetaTweet.sln
