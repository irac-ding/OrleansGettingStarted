#/bin/bash
basepath=$(cd `dirname $0`; pwd)
output=`bash build.sh`
# bash consulRun.sh
sleep 5s
chmod 755 runServer.sh
exec ./runServer.sh
sleep 5s
chmod 755 runServer.sh
exec ./runClient.sh
