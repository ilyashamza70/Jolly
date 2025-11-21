# Jolly - Detailed Setup Guide

## Quick Start

### Step 1: Prerequisites

Make sure you have the following installed on your system:
- **.NET 10.0 SDK** or later
  - Download from: https://dotnet.microsoft.com/download
  - Verify installation: `dotnet --version`

### Step 2: Clone and Run

```bash
# Clone the repository
git clone https://github.com/ilyashamza70/Jolly.git
cd Jolly

# Build the application
dotnet build

# Run the application
dotnet run
```

### Step 3: Access the Application

Once running, you'll see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

Open your web browser and navigate to:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001 (recommended)

## Platform-Specific Instructions

### Windows (PC)

1. Install .NET SDK from Microsoft's website
2. Open Command Prompt or PowerShell
3. Navigate to the project directory
4. Run: `dotnet run`
5. Access via browser at localhost:5001

### macOS

1. Install .NET SDK using the installer or Homebrew:
   ```bash
   brew install --cask dotnet-sdk
   ```
2. Open Terminal
3. Navigate to the project directory
4. Run: `dotnet run`
5. Access via browser at localhost:5001

### Linux

1. Install .NET SDK using your package manager:
   ```bash
   # Ubuntu/Debian
   sudo apt-get update
   sudo apt-get install -y dotnet-sdk-10.0
   
   # Fedora
   sudo dnf install dotnet-sdk-10.0
   ```
2. Open Terminal
3. Navigate to the project directory
4. Run: `dotnet run`
5. Access via browser at localhost:5001

### Android

1. Ensure the application is running on a PC within your network
2. Find your PC's local IP address:
   - Windows: `ipconfig`
   - Mac/Linux: `ifconfig` or `ip addr`
3. Open Chrome or any browser on Android
4. Navigate to: `http://[YOUR_PC_IP]:5000`
5. Add to home screen for app-like experience

### iOS

1. Ensure the application is running on a PC within your network
2. Find your PC's local IP address (same as Android instructions)
3. Open Safari or any browser on iOS
4. Navigate to: `http://[YOUR_PC_IP]:5000`
5. Tap Share → Add to Home Screen for app-like experience

## Network Access Configuration

### Allowing External Connections

By default, the app only accepts local connections. To allow access from other devices:

1. **Modify Program.cs** or **appsettings.json** to specify URLs:

Edit `Program.cs` or run with:
```bash
dotnet run --urls "http://0.0.0.0:5000;https://0.0.0.0:5001"
```

2. **Configure Firewall**:
   - Windows: Allow incoming connections on ports 5000 and 5001
   - Mac: System Preferences → Security & Privacy → Firewall
   - Linux: Use `ufw` or `iptables` to allow ports

3. **Find Your Local IP**:
   - Windows: `ipconfig` → Look for IPv4 Address
   - Mac: System Preferences → Network
   - Linux: `ip addr show` or `ifconfig`

## Production Deployment

### Using IIS (Windows)

1. Publish the application:
   ```bash
   dotnet publish -c Release
   ```

2. Install IIS with ASP.NET Core Module
3. Create a new website in IIS Manager
4. Point to the published folder
5. Configure application pool to use "No Managed Code"

### Using Nginx (Linux)

1. Publish the application:
   ```bash
   dotnet publish -c Release
   ```

2. Install Nginx and configure as reverse proxy:
   ```nginx
   server {
       listen 80;
       server_name yourdomain.com;
       
       location / {
           proxy_pass http://localhost:5000;
           proxy_http_version 1.1;
           proxy_set_header Upgrade $http_upgrade;
           proxy_set_header Connection keep-alive;
           proxy_set_header Host $host;
       }
   }
   ```

3. Create a systemd service for the application

### Using Docker

Create a `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY bin/Release/net10.0/publish/ .
ENTRYPOINT ["dotnet", "JollyApp.dll"]
```

Build and run:
```bash
dotnet publish -c Release
docker build -t jolly-app .
docker run -p 5000:5000 -p 5001:5001 jolly-app
```

## Troubleshooting

### Application Won't Start

1. **Check .NET Version**:
   ```bash
   dotnet --version
   ```
   Should be 10.0 or higher

2. **Check Port Availability**:
   - Ports 5000 and 5001 might be in use
   - Change ports in `Properties/launchSettings.json`

3. **Build Errors**:
   ```bash
   dotnet clean
   dotnet restore
   dotnet build
   ```

### Can't Access From Other Devices

1. **Check Firewall**: Ensure ports are open
2. **Check Network**: Devices must be on same network
3. **Use IP Address**: Don't use "localhost" from other devices
4. **Check Binding**: Application must bind to 0.0.0.0, not 127.0.0.1

### Search Not Working

- Currently uses mock data for demonstration
- Real Spotify API integration requires API credentials

## Development Tips

### Running in Development Mode

```bash
dotnet run --environment Development
```

### Watching for Changes

```bash
dotnet watch run
```

This automatically rebuilds when you change code files.

### Debugging in Visual Studio

1. Open `JollyApp.csproj` or the solution
2. Press F5 to start debugging
3. Set breakpoints in your code
4. Use the debugging tools to inspect variables

### Debugging in VS Code

1. Install C# extension
2. Open folder in VS Code
3. Press F5 to start debugging
4. Configuration is auto-generated

## Performance Optimization

### For Production Use

1. **Enable Response Compression**:
   ```csharp
   builder.Services.AddResponseCompression();
   ```

2. **Add Caching**:
   ```csharp
   builder.Services.AddResponseCaching();
   ```

3. **Use Production Build**:
   ```bash
   dotnet publish -c Release
   ```

## Security Considerations

1. **HTTPS**: Always use HTTPS in production
2. **Authentication**: Add authentication for business panel
3. **Rate Limiting**: Implement to prevent abuse
4. **Input Validation**: Already implemented in API controllers
5. **CORS**: Configure appropriate CORS policy for production

## Getting Help

- **Issues**: Open an issue on GitHub
- **Discussions**: Use GitHub Discussions
- **Documentation**: Refer to README.md

---

For more information, visit the [main README](README.md).
