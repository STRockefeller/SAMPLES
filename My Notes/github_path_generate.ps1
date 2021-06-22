$createFileName = "github_path_generate.md"
$linkPath="https://github.com/STRockefeller/MyProgrammingNote/tree/master/My%20Notes"
$localPath = "Microsoft.PowerShell.Core\FileSystem::$pwd"
function searchNote($location,[string]$header)
{
    if($header -eq ""){$header="##"}
    $dir = $location|dir
    foreach($item in $dir)
    {
        $name = $($item.Name)
        if($name.Contains("."))
        {
            $link=$item.PSpath.Replace($localPath,$linkPath).Replace("\","/").Replace(" ","%20")

            "* [$name]($link)"
        }
        else
        {
            "$header $name"
            $newLocation = "$location\$name"
            $newHeader = "#$header"
            searchNote $newLocation $newHeader
        }
    }
}
function startup()
{
    "# Github Note Path"
    Get-Date
    searchNote (pwd)
}
startup|Out-File $createFileName