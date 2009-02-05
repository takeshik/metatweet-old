@echo off

mkdir ..\..\bin\Release\
mkdir ..\..\bin\Release\module
mkdir ..\..\bin\Release\cache
cd ..\..\bin\Release
copy /Y ..\..\lib\Achiral.dll
copy /Y ..\..\lib\log4net.dll
copy /Y ..\..\lib\System.Data.SQLite.dll
copy /Y ..\..\lib\TidyNet.dll
copy /Y ..\..\MetaTweetClient\bin\Release\MetaTweetClient.exe
copy /Y ..\..\MetaTweetHostService\bin\Release\MetaTweetHostService.exe
copy /Y ..\..\MetaTweetObjectModel\bin\Release\MetaTweetObjectModel.dll
copy /Y ..\..\MetaTweetServer\bin\Release\MetaTweetServer.dll
copy /Y ..\..\MetaTweetTest\Script\init.cs
copy /Y ..\..\MetaTweetTest\Script\rc.cs
copy /Y ..\..\XSpectCommonFramework\bin\Release\XSpectCommonFramework.dll
cd module
copy /Y ..\..\..\RemotingServant\bin\Release\RemotingServant.dll
copy /Y ..\..\..\SQLiteStorage\bin\Release\SQLiteStorage.dll
copy /Y ..\..\..\TwitterApiFlow\bin\Release\TwitterApiFlow.dll
