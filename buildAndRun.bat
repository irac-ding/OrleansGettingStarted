cd /d %~dp0
start /wait  build.bat
echo "build done"
start  runServer.bat
TIMEOUT /T 5
start  runClient.bat
