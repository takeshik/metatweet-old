@echo off

mkdir ..\..\bin\Debug\
mkdir ..\..\bin\Debug\module
mkdir ..\..\bin\Debug\cache
cd ..\..\bin\Debug
copy /Y ..\..\lib\Achiral.dll
copy /Y ..\..\lib\log4net.dll
copy /Y ..\..\lib\System.Data.SQLite.dll
copy /Y ..\..\lib\TidyNet.dll
copy /Y ..\..\MetaTweetClient\bin\Debug\MetaTweetClient.exe
copy /Y ..\..\MetaTweetClient\bin\Debug\MetaTweetClient.pdb
copy /Y ..\..\MetaTweetHostService\bin\Debug\MetaTweetHostService.exe
copy /Y ..\..\MetaTweetHostService\bin\Debug\MetaTweetHostService.pdb
copy /Y ..\..\MetaTweetObjectModel\bin\Debug\MetaTweetObjectModel.dll
copy /Y ..\..\MetaTweetObjectModel\bin\Debug\MetaTweetObjectModel.pdb
copy /Y ..\..\MetaTweetServer\bin\Debug\MetaTweetServer.dll
copy /Y ..\..\MetaTweetServer\bin\Debug\MetaTweetServer.pdb
copy /Y ..\..\MetaTweetTest\Script\init.cs
copy /Y ..\..\MetaTweetTest\Script\rc.cs
copy /Y ..\..\XSpectCommonFramework\bin\Debug\XSpectCommonFramework.dll
copy /Y ..\..\XSpectCommonFramework\bin\Debug\XSpectCommonFramework.pdb
cd module
copy /Y ..\..\..\LocalServant\bin\Debug\LocalServant.dll
copy /Y ..\..\..\LocalServant\bin\Debug\LocalServant.pdb
copy /Y ..\..\..\RemotingServant\bin\Debug\RemotingServant.dll
copy /Y ..\..\..\RemotingServant\bin\Debug\RemotingServant.pdb
copy /Y ..\..\..\SQLiteStorage\bin\Debug\SQLiteStorage.dll
copy /Y ..\..\..\SQLiteStorage\bin\Debug\SQLiteStorage.pdb
copy /Y ..\..\..\StorageFlow\bin\Debug\StorageFlow.dll
copy /Y ..\..\..\StorageFlow\bin\Debug\StorageFlow.pdb
copy /Y ..\..\..\TwitterApiFlow\bin\Debug\TwitterApiFlow.dll
copy /Y ..\..\..\TwitterApiFlow\bin\Debug\TwitterApiFlow.pdb
