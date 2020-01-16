#/bin/bash
basepath=$(cd `dirname $0`; pwd)
output=`bash build.sh`
# bash consulRun.sh
sleep 5s
exec ./runServer.sh
sleep 5s
exec ./runClient.sh
