@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild ReleaseNoSign AnyCPU ..\..\..\MetaTweet.sln
