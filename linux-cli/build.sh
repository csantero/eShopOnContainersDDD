#!/bin/bash


docker-compose -f ../docker-compose.yml -f ../docker-compose.override.yml build

docker rm $(docker ps -a -q)
