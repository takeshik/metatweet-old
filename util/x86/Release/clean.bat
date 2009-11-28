@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean Release x86 ..\..\..\MetaTweet.sln
