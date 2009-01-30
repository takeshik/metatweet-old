@MSBuild /target:Build /property:Configuration=Release ../MetaTweet.sln || (
	@ECHO Press ENTER key to exit.
	@PAUSE > NUL
)
