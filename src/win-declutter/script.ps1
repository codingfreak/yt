#Install-Module Microsoft.WinGet.Client -Scope CurrentUser

Import-Module Microsoft.WinGet.Client

$ignoreList = @(
    "MSIX\Microsoft.SecHealthUI_1000.29628.1000.0_x64__8wekyb3d8bbwe",
    "MSIX\Microsoft.NET.Native.Framework.2.2_2.2.29512.0_x86__8wekyb3d8bbwe",
    "MSIX\Microsoft.Winget.Source_2026.903.1758.43_neutral__8wekyb3d8bbwe"
)
Get-WinGetPackage | Where-Object { !$ignoreList.Contains($_.Id) -and $_.Source -ne 'winget' } | Uninstall-WinGetPackage

$additional = @(
    "Microsoft.Outlook",
    "Microsoft.Edge",
    "Microsoft.Teams"
)
Get-WinGetPackage | Where-Object { $additional.Contains($_.Id) } | Uninstall-WinGetPackage
