# Instalación desarrollo

## Requerimientos

!!! note
    Probado en un entorno con

    * Centos 7.4
    * PostgresSQL 10
    * .NET SDK 2.1.104
    * Nodejs
    * Postfix dovecot

## Post instalación Centos 7

Actualizar el sistema

```bash
yum -y update
```

Establecer **timezone** y **keymap**

```bash
timedatectl set-timezone Europe/Madrid
localectl set-keymap es
```

Remi y Epel

```bash
# epel
yum install -y epel-release

# remi
yum install -y http://rpms.remirepo.net/enterprise/remi-release-7.rpm
```

Instalación de programas/librerías básicas

```bash
yum -y install \
    bash-completion \
    cpp \
    firewalld \
    gcc \
    git \
    htop \
    kernel-devel \
    kernel-headers \
    make \
    mutt \
    net-tools \
    openssh \
    policycoreutils-python \
    vim \
    wget \
    yum-utils
```

Activar **FirewallD**

```bash
systemctl start firewalld.service
systemctl enable firewalld.service
```

## Creación de usuarios y grupo

Cambiar nombre de `USUARIO`

```bash
groupadd webapps

adduser USUARIO
usermod -aG wheel USUARIO
usermod -aG webapps USUARIO
```

Crear directorio para la aplicación

```bash
mkdir -p /var/webapps

chown USUARIO:webapps /var/webapps
chgrp webapps -R /var/webapps

# Si SELinux esta activado.
chcon -R -t  httpd_sys_content_t /var/webapps
```

## Configurar SSH

Cambiar puerto `22222` por otro menos evidente :)

```bash
systemctl start sshd.service
systemctl enable sshd.service
```

```bash
vim /etc/ssh/sshd_config
```

```bash
# Puerto
# Linea 17 (17), descomentar y cambiar puerto por defecto
Port 22222

# Permitir login con root?
# line 38 (49): uncomment and change 'no'
PermitRootLogin no

# line 64 (79): uncomment
PermitEmptyPasswords no

# Disable password authentication forcing use of keys
# line 65 (78):
PasswordAuthentication yes

# Usuarios a los que se les permite conectarse
# Añadir al final
AllowUsers USUARIO
```

Muy importante, abrir puerto antes de salir de **root**

```bash
firewall-cmd --permanent --zone=public --add-port=22222/tcp
firewall-cmd --reload

# Si se usa SELInux
semanage port -a -t ssh_port_t -p tcp 22222
```

Salir de **SSH** y [entrar con tu usuario](#creacion-ssh-key)

```bash
ssh -p 22222 usuario@serverip
```

## Nodejs y Gulp

Nodejs

```bash
su -

curl --silent --location https://rpm.nodesource.com/setup_9.x | bash -

yum -y install nodejs
```

Yarn

```bash
wget https://dl.yarnpkg.com/rpm/yarn.repo -O /etc/yum.repos.d/yarn.repo

yum install -y yarn
exit

# Como usuario
yarn global add gulp node-sass
```

## Instalación NET SDK

```bash
su -

rpm --import https://packages.microsoft.com/keys/microsoft.asc
sh -c 'echo -e "[packages-microsoft-com-prod]\nname=packages-microsoft-com-prod \nbaseurl= https://packages.microsoft.com/yumrepos/microsoft-rhel7.3-prod\nenabled=1\ngpgcheck=1\ngpgkey=https://packages.microsoft.com/keys/microsoft.asc" > /etc/yum.repos.d/dotnetdev.repo'
```

Elegir la última versión del **SDK**

```bash
yum update
yum install libunwind libicu
yum install dotnet-sdk-2.1.200
```

## Instalación PostgreSQL

* [https://yum.postgresql.org/repopackages.php](https://yum.postgresql.org/repopackages.php)

Elegir la versión a instalar, en este Wiki se usa **10.2**

```bash
su -

yum install https://download.postgresql.org/pub/repos/yum/10/redhat/rhel-7-x86_64/pgdg-centos10-10-2.noarch.rpm

yum install postgresql10 postgresql10-server postgresql10-devel postgresql10-contrib
```

Añadir al **PATH**

```bash
vim /etc/profile

export PATH="$PATH:/usr/pgsql-10/bin/"

# Requiere reiniciar el sistema
reboot
```

Una vez dentro de nuevo en el sistema, inicializar postgres y añadir password a `postgres`

```bash
su -
postgresql-10-setup initdb
systemctl start postgresql-10
systemctl enable postgresql-10

# Añadir contraseña a postgres.
su - postgres
psql
\password postgres
\q
exit
```

Configuración de PostgreSQL

```bash
vim /var/lib/pgsql/10/data/postgresql.conf
```

```bash
# linea 59
listen_addresses = 'localhost'

# linea 63 descomentar
port = 5432
```

!!! note
    Para entrar a PostgreSQL desde un manager, requiere de un **tunel ssh**

```bash
vim  /var/lib/pgsql/data/pg_hba.conf
```

Ir a la parte final del documento y remplazar los `peer` por `md5`

```bash
# TYPE  DATABASE        USER            ADDRESS                 METHOD

# "local" is for Unix domain socket connections only
local   all         all                               md5
# IPv4 local connections:
host    all         all         127.0.0.1/32          md5
# IPv6 local connections:
host    all         all         ::1/128               md5
```

Si usa **SELInux**

```bash
setsebool -P allow_user_postgresql_connect 1
```

Restart

```bash
systemctl restart postgresql.service
```

Creación base de datos y usuario, igual a `src/Web/appsettings.json`

```bash
psql -U postgres

CREATE USER usuario WITH PASSWORD 'PASSWORD';

CREATE DATABASE "DBNAME" WITH OWNER usuario;
\q
```

## Instalación de postfix dovecot

Omitir la parte **Para crear el certificado SSL, Crear SSL** pero si modificar los archivos para luego cambiar los certificados por Let’s Encrypt

[Instalación postfix y dovecot](http://apuntes-snicoper.readthedocs.io/es/latest/linux/fedora-centos/postfix.html)

## Instalación de Nginx

```bash
su -
yum install nginx

systemctl start nginx.service
systemctl enable nginx.service

firewall-cmd --permanent --zone=public --add-service=http
firewall-cmd --permanent --zone=public --add-service=https
firewall-cmd --reload
```

Eliminar de `vim /etc/nginx/nginx.conf` en la linea **36** y **37** `default_server`

Copiar archivo de Configuración

```bash
cp compose/configs/nginx_https.conf /etc/nginx/conf.d/MIDOMAIN.com.conf
```

```bash
systemctl enable nginx.service
```

## Let’s Encrypt

Requiere tener dominio y que las DNSs del dominio apunte al servidor

```bash
su -

yum install certbot
```

```bash
su -

vim /etc/nginx/default.d/le-well-known.conf
```

```bash
location ~ /.well-known {
  allow all;
}
```

```bash
systemctl restart nginx
```

Cambiar `MIDOMAIN` por nombre de dominio en el siguiente comando y seguir las instrucciones.

```bash
certbot certonly -a webroot --webroot-path=/usr/share/nginx/html -d MIDOMAIN.com -d www.MIDOMAIN.com -d mail.MIDOMAIN.com

openssl dhparam -out /etc/ssl/certs/dhparam.pem 2048
```

Configuraciones de [Postfix](#instalacion-de-postfix-dovecot)

Editar `vim /etc/postfix/main.cf`

```bash
myhostname = mail.MIDOMAIN.com
mydomain = MIDOMAIN.com
mynetworks = 192.168.1.0/24, 127.0.0.0/8, IP_PUBLICA # <---- Ip publica
smtpd_tls_cert_file = /etc/letsencrypt/live/MIDOMAIN.com/fullchain.pem
smtpd_tls_key_file = /etc/letsencrypt/live/MIDOMAIN.com/privkey.pem
```

Editar `vim /etc/dovecot/conf.d/10-ssl.conf`

```bash
ssl_cert = </etc/letsencrypt/live/MIDOMAIN.com/fullchain.pem
ssl_key = </etc/letsencrypt/live/MIDOMAIN.com/privkey.pem
```

```bash
systemctl enable nginx.service
```

## Configuración de usuario

Todo como usuario a no ser que se diga de manera explicita.

### Creación ssh-key

Desde tu sistema generar clave **ssh**

```bash
ssh-keygen -t rsa -b 4096

# Entrar con tu usuario
ssh-copy-id -p 65432 usuario@SERVER_IP_ADDRESS
```

### PostgreSQL

Para evitar que pregunte por los passwords cada vez que se ejecute `psql`, crear archivo en `~/.pgpass`

```bash
vim .pgpass
```

Añadir las dos siguientes lineas

```bash
localhost:5432:*:postgres:PASSWORD
localhost:5432:DBNAME:USERNAME:PASSWORD
```

```bash
chmod 600 ~/.pgpass
```

### Git

```bash
vim ~/.gitconfig
```

Cambiar `name` y `email`

```bash
[user]
    name = NOMBRE APELLIDO
    email = email@gmail.com
[color]
    ui = true
[core]
    editor = vim
[alias]
    lg = log --pretty=format:'%Cred%h%Creset -%C(yellow)%d%Creset %s %Cgreen(%cr %an)%Creset' --abbrev-commit --date=relative
    co = checkout
    cm = commit
    st = status
    br = branch
[push]
    default = simple
```

## SystemD

Copiar de **compose** archivos para **SystemD**

```bash
su-

cp compose/configs/kestrel.service /etc/systemd/system/kestrel.service
systemctl start kestrel.service
systemctl enable kestrel.service
```

## Cron

```bash
# Como usuario
crontab -e
```

```bash
MAILTO=email@example.com

# Crear un backup de la base de datos
2 1 * * * /home/TU_USER/projects/BlogNet/cron/postgres_db_backup.sh
```

## Publish

Como usuario

```bash
cd /home/USUARIO/projects

git clone repositorio

cd PROYECTO

dotnet restore

cd PROYECTO/src/Web
```

Si es la primera vez que se descarga el código

```bash
dotnet ef database update -p ../ApplicationCore
```

Transpilar archivos **JS/SCSS**

```bash
cd wwwroot

yarn
gulp

cd ..
```

Compilar

```bash
pwd
# /home/USUARIO/projects/PROYECTO

sudo systemctl stop kestrel.service

dotnet publish -o /var/webapps/PROYECTO/

sudo systemctl start kestrel.service
```

## Publish automatizado

Automatizar actualización en producción

```bash
cd ~/projects/PROYECTO/commands

./reload_prod.sh
```
