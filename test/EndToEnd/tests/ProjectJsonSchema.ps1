function Test-BuildIntegratedSchemaUrl {
    # Arrange
    $p = New-BuildIntegratedProj UAPApp

    # Act
	$componentModel = Get-VSComponentModel
	# Multiple exports can be found for this interface.
    $schemaSelectors = $componentModel.GetExtensions([Microsoft.JSON.Core.Schema.IJSONSchemaSelector])

	$item = Get-ProjectItem $p project.json
	$path = $item.Properties.Item("FullPath").Value

	# NuGet schema selector is called following a fixed order
	$nugetSelector = $schemaSelectors[3]
	$schemaUrl = $nugetSelector.GetSchemaFor($path)

    # Assert
	Assert-AreEqual $nugetSelector.ToString() NuGet.VisualStudio.NuGetProjectJSONSchemaSelector
	Assert-NotNull $schemaUrl
	Assert-AreEqual $schemaUrl 'http://json.schemastore.org/nuget-project'
}

function Test-LegacyProjectSchemaUrl {
    # Arrange
    $p = New-ClassLibrary
	Install-Package jQuery -version 2.1.4

    # Act
	$componentModel = Get-VSComponentModel
    $schemaSelectors = $componentModel.GetExtensions([Microsoft.JSON.Core.Schema.IJSONSchemaSelector])

	$item = Get-ProjectItem $p packages.config
	$path = $item.Properties.Item("FullPath").Value

	$nugetSelector = $schemaSelectors[3]
	$schemaUrl = $nugetSelector.GetSchemaFor($path)

    # Assert
	Assert-AreEqual $nugetSelector.ToString() NuGet.VisualStudio.NuGetProjectJSONSchemaSelector
	Assert-Null $schemaUrl
}

function Test-DNXProjectSchemaUrl {
    # Arrange
    $p = New-DNXClassLibrary

    # Act
	$componentModel = Get-VSComponentModel
    $schemaSelectors = $componentModel.GetExtensions([Microsoft.JSON.Core.Schema.IJSONSchemaSelector])

	$item = Get-ProjectItem $p packages.config
	$path = $item.Properties.Item("FullPath").Value

	$nugetSelector = $schemaSelectors[3]
	$schemaUrl = $nugetSelector.GetSchemaFor($path)

    # Assert
	Assert-AreEqual $nugetSelector.ToString() NuGet.VisualStudio.NuGetProjectJSONSchemaSelector
	Assert-Null $schemaUrl
}