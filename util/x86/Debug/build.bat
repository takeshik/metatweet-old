@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Build Debug x86 ..\..\..\MetaTweet.sln
