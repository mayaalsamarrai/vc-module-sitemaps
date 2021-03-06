SET "SOURCE_DIR=%~dp0%.."
SET "TARGET_DIR=%SOURCE_DIR%\NuGet"

IF NOT DEFINED ProgramFiles(x86) SET ProgramFiles(x86)=%ProgramFiles%
IF NOT DEFINED MSBUILD_PATH IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" (
    FOR /f "usebackq tokens=*" %%i IN (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath`) DO (
        IF EXIST "%%i\MSBuild\15.0\Bin\MSBuild.exe" (
            SET "MSBUILD_PATH=%%i\MSBuild\15.0\Bin\MSBuild.exe"
        )
    )
)
IF NOT DEFINED MSBUILD_PATH IF EXIST "%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" SET MSBUILD_PATH=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
IF NOT DEFINED MSBUILD_PATH IF EXIST "%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe" SET MSBUILD_PATH=%ProgramFiles(x86)%\MSBuild\12.0\Bin\MSBuild.exe
IF NOT DEFINED MSBUILD_PATH SET MSBUILD_PATH=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe

"%MSBUILD_PATH%" "%SOURCE_DIR%\VirtoCommerce.SitemapsModule.sln" /nologo /verbosity:n /t:Build /p:Configuration=Release;Platform="Any CPU"

nuget pack "%SOURCE_DIR%\VirtoCommerce.SitemapsModule.Core\VirtoCommerce.SitemapsModule.Core.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"
nuget pack "%SOURCE_DIR%\VirtoCommerce.SitemapsModule.Data\VirtoCommerce.SitemapsModule.Data.csproj" -IncludeReferencedProjects -Symbols -Properties Configuration=Release -o "%TARGET_DIR%"

@pause