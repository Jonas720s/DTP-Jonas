<#
Übung 7a: - Erstelle ein Module Math mit den Funktionen "Sum", "Sub", "Div" und "Mul"
             Als Beispiel fuer die Methode Mul soll folgender String returned werden: 3 * 9 = 27
             Erstelle ein Script, in welchem das Module geladen wird und rufe alle Funktionen auf
Übung 7b: - Gib alle Funktionen des Moduls Math auf die Konsole aus
#>

Import-Module ./Math.psm1

Get-Command -Module Math

Write-Output (Sum 5 3)
Write-Output (Sub 5 3)
Write-Output (Div 5 3)
Write-Output (Mul 3 9)