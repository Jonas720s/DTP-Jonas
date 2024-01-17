<#
a: - Überprüfe auf deinem Gerät ein Prozess mit dem Namen "Teams" läuft und gebe die ID des Prozesses aus
b: - Wenn der Prozess nicht läuft, gebe einen folgenden String aus "Prozess Teams läuft nicht."
c: - Überprüfe ob in deinem Scriptverzeichnis die Datei "Scriptlog.log" existiert. Falls nicht erstelle die Datei mit dem Inhalt "Script gestartet"
#>

$process = Get-Process | Where-Object { $_.Name -eq "Teams" }
if ($process) {
    Write-Output "Teams läuft mit der ID: $($process.Id)"
} else {
    Write-Output "Teams läuft nicht"
}
$filePath = Join-Path -Path $PSScriptRoot -ChildPath "Scriptlog.log"

$currentDate = Get-Date
if (-not (Test-Path -Path $filePath)) {
    Set-Content -Path $filePath -Value "Script gestartet um $currentDate"
}
else {
    Add-Content -Path $filePath -Value "Script gestartet um $currentDate"
}