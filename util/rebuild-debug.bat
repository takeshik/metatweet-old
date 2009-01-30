@MSBuild /target:Rebuild /property:Configuration=Debug ../MetaTweet.sln || (
	@ECHO Press ENTER key to exit.
	@PAUSE > NUL
)