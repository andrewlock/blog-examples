#!/bin/bash
set -euo pipefail

# Change to this directory if you're running outside the directory
cd "$(dirname "$0")"

# Build the current docker images
./docker_build.sh $*

# Deploy and wait for success
CHART="./charts/test-app" \
RELEASE_NAME="my-test-app-release" \
NAMESPACE="local" \
HELM_ARGS="--set test-app-cli.image.tag=$1 \
  --set test-app-cli-exec-host.image.tag=$1 \
  --set test-app-api.image.tag=$1 \
  --set test-app-service.image.tag=$1 \
" \
./deploy_and_wait.sh