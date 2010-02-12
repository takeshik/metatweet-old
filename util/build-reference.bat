@echo off
cd %~DP0

..\lib\ipy.exe MSBuild.py Rebuild Release AnyCPU ..\MetaTweetReference\MetaTweetReference.shfbproj 32
