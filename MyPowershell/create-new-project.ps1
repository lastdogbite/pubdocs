# Dev RS
# Script created on: 2021-09-14
# This script creates a .NET 6 C# Web API project, a Razor project, a C# Library project, and a solution file that includes all the projects.
# Also, it installs Newtonsoft.Json and MySql.Data packages to the Web API project, and restores the project's dependencies.

$appName = "MyApp11"

$location = "C:\All\Repos\$appName\"
$apiProjectName = "$appName.API"
$razorProjectName = "$appName.Web"
$libProjectName = "$appName.Objects"
$libProjectName2 = "$appName.BizLogic"
$ConsoleProjectName = "$appName.Console"
$pathToReference = "C:\MyPath"
$netver = "net6.0"

$solutionName = $appName

cls 

# Create the project directory
New-Item -ItemType Directory -Path $location

# Navigate to the project directory
cd $location

#Create the console project
dotnet new console -n $ConsoleProjectName --framework $netver


# Create the new Web API project
dotnet new webapi -n $apiProjectName --framework $netver

# Add a new Razor project to the solution
dotnet new razor -n $razorProjectName -o $razorProjectName --no-https --framework $netver

# Add a new C# library project to the solution
dotnet new classlib -n $libProjectName -o $libProjectName --framework $netver
dotnet new classlib -n $libProjectName2 -o $libProjectName2 --framework $netver

# Create the solution file
dotnet new sln -n $solutionName

# Add the projects to the solution
dotnet sln "$solutionName.sln" add $apiProjectName/$apiProjectName.csproj
dotnet sln "$solutionName.sln" add $razorProjectName/$razorProjectName.csproj
dotnet sln "$solutionName.sln" add $libProjectName/$libProjectName.csproj
dotnet sln "$solutionName.sln" add $libProjectName2/$libProjectName2.csproj
dotnet sln "$solutionName.sln" add $ConsoleProjectName/$ConsoleProjectName.csproj

# Create a Readme.md file in each project
New-Item -ItemType Directory -Path "$apiProjectName/App"
New-Item -ItemType File -Path "$apiProjectName/Readme.md"
New-Item -ItemType File -Path "$razorProjectName/Readme.md"
New-Item -ItemType File -Path "$libProjectName/Readme.md"
New-Item -ItemType File -Path "$libProjectName2/Readme.md"
New-Item -ItemType File -Path "$ConsoleProjectName/Readme.md"


# Install Newtonsoft.Json package
dotnet add $apiProjectName package Newtonsoft.Json -v "13.0.1"
dotnet add $razorProjectName package Newtonsoft.Json -v "13.0.1"
dotnet add $libProjectName package Newtonsoft.Json -v "12.0.1"
dotnet add $libProjectName2 package Newtonsoft.Json
dotnet add $ConsoleProjectName package Newtonsoft.Json

# Install MySql.Data package
dotnet add $apiProjectName package MySql.Data
dotnet add $libProjectName2 package MySqlConnector
dotnet add $libProjectName2 package MySql.Data

# add file references
#dotnet add "$projectName/$projectName.csproj" reference "$pathToReference/my.dll"

# add project reference
dotnet add "$ConsoleProjectName/$ConsoleProjectName.csproj" reference "$libProjectName/$libProjectName.csproj"
dotnet add "$ConsoleProjectName/$ConsoleProjectName.csproj" reference "$libProjectName2/$libProjectName2.csproj"

# Restore dependencies
dotnet restore

# build
dotnet build "$solutionName.sln"