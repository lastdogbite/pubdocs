# Define variables
$appName = "MyApp6"
$now = Get-Date
$projectsDirectory = "C:\All\Repos\$appName\"
$versionNumber = "{0:1.0.yyMM.ddhhmm}" -f $now
$copyrightVal = "Copyright my_company"

# Get list of .NET 6 projects
$projects = Get-ChildItem -Path $projectsDirectory -Filter *.csproj -Recurse | Where-Object { $_.Name -notlike "*obj*" -and $_.Name -notlike "*bin*" }



foreach ($project in $projects) {

    # Check if version property exists, and add it if it doesn't
    $projectXml = [xml](Get-Content $project.FullName)
    $propertyGroup = $projectXml.Project.PropertyGroup

    # version
    if ($propertyGroup.Version) {
        $major = [int]$propertyGroup.Version.Split(".")[0]
        $minor = [int]$propertyGroup.Version.Split(".")[1] + 1
        if ($minor -gt 50) {$major = $major + 1; $minor = 0;}
        $versionNumber = $major.ToString() + "." + $minor.ToString() + "." + ("{0:yyMM.ddhhmm}" -f $now)
        $versionNumber
        $propertyGroup.Version = $versionNumber
    } else {        
        $newVersion = $projectXml.CreateElement("Version")
        $newVersion.InnerText = $versionNumber
        $propertyGroup.AppendChild($newVersion)        
    }

    # copyright
    if ($propertyGroup.Copyright) {
        $propertyGroup.Copyright = $copyrightVal
    } else {        
        $Copyright = $projectXml.CreateElement("Copyright")
        $Copyright.InnerText = $copyrightVal
        $propertyGroup.AppendChild($Copyright)        
    }

      # disable nullable
    if ($propertyGroup.Nullable) {
        $propertyGroup.Nullable = "disable"
    } 




    $projectXml.Save($project.FullName)
}

Write-Host "All projects have been updated to version $versionNumber."