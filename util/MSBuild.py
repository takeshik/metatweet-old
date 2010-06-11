###
### MSBuild.py
###
### Usage: lib/ipy util/MSBuild.py <Target> <Configuration> <Platform> <Solution> [32 | 64]
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

file = fxdir.GetFiles("v4.0.30319/MSBuild.exe")[0]

target = sys.argv[1]
configuration = sys.argv[2]
platform = sys.argv[3]
solution = sys.argv[4]

Console.WriteLine("*** MSBuild [ Target = {0}, Configuration = {1}, Platform = {2} ]\n", target, configuration, platform)

if platform == "AnyCPU":
	platform = '"Any CPU"'

info = ProcessStartInfo(
	file.FullName,
	"/target:%(target)s /property:Configuration=%(configuration)s;Platform=%(platform)s /consoleloggerparameters:NoSummary;ShowTimestamp %(solution)s" % locals()
)
info.UseShellExecute = False
proc = Process.Start(info)
proc.WaitForExit()
if proc.ExitCode != 0:
	Console.Write("Press ENTER key to exit.")
	Console.ReadLine()
