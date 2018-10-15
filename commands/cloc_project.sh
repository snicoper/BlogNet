#!/bin/bash

# Curiosidad para contar lineas
# Requiere de cloc dnf install cloc

commands_path="$( cd "$(dirname "$0")" ; pwd -P )"
source "$commands_path/_variables.sh"

cloc $root_path \
--exclude-dir=\
.csproj,\
.eslintrc,\
.git,\
.htmlhintrc,\
.idea,\
.lock,\
.package.json,\
.stylelintrc,\
.user,\
.vs,\
.vscode,\
bin,\
compose,\
dist,\
docs,\
font-awesome,\
LICENCE.md,\
logs,\
media,\
Migrations,\
node_modules,\
obj,\
Properties,\
README.md,\
 \
--exclude-ext=json
