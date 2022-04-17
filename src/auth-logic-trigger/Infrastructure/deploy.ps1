[CmdletBinding()]
param (	
	$TenantId,
	$SubscriptionId,
	$Location = 'West Europe',
	[switch]
	$WhatIf
)

if ($PSScriptRoot.Contains(' ') -and $PSScriptRoot -ne $PWD) {
	throw "This script needs to be executed from inside its folder because white spaces where detected."
}
$root = $PSScriptRoot.Contains(' ') ? '.' : $PSScriptRoot

Write-Host "Retrieving DEVDEER options from AKV and writing to $root/bicepSettings.json..."
$targetFileUri = "$root/bicepSettings.json"
$tmpFileUri = "$root/tmp.json"
Copy-Item $targetFileUri $tmpFileUri -Force
Get-AzKeyVaultSecret -VaultName akv-devdeer -Name SpockBicepSettings -AsPlainText | ConvertFrom-Json | ConvertTo-Json | Set-Content $targetFileUri
Write-Host "Settings retrieved."

$templateFile = "$root\main.bicep"
$templateParameterFile = "$root\parameters.json"

Set-AzdSubscriptionContext -TenantId $TenantId `
	-SubscriptionId $SubscriptionId

New-AzDeployment -Location $Location `
	-TemplateFile $templateFile `
	-TemplateParameterFile $templateParameterFile `
	-WhatIf:$WhatIf	

Move-Item $tmpFileUri $targetFileUri -Force