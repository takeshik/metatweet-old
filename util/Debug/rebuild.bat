@MSBuild /target:Rebuild /property:Configuration=Debug /nodeReuse:False ../../MetaTweet.sln || (
	@ECHO Press ENTER key to exit.
	@PAUSE > NUL
)