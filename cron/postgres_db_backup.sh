#!/bin/bash

# Crea un backup de la db, es necesario tener el archivo ~/.pgpass
# hostname:port:database:username:password
# Tener el archivo ~/.pgpass con permisos chmod 600

# Nombre db producción
PROD_DATABASE_NAME="BlogNet"

# Usuario db
PROD_DATABASE_USER="snicoper"

# Numero de días que conservara los backups
PROD_DATABASE_NUMBER_OF_DAYS=7

# Location to place backups.
PROD_DATABASE_BACKUP_DIR="$HOME/backups/db/"

# String to append to the name of the backup files
PROD_DATABASE_BACKUP_DATE=`date +%Y-%m-%d_%H-%M`

if [ ! -d "$PROD_DATABASE_BACKUP_DIR" ]; then
  mkdir -p $PROD_DATABASE_BACKUP_DIR
fi

# Numbers of days you want to keep copie of your databases

pg_dump -U $PROD_DATABASE_USER --no-password $PROD_DATABASE_NAME > $PROD_DATABASE_BACKUP_DIR$PROD_DATABASE_NAME.$PROD_DATABASE_BACKUP_DATE.psql

# Eliminar copias con mas 'number_of_days' días
find $PROD_DATABASE_BACKUP_DIR -type f -prune -mtime +$PROD_DATABASE_NUMBER_OF_DAYS -exec rm -f {} \;

echo "Backup de la db realizado con éxito"
