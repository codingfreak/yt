targetScope = 'subscription'

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

@description('A value that distinguishes your environment from others on the planet.')
param yourSeed string

var os = useLinux ? 'lin' : 'win'

resource mainGroup 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: 'rg-${toLower(projectName)}-${os}-${toLower(stage)}'
  location: location
  tags: {
    purpose: 'demo'
    project: projectName
  }
}

module mainResources 'mainResources.bicep' = {
  name: 'resources-main-${os}'
  scope: mainGroup
  params: {
    location: location
    yourSeed: yourSeed
    projectName: projectName
    stage: stage
    useLinux: useLinux
    runtimeStack: runtimeStack
    runtimeStackVersion: runtimeStackVersion
    platformReleaseChannel: platformReleaseChannel
  }
}

output linuxFxVersion string? = mainResources.outputs.?linuxFxVersion

output netFxVersion string? = mainResources.outputs.?netFxVersion

output metaData [] = mainResources.outputs.metaData

output webAppName string = mainResources.outputs.webAppName
