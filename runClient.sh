#/bin/bash
basepath=$(cd `dirname $0`; pwd)
dotnet run --project ./src/Kritner.OrleansGettingStarted.Client --no-build 