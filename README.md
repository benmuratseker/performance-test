# performance-test
performance test for automation-testing

## running grafana and influxdb
docker pull influxdb:1.8.1

docker pull grafana/grafana:8.5.2

docker compose up --detach (in the performance test folder)

docker compose down
