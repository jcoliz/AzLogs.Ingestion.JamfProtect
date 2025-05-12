param(
    [Parameter(Mandatory=$true)]
    [string]
    $ResourceGroup,
    [Parameter(Mandatory=$true)]
    [string]
    $Location
)

Write-Output "Creating Resource Group $ResourceGroup in $Location"
az group create --name $ResourceGroup --location $Location

Write-Output "Deploying to Resource Group $ResourceGroup"
$result = az deployment group create --name "Deploy-$(Get-Random)" --resource-group $ResourceGroup --template-file $PSScriptRoot\AzDeploy.Bicep\SecurityInsights\sentinel-complete.bicep | ConvertFrom-Json

Write-Output "OK"
Write-Output ""

$logAnalyticsName = $result.properties.outputs.logAnalyticsName.value

Write-Output "Deployed sentinel workspace $logAnalyticsName"