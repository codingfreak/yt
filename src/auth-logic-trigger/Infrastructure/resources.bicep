targetScope = 'resourceGroup'

param location string

var coreEnvironment = [
  {
    name: 'ASPNETCORE_ENVIRONMENT'
    value: 'Integration'
  }
]

resource asp 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: 'asp-cf-sample-int'  
  location: location
  kind: 'linux'
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
  properties: {
    reserved: true
  }
}

resource webapp 'Microsoft.Web/sites@2021-03-01' = {
  name: 'api-dd-sample-int'
  location:location  
  kind: 'app,linux'
  properties: {
    serverFarmId: asp.id
    httpsOnly: true
    reserved: true    
    siteConfig: {
      appSettings: coreEnvironment
      alwaysOn: false
      http20Enabled: true
      ftpsState: 'Disabled' 
      linuxFxVersion: 'DOTNETCORE|6.0'     
    }
  }
}

