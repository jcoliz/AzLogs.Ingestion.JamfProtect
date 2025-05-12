# Sample logs generator for Jamf Protect

## Prerequisites

In order to follow the instructions shown here, and run this sample, you will first need:

* An Azure Account. Set up a [Free Azure Account](https://azure.microsoft.com/en-us/pricing/purchase-options/azure-account) to get started.
* [Azure CLI tool with Bicep](https://learn.microsoft.com/en-us/azure/azure-resource-manager/bicep/install#azure-cli)

Please read through the [Logs Ingestion API in Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/logs-ingestion-api-overview) article carefully before proceeding.

## Getting started

1. Clone this repository with submodules
1. Ensure you are logged into your Azure Subscription to the correct account using the Azure CLI tool
1. Deploy a fresh Microsoft Sentinel workspace to your Azure Subscription
1. Navigate to your fresh workspace using the Azure Portal
1. Find the "Jamf Protect" solution in the Content Hub. Install it.
1. Find the "Jamf Protect Push Connector" in the Data Connectors. Open the connector page.
1. Press "Deploy Jamf Protect Connector Resources"
1. Open your favorite code editor, e.g. VS Code. Open the local folder where you have cloned this repository.
1. Copy the [config.template.toml](./WorkerApp/config.template.toml) file to `config.toml`, also in the [WorkerApp](./WorkerApp/) folder.
1. Fill the blanks in `config.toml` using the information shown on the connector page
1. Run the `WorkerApp` project
1. Check your DCR metrics
1. Check your Sentinel logs
1. Check your connector page

### Deploy Microsoft Sentinel workspace

Deploy a fresh Microsoft Sentinel workspace to your Azure Subscription

```dotnetcli
.\.azure\deploy\Deploy-Services.ps1 -ResourceGroup sentinel-jamf -Location westus
```

### Run the `WorkerApp` project

```dotnetcli
dotnet run --project .\WorkerApp\
```

```dotnetcli
<6> [ 12/05/2025 13:28:20 ] Microsoft.Hosting.Lifetime[0] Application started. Press Ctrl+C to shut down.
<6> [ 12/05/2025 13:28:20 ] Microsoft.Hosting.Lifetime[0] Hosting environment: Development
<6> [ 12/05/2025 13:28:20 ] Microsoft.Hosting.Lifetime[0] Content root path: .\AzLogs.Ingestion.JamfProtect\WorkerApp
<6> [ 12/05/2025 13:28:21 ] WorkerApp.Worker[1100] UploadToLogsAsync: Sent OK 204 to Custom-jamfprotecttelemetryv2
<6> [ 12/05/2025 13:28:21 ] WorkerApp.Worker[1100] UploadToLogsAsync: Sent OK 204 to Custom-jamfprotectunifiedlogs
<6> [ 12/05/2025 13:28:21 ] WorkerApp.Worker[1100] UploadToLogsAsync: Sent OK 204 to Custom-jamfprotectalerts
<6> [ 12/05/2025 13:28:21 ] WorkerApp.Worker[1000] ExecuteAsync: OK
<6> [ 12/05/2025 13:28:26 ] WorkerApp.Worker[1100] UploadToLogsAsync: Sent OK 204 to Custom-jamfprotecttelemetryv2
<6> [ 12/05/2025 13:28:26 ] WorkerApp.Worker[1100] UploadToLogsAsync: Sent OK 204 to Custom-jamfprotectunifiedlogs
<6> [ 12/05/2025 13:28:26 ] WorkerApp.Worker[1100] UploadToLogsAsync: Sent OK 204 to Custom-jamfprotectalerts
<6> [ 12/05/2025 13:28:26 ] WorkerApp.Worker[1000] ExecuteAsync: OK
```
