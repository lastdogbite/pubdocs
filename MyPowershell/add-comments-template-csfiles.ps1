$rootFolder = "C:\All\Repos\MyApp11"

$commentBlock = @"
/*  NOTE: Intenal Use Only
    
CreateDate: 
CreatedBy: 
Comments:

*/


"@ -replace '\r\n', "`r`n"
cls 


Get-ChildItem -Path $rootFolder -Recurse -Include "*.cs" |`
     Where-Object { $_.FullName -notmatch '\\bin\\|\\obj\\' } |`
     ForEach-Object {
    $_.FullName
    $content = Get-Content $_.FullName -Raw
    $content
    if ($content.Trim().StartsWith("/*") -eq $false)
    {
        $content = $commentBlock + "`r`n" + $content;
        Set-Content -Path $_.FullName -Value $content
    }

    
}