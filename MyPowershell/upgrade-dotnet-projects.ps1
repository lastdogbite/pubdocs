# Define variables
$projectsDirectory = "C:\All\Repos\myapp11"
$dotNetVersion = "7.0"
cls
# Get list of .NET 6 projects
$projects = Get-ChildItem -Path $projectsDirectory -Filter *.csproj -Recurse | Where-Object { $_.Name -notlike "obj*" -and $_.Name -notlike "*bin*" }

foreach ($project in $projects) {

    # Upgrade .NET version
    $projectXml = [xml](Get-Content $project.FullName)
    $projectXml.Project.PropertyGroup.TargetFramework = "net$dotNetVersion"
    $projectXml.Save($project.FullName)
    
    # Update NuGet packages
    $list = dotnet list "$($project.DirectoryName)/$($project.BaseName).csproj" package --outdated
    foreach($item in $list)
    {
        $item 
        if ($item.Contains("> "))
        {
            
            $item = $item.Trim().Replace(" ","+")            
            $package = $item.Substring($item.IndexOf(">+")+2)
            $version = $item.Substring($item.LastIndexOf("+")+1)
            $package = $package.Substring(0, $package.IndexOf("+"))
            Write-Host "Upgrading $package to $version..."            
            dotnet add "$($project.DirectoryName)/$($project.BaseName).csproj" package $package --version $version
            Write-Host "Upgrading $package to $version...done!"
           
        }#if
    }#foreach    
}#foreach


Write-Host "All projects have been upgraded to .NET $dotNetVersion and their NuGet packages have been updated to their latest versions."
