@ECHO off
REM Copyright (c) 2025 Ekkodale GmbH. All rights reserved.
REM
REM This file is part of the gaeco platform system.
REM
REM Use of this file is governed by the terms of the license
REM in LICENSE.md at the root of this repository.
REM Unauthorized copying, modification, distribution, or use of this file,
REM via any medium, is strictly prohibited except as expressly permitted
REM under that license.

set dockerFileName=usecaseservice
set dockerTagVersion=latest
set csprojFileName=UseCaseService.csproj

@RD /S /Q "out"
dotnet clean %csprojFileName%
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet build %csprojFileName%
if %errorlevel% neq 0 exit /b %errorlevel%
dotnet publish --configuration Release %csprojFileName% -o "out"
if %errorlevel% neq 0 exit /b %errorlevel%

docker build --no-cache --rm  --label "created-by:ekkodale" -f "Dockerfile-bat" -t %dockerFileName%:%dockerTagVersion% "."
if %errorlevel% neq 0 exit /b %errorlevel%

