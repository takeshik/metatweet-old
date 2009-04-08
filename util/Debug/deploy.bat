@echo off
cd %~DP0

mkdir ..\..\bin\Debug\
mkdir ..\..\bin\Debug\module
mkdir ..\..\bin\Debug\conf
mkdir ..\..\bin\Debug\temp
mkdir ..\..\bin\Debug\ja
cd ..\..\bin\Debug
copy /Y ..\..\COPYING
copy /Y ..\..\lib\NOTICE
copy /Y ..\..\lib\Achiral.dll
copy /Y ..\..\lib\log4net.dll
copy /Y ..\..\lib\sqlite3.dll
copy /Y ..\..\lib\System.Data.SQLite.dll
copy /Y ..\..\MetaTweetClient\bin\Debug\MetaTweetClient.exe
copy /Y ..\..\MetaTweetClient\bin\Debug\MetaTweetClient.pdb
copy /Y ..\..\MetaTweetClient\Properties\MetaTweetClient.conf.xml
copy /Y ..\..\MetaTweetHostService\bin\Debug\MetaTweetHostService.exe
copy /Y ..\..\MetaTweetHostService\bin\Debug\MetaTweetHostService.pdb
copy /Y ..\..\MetaTweetObjectModel\bin\Debug\MetaTweetObjectModel.dll
copy /Y ..\..\MetaTweetObjectModel\bin\Debug\MetaTweetObjectModel.pdb
copy /Y ..\..\MetaTweetServer\bin\Debug\MetaTweetServer.dll
copy /Y ..\..\MetaTweetServer\bin\Debug\MetaTweetServer.pdb
copy /Y ..\..\MetaTweetServer\Properties\log4net.config
copy /Y ..\..\XSpectCommonFramework\bin\Debug\XSpectCommonFramework.dll
copy /Y ..\..\XSpectCommonFramework\bin\Debug\XSpectCommonFramework.pdb
copy /Y ..\..\XSpectWindowsFormsSupplement\bin\Debug\XSpectWindowsFormsSupplement.dll
copy /Y ..\..\XSpectWindowsFormsSupplement\bin\Debug\XSpectWindowsFormsSupplement.pdb
cd ja
copy /Y ..\..\..\XSpectWindowsFormsSupplement\bin\Debug\ja\XSpectWindowsFormsSupplement.resources.dll
cd ..\module
copy /Y ..\..\..\LocalServant\bin\Debug\LocalServant.dll
copy /Y ..\..\..\LocalServant\bin\Debug\LocalServant.pdb
copy /Y ..\..\..\RemotingServant\bin\Debug\RemotingServant.dll
copy /Y ..\..\..\RemotingServant\bin\Debug\RemotingServant.pdb
copy /Y ..\..\..\SQLiteStorage\bin\Debug\SQLiteStorage.dll
copy /Y ..\..\..\SQLiteStorage\bin\Debug\SQLiteStorage.pdb
copy /Y ..\..\..\SystemFlow\bin\Debug\SystemFlow.dll
copy /Y ..\..\..\SystemFlow\bin\Debug\SystemFlow.pdb
copy /Y ..\..\..\TwitterApiFlow\bin\Debug\TwitterApiFlow.dll
copy /Y ..\..\..\TwitterApiFlow\bin\Debug\TwitterApiFlow.pdb
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
