#!/bin/bash

# Cuando se hace un commit desde producción, con este archivo automatizara
# procesos típicos para implementar los cambios.

commands_path="$( cd "$(dirname "$0")" ; pwd -P )"
source "$commands_path/_variables.sh"

cd $root_path

#####################################################
# Git
#####################################################
read -p "${GREEN}git pull origin $git_branch_prod? [${YELLOW}N/${GREEN}y] $END" yn
if [ "$yn" == "y" -o "$yn" == "Y" ]
then
  git pull origin $git_branch_prod
fi

#####################################################
# GULP
#####################################################
read -p "${GREEN}Ejecutar gulp? [${YELLOW}N/${GREEN}y] $END" yn
if [ "$yn" == "y" -o "$yn" == "Y" ]
then
  if ! hash node 2>/dev/null
  then
    echo "${RED}Necesitas instalar NodeJS desde https://nodejs.org/es/${END}"
    exit
  fi

  if ! hash yarn 2>/dev/null
  then
    echo "${RED}Necesitas instalar Yarn desde https://yarnpkg.com/es-ES/${END}"
    exit
  fi

  cd $www_root_path
  yarn install
  NODE_ENV=production gulp
  cd $commands_path
fi
#####################################################
# Backup database
#####################################################
read -p "${GREEN}¿Backup database? [${YELLOW}N/${GREEN}y] $END" yn
if [ "$yn" == "y" -o "$yn" == "Y" ]
then
  $root_path/cron/postgres_db_backup.sh
fi

#####################################################
# Ejecutar migrate.
#####################################################
read -p "${GREEN}¿Ejecutar migrate? [${YELLOW}N/${GREEN}y] $END" yn
if [ "$yn" == "y" -o "$yn" == "Y" ]
then
  cd $web_path
  ASPNETCORE_ENVIRONMENT=Production dotnet ef database update -p ../ApplicationCore
  cd $root_path
fi

#####################################################
# Reiniciar server.
#####################################################
read -p "${GREEN}¿Reiniciar server? [${YELLOW}N/${GREEN}y] $END" yn
if [ "$yn" == "y" -o "$yn" == "Y" ]
then
  cd $web_path
  sudo systemctl stop kestrel
  dotnet publish -o /var/webapps/BlogNet
  sudo systemctl start kestrel
  cd $root_path
fi
