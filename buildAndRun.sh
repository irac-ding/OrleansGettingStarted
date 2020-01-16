#/bin/bash
basepath=$(cd `dirname $0`; pwd)
output=`bash build.sh`
# bash consulRun.sh
sleep 5s
bash runServer.sh
sleep 5s
bash runClient.sh
