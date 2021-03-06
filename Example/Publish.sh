echo "Starting a build process..."
sleep 1
echo "Building for Windows in 2 seconds..."
sleep 2
dotnet publish Example.csproj --runtime win-x64 --output bin/publish/win-x64
echo "Building for Linux in 2 seconds..."
sleep 2
dotnet publish Example.csproj --runtime linux-x64 --output bin/publish/linux-x64
echo "Done!"
rm -rf bin/Debug/