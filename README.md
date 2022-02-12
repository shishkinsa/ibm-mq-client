# Установка тестового сервера в Docker
### Шаг1: docker pull docker.io/ibmcom/mq:latest
### Шаг2: docker volume create qm1data
### Шаг3: docker run --env LICENSE=accept --env MQ_QMGR_NAME=QM1 --volume qm1data:/mnt/mqm --publish 1414:1414 --publish 9443:9443 --detach --env MQ_APP_PASSWORD=passw0rd ibmcom/mq:latest
## Проверка работы сервера
#### docker exec -it (your container id) /bin/bash
#### Вводим: dspmqver
### Просмотр статуса очереди
#### dspmq