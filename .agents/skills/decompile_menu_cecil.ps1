$cecilPath = "c:\Users\mdfar\Downloads\Mad.Games.Tycoon.2.Build.20855306\Mad.Games.Tycoon.2.Build.20855306\BepInEx\core\Mono.Cecil.dll"
$assemblyPath = "c:\Users\mdfar\Downloads\Mad.Games.Tycoon.2.Build.20855306\Mad.Games.Tycoon.2.Build.20855306\Mad Games Tycoon 2_Data\Managed\Assembly-CSharp.dll"
$outputPath = "C:\Users\mdfar\.gemini\antigravity\brain\4578a489-06b6-47fc-81a3-8946b90ea5f4\scratch\decompiled_details_init.txt"

[Reflection.Assembly]::LoadFrom($cecilPath) | Out-Null
$readerParameters = New-Object Mono.Cecil.ReaderParameters
$assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($assemblyPath, $readerParameters)

$out = New-Object System.Text.StringBuilder

function Dump-Method($className, $methodName) {
    $type = $assembly.MainModule.Types | Where-Object { $_.Name -eq $className }
    if (-not $type) {
        [void]$out.AppendLine("Type $className not found.")
        return
    }
    $method = $type.Methods | Where-Object { $_.Name -eq $methodName }
    if (-not $method) {
        [void]$out.AppendLine("Method $methodName on $className not found.")
        return
    }
    [void]$out.AppendLine("=== $className.$methodName ===")
    foreach ($inst in $method.Body.Instructions) {
        [void]$out.AppendLine(("{0:X4}: {1} {2}" -f $inst.Offset, $inst.OpCode, $inst.Operand))
    }
}

Dump-Method "Menu_Stats_Developer_Main" "Init"
Dump-Method "Menu_Stats_Publisher_Main" "Init"

[System.IO.File]::WriteAllText($outputPath, $out.ToString())
Write-Host "Written to $outputPath"
