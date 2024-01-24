function Sum($a, $b) {
    return "$a + $b = " + ($a + $b)
}

function Sub($a, $b) {
    return "$a - $b = " + ($a - $b)
}

function Div($a, $b) {
    if ($b -eq 0) {
        return "Error: Division by zero"
    } else {
        return "$a / $b = " + ($a / $b)
    }
}

function Mul($a, $b) {
    return "$a * $b = " + ($a * $b)
}

Export-ModuleMember -Function Sum, Sub, Div, Mul