#!/bin/bash
set -euo pipefail

# Required Variables: 
[ -z "$CHART" ] && echo "Need to set CHART" && exit 1;
[ -z "$RELEASE_NAME" ] && echo "Need to set RELEASE_NAME" && exit 1;
[ -z "$NAMESPACE" ] && echo "Need to set NAMESPACE" && exit 1;

# Set the helm context to the same as the kubectl context
KUBE_CONTEXT=$(kubectl config current-context)

# Install/upgrade the chart
helm upgrade --install \
  $RELEASE_NAME \
  $CHART \
  --kube-context "${KUBE_CONTEXT}" \
  --namespace="$NAMESPACE" \
  $HELM_ARGS

echo 'LOG: Watching for successful release...'

# Timeout after 6 repeats = 60 seconds
release_timeout=6
counter=0

# Loop while $counter < $release_timeout
while [ $counter -lt $release_timeout ]; do
    # Fetch a list of release names
    releases="$(helm ls -q --kube-context "${KUBE_CONTEXT}")"

    # Check if $releases contains RELEASE_NAME
    if ! echo "${releases}" | grep -qF "${RELEASE_NAME}"; then

        echo "${releases}"
        echo "LOG: ${RELEASE_NAME} not found. ${counter}/${release_timeout} checks completed; retrying."

        # NOTE: The pre-increment usage. This makes the arithmatic expression
        # always exit 0. The post-increment form exits non-zero when counter
        # is zero. More information here: http://wiki.bash-hackers.org/syntax/arith_expr#arithmetic_expressions_and_return_codes
        ((++counter))
        sleep 10
    else
        # Our release is there, we can stop checking
        break
    fi
done

if [ $counter -eq $release_timeout ]; then
    echo "LOG: ${RELEASE_NAME} failed to appear." 1>&2
    exit 1
fi


# Timeout after 20 mins (to leave time for migrations)
timeout=120
counter=0

# While $counter < $timeout
while [ $counter -lt $timeout ]; do
    
    # Fetch all pods tagged with the release
    release_pods="$(kubectl get pods \
        -l "app.kubernetes.io/instance=${RELEASE_NAME}" \
        -o 'custom-columns=NAME:.metadata.name,STATUS:.status.phase' \
        -n "${NAMESPACE}" \
        --context "${KUBE_CONTEXT}" \
        --no-headers \
    )"
    
    # If we have any failures, then the release failed
    if echo "${release_pods}" | grep -qE 'Failed'; then
      echo "LOG: ${RELEASE_NAME} failed. Check the pod logs."
      exit 1
    fi

    # Are any of the pods _not_ in the Running/Succeeded status?
    if echo "${release_pods}" | grep -qvE 'Running|Succeeded'; then

        echo "${release_pods}" | grep -vE 'Running|Succeeded'
        echo "${RELEASE_NAME} pods not ready. ${counter}/${timeout} checks completed; retrying."

        # NOTE: The pre-increment usage. This makes the arithmatic expression
        # always exit 0. The post-increment form exits non-zero when counter
        # is zero. More information here: http://wiki.bash-hackers.org/syntax/arith_expr#arithmetic_expressions_and_return_codes
        ((++counter))
        sleep 10
    else
        #All succeeded, we're done!
        echo "${release_pods}"
        echo "LOG: All ${RELEASE_NAME} pods running. Done!"
        exit 0
    fi
done

# We timed out
echo "LOG: Release ${RELEASE_NAME} did not complete in time" 1>&2
exit 1