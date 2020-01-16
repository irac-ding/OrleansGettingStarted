#/bin/bash
basepath=$(cd `dirname $0`; pwd)
dotnet restore
dotnet build --no-restore