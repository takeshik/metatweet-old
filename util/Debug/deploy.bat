@echo off
cd %~DP0

mkdir ..\..\dist\Debug
cd ..\..\dist\Debug
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
copy /Y ..\..\..\MetaTweetClient\bin\Debug\MetaTweetClient.exe
copy /Y ..\..\..\MetaTweetClient\bin\Debug\MetaTweetClient.pdb
copy /Y ..\..\..\MetaTweetClient\Properties\App.config MetaTweetClient.exe.config
copy /Y ..\..\..\MetaTweetConsole\bin\Debug\MetaTweetConsole.exe
copy /Y ..\..\..\MetaTweetConsole\bin\Debug\MetaTweetConsole.pdb
copy /Y ..\..\..\MetaTweetConsole\Properties\App.config MetaTweetConsole.exe.config

cd ..\sbin
copy /Y ..\..\..\MetaTweetHostService\bin\Debug\MetaTweetHostService.exe
copy /Y ..\..\..\MetaTweetHostService\bin\Debug\MetaTweetHostService.pdb
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
copy /Y ..\..\..\..\util\Debug-Deployed\*.*