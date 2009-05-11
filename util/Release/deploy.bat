@echo off
cd %~DP0

mkdir ..\..\bin\Release\
mkdir ..\..\bin\Release\module
mkdir ..\..\bin\Release\conf
mkdir ..\..\bin\Release\temp
mkdir ..\..\bin\Release\ja
cd ..\..\bin\Release
copy /Y ..\..\COPYING
copy /Y ..\..\lib\NOTICE
copy /Y ..\..\lib\AchiralPlus.dll
copy /Y ..\..\lib\log4net.dll
copy /Y ..\..\lib\sqlite3.dll
copy /Y ..\..\lib\System.Data.SQLite.dll
copy /Y ..\..\lib\TidyNet.dll
copy /Y ..\..\MetaTweetClient\bin\Release\MetaTweetClient.exe
copy /Y ..\..\MetaTweetClient\Properties\MetaTweetClient.conf.xml
copy /Y ..\..\MetaTweetHostService\bin\Release\MetaTweetHostService.exe
copy /Y ..\..\MetaTweetObjectModel\bin\Release\MetaTweetObjectModel.dll
copy /Y ..\..\MetaTweetServer\bin\Release\MetaTweetServer.dll
copy /Y ..\..\MetaTweetServer\Properties\log4net.config
copy /Y ..\..\XSpectCommonFramework\bin\Release\XSpectCommonFramework.dll
copy /Y ..\..\XSpectWindowsFormsSupplement\bin\Release\XSpectWindowsFormsSupplement.dll
cd ja
copy /Y ..\..\..\XSpectWindowsFormsSupplement\bin\Release\ja\XSpectWindowsFormsSupplement.resources.dll
cd ..\module
copy /Y ..\..\..\LocalServant\bin\Release\LocalServant.dll
copy /Y ..\..\..\RemotingServant\bin\Release\RemotingServant.dll
copy /Y ..\..\..\SQLiteStorage\bin\Release\SQLiteStorage.dll
copy /Y ..\..\..\SystemFlow\bin\Release\SystemFlow.dll
copy /Y ..\..\..\TwitterApiFlow\bin\Release\TwitterApiFlow.dll
copy /Y ..\..\..\TwitterWebFlow\bin\Release\TwitterWebFlow.dll
cd ..\conf
if not exist suppress_deploy (
	copy /Y ..\..\..\resource\configuration\_modules.conf.xml
	copy /Y ..\..\..\resource\configuration\LocalServant-local.conf.xml
	copy /Y ..\..\..\resource\configuration\RemotingTcpServant-remoting.conf.xml
	copy /Y ..\..\..\resource\configuration\RemotingIpcServant-remoting.conf.xml
	copy /Y ..\..\..\resource\configuration\SQLiteStorage-main.conf.xml
	copy /Y ..\..\..\resource\configuration\SystemInput-sys.conf.xml
	copy /Y ..\..\..\resource\configuration\SystemFilter-sys.conf.xml
	copy /Y ..\..\..\resource\configuration\SystemOutput-sys.conf.xml
	copy /Y ..\..\..\resource\configuration\TwitterApiInput-twitter.conf.xml
	copy /Y ..\..\..\resource\configuration\TwitterApiOutput-twitter.conf.xml
	copy /Y ..\..\..\resource\configuration\TwitterWebInput-twitter_w.conf.xml
)
