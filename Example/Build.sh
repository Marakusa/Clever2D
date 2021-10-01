echo "Starting a build process..."
sleep 1
echo "Building for Windows in 2 seconds..."
sleep 2
dotnet build Example.csproj --runtime win-x64 --output bin/build/win-x64
echo "Building for Linux in 2 seconds..."
sleep 2
dotnet build Example.csproj --runtime ubuntu.18.04-x64 --output bin/build/linux-x64
echo "Done!"