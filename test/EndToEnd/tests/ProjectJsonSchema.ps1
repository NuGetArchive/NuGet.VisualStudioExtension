function Test-BuildIntegratedSchemaUrl {
    # Arrange
    $p = New-BuildIntegratedProj UAPApp

    # Act
	#$componentModel = Get-VSComponentModel
    #$schemaSelector = $componentModel.GetService([Microsoft.JSON.Core.Schema.IJSONSchemaSelector])

	$schemaSelector = [NuGet.PackageManagement.VisualStudio.ServiceLocator]::GetInstanceSafe([IJSONSchemaSelector])

	Write-Host $schemaSelector	
	Assert-NotNull $schemaSelector "selector"
	$item = Get-ProjectItem $p project.json
	Write-Host $item
	Assert-NotNull $item "item"
	$schemaUrl = $schemaSelector.GetSchemaFor($item)

    # Assert
	Assert-NotNull $schemaUrl
	Assert-AreEqual($schemaUrl, "http://json.schemastore.org/nuget-project")
}

function LegacyProjectSchemaUrl {
    # Arrange
    $p = New-ClassLibrary

    # Act
	$componentModel = Get-VSComponentModel
    $schemaSelector = $componentModel.GetService([Microsoft.JSON.Core.Schema.IJSONSchemaSelector])
	$item = Get-ProjectItem $p packages.config
	$schemaUrl = $schemaSelector.GetSchemaFor($item)

    # Assert
	Assert-Null $schemaUrl
}