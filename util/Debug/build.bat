@MSBuild /target:Build /property:Configuration=Debug /nodeReuse:False ../../MetaTweet.sln || (
	@ECHO Press ENTER key to exit.
	@PAUSE > NUL
)