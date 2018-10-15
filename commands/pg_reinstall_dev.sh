#!/bin/bash

commands_path="$( cd "$(dirname "$0")" ; pwd -P )"
source "$commands_path/_variables.sh"

#####################################################
# Git
#####################################################
read -p "${GREEN}Git fetch y difftool $git_branch_dev? [${YELLOW}N/${GREEN}y] $END" yn
if [ "$yn" == "y" -o "$yn" == "Y" ]
then
  git fetch origin $git_branch_dev
  git difftool -d origin/$git_branch_dev
fi

read -p "${GREEN}Git pull origin $git_branch_dev? [${YELLOW}N/${GREEN}y] $END" yn
if [ "$yn" == "y" -o "$yn" == "Y" ]
then
  git pull origin $git_branch_dev
fi

#####################################################
# Base de datos
#####################################################
read -p "${GREEN}Restaurar la base de datos? [${YELLOW}N/${GREEN}y] $END" yn
if [ "$yn" == "y" -o "$yn" == "Y" ]
then
  cd $web_path

  # Solo usar en etapa de desarrollo, si llega a producción, eliminar
  # para no eliminar las migraciones.
  read -p "${RED}Eliminar migraciones y crear ${GREEN}Initial${RED}? [${YELLOW}N/${GREEN}y] $END" yn
  if [ "$yn" == "y" -o "$yn" == "Y" ]
  then
    migrations_path="$root_path/src/ApplicationCore/Migrations"

    if [ -d $migrations_path ]
    then
      rm -rf $migrations_path
      cd $web_path
      echo "${YELLOW}Migraciones eliminadas con éxito${END}"
    fi

    echo "${YELLOW}Creando migración ${GREEN}Initial${YELLOW}...${END}"
    dotnet ef migrations add Initial -p ../ApplicationCore
  fi

  psql -U postgres -c "drop database if exists \"${db_name_dev}\""

  dotnet restore
  echo "${YELLOW}Restaurando la base de datos...${END}"
  dotnet ef database update -p ../ApplicationCore
  cd $commands_path
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

  if [ -d $static_dist_path ]
  then
    rm -rf $static_dist_path
  fi

  cd $www_root_path
  yarn install
  gulp
  cd $commands_path
fi

#####################################################
# Media
#####################################################
read -p "${GREEN}Restore media? [${YELLOW}N/${GREEN}y] $END" yn
if [ "$yn" == "y" -o "$yn" == "Y" ]
then
  media_dir=$www_root_path/media
  compose_media_dir=$root_path/compose/media
  if [ -d $media_dir ]
  then
    rm -rf $media_dir
  fi
  cp -r $compose_media_dir $www_root_path
fi

echo "${BLUE}Todas las tareas han terminado${END}"
