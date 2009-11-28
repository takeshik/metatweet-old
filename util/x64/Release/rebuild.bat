@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild Release x64 ..\..\..\MetaTweet.sln
