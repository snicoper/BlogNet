# Instalación en desarrollo

## Nodejs

* [https://nodejs.org/es/](https://nodejs.org/es/)

## Yarn

* [https://yarnpkg.com/es-ES/](https://yarnpkg.com/es-ES/)

Requiere de algunos packages globales para la transpilación

```bash
yarn global add gulp node-sass eslint htmlhint stylelint stylelint-config-standard
```

Para transpilar los archivos, se ha de navegar hasta el directorio `wwwroot`

```bash
cd src/Web/wwwroot

# Instalar dependencias
yarn
```

Cada vez que hay cambios en `src/Web/wwwroot/packages.json` se ha de volver a ejecutar el comando `yarn`

El siguiente comando se ha de ejecutar cada vez que se ejecuta `yarn`. Con el siguiente comando transpila todos los archivos, tanto **production** como **development**.

El **Javascript** separa los locales de los packages de terceros por cuestiones de rendimiento cuando se esta en `watch`, pero los `scss` los pone todos en uno por que no tarda apenas nada en re-transpilar.

Por eso siempre se ha de ejecutar `gulp` cuando hay cambios de terceros, ya que en `watch` los Javascript de terceros no los toca.

```bash
gulp
```

Para entrar en modo `watch` con gulp.

```bash
gulp watch
```

Se pone en modo escucha para todos los `scss` y `js` locales, siempre que se modifique/guarde un archivo, se auto transpilara.

Cuando se crea un nuevo archivo `scss` o `js` se ha de parar `Ctrl+c` y volver a ejecutar `gulp watch` o no se verán los cambios.

El archivo de configuración de gulp se encuentra en `src/Web/wwwroot/gulpfile.js`

## PostgreSQL

* [https://www.postgresql.org/](https://www.postgresql.org/)
* [https://dbeaver.jkiss.org/](https://dbeaver.jkiss.org/)

Se ha de crear un usuario y password igual que en `src/Web/appsettings.Development.json`

!!! note
    Para evitar passwords en Windows [pgpass](http://apuntes-snicoper.readthedocs.io/es/latest/windows/pgpass_windows.html), en linux igual pero se guarda en `~/.pgpass`

```sql
psql -U postgres

CREATE USER usuario WITH PASSWORD '123456' CREATEDB;
CREATE DATABASE "MiBaseDeDatos" WITH OWNER usuario;
```

Con `CREATEDB` y/o `SUPERUSER` si se quiere dar permisos para creación de bases de datos o súper usuario.

!!! notes
    En **Windows** Aunque los puertos del router y sistema están cerrados por defecto para `5432`, si se quiere un poco mas de seguridad, editar `C:\PostgreSQL\10\data\postgresql.conf` linea **59** y cambiar `*` por `localhost`, de esa manera solo se puede acceder desde `localhost`

    En linux [instalación postgres en Fedora/Centos](http://apuntes-snicoper.readthedocs.io/es/latest/linux/postgresql/instalacion_postgresql.html)

## Git

Estas son mis configuraciones

* [Windows](http://apuntes-snicoper.readthedocs.io/es/latest/git/git_windows.html)
* [Linux](http://apuntes-snicoper.readthedocs.io/es/latest/git/gitconfig_linux.html)

## .NET Core SDK

* [https://www.microsoft.com/net/download](https://www.microsoft.com/net/download)

## Migraciones

La primera vez que se descarga el proyecto se ha de ejecutar desde la raíz del proyecto.

También cada vez que se añade/actualiza un paquete.

```bash
dotnet restore
```

Si se usa **Visual Studio** ya lo hace siempre al ejecutar el proyecto

Las migraciones las crea en el proyecto `ApplicationCore` directorio `Migrations`

```bash
cd src/Web

dotnet ef migrations add Initial -p ../ApplicationCore
```

Bandera `-p` es igual a `--project` indicándole el proyecto de destino

Para actualizar la base de datos.

```bash
dotnet ef database update -p ../ApplicationCore
```

## pg_reinstall_dev

En el directorio `./commands` ejecutando desde `.\pg_reinstall_dev.ps1` y `./pg_reinstall_dev.sh` automatizara tareas de reinstalación.

* **Git:** `fetch` y `difftool`
* **Git:** `pull origin $GitBranch` `$GitBranch` se obtiene de `_variables.ps1` y `_variables.sh`
* **dotnet:** Regenera migraciones, elimina por completo la migración `Initial`
* **dotnet:** Elimina la base de datos, la crea de nuevo y restaura las migraciones (se pierden los datos de la base de datos)
* **Gulp:** ejecuta comandos `yarn` y `gulp`, se asegura de tener actualizados los paquetes
* **media dir** Restaura `src/Web/wwwroot/media` de `compose/media`

## Problemas conocidos

### 2.1.104

En **Fedora 28** [https://github.com/dotnet/corefx/issues/26966](https://github.com/dotnet/corefx/issues/26966)

Para **zsh** o **bash** de momento la solución es añadir en `.bashrc` o `.zshrc`

```bash
alias dotnet="TERM=xterm dotnet"
```

O directamente al ejecutar un comando `dotnet`

```bash
TERM=xterm dotnet comando
```

Con **Rider** en la configuración del servidor añadir una **variable de entorno** `TERM` con valor `xterm`

Con **vscode** el `launch.json` no consigo solucionarlo.
