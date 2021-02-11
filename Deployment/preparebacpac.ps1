
# returns true if input is valid GUID else false
function IsValidGuid {
    [OutputType([bool])]
    param
    (
        [Parameter(Mandatory = $true)]
        [string]$ObjectGuid
    )

    # Define verification regex
    [regex]$guidRegex = '(?im)^[{(]?[0-9A-F]{8}[-]?(?:[0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$'

    # Check guid against regex
    return $ObjectGuid -match $guidRegex
}

function SetupParameters{

    # The TenantId in which to create these objects
    do{
        $tenantId = Read-Host -Prompt "Enter tenant id where Azure resources are going to be created"
    }while(-not(IsValidGuid -ObjectGuid $tenantId))

    # The SubscriptionId in which to create these objects
    do{
        $subscriptionId = Read-Host -Prompt "Enter Azure subscription id"
    }while(-not(IsValidGuid -ObjectGuid $subscriptionId))

    # Base resource name where storage will be created to host bacpac file
    do{
        $baseResourceName = Read-Host -Prompt "Enter a base resource name between 2-16 characters only with no special character"
    }while($baseResourceName.Length -lt 2 -or $baseResourceName.Length -gt 16)


    $location = Read-Host -Prompt "Enter the location where resource group name is located (e.g. westus, eastus, etc.)"

    $resourceGroupName = Read-Host -Prompt "Enter resource group name where new storage should be created"

    $storageAccountName = "${baseResourceName}storage"

    $containerName = "${baseResourceName}container"

    $bacpacFileName = "LearnNowDB.bacpac"

    $bacpacUrl = Read-Host -Prompt "Enter raw GitHub file path to bacpac like https://github.com/officedev/msteams-app-coursecompanion/blob/master/Deployment/LearnNowDB.bacpac?raw=true"

}

function InstallAzModule {
    if ($PSVersionTable.PSEdition -eq 'Desktop' -and (Get-Module -Name AzureRM -ListAvailable)) {
    Write-Warning -Message ('Az module not installed. Having both the AzureRM and ' +
      'Az modules installed at the same time is not supported.')
    } else {
        Install-Module -Name Az -AllowClobber -Scope CurrentUser
    }
}

function SetCurrentSubscriptionContext {
    
    Connect-AzAccount -TenantId $tenantId

    Set-AzContext -SubscriptionId $subscriptionId
}

function CreateStorageWithBacpac {

    # Install Az module if not already for current user
    InstallAzModule

    # Set up Azure context for provided subscription id
    SetCurrentSubscriptionContext

    # Download the bacpac file
    Invoke-WebRequest -Uri $bacpacUrl -OutFile "$HOME/$bacpacFileName"

    # Create a storage account
    $storageAccount = New-AzStorageAccount -ResourceGroupName $resourceGroupName `
                                           -Name $storageAccountName `
                                           -SkuName Standard_LRS `
                                           -Location $location
									   
    $storageAccountKey = (Get-AzStorageAccountKey -ResourceGroupName $resourceGroupName `
                                                  -Name $storageAccountName).Value[0]

    # Create a container
    New-AzStorageContainer -Name $containerName -Context $storageAccount.Context

    # Upload the BACPAC file to the container
    Set-AzStorageBlobContent -File $HOME/$bacpacFileName `
                             -Container $containerName `s
                             -Blob $bacpacFileName `
                             -Context $storageAccount.Context

    if($storageAccountKey -ne "")
    {
        Write-Host "The storage account name is $storageAccountName"
        Write-Host "The container name is $containerName"
        Write-Host "The storage account key is $storageAccountKey"
        Write-Host "The BACPAC file URL is https://$storageAccountName.blob.core.windows.net/$containerName/$bacpacFileName"
        Write-Host "Press [ENTER] to continue ..."
    }
    else
    {
        Write-Host "An exception occured while generating the storage key. Please check all parameters and run script again."
    }

}

# step 1 - execute set up parameters; 
SetupParameters

# step 2 - create storage and upload bacpac file
CreateStorageWithBacpac