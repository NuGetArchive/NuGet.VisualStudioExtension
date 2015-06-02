@echo off
%teamcity.tool.NuGet.CommandLine.2.8.5.nupkg%\tools\nuget.exe restore NuGet.VisualStudioExtension.sln -source %env.NUGET_PUSH_TARGET%;https://www.nuget.org/api/v2

build.cmd