#!/bin/sh

# https://superuser.com/a/1299463
die_func() {
    echo "Terminating"
    exit 1
}

trap die_func TERM

echo "Sleeping..."
# restarts once a day
sleep 86400 &
wait