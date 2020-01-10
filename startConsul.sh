#! /bin/bash
consul agent -ui -server -bootstrap -data-dir "/data/Consul/Data" -client=0.0.0.0