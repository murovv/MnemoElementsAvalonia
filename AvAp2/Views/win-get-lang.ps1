$ScriptBlock = {
    Add-Type -AssemblyName System.Windows.Forms
    [System.Windows.Forms.InputLanguage]::CurrentInputLanguage
}
$Job = Start-Job -ScriptBlock $ScriptBlock
$Null = Wait-Job -Job $Job
$CurrentLanguage = Receive-Job -Job $Job
Remove-Job -Job $Job
($CurrentLanguage).Culture.Name
<# Write-Host -NoNewLine 'Press any key to continue...';
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown'); #>
