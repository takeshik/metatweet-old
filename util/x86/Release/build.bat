@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Build Release x86 ..\..\..\MetaTweet.sln
