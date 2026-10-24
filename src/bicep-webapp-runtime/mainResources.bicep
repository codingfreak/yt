targetScope = 'resourceGroup'

@description('The name of the project.')
param projectName string

@description('The Azure region where to deploy to.')
param location string

@description('The name of the stage.')
param stage string

@description('Determines if Linux should be used')
param useLinux bool = false

@description('The tech stack to run on the web app.')
@allowed([
  'dotnet'
  'PHP'
])
param runtimeStack string = 'dotnet'

@description('The version for the selected runtime stack to run.')
param runtimeStackVersion string = '10.0'

@description('The channel for releases to use for the runtime stack. This is only supported for PHP.')
@allowed([
  'Latest'
  'Standard'
  'Extended'
])
param platformReleaseChannel string = 'Standard'

var os = useLinux ? 'lin' : 'win'
var suffix = 'cf-${toLower(projectName)}-${os}-${toLower(stage)}'
var netFxVersion = runtimeStack == 'dotnet' ? 'v${runtimeStackVersion}' : null
var linuxFxVersion = useLinux ? '${runtimeStack}|${runtimeStackVersion}' : null
var metaData = !useLinux
  ? [
      { name: 'CURRENT_STACK', value: 'dotnet' }
    ]
  : []
var siteConfig resourceInput<'Microsoft.Web/sites@2025-03-01'>.properties.siteConfig = {
  ftpsState: 'Disabled'
  http20Enabled: true
  minTlsVersion: '1.3'
  scmMinTlsVersion: '1.3'
  healthCheckPath: '/'
  use32BitWorkerProcess: false
  netFrameworkVersion: netFxVersion
  linuxFxVersion: linuxFxVersion
  metadata: metaData
}
var properties resourceInput<'Microsoft.Web/sites@2025-03-01'>.properties = {
  serverFarmId: appServicePlan.id
  siteConfig: siteConfig
  httpsOnly: true
  clientAffinityEnabled: true
}
var resolvedProperties = union(properties, {
  platformReleaseChannel: platformReleaseChannel
})

resource appServicePlan 'Microsoft.Web/serverfarms@2025-03-01' = {
  name: 'asp-${suffix}'
  location: location
  sku: {
    family: 'B'
    name: 'B1'
  }
  properties: {
    reserved: useLinux
  }
}

resource webApp 'Microsoft.Web/sites@2025-03-01' = {
  name: 'ui-${suffix}'
  location: location
  kind: useLinux ? 'app,linux' : 'app'
  properties: resolvedProperties
}

output linuxFxVersion string? = linuxFxVersion

output netFxVersion string? = netFxVersion

output metaData [] = metaData
