# Container Apps in westeurope

## Summary

Starting in approximately May 2026 there where errors when one tried to deploy container apps to the West Europe region in Azure:

> Provisioning 'ErrorCode: ManagedEnvironmentCapacityHeavyUsageError, Message: Creating a new managed environment is unavailable at this time due to high demand in current region. To create a new managed environment, we recommend using an alternate region. For a list of all the Azure regions, visit Azure Product by Region | Microsoft Azure. For more details, visit https://aka.ms/akscapacityheavyusage' failed.

The only option was to deploy somewhere else (lets say Sweden Central).

I wanted to showcase this in my [Azure F\*\*\*Up Series]() on YT but today it worked like a charm (of course!).

There was some discussion on [Reddit](https://www.reddit.com/r/AZURE/comments/1tp0fae/container_apps_environment_akscapacityheavyusage/) as well so it wasn't just me.

## Usage

So if you want to try it yourself using this code here, be sure to place a PowerShell prompt in this directory and then:

```powershell
$tenantId = 'YOUR_TENANT_ID'
$subscriptionId = 'YOUR_SUBSCRIPTION_ID'
$rgName = 'YOUR_RG_NAME'
Connect-AzAccount -Tenant $tenantId -Subscription $subscriptionId
New-AzResourceGroup -Name $resourceGroupName -Location westeurope
New-AzResourceGroupDeployment -ResourceGroupName $rgName `
    -TemplateFile .\main.bicep `
    -TemplateParameterFile .\weu.bicepparam`
```

If this works without error and you can visit the URL posted in the output, you're good. If not remove everything with

```powershell
Remove-AzResourceGroup -Name $rgName -Force
```

And wait a little bit because the environment removal can take up to 15 minutes and try again with

```powershell
New-AzResourceGroup -Name $resourceGroupName -Location swedencentral
New-AzResourceGroupDeployment -ResourceGroupName $rgName `
    -TemplateFile .\main.bicep `
    -TemplateParameterFile .\swe.bicepparam`
```
