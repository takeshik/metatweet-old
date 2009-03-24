@echo off
cd %~DP0

mkdir ..\..\bin\Release\
mkdir ..\..\bin\Release\module
mkdir ..\..\bin\Release\conf
mkdir ..\..\bin\Release\temp
cd ..\..\bin\Release
copy /Y ..\..\COPYING
copy /Y ..\..\lib\NOTICE
copy /Y ..\..\lib\Achiral.dll
copy /Y ..\..\lib\log4net.dll
copy /Y ..\..\lib\sqlite3.dll
copy /Y ..\..\lib\System.Data.SQLite.dll
copy /Y ..\..\lib\TidyNet.dll
copy /Y ..\..\MetaTweetClient\bin\Release\MetaTweetClient.exe
copy /Y ..\..\MetaTweetHostService\bin\Release\MetaTweetHostService.exe
copy /Y ..\..\MetaTweetObjectModel\bin\Release\MetaTweetObjectModel.dll
copy /Y ..\..\MetaTweetServer\bin\Release\MetaTweetServer.dll
copy /Y ..\..\MetaTweetServer\Properties\log4net.config
copy /Y ..\..\XSpectCommonFramework\bin\Release\XSpectCommonFramework.dll
cd module
copy /Y ..\..\..\LocalServant\bin\Release\LocalServant.dll
copy /Y ..\..\..\RemotingServant\bin\Release\RemotingServant.dll
copy /Y ..\..\..\SQLiteStorage\bin\Release\SQLiteStorage.dll
copy /Y ..\..\..\SystemFlow\bin\Release\SystemFlow.dll
copy /Y ..\..\..\TwitterApiFlow\bin\Release\TwitterApiFlow.dll
cd ..\conf
if not exist suppress_deploy (
	copy /Y ..\..\..\MetaTweetTest\Sample\_modules.conf.xml
	copy /Y ..\..\..\MetaTweetTest\Sample\LocalServant-local.conf.xml
	copy /Y ..\..\..\MetaTweetTest\Sample\RemotingTcpServant-remoting.conf.xml
	copy /Y ..\..\..\MetaTweetTest\Sample\RemotingIpcServant-remoting.conf.xml
	copy /Y ..\..\..\MetaTweetTest\Sample\SQLiteStorage-main.conf.xml
	copy /Y ..\..\..\MetaTweetTest\Sample\SystemInput-sys.conf.xml
	copy /Y ..\..\..\MetaTweetTest\Sample\SystemFilter-sys.conf.xml
	copy /Y ..\..\..\MetaTweetTest\Sample\SystemOutput-sys.conf.xml
	copy /Y ..\..\..\MetaTweetTest\Sample\TwitterApiInput-twitter.conf.xml
	copy /Y ..\..\..\MetaTweetTest\Sample\TwitterApiOutput-twitter.conf.xml
)
