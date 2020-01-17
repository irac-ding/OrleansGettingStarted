cd /d %~dp0
start /wait  build.bat
echo "build done"
start  runServer.bat
TIMEOUT /T 10
start  runClient.bat
start "C:\Program Files\Google\Chrome\Application\chrome.exe" http://localhost:8500/ui
start "C:\Program Files\Google\Chrome\Application\chrome.exe" http://localhost:8080/
