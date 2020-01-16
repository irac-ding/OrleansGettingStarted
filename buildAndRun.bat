cd /d %~dp0
start /wait  build.bat
echo "build done"
start  consulRun.bat
TIMEOUT /T 5
start  runServer.bat
TIMEOUT /T 5
start  runClient.bat
start "C:\Program Files\Google\Chrome\Application\chrome.exe" http://localhost:8500/ui