# Jolly - Architecture Overview

## System Architecture

Jolly is a cross-platform web-based application built with a modern client-server architecture.

```
┌─────────────────────────────────────────────────────────────┐
│                         Client Layer                         │
│  (HTML5 + CSS3 + Vanilla JavaScript - runs in browser)      │
│                                                              │
│  ┌──────────────┐  ┌──────────────────────────────────┐    │
│  │   Customer   │  │   Business Owner Panel            │    │
│  │   Interface  │  │   (Approval System)               │    │
│  │              │  │                                    │    │
│  │ - Search     │  │ - View Requests                   │    │
│  │ - Request    │  │ - Accept/Refuse/Maybe             │    │
│  └──────────────┘  └──────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ HTTP/REST API
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      Server Layer (ASP.NET Core)            │
│                                                              │
│  ┌──────────────────────────────────────────────────────┐  │
│  │              API Controllers                          │  │
│  │  ┌─────────────────┐  ┌────────────────────────┐    │  │
│  │  │ SpotifyController│  │  QueueController       │    │  │
│  │  │                 │  │                        │    │  │
│  │  │ - Search Songs  │  │ - Add Request          │    │  │
│  │  │ - Get Song      │  │ - Get Requests         │    │  │
│  │  └─────────────────┘  │ - Update Status        │    │  │
│  │                       │ - Remove Request       │    │  │
│  │                       └────────────────────────┘    │  │
│  └──────────────────────────────────────────────────────┘  │
│                            │                                │
│  ┌──────────────────────────────────────────────────────┐  │
│  │              Service Layer                            │  │
│  │  ┌─────────────────┐  ┌────────────────────────┐    │  │
│  │  │ SpotifyService  │  │  QueueService          │    │  │
│  │  │                 │  │                        │    │  │
│  │  │ - Search Logic  │  │ - Queue Management     │    │  │
│  │  │ - Song Retrieval│  │ - Status Updates       │    │  │
│  │  └─────────────────┘  └────────────────────────┘    │  │
│  └──────────────────────────────────────────────────────┘  │
│                            │                                │
│  ┌──────────────────────────────────────────────────────┐  │
│  │                  Data Layer                           │  │
│  │           (In-Memory Storage - Demo)                  │  │
│  │                                                        │  │
│  │  - Song Collection (Mock Data)                        │  │
│  │  - Queue Requests (Runtime Storage)                   │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

## Component Details

### 1. Client Layer (Frontend)

**Location**: `wwwroot/`

#### Files:
- **index.html**: Single-page application structure
- **css/style.css**: Responsive styling with Spotify-inspired design
- **js/app.js**: Client-side logic and API interaction

#### Key Features:
- Vanilla JavaScript (no frameworks required)
- Responsive design for mobile, tablet, and desktop
- Real-time API communication
- Auto-refresh mechanism (30-second intervals)

### 2. API Controllers

**Location**: `Controllers/`

#### SpotifyController
- **Endpoint**: `/api/spotify`
- **Methods**:
  - `GET /search?query={query}` - Search for songs
  - `GET /song/{id}` - Get specific song details
- **Responsibility**: Handle Spotify-related API requests

#### QueueController
- **Endpoint**: `/api/queue`
- **Methods**:
  - `POST /add` - Add song to queue
  - `GET /pending` - Get pending requests
  - `GET /all` - Get all requests
  - `PUT /{id}/status` - Update request status
  - `DELETE /{id}` - Remove request
- **Responsibility**: Manage queue operations

### 3. Service Layer

**Location**: `Services/`

#### ISpotifyService / SpotifyService
- **Purpose**: Abstract Spotify operations
- **Current Implementation**: Mock data for demonstration
- **Future**: Real Spotify API integration with OAuth2

#### IQueueService / QueueService
- **Purpose**: Business logic for queue management
- **Features**:
  - Add songs to queue
  - Retrieve requests (filtered or all)
  - Update request status (Accept/Refuse/Maybe)
  - Remove requests

### 4. Models

**Location**: `Models/`

#### Song
```csharp
- Id: string
- Title: string
- Artist: string
- Album: string
- DurationMs: int
- AlbumArtUrl: string
- SpotifyUri: string
```

#### QueueRequest
```csharp
- Id: string
- Song: Song
- RequestedBy: string
- RequestedAt: DateTime
- Status: RequestStatus (Pending/Accepted/Refused/Maybe)
- Notes: string
```

#### SpotifySearchResult
```csharp
- Songs: List<Song>
- TotalResults: int
```

## Data Flow

### Customer Requests a Song

```
1. Customer searches for song
   → GET /api/spotify/search?query=...
   → SpotifyService.SearchSongsAsync()
   → Returns list of songs

2. Customer selects and requests song
   → POST /api/queue/add { songId, requestedBy }
   → QueueService.AddToQueueAsync()
   → Creates QueueRequest with Status=Pending
   → Returns confirmation

3. Business panel auto-refreshes
   → GET /api/queue/pending
   → QueueService.GetPendingRequestsAsync()
   → Returns list of pending requests
```

### Business Owner Reviews Request

```
1. Owner views pending request in panel
   → Already loaded via auto-refresh

2. Owner clicks Accept/Refuse/Maybe
   → PUT /api/queue/{id}/status { status, notes }
   → QueueService.UpdateRequestStatusAsync()
   → Updates request status
   → Returns updated request

3. UI refreshes to show updated status
   → GET /api/queue/all (if viewing all)
   → GET /api/queue/pending (if viewing pending)
```

## Design Patterns Used

### 1. Dependency Injection
- Services registered in `Program.cs`
- Controllers receive dependencies via constructor injection
- Promotes loose coupling and testability

### 2. Repository Pattern (Service Layer)
- Services act as repositories for data operations
- Abstract data access from business logic
- Easy to swap implementations (mock → real Spotify API)

### 3. RESTful API Design
- Resource-based URLs
- Standard HTTP methods (GET, POST, PUT, DELETE)
- JSON request/response format
- Proper status codes

### 4. Single Page Application (SPA)
- Single HTML page
- Dynamic content updates via JavaScript
- No page reloads required
- Better user experience

### 5. MVC Pattern
- **Models**: Data structures (Song, QueueRequest)
- **Views**: HTML/CSS/JavaScript frontend
- **Controllers**: API endpoints handling requests

## Security Considerations

### Current Implementation
- Input validation in controllers
- HTML escaping in JavaScript to prevent XSS
- CORS configured for cross-origin requests
- No authentication (intended for demo)

### Production Recommendations
1. **Authentication & Authorization**
   - Add user authentication for business panel
   - Implement role-based access control
   - Use JWT tokens or session management

2. **Rate Limiting**
   - Prevent abuse of search endpoint
   - Limit request submissions per user

3. **HTTPS Only**
   - Enforce HTTPS in production
   - Redirect HTTP to HTTPS

4. **Input Validation**
   - Server-side validation for all inputs
   - Sanitize user-provided data

5. **API Keys**
   - Secure Spotify API credentials
   - Use environment variables
   - Never commit secrets to repository

## Scalability Considerations

### Current Limitations (Demo)
- In-memory storage (data lost on restart)
- Single server instance
- No caching
- No load balancing

### Production Enhancements
1. **Database Integration**
   - Use SQL Server, PostgreSQL, or MongoDB
   - Entity Framework Core for ORM
   - Persistent storage

2. **Caching**
   - Redis for distributed caching
   - Cache search results
   - Cache user sessions

3. **Message Queue**
   - RabbitMQ or Azure Service Bus
   - Decouple request processing
   - Handle high load

4. **Load Balancing**
   - Multiple server instances
   - Azure App Service or AWS Elastic Beanstalk
   - Auto-scaling based on load

5. **CDN**
   - Serve static files from CDN
   - Reduce server load
   - Improve global performance

## Technology Stack

### Backend
- **Framework**: ASP.NET Core 10.0
- **Language**: C# 13
- **Architecture**: Web API (RESTful)
- **Dependency Injection**: Built-in DI container

### Frontend
- **HTML5**: Semantic markup
- **CSS3**: Modern styling with flexbox/grid
- **JavaScript**: ES6+ vanilla JavaScript
- **No frameworks**: Pure web technologies

### Development Tools
- **.NET SDK 10.0**: Build and runtime
- **Visual Studio / VS Code**: IDE options
- **Git**: Version control

## Deployment Architecture

### Local Development
```
Developer Machine
└── .NET SDK 10.0
    └── dotnet run
        └── Kestrel Web Server
            └── localhost:5000 (HTTP)
            └── localhost:5001 (HTTPS)
```

### Production Options

#### Option 1: IIS (Windows)
```
Windows Server
└── IIS
    └── ASP.NET Core Module
        └── Jolly Application
```

#### Option 2: Docker Container
```
Docker Host
└── Docker Container
    └── .NET Runtime
        └── Jolly Application
```

#### Option 3: Cloud Platform
```
Azure App Service / AWS Elastic Beanstalk
└── Managed Runtime Environment
    └── Jolly Application
    └── Auto-scaling
    └── Load Balancing
```

## Future Enhancements

### Phase 1: Real Spotify Integration
- OAuth2 authentication
- Real Spotify API calls
- User playlist integration

### Phase 2: Bluetooth Support
- Bluetooth device discovery
- Connect to sound systems
- Control playback

### Phase 3: Advanced Features
- Queue reordering (drag & drop)
- User voting system
- Song history and analytics
- Popular songs dashboard
- Request limits per user
- Scheduled playlists

### Phase 4: Real-time Updates
- SignalR for WebSocket communication
- Live queue updates without polling
- Real-time notifications

### Phase 5: Mobile Apps
- Native iOS app (Swift)
- Native Android app (Kotlin)
- Shared backend API

## Monitoring and Observability

### Recommended Tools
- **Application Insights**: Azure monitoring
- **Serilog**: Structured logging
- **Health Checks**: Endpoint monitoring
- **Metrics**: Performance tracking

## Conclusion

Jolly is designed as a scalable, maintainable, and extensible system. The current implementation provides a solid foundation with mock data for demonstration, and the architecture supports easy integration of real services and databases for production deployment.
