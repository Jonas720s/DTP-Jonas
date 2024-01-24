
<#.DESCRIPTION
    adds two numbers
#>
function Sum($a, $b) {
    return "$a + $b = " + ($a + $b)
}

<#.DESCRIPTION
    subtracts two numbers
#>
function Sub($a, $b) {
    return "$a - $b = " + ($a - $b)
}

<#.DESCRIPTION
    divides two numbers
#>
function Div($a, $b) {
    if ($b -eq 0) {
        return "Error: Division by zero"
    } else {
        return "$a / $b = " + ($a / $b)
    }
}

<#.DESCRIPTION
    multiplies two numbers
#>
function Mul($a, $b) {
    return "$a * $b = " + ($a * $b)
}

Export-ModuleMember -Function Sum, Sub, Div, Mul