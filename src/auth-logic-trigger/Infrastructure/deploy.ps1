

New-AzDeployment `
		-Name $deploymentName `
		-Location $location `
		-TemplateFile $templateFile `
		-TemplateParameterFile $templateParameterFile `
		-DeploymentDebugLogLevel All `
		-Mode Incremental `
		-WhatIf:$WhatIf