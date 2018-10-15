# Ejecutar: powershell -ExecutionPolicy ByPass -File pg_reinstall_dev.ps1

. .\_variables.ps1

#####################################################
# Git
#####################################################
Write-Host "Git fetch y difftool $GitBranch? [N/y] " -NoNewline -ForegroundColor Green
$GitFetch = Read-Host
if ($GitFetch -eq "y")
{
    git fetch origin $GitBranch
    git difftool -d origin/$GitBranch
}

Write-Host "Git pull origin $GitBranch? [N/y] " -NoNewline -ForegroundColor Green
$GitPull = Read-Host
if ($GitPull -eq "y")
{
    git pull origin $GitBranch
}

#####################################################
# Base de datos
#####################################################
Write-Host "Restaurar la base de datos? [N/y] " -NoNewline -ForegroundColor Green
$PgReinstall = Read-Host
if ($PgReinstall -eq "y")
{
    psql -U postgres -c @"
    DROP DATABASE IF EXISTS ""$DbName""
"@

    Write-Host "Restaurando la base de datos..." -ForegroundColor Yellow
    Set-Location $RootPath
    dotnet restore
    Set-Location $WebPath
    dotnet ef database update -p ../ApplicationCore
    Set-Location $RootPath/commands
}

#####################################################
# GULP
#####################################################
Write-Host "Ejecutar gulp? [N/y] " -NoNewline -ForegroundColor Green
$Gulp = Read-Host

if ($Gulp -eq "y")
{
    if ((Get-Command "node.exe" -ErrorAction SilentlyContinue) -eq $null)
    {
        Write-Host "Necesitas instalar NodeJS desde https://nodejs.org/es/ " -NoNewline -ForegroundColor Yellow
        Write-Host "Version 9.x" -ForegroundColor Red
        Exit
    }

    if ((Get-Command "yarn.cmd" -ErrorAction SilentlyContinue) -eq $null)
    {
        Write-Host "Necesitas instalar Yarn desde https://yarnpkg.com/es-ES/" -ForegroundColor Yellow
        Exit
    }

    if ((Get-Command "gulp.cmd" -ErrorAction SilentlyContinue) -eq $null)
    {
        Write-Host "Necesitas instalar gulp y node-sass a nivel global" -ForegroundColor Yellow
        Write-Host "yarn global add gulp node-sass eslint htmlhint stylelint stylelint-config-standard" -ForegroundColor Yellow
        Write-Host "Ejecutar gulp? [N/y] " -NoNewline -ForegroundColor Green
        $InstallGulp = Read-Host
        if ($InstallGulp -eq "y")
        {
            yarn global add gulp node-sass eslint htmlhint stylelint stylelint-config-standard
        }
        else
        {
            Exit
        }
    }

    Set-Location $WWWRootPath
    if (Test-Path -Path $StaticDistPath)
    {
        Remove-Item $StaticDistPath -Force -Recurse
    }
    yarn
    gulp
}

#####################################################
# Media
#####################################################
Write-Host "Restaurar media? [N/y] " -NoNewline -ForegroundColor Green
$RetoreMedia = Read-Host
if ($RetoreMedia -eq "y")
{
    $MediaDir = "$WWWRootPath/media"
    $ComposeMediaDir = "$RootPath/compose/media"
    if (Test-Path -Path $MediaDir)
    {
        Remove-Item $MediaDir -Force -Recurse
    }
    Copy-Item -Path $ComposeMediaDir -Recurse -Destination $MediaDir
}

#####################################################
# Restore envs
#####################################################
$env:NODE_ENV = $OLD_NODE_ENV
$env:ASPNETCORE_ENVIRONMENT = $OLD_ASPNETCORE_ENVIRONMENT
Set-Location $RootPath/commands

Write-Host "Todas las tareas han terminado" -ForegroundColor Green
