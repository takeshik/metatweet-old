@echo off
cd %~DP0

..\lib\ipy.exe MSBuild.py Rebuild Debug AnyCPU ..\MetaTweetReference\MetaTweetReference.shfbproj 32
