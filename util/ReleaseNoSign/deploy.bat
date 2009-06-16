@echo off
cd %~DP0

mkdir ..\..\dist\Release
cd ..\..\dist\Release
mkdir bin
mkdir sbin
mkdir etc
mkdir ja
mkdir lib
mkdir libexec
mkdir share
mkdir var
mkdir var\run
mkdir var\cache

copy /Y ..\..\COPYING
copy /Y ..\..\lib\NOTICE

cd bin
copy /Y ..\..\..\MetaTweetClient\bin\Release\MetaTweetClient.exe
copy /Y ..\..\..\MetaTweetClient\Properties\App.config MetaTweetClient.exe.config
copy /Y ..\..\..\MetaTweetConsole\bin\Release\MetaTweetConsole.exe
copy /Y ..\..\..\MetaTweetConsole\Properties\App.config MetaTweetConsole.exe.config

cd ..\sbin
copy /Y ..\..\..\MetaTweetHostService\bin\Release\MetaTweetHostService.exe
copy /Y ..\..\..\MetaTweetHostService\Properties\App.config MetaTweetHostService.exe.config

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
copy /Y ..\..\..\XSpectWindowsFormsSupplement\bin\Release\ja\XSpectWindowsFormsSupplement.resources.dll

cd ..\lib
copy /Y ..\..\..\lib\AchiralPlus.dll
copy /Y ..\..\..\lib\log4net.dll
copy /Y ..\..\..\MetaTweetObjectModel\bin\Release\MetaTweetObjectModel.dll
copy /Y ..\..\..\MetaTweetServer\bin\Release\MetaTweetServer.dll
copy /Y ..\..\..\XSpectCommonFramework\bin\Release\XSpectCommonFramework.dll
copy /Y ..\..\..\XSpectWindowsFormsSupplement\bin\Release\XSpectWindowsFormsSupplement.dll

cd ..\libexec

mkdir LocalServant
cd LocalServant
copy /Y ..\..\..\..\LocalServant\bin\Release\LocalServant.dll
cd ..

mkdir RemotingServant
cd RemotingServant
copy /Y ..\..\..\..\RemotingServant\bin\Release\RemotingServant.dll
cd ..

mkdir SQLiteStorage
cd SQLiteStorage
copy /Y ..\..\..\..\SQLiteStorage\bin\Release\SQLiteStorage.dll
copy /Y ..\..\..\..\lib\sqlite3.dll
copy /Y ..\..\..\..\lib\System.Data.SQLite.dll
cd ..

mkdir SystemFlow
cd SystemFlow
copy /Y ..\..\..\..\SystemFlow\bin\Release\SystemFlow.dll
cd ..

mkdir TwitterApiFlow
cd TwitterApiFlow
copy /Y ..\..\..\..\TwitterApiFlow\bin\Release\TwitterApiFlow.dll
cd ..

mkdir TwitterWebFlow
cd TwitterWebFlow
copy /Y ..\..\..\..\TwitterWebFlow\bin\Release\TwitterWebFlow.dll
copy /Y ..\..\..\..\lib\TidyNet.dll
cd ..

cd ..\share
copy /Y ..\..\..\resource\COPYING

mkdir doc
cd doc
copy /Y ..\..\..\..\resource\reference\MetaTweetLibs.chm
copy /Y ..\..\..\..\resource\specification\MetaTweetObjectModel.png
copy /Y ..\..\..\..\resource\specification\MetaTweetServer.png
cd ..

mkdir misc\logo
cd misc\logo
copy /Y ..\..\..\..\..\resource\logo\*.*
cd ..\..

mkdir util
cd util\
copy /Y ..\..\..\..\util\Release-Deployed\*.*