@echo off
if "%VisualStudioVersion%"=="" call %VS2012CommandPromptBat%

msbuild Build\Build.proj /t:RestorePackages
IF %ERRORLEVEL% NEQ 0 exit /b %ERRORLEVEL%

msbuild NuGet.VisualStudioExtension.sln /p:VisualStudioVersion="14.0" /p:DeployExtension=false /p:EnableCodeAnalysis=true  /p:PlatformTarget=AnyCPU /v:M /m /fl /flp:v=D