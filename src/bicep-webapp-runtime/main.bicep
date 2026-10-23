targetScope = 'subscription'

@description('The name of the project.')
param projectName string

@description('The Azure region where to deploy to.')
param location string

@description('The name of the stage.')
param stage string

resource mainGroup 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: 'rg-${toLower(projectName)}-${toLower(stage)}'
  location: location
  tags: {
    purpose: 'demo'
    project: projectName
  }
}

module mainResources 'mainResources.bicep' = {
  name: 'resources-main'
  scope: mainGroup
  params: {
    location: location
    projectName: projectName
    stage: stage
  }
}
