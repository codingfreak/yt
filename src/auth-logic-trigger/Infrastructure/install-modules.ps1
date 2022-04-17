# !!! We need to change directory because -OutputDirectory of nuget install will ignore the current
# script root !!!
if ($PSScriptRoot.Contains(' ') -and $PSScriptRoot -ne $PWD) {
	throw "This script needs to be executed from inside its folder because white spaces where detected."
}
$root = $PSScriptRoot.Contains(' ') ? '.' : $PSScriptRoot

nuget install devdeer.Templates.Bicep -Source nuget.org -OutputDirectory $root -Version 2.0.2

# remove existing modules and components
if (Test-Path -Path "$root/modules") {
	Remove-Item "$root/modules" -Recurse
}
if (Test-Path -Path "$root/components") {
	Remove-Item "$root/components" -Recurse
}
# move items modules and components one level up from the nuget path
Move-Item "$root/devdeer.Templates.Bicep*/modules" $root -Force
Move-Item "$root/devdeer.Templates.Bicep*/components" $root -Force
# try to remove the nuget installation package
try {
	Remove-Item "$root/devdeer.Templates.Bicep*" -Recurse	
}
catch {
	# probably we are on the build server herebi
	Write-Host "Could not remove nuget installation folder"
}
