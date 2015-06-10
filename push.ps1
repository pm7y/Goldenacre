Get-ChildItem "./pkg" -Filter *.nupkg | `
Foreach-Object{
    Write-Host "Pusing $($_.Name)";

    ./.nuget/nuget push $_.Fullname -s https://www.nuget.org
}


#./.nuget/nuget push ./Goldenacre.Core.1.0.0.nupkg -s https://www.nuget.org
#./.nuget/nuget push ./Goldenacre.Web.1.0.0.nupkg -s https://www.nuget.org

Write-Host "Press any key...";
Read-Host;