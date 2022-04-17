targetScope = 'resourceGroup'

param location string

var options = opt.outputs.options

module opt 'modules/options.bicep' = {
  name: 'options'
  params: {
    location: location
    projectName: 'logicapp'
    stageName: 'int'
  }
}

module api 'components/appservice-dotnet.bicep' = {
  name: 'api'
  params: {
    options: options
    prefix: 'api'
    runtimeVersion: 'v6.0'
    skuSubTier: '1'
    skuTier: 'Basic'    
  }
}
