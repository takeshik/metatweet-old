@echo off
cd %~DP0

mkdir ..\..\dist\Debug
cd ..\..\dist\Debug
mkdir bin
mkdir etc
mkdir ja
mkdir lib
mkdir libexec
mkdir var
mkdir var\run
mkdir var\cache

copy /Y ..\..\COPYING
copy /Y ..\..\lib\NOTICE

cd bin
copy /Y ..\..\..\MetaTweetClient\bin\Debug\MetaTweetClient.exe
copy /Y ..\..\..\MetaTweetClient\bin\Debug\MetaTweetClient.pdb
copy /Y ..\..\..\MetaTweetConsole\bin\Debug\MetaTweetConsole.exe
copy /Y ..\..\..\MetaTweetConsole\bin\Debug\MetaTweetConsole.pdb
copy /Y ..\..\..\MetaTweetHostService\bin\Debug\MetaTweetHostService.exe
copy /Y ..\..\..\MetaTweetHostService\bin\Debug\MetaTweetHostService.pdb

cd ..\etc
if not exist suppress_deploy (
    copy /Y ..\..\..\resource\configuration\MetaTweetClient.conf.xml
    copy /Y ..\..\..\resource\configuration\MetaTweetServer.conf.xml
    copy /Y ..\..\..\resource\configuration\log4net.config
    copy /Y ..\..\..\resource\configuration\modules.conf.xml
    mkdir modules.d
    cd modules.d
	copy /Y ..\..\..\..\resource\configuration\LocalServant-local.conf.xml
	copy /Y ..\..\..\..\resource\configuration\RemotingTcpServant-remoting.conf.xml
	copy /Y ..\..\..\..\resource\configuration\RemotingIpcServant-remoting.conf.xml
	copy /Y ..\..\..\..\resource\configuration\SQLiteStorage-main.conf.xml
	copy /Y ..\..\..\..\resource\configuration\SystemInput-sys.conf.xml
	copy /Y ..\..\..\..\resource\configuration\SystemFilter-sys.conf.xml
	copy /Y ..\..\..\..\resource\configuration\SystemOutput-sys.conf.xml
	copy /Y ..\..\..\..\resource\configuration\TwitterApiInput-twitter.conf.xml
	copy /Y ..\..\..\..\resource\configuration\TwitterApiOutput-twitter.conf.xml
	copy /Y ..\..\..\..\resource\configuration\TwitterWebInput-twitter_w.conf.xml
	cd ..
)

cd ..\ja
copy /Y ..\..\..\XSpectWindowsFormsSupplement\bin\Debug\ja\XSpectWindowsFormsSupplement.resources.dll

cd ..\lib
copy /Y ..\..\..\lib\AchiralPlus.dll
copy /Y ..\..\..\lib\log4net.dll
copy /Y ..\..\..\MetaTweetObjectModel\bin\Debug\MetaTweetObjectModel.dll
copy /Y ..\..\..\MetaTweetObjectModel\bin\Debug\MetaTweetObjectModel.pdb
copy /Y ..\..\..\MetaTweetServer\bin\Debug\MetaTweetServer.dll
copy /Y ..\..\..\MetaTweetServer\bin\Debug\MetaTweetServer.pdb
copy /Y ..\..\..\XSpectCommonFramework\bin\Debug\XSpectCommonFramework.dll
copy /Y ..\..\..\XSpectCommonFramework\bin\Debug\XSpectCommonFramework.pdb
copy /Y ..\..\..\XSpectWindowsFormsSupplement\bin\Debug\XSpectWindowsFormsSupplement.dll
copy /Y ..\..\..\XSpectWindowsFormsSupplement\bin\Debug\XSpectWindowsFormsSupplement.pdb

cd ..\libexec

mkdir LocalServant
cd LocalServant
copy /Y ..\..\..\..\LocalServant\bin\Debug\LocalServant.dll
copy /Y ..\..\..\..\LocalServant\bin\Debug\LocalServant.pdb
cd ..

mkdir RemotingServant
cd RemotingServant
copy /Y ..\..\..\..\RemotingServant\bin\Debug\RemotingServant.dll
copy /Y ..\..\..\..\RemotingServant\bin\Debug\RemotingServant.pdb
cd ..

mkdir SQLiteStorage
cd SQLiteStorage
copy /Y ..\..\..\..\SQLiteStorage\bin\Debug\SQLiteStorage.dll
copy /Y ..\..\..\..\SQLiteStorage\bin\Debug\SQLiteStorage.pdb
copy /Y ..\..\..\..\lib\sqlite3.dll
copy /Y ..\..\..\..\lib\System.Data.SQLite.dll
cd ..

mkdir SystemFlow
cd SystemFlow
copy /Y ..\..\..\..\SystemFlow\bin\Debug\SystemFlow.dll
copy /Y ..\..\..\..\SystemFlow\bin\Debug\SystemFlow.pdb
cd ..

mkdir TwitterApiFlow
cd TwitterApiFlow
copy /Y ..\..\..\..\TwitterApiFlow\bin\Debug\TwitterApiFlow.dll
copy /Y ..\..\..\..\TwitterApiFlow\bin\Debug\TwitterApiFlow.pdb
cd ..

mkdir TwitterWebFlow
cd TwitterWebFlow
copy /Y ..\..\..\..\TwitterWebFlow\bin\Debug\TwitterWebFlow.dll
copy /Y ..\..\..\..\TwitterWebFlow\bin\Debug\TwitterWebFlow.pdb
copy /Y ..\..\..\..\lib\TidyNet.dll
