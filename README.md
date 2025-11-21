# Jolly - Spotify Queue Management System

A cross-platform web-based application that allows businesses to manage Spotify song requests with an approval workflow. Perfect for restaurants, cafes, bars, and other businesses that want to give customers controlled music selection.

## Features

- 🎵 **Song Search**: Search for songs, artists, and albums via Spotify integration
- 📝 **Queue Management**: Customers can request songs with their name
- ✅ **Business Approval**: Accept, refuse, or mark requests as "maybe" 
- 🌐 **Cross-Platform**: Works on PC, Android, and iOS via web browser
- 🔄 **Real-time Updates**: Auto-refresh queue every 30 seconds
- 💼 **Business Control**: Complete approval access for business owners

## Technology Stack

- **Backend**: C# ASP.NET Core 10.0 Web API
- **Frontend**: HTML5, CSS3, Vanilla JavaScript
- **Architecture**: RESTful API with in-memory data storage

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later

## Installation & Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/ilyashamza70/Jolly.git
   cd Jolly
   ```

2. **Build the application**
   ```bash
   dotnet build
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the application**
   - Open your browser and navigate to: `http://localhost:5000` or `https://localhost:5001`
   - The application will display both customer and business panels

## Usage

### For Customers (Song Requests)

1. **Enter Your Name**: Type your name in the "Your name" field
2. **Search for Songs**: Enter a song, artist, or album name in the search bar
3. **Request a Song**: Click "Request Song" on your desired track
4. **Wait for Approval**: Your request will appear in the business approval panel

### For Business Owners (Approval Panel)

1. **View Requests**: See all pending song requests in the approval panel
2. **Review Details**: Each request shows:
   - Song title, artist, and album
   - Who requested it
   - When it was requested
3. **Take Action**:
   - **Accept** ✓: Approve the song to be played
   - **Maybe** ?: Mark for later consideration
   - **Refuse** ✗: Decline the request
4. **Toggle Views**: Click "Show All Requests" to see all requests (not just pending)
5. **Refresh**: Click "Refresh Queue" to manually update the list

## API Endpoints

### Spotify Endpoints

- `GET /api/spotify/search?query={query}&limit={limit}` - Search for songs
- `GET /api/spotify/song/{id}` - Get song details by ID

### Queue Management Endpoints

- `POST /api/queue/add` - Add a song request to the queue
- `GET /api/queue/all` - Get all queue requests
- `GET /api/queue/pending` - Get pending requests only
- `PUT /api/queue/{id}/status` - Update request status (Accept/Refuse/Maybe)
- `DELETE /api/queue/{id}` - Remove a request from queue
- `GET /api/queue/{id}` - Get a specific request by ID

## Project Structure

```
Jolly/
├── Controllers/          # API Controllers
│   ├── SpotifyController.cs
│   └── QueueController.cs
├── Models/              # Data models
│   ├── Song.cs
│   ├── QueueRequest.cs
│   └── SpotifySearchResult.cs
├── Services/            # Business logic services
│   ├── ISpotifyService.cs
│   ├── SpotifyService.cs
│   ├── IQueueService.cs
│   └── QueueService.cs
├── wwwroot/             # Static web files
│   ├── index.html
│   ├── css/
│   │   └── style.css
│   └── js/
│       └── app.js
├── Program.cs           # Application entry point
└── JollyApp.csproj      # Project configuration
```

## Configuration

The application uses in-memory storage for demonstration purposes. For production use, consider:

1. **Database Integration**: Replace in-memory storage with a database (SQL Server, PostgreSQL, etc.)
2. **Spotify API Integration**: Implement real Spotify Web API integration with OAuth2
3. **Bluetooth Support**: Add Bluetooth connectivity for sound system control
4. **Authentication**: Add user authentication for business owner access
5. **Persistence**: Implement data persistence across application restarts

## Future Enhancements

- [ ] Real Spotify API integration with OAuth2 authentication
- [ ] Bluetooth connectivity for direct sound system control
- [ ] User authentication and role-based access control
- [ ] Database persistence (Entity Framework Core)
- [ ] Mobile-optimized responsive design
- [ ] Queue playback automation
- [ ] Analytics and reporting dashboard
- [ ] Volume and playback controls
- [ ] Playlist management
- [ ] Customer request limits and cooldowns

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

For issues, questions, or contributions, please open an issue on GitHub.

---

**Note**: This application currently uses mock Spotify data for demonstration. To integrate with the real Spotify API, you'll need to:
1. Register your application at [Spotify Developer Dashboard](https://developer.spotify.com/dashboard)
2. Obtain Client ID and Client Secret
3. Implement OAuth2 authentication flow
4. Replace the mock `SpotifyService` with real API calls
