# basic install into a build integrated project
function BuildIntegratedInstallPackage {
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act
    Install-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.7
    
    # Assert
    Assert-ProjectJsonDependency $project NuGet.Versioning 1.0.7
    Assert-ProjectJsonLockFilePackage $project NuGet.Versioning 1.0.7
    Assert-ProjectJsonLockFileRuntimeAssembly $project lib/portable-net40+win/NuGet.Versioning.dll
}

# install multiple packages into a project
function BuildIntegratedInstallMultiplePackages {
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act
    Install-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.7
    Install-Package DotNetRDF -version 1.0.8.3533
    
    # Assert
    Assert-ProjectJsonDependency $project NuGet.Versioning 1.0.7
    Assert-ProjectJsonDependency $project DotNetRDF 1.0.8.3533
    Assert-ProjectJsonLockFilePackage $project NuGet.Versioning 1.0.7
    Assert-ProjectJsonLockFilePackage $project DotNetRDF 1.0.8.3533
    Assert-ProjectJsonLockFilePackage $project Newtonsoft.Json 6.0.8
    Assert-ProjectJsonLockFileRuntimeAssembly $project lib/portable-net40+win/NuGet.Versioning.dll
    Assert-ProjectJsonLockFileRuntimeAssembly $project lib/netcore45/Newtonsoft.Json.dll
    Assert-ProjectJsonLockFileRuntimeAssembly $project lib/portable-net4+sl5+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1/dotNetRDF.dll
    Assert-ProjectJsonLockFileRuntimeAssembly $project lib/portable-net4+sl5+netcore45+wpa81+wp8+MonoAndroid1+MonoTouch1/Portable.Runtime.dll
}

# install and then uninstall multiple packages
function BuildIntegratedInstallAndUninstallAll {
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act
    Install-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.7
    Install-Package DotNetRDF  -ProjectName $project.Name -version 1.0.8.3533
    Uninstall-Package NuGet.Versioning -ProjectName $project.Name
    Uninstall-Package DotNetRDF -ProjectName $project.Name
    
    # Assert
    Assert-ProjectJsonDependencyNotFound $project NuGet.Versioning
    Assert-ProjectJsonDependencyNotFound $project DotNetRDF
    Assert-ProjectJsonLockFilePackageNotFound $project NuGet.Versioning
    Assert-ProjectJsonLockFilePackageNotFound $project DotNetRDF
    Assert-ProjectJsonLockFilePackageNotFound $project Newtonsoft.Json
}

# install a package with dependencies
function BuildIntegratedInstallAndVerifyLockFileContainsChildDependency {
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act
    Install-Package json-ld.net -ProjectName $project.Name -version 1.0.4
    
    # Assert
    Assert-ProjectJsonLockFilePackage $project Newtonsoft.Json 4.0.1
    Assert-ProjectJsonDependencyNotFound $project Newtonsoft.Json
} 

# basic uninstall
function BuildIntegratedUninstallPackage {    
    # Arrange
    $project = New-UAPApplication UAPApp
    Install-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.7

    # Act
    Uninstall-Package NuGet.Versioning -ProjectName $project.Name
    
    # Assert
    Assert-ProjectJsonDependencyNotFound $project NuGet.Versioning
    Assert-ProjectJsonLockFilePackageNotFound $project NuGet.Versioning
}

# basic update package
function BuildIntegratedUpdatePackage {    
    # Arrange
    $project = New-UAPApplication UAPApp
    Install-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.5

    # Act
    Update-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.6
    
    # Assert
    Assert-ProjectJsonDependency $project NuGet.Versioning 1.0.6
    Assert-ProjectJsonLockFilePackage $project NuGet.Versioning 1.0.6
    Assert-ProjectJsonLockFileRuntimeAssembly $project lib/portable-net40+win/NuGet.Versioning.dll
}

function BuildIntegratedUpdateNonExistantPackage {    
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act and Assert
    Assert-Throws { Update-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.6 } "'NuGet.Versioning' was not installed in any project. Update failed."
}

function BuildIntegratedUninstallNonExistantPackage {    
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act and Assert
    Assert-Throws { Uninstall-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.6 } "Package 'NuGet.Versioning' to be uninstalled could not be found in project 'UAPApp'"
}

function BuildIntegratedLockFileIsCreatedOnBuild {
    # Arrange
    $project = New-UAPApplication UAPApp
    Install-Package NuGet.Versioning -ProjectName $project.Name -version 1.0.7
    Remove-ProjectJsonLockFile $project

    # Act
    Build-Solution
    
    # Assert
    Assert-ProjectJsonLockFilePackage $project NuGet.Versioning 1.0.7
}

function BuildIntegratedInstallPackagePrefersWindowsOverWindowsPhoneApp {
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act
    Install-Package automapper -ProjectName $project.Name -version 3.3.1
    
    # Assert
    Assert-ProjectJsonLockFileRuntimeAssembly $project lib/windows8/AutoMapper.dll
}

function BuildIntegratedInstallPackageWithWPA81 {
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act
    Install-Package kinnara.toolkit -ProjectName $project.Name -version 0.3.0
    
    # Assert
    Assert-ProjectJsonLockFileRuntimeAssembly $project lib/wpa81/Kinnara.Toolkit.dll
}

function BuildIntegratedPackageOverrideDependencyRequirement {
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act
    Install-Package Newtonsoft.Json -ProjectName $project.Name -version 6.0.4
    Install-Package DotNetRDF  -ProjectName $project.Name -version 1.0.8.3533
    
    # Assert
    # DotNetRDF requires json.net >= 6.0.8, but the direct dependency overrides it
    Assert-ProjectJsonLockFilePackage $project Newtonsoft.Json 6.0.4
}

function BuildIntegratedDependencyUpdatedByInstall {
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act
    Install-Package DotNetRDF  -ProjectName $project.Name -version 1.0.8.3533
    Install-Package Newtonsoft.Json -ProjectName $project.Name -version 7.0.1-beta3
    
    # Assert
    # DotNetRDF requires json.net 6.0.8
    Assert-ProjectJsonLockFilePackage $project Newtonsoft.Json 7.0.1-beta3
}

function BuildIntegratedInstallPackageJsonNet701Beta3 {
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act
    Install-Package newtonsoft.json -ProjectName $project.Name -version 7.0.1-beta3
    
    # Assert
    Assert-ProjectJsonLockFileRuntimeAssembly $project "lib/portable-net45+wp80+win8+wpa81+dnxcore50/Newtonsoft.Json.dll"
}


function BuildIntegratedProjectClosure {
    # Arrange
    $project1 = New-UAPApplication Project1
    $project2 = New-UAPApplication Project2
    $project3 = New-UAPApplication Project3

    Add-ProjectReference $project1 $project2
    Add-ProjectReference $project2 $project3

    Install-Package NuGet.Versioning -ProjectName $project3.Name -version 1.0.7
    Remove-ProjectJsonLockFile $project3

    # Act
    Build-Solution
    
    # Assert
    Assert-ProjectJsonLockFilePackage $project3 NuGet.Versioning 1.0.7
    Assert-ProjectJsonLockFilePackage $project2 NuGet.Versioning 1.0.7
    Assert-ProjectJsonLockFilePackage $project1 NuGet.Versioning 1.0.7
}

function Test-BuildIntegratedInstallNonExistantPackage {
    
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act and Assert
    Assert-Throws { Install-Package NuGet.Versioning -ProjectName $project.Name -version 988.922.188 } "Package restore failed. Rolling back package changes."
}

function Test-BuildIntegratedInstallNonExistantPackageId {    
    # Arrange
    $project = New-UAPApplication UAPApp

    # Act and Assert
    Assert-Throws { Install-Package NuGet.NotFound -ProjectName $project.Name -version 988.922.188 } "Package restore failed. Rolling back package changes."
}

function Test-BuildIntegratedProjectClosureWithLegacyProjects {
    # Arrange
    $project1 = New-UAPApplication Project1
    $project2 = New-WindowsPhoneClassLibrary Project2
    $project3 = New-WindowsPhoneClassLibrary Project3

    Add-ProjectReference $project1 $project2
    Add-ProjectReference $project2 $project3

    Install-Package Comparers -ProjectName $project2.Name -version 4.0.0

    # Act
    Build-Solution
    
    # Assert
    Assert-NotNull Get-ProjectJsonLockFile $project1
}

# Tests that packages are restored on build
function Test-BuildIntegratedMixedLegacyProjects {
    param($context)

    # Arrange
    $p1 = New-ClassLibrary
    $p1 | Install-Package Newtonsoft.Json -Version 5.0.6

    $p2 = New-UAPApplication UAPApp
    $p2 | Install-Package NuGet.Versioning -Version 1.0.7

    # delete the packages folder
    $packagesDir = Get-PackagesDir
    RemoveDirectory $packagesDir
    Assert-False (Test-Path $packagesDir)

    # Act
    Build-Solution

    # Assert
    Assert-True (Test-Path $packagesDir)
    Assert-Package $p1 Newtonsoft.Json
    Assert-ProjectJsonLockFilePackage $project NuGet.Versioning 1.0.7
}