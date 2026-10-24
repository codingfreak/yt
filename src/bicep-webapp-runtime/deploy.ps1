[CmdletBinding()]
param (
    [ValidateSet("windows", "linux")]
    $Type,
    [switch]
    $WhatIf
)

New-AzDeployment -Name 'deploy-cf-sample' `
    -Location 'westeurope' `
    -TemplateFile ./main.bicep `
    -TemplateParameterFile "./$Type.bicepparam" `
    -WhatIf:$WhatIf `
    -WhatIfResultFormat ResourceIdOnly