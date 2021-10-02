echo "Starting a build process..."
echo "Building for Windows..."
dotnet build Example.csproj --runtime win-x64 --output bin/build/win-x64
echo "Building for Linux in 1 second..."
sleep 1
dotnet build Example.csproj --runtime linux-x64 --output bin/build/linux-x64
sleep 1
echo "Done!"