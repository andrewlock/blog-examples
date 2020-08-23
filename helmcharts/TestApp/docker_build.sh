#!/bin/sh

# in case we are being run from outside this directory
cd "$(dirname "$0")"

if [ -z "$*" ]; then 
    echo "You must provide a tag number"
    exit 1;
fi

docker build -f TestApp.Api.Dockerfile . -t andrewlock/my-test-api:$1
docker build -f TestApp.Service.Dockerfile . -t andrewlock/my-test-service:$1