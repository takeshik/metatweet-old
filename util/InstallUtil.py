###
### InstallUtil.py
###
### Usage: lib/ipy util/MSBuild.py <Verb> <Configuration> <Platform> <DistDir>
###

import clr
import sys
from System import *
from System.IO import *
from System.Diagnostics import *

fxdir = DirectoryInfo(Path.Combine(
	Environment.GetEnvironmentVariable("WINDIR"),
	"Microsoft.NET"
))

if len(sys.argv) == 6:
	if sys.argv[5] == "32":
		fxdir = fxdir.GetDirectories("Framework")[0]
	elif sys.argv[5] == "64":
		fxdir = fxdir.GetDirectories("Framework64")[0]

if fxdir.Name == "Microsoft.NET":
	if fxdir.GetDirectories("Framework*").Length == 1:
		fxdir = fxdir.GetDirectories("Framework")[0]
	else:
		fxdir = fxdir.GetDirectories("Framework64")[0]

file = fxdir.GetFiles("v2.0.50727/InstallUtil.exe")[0]

target = sys.argv[1]
configuration = sys.argv[2]
platform = sys.argv[3]
dist = sys.argv[4]

if target == "Uninstall":
	opt = "-u"
else:
	opt = ""

Console.WriteLine("*** MSBuild [ Verb = {0}, Configuration = {1}, Platform = {2} ]\n", target, configuration, platform)

info = ProcessStartInfo(
	file.FullName,
	"%(opt)s %(dist)s/%(platform)s/%(configuration)s/sbin/MetaTweetHostService.exe" % locals()
)
info.UseShellExecute = False
proc = Process.Start(info)
proc.WaitForExit()
if proc.ExitCode != 0:
	Console.Write("Press ENTER key to exit.")
	Console.ReadLine()
