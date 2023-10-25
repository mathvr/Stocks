#!/bin/bash
gnome-terminal --window-with-profile=Mathieu -- docker-compose up --build sqlserver
gnome-terminal --window-with-profile=Mathieu -- docker-compose up -d --build app
sleep 60
gnome-terminal --window-with-profile=Mathieu -- systemctl status docker

