#!/bin/bash
#gnome-terminal -- sudo docker-compose up --build sqlserver
#gnome-terminal --window-with-profile=Mathieu -- sudo docker-compose up --build config
#gnome-terminal  -- sudo docker-compose up --build app
#sleep 60
#gnome-terminal -- sudo docker inspect   -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' stocks-app-1
sudo docker-compose up --build config
sleep 100
sudo docker-compose up --build app