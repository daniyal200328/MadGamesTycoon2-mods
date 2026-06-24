$ErrorActionPreference = "Stop"

$root = (Get-Item $PSScriptRoot).Parent.Parent.FullName
$gameRoot = "C:\Users\mdfar\Downloads\Mad.Games.Tycoon.2.Build.20855306\Mad.Games.Tycoon.2.Build.20855306"
$dotnet = "C:\tmp\dotnet\dotnet.exe"
$csc = "C:\tmp\dotnet\sdk\8.0.422\Roslyn\bincore\csc.dll"
$netstandardDir = "C:\tmp\dotnet\packs\NETStandard.Library.Ref\2.1.0\ref\netstandard2.1"
$src = Join-Path $PSScriptRoot "SubsidiaryTeamSlotsPlugin.cs"
$out = Join-Path $root "BepInEx\plugins\SubsidiaryTeamSlots.dll"
$managedDir = Join-Path $gameRoot "Mad Games Tycoon 2_Data\Managed"
$bepCoreDir = Join-Path $gameRoot "BepInEx\core"
$pluginsDir = Join-Path $root "BepInEx\plugins"

if (!(Test-Path $dotnet) -or !(Test-Path $csc) -or !(Test-Path $netstandardDir)) {
    throw "Missing local .NET SDK files under C:\tmp\dotnet. Reinstall the local SDK before compiling."
}

New-Item -ItemType Directory -Force -Path (Split-Path -Parent $out) | Out-Null

$refs = @()
$refs += Get-ChildItem -Path $netstandardDir -Filter "*.dll" | ForEach-Object { $_.FullName }
$refs += @(
    (Join-Path $bepCoreDir "BepInEx.dll"),
    (Join-Path $bepCoreDir "0Harmony.dll"),
    (Join-Path $managedDir "UnityEngine.dll"),
    (Join-Path $managedDir "UnityEngine.CoreModule.dll"),
    (Join-Path $managedDir "UnityEngine.UI.dll"),
    (Join-Path $managedDir "UnityEngine.IMGUIModule.dll"),
    (Join-Path $managedDir "UnityEngine.JSONSerializeModule.dll"),
    (Join-Path $managedDir "UnityEngine.InputLegacyModule.dll"),
    (Join-Path $managedDir "UnityEngine.InputModule.dll"),
    (Join-Path $managedDir "UnityEngine.TextRenderingModule.dll"),
    (Join-Path $managedDir "UnityEngine.UIModule.dll"),
    (Join-Path $managedDir "Assembly-CSharp.dll")
)

# Add optional soft dependencies if they exist
foreach ($optionalPlugin in @("SubsidiaryProjectDirector", "DynamicSubsidiaryTimeline", "DynamicStudioGoodwill", "Subsidiary2")) {
    $optionalDll = Join-Path $pluginsDir "$optionalPlugin.dll"
    if (Test-Path $optionalDll) {
        $refs += $optionalDll
    }
}

$refArgs = $refs | ForEach-Object { "/r:$_" }
$args = @(
    $csc,
    "/noconfig",
    "/nostdlib+",
    "/target:library",
    "/langversion:latest",
    "/out:$out"
) + $refArgs + $src

Write-Host "Compiling $src to $out..."
& $dotnet $args
if ($LASTEXITCODE -eq 0) {
    Write-Host "Compilation successful!"
} else {
    Write-Host "Compilation failed with code $LASTEXITCODE."
    exit $LASTEXITCODE
}
