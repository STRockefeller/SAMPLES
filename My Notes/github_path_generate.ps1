$createFileName = "github_path_generate.md"
$linkPath="https://github.com/STRockefeller/SAMPLES/tree/master/My%20Notes"
$localPath = "Microsoft.PowerShell.Core\FileSystem::C:\Users\admin\Desktop\SAMPLES\My Notes"
function searchNote($location,[string]$header)
{
    if($header -eq ""){$header="##"}
    $dir = $location|dir
    foreach($item in $dir)
    {
        $name = $($item.Name)
        if($name.Contains("."))
        {
            $link=$item.PSpath.Replace($localPath,$linkPath).Replace("\","/")

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