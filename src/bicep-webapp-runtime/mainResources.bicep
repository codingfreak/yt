targetScope = 'resourceGroup'

@description('The name of the project.')
param projectName string

@description('The Azure region where to deploy to.')
param location string

@description('The name of the stage.')
param stage string

var suffix = 'cf-${toLower(projectName)}-${toLower(stage)}'

resource appServicePlan 'Microsoft.Web/serverfarms@2025-03-01' = {
  name: 'asp-${suffix}'
  location: location
  properties: {}
}

resource webApp 'Microsoft.Web/sites@2025-03-01' = {
  name: 'ui-${suffix}'
  location: location
  properties: {}
}
