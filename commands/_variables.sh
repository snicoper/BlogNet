#!/bin/bash

#####################################################
# Global variables
#####################################################
# PATHS
root_path=$(dirname "$commands_path")
web_path="$root_path/src/Web"
www_root_path="$web_path/wwwroot"
static_dist_path="$www_root_path/static/dist"

# Git
git_branch_dev="master"
git_branch_prod="master"

# Base de datos
db_name_dev="DotnetBoilerplate"

# Colors
RED=$'\e[1;31m'
GREEN=$'\e[1;32m'
YELLOW=$'\e[1;33m'
BLUE=$'\e[1;34m'
MAGENTA=$'\e[1;35m'
CYAN=$'\e[1;36m'
END=$'\e[0m'
