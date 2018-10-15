#####################################################
# Global variables
#####################################################
# PATHS
$RootPath = (Get-Item $PSScriptRoot).Parent.FullName
$WebPath = "$RootPath\src\Web"
$WWWRootPath = "$WebPath\wwwroot"
$StaticDistPath = "$WWWRootPath\static\dist"

# Git
$GitBranch = "master"

# Base de datos
$DbName = "BlogNet"

# Envs
$OLD_ASPNETCORE_ENVIRONMENT = $env:ASPNETCORE_ENVIRONMENT
$env:ASPNETCORE_ENVIRONMENT = "Development"
$OLD_NODE_ENV = $env:NODE_ENV
$env:NODE_ENV = "development"
