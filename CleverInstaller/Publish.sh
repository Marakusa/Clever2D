echo "Starting a build process..."
echo "Building for Windows..."
dotnet publish CleverInstaller.csproj --runtime win-x64 --output bin/publish/win-x64
echo "Building for Linux..."
dotnet publish CleverInstaller.csproj --runtime ubuntu.18.04-x64 --output bin/publish/linux-x64
echo "Done!"
rm -rf bin/Debug/