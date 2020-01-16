#! /bin/bash
basepath=$(cd `dirname $0`; pwd)
consul agent -ui -server -bootstrap -data-dir "/data/Consul/Data" -client=0.0.0.0