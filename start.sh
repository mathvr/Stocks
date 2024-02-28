#!/bin/bash
gnome-terminal --window-with-profile=Mathieu -- sudo docker-compose up --build sqlserver
gnome-terminal --window-with-profile=Mathieu -- sudo docker-compose up --build config
gnome-terminal --window-with-profile=Mathieu -- sudo docker-compose up --build app
sleep 60
gnome-terminal --window-with-profile=Mathieu -- sudo docker inspect   -f '{{range.NetworkSettings.Networks}}{{.IPAddress}}{{end}}' stocks-app-1

