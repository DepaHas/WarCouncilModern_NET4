param(
    [string]$SolutionDir = $(Get-Location)
)

$moduleName = "WarCouncilModern"
$buildDll = Join-Path $SolutionDir "WarCouncilModern_NET4\bin\x64\WarCouncilModern.dll"
$target = Join-Path $env:USERPROFILE "Documents\Mount & Blade II Bannerlord\Modules\$moduleName\bin\x64"

if (Test-Path $buildDll) {
    New-Item -ItemType Directory -Force -Path $target | Out-Null
    Copy-Item $buildDll -Destination $target -Force
    Write-Host "Copied DLL to $target"
} else {
    Write-Host "Build DLL not found: $buildDll"
}