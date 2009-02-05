@MSBuild /target:Rebuild /property:Configuration=Release /nodeReuse:False ../../MetaTweet.sln || (
	@ECHO Press ENTER key to exit.
	@PAUSE > NUL
)