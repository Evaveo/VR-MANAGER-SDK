# EVAVEO VR Manager SDK - GitHub Upload Script
# This script will upload the SDK to GitHub

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "EVAVEO VR Manager SDK - GitHub Upload" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if git is installed
if (!(Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Host "ERROR: Git is not installed!" -ForegroundColor Red
    Write-Host "Please install Git from: https://git-scm.com/download/win" -ForegroundColor Yellow
    exit 1
}

# Navigate to SDK directory
$sdkPath = "C:\Users\Utilisateur\CascadeProjects\EVA-OCTOVR\SDK"
Set-Location $sdkPath

Write-Host "Current directory: $sdkPath" -ForegroundColor Green
Write-Host ""

# Initialize git if not already initialized
if (!(Test-Path ".git")) {
    Write-Host "Initializing Git repository..." -ForegroundColor Yellow
    git init
    git branch -M main
    Write-Host "✓ Git repository initialized" -ForegroundColor Green
} else {
    Write-Host "✓ Git repository already initialized" -ForegroundColor Green
}

Write-Host ""

# Add all files
Write-Host "Adding files to Git..." -ForegroundColor Yellow
git add .
Write-Host "✓ Files added" -ForegroundColor Green
Write-Host ""

# Create commit
Write-Host "Creating commit..." -ForegroundColor Yellow
$commitMessage = "Initial commit: EVAVEO VR Manager SDK v1.0.0

- Unity SDK with full analytics support
- Unreal Engine SDK support
- Comprehensive documentation
- Example projects
- MIT License"

git commit -m $commitMessage
Write-Host "✓ Commit created" -ForegroundColor Green
Write-Host ""

# Add remote
Write-Host "Adding GitHub remote..." -ForegroundColor Yellow
$remoteUrl = "https://github.com/Evaveo/EVAVEO_VR_Manager_SDK.git"

# Check if remote already exists
$existingRemote = git remote get-url origin 2>$null
if ($existingRemote) {
    Write-Host "Remote 'origin' already exists: $existingRemote" -ForegroundColor Yellow
    $response = Read-Host "Do you want to update it? (y/n)"
    if ($response -eq 'y') {
        git remote set-url origin $remoteUrl
        Write-Host "✓ Remote updated" -ForegroundColor Green
    }
} else {
    git remote add origin $remoteUrl
    Write-Host "✓ Remote added" -ForegroundColor Green
}

Write-Host ""

# Push to GitHub
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Ready to push to GitHub!" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Repository: $remoteUrl" -ForegroundColor Yellow
Write-Host ""
Write-Host "This will upload all SDK files to GitHub." -ForegroundColor Yellow
$pushConfirm = Read-Host "Do you want to push now? (y/n)"

if ($pushConfirm -eq 'y') {
    Write-Host ""
    Write-Host "Pushing to GitHub..." -ForegroundColor Yellow
    Write-Host "(You may be prompted for GitHub credentials)" -ForegroundColor Cyan
    Write-Host ""
    
    git push -u origin main
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "========================================" -ForegroundColor Green
        Write-Host "✓ SUCCESS!" -ForegroundColor Green
        Write-Host "========================================" -ForegroundColor Green
        Write-Host ""
        Write-Host "SDK uploaded to: $remoteUrl" -ForegroundColor Green
        Write-Host ""
        Write-Host "Next steps:" -ForegroundColor Cyan
        Write-Host "1. Visit: https://github.com/Evaveo/VR-MANAGER-SDK-" -ForegroundColor White
        Write-Host "2. Create a release (v1.0.0)" -ForegroundColor White
        Write-Host "3. Test the SDK in Unity/Unreal" -ForegroundColor White
        Write-Host "4. Share with developers!" -ForegroundColor White
        Write-Host ""
    } else {
        Write-Host ""
        Write-Host "ERROR: Push failed!" -ForegroundColor Red
        Write-Host "Please check your GitHub credentials and try again." -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Manual push command:" -ForegroundColor Cyan
        Write-Host "git push -u origin main" -ForegroundColor White
    }
} else {
    Write-Host ""
    Write-Host "Push cancelled." -ForegroundColor Yellow
    Write-Host "To push manually later, run:" -ForegroundColor Cyan
    Write-Host "git push -u origin main" -ForegroundColor White
    Write-Host ""
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
