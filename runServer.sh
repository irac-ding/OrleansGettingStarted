#/bin/bash
basepath=$(cd `dirname $0`; pwd)
dotnet run --project ./src/Kritner.OrleansGettingStarted.SiloHost --no-build 