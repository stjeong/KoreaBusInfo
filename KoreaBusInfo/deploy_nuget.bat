
SET BUILDCONFIG=Release
SET PRJNAME=KoreaBusInfo

if '%1' == 'local' SET BUILDCONFIG=Debug

FOR /F %%I IN ("%0") DO SET CURRENTDIR=%%~dpI

msbuild %CURRENTDIR%%PRJNAME%.csproj /p:Configuration=%BUILDCONFIG%;DefineConstants="TRACE" /t:Rebuild
if ERRORLEVEL 1 goto BuildError

robocopy %CURRENTDIR%bin\%BUILDCONFIG%\ %CURRENTDIR%nuget\root\lib\netstandard2.1 /XF *.json
robocopy %CURRENTDIR% %CURRENTDIR%nuget\root %PRJNAME%.nuspec

nuget pack %CURRENTDIR%\nuget\root\%PRJNAME%.nuspec -OutputDirectory %CURRENTDIR%nuget_output
if %ERRORLEVEL% GTR 0 goto BuildError

for /f %%i in ('powershell.exe -ExecutionPolicy RemoteSigned -file %CURRENTDIR%..\getNugetVersion.ps1 %CURRENTDIR%nuget\root\%PRJNAME%.nuspec') do set NUGETVERSION=%%i
echo %NUGETVERSION%

for /f %%i in ('powershell.exe -ExecutionPolicy RemoteSigned -file %CURRENTDIR%..\readFile.ps1 d:\settings\nuget_key.txt') do set NUGETKEY=%%i
echo %NUGETKEY%

if EXIST f:\my_nuget\ (
    robocopy %CURRENTDIR%nuget_output\ f:\my_nuget %PRJNAME%.%NUGETVERSION%.nupkg
    if NOT '%NUGET_PACKAGES%' == '' (
        if EXIST %NUGET_PACKAGES%\%PRJNAME%\%NUGETVERSION% (
            rmdir %NUGET_PACKAGES%\%PRJNAME%\%NUGETVERSION% /S /Q
        )
    )
) ELSE (
    echo No local nuget directory
)

if '%1' == 'local' goto EndOfBuild

nuget push %CURRENTDIR%nuget_output\%PRJNAME%.%NUGETVERSION%.nupkg %NUGETKEY% -src https://www.nuget.org/api/v2/package
goto EndOfBuild

:BuildError
Echo Failed to build


:EndOfBuild