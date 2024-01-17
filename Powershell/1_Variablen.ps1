<#
Fuehre das Script aus und finde heraus welche Variable welchen Typ hat.
#>

$Var1 = "Scripting mit Powershell"
$Var2 = "123"
$Var3 = 123
$Var4 = "ÜBUNG1","ÜBUNG2","ÜBUNG3","ÜBUNG4"
$Var5 = 1 -eq 2
$Var6 = @{
    Name = "GBSSG"
    Modul = "Scripting mit PowerShell"}
$Var7 = Get-Date

$Var1.GetType()
$Var2.GetType()
$Var3.GetType()
$Var4.GetType()
$Var5.GetType()
$Var6.GetType()
$Var7.GetType()
