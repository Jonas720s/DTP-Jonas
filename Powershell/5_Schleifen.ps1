<#
a: - Schreibe alle Dateinamen und ihre Dateigrösse des Verzeichnisses C:\Windows\System32 in ein Array
b: - Erstelle ein neues Array mit allen Files die grösser als 3MB sind. Speichere nur Dateinamen und Dateigrösse
c: - Loope durch alle laufenden Prozesse und beende die Schlaufe beim ersten Prozess der im Namen mit einem O beginnt. 
            Gebe den Namen des Prozesses und Anzahl Durchgänge auf der Konsole aus.
#>

$dateien = Get-ChildItem -Path C:\Windows\System32 -File
$grosseDateien
foreach ($datei in $dateien) {
    if ($datei.Length -gt 3MB) {
        $grosseDateien += $datei.Name, $datei.Length
    }
}
$processes = Get-Process
$counter = 0
foreach ($process in $processes) {
    $counter++
    if ($process.Name.StartsWith('O')) {
        Write-Host "Process Name: $($process.Name), Iteration: $counter"
        break
    }
}