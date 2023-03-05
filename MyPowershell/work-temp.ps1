$projectFolder = "C:\All\Repos\MyApp11\MyApp11.Console"
cls
# Create a new appsettings.json file

$content = @"
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Settings": {

  }

}
"@ -replace '\r\n', "`r`n"


if (!(Test-Path "$projectFolder\appsettings.json")) {
    New-Item "$projectFolder\appsettings.json" -ItemType File
    Set-Content -Path "$projectFolder\appsettings.json" -Value $content
}

