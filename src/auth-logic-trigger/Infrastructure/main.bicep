targetScope = 'subscription'

param location string

resource group 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-cf-logicapp'
  location: location
  tags: {
    purpose: 'demo'
  }
}

module resources 'resources.bicep' = {
  scope: group
  name: 'resources-deployment'
  params: {
    location: location
  }
}
