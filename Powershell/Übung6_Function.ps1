<#
Übung 6a: - Schreibe eine Funktion die ein Logfile in deinem Übungsverzeichnis schreibt.
            Als Parameter soll die Lognachricht, der Nachrichttyp(Info oder Error) mitgegeben werden.
            Die Lognachricht soll folgendes Format haben: 22.08.2023 07:16 - Info - Das ist die Lognachricht
Übung 6b: - Schreibe die Dateiennamen eures %TEMP% Verzeichnisses mit Hilfe der Funktion in das Logfile.
#>

# Übung 6a
function Write-Log {
    param (
        [Parameter(Mandatory=$true)]
        [string]$Message,

        [Parameter(Mandatory=$true)]
        [ValidateSet("Info", "Error")]
        [string]$MessageType
    )

    $timestamp = Get-Date -Format "dd.MM.yyyy HH:mm"
    $logMessage = "$timestamp - $MessageType - $Message"
    $logFile = Join-Path -Path $PSScriptRoot -ChildPath "log.txt"

    Add-Content -Path $logFile -Value $logMessage
}

# Übung 6b
function WriteTempFilesToLog {
    
    $TempDir = $env:TEMP
    $TempFiles = Get-ChildItem -Path $TempDir

    foreach ($file in $TempFiles) {
        Write-Log -Message "File found: $($file.Name)" -MessageType "Info"
    }
}
WriteTempFilesToLog