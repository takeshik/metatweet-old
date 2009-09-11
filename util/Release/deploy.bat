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
copy /Y ..\..\COPYING.ja
copy /Y ..\..\lib\NOTICE
copy /Y ..\..\resource\manual\README.dist README
copy /Y ..\..\resource\manual\README.dist.ja README.ja
copy /Y ..\..\COPYING

cd bin
copy /Y ..\..\..\MetaTweetMint\bin\Release\MetaTweetMint.exe
copy /Y ..\..\..\MetaTweetMint\Properties\App.config MetaTweetMint.exe.config

cd ..\sbin
copy /Y ..\..\..\MetaTweetHostService\bin\Release\MetaTweetHostService.exe
copy /Y ..\..\..\MetaTweetHostService\Properties\App.config MetaTweetHostService.exe.config

cd ..\etc
if not exist suppress_deploy (
    mkdir Mint
    cd Mint
    copy /Y ..\..\..\..\resource\configuration\MetaTweetMint.conf.xml
    copy /Y ..\..\..\..\resource\configuration\MetaTweetMint.conf.default
    cd ..
    copy /Y ..\..\..\resource\configuration\MetaTweet.conf.xml
    copy /Y ..\..\..\resource\configuration\MetaTweet.conf.default
    copy /Y ..\..\..\resource\configuration\MetaTweetServer.conf.xml
    copy /Y ..\..\..\resource\configuration\MetaTweetServer.conf.default
    copy /Y ..\..\..\resource\configuration\MetaTweetServer.modules.conf.xml
    copy /Y ..\..\..\resource\configuration\MetaTweetServer.modules.conf.default
    copy /Y ..\..\..\resource\configuration\ModuleManager.conf.xml
    copy /Y ..\..\..\resource\configuration\ModuleManager.conf.default
    copy /Y ..\..\..\resource\configuration\log4net.config
    mkdir modules.d
    cd modules.d
    copy /Y ..\..\..\..\resource\configuration\LocalServant-local.conf.xml
    copy /Y ..\..\..\..\resource\configuration\LocalServant-local.conf.default
    copy /Y ..\..\..\..\resource\configuration\RemotingTcpServant-remoting.conf.xml
    copy /Y ..\..\..\..\resource\configuration\RemotingTcpServant-remoting.conf.default
    copy /Y ..\..\..\..\resource\configuration\RemotingIpcServant-remoting.conf.xml
    copy /Y ..\..\..\..\resource\configuration\RemotingIpcServant-remoting.conf.default
    copy /Y ..\..\..\..\resource\configuration\SQLiteStorage-main.conf.xml
    copy /Y ..\..\..\..\resource\configuration\SQLiteStorage-main.conf.default
    copy /Y ..\..\..\..\resource\configuration\SystemInput-sys.conf.xml
    copy /Y ..\..\..\..\resource\configuration\SystemInput-sys.conf.default
    copy /Y ..\..\..\..\resource\configuration\SystemFilter-sys.conf.xml
    copy /Y ..\..\..\..\resource\configuration\SystemFilter-sys.conf.default
    copy /Y ..\..\..\..\resource\configuration\SystemOutput-sys.conf.xml
    copy /Y ..\..\..\..\resource\configuration\SystemOutput-sys.conf.default
    copy /Y ..\..\..\..\resource\configuration\TwitterApiInput-twitter.conf.xml
    copy /Y ..\..\..\..\resource\configuration\TwitterApiInput-twitter.conf.default
    copy /Y ..\..\..\..\resource\configuration\TwitterApiOutput-twitter.conf.xml
    copy /Y ..\..\..\..\resource\configuration\TwitterApiOutput-twitter.conf.default
    copy /Y ..\..\..\..\resource\configuration\TwitterWebInput-twitter_w.conf.xml
    copy /Y ..\..\..\..\resource\configuration\TwitterWebInput-twitter_w.conf.default
    copy /Y ..\..\..\..\resource\configuration\TwitterWebInput-twitter_w.scrapingKeys.conf.xml
    copy /Y ..\..\..\..\resource\configuration\TwitterWebInput-twitter_w.scrapingKeys.conf.default
    cd ..
)

cd ..\ja
copy /Y ..\..\..\XSpectWindowsFormsSupplement\bin\Release\ja\XSpectWindowsFormsSupplement.resources.dll

cd ..\lib
copy /Y ..\..\..\lib\AchiralPlus.dll
copy /Y ..\..\..\lib\IronPython.dll
copy /Y ..\..\..\lib\IronPython.Modules.dll
copy /Y ..\..\..\lib\Microsoft.Scripting.dll
copy /Y ..\..\..\lib\Microsoft.Scripting.Core.dll
copy /Y ..\..\..\lib\log4net.dll
copy /Y ..\..\..\lib\WeifenLuo.WinFormsUI.Docking.dll
copy /Y ..\..\..\MetaTweetObjectModel\bin\Release\MetaTweetObjectModel.dll
copy /Y ..\..\..\MetaTweetServer\bin\Release\MetaTweetServer.dll
copy /Y ..\..\..\Linx\Linx\bin\Debug\Linx.dll
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