// API Base URL
const API_BASE = window.location.origin;

// State
let currentView = 'pending'; // 'pending' or 'all'

// DOM Elements
const searchInput = document.getElementById('search-input');
const searchBtn = document.getElementById('search-btn');
const requesterName = document.getElementById('requester-name');
const searchResults = document.getElementById('search-results');
const pendingRequests = document.getElementById('pending-requests');
const refreshBtn = document.getElementById('refresh-btn');
const toggleViewBtn = document.getElementById('toggle-view-btn');

// Event Listeners
searchBtn.addEventListener('click', handleSearch);
searchInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter') handleSearch();
});
refreshBtn.addEventListener('click', loadRequests);
toggleViewBtn.addEventListener('click', toggleView);

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    loadRequests();
    // Auto-refresh every 30 seconds
    setInterval(loadRequests, 30000);
});

// Search for songs
async function handleSearch() {
    const query = searchInput.value.trim();
    
    if (!query) {
        alert('Please enter a search query');
        return;
    }

    searchResults.innerHTML = '<div class="loading">Searching...</div>';

    try {
        const response = await fetch(`${API_BASE}/api/spotify/search?query=${encodeURIComponent(query)}`);
        
        if (!response.ok) {
            throw new Error('Search failed');
        }

        const data = await response.json();
        displaySearchResults(data.songs);
    } catch (error) {
        console.error('Error searching songs:', error);
        searchResults.innerHTML = '<div class="empty-state"><p>Error searching songs. Please try again.</p></div>';
    }
}

// Display search results
function displaySearchResults(songs) {
    if (!songs || songs.length === 0) {
        searchResults.innerHTML = '<div class="empty-state"><p>No songs found. Try a different search.</p></div>';
        return;
    }

    searchResults.innerHTML = songs.map(song => `
        <div class="song-card">
            <div class="song-title">${escapeHtml(song.title)}</div>
            <div class="song-artist">${escapeHtml(song.artist)}</div>
            <div class="song-album">${escapeHtml(song.album)}</div>
            <div class="song-duration">${formatDuration(song.durationMs)}</div>
            <button class="btn btn-primary" data-song-id="${escapeHtml(song.id)}" data-song-title="${escapeHtml(song.title)}" onclick="requestSong(this.dataset.songId, this.dataset.songTitle)">
                Request Song
            </button>
        </div>
    `).join('');
}

// Request a song
async function requestSong(songId, songTitle) {
    const name = requesterName.value.trim();
    
    if (!name) {
        alert('Please enter your name before requesting a song');
        requesterName.focus();
        return;
    }

    try {
        const response = await fetch(`${API_BASE}/api/queue/add`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                songId: songId,
                requestedBy: name
            })
        });

        if (!response.ok) {
            throw new Error('Failed to add song to queue');
        }

        alert(`"${songTitle}" has been requested! Waiting for approval.`);
        loadRequests(); // Refresh the queue
    } catch (error) {
        console.error('Error requesting song:', error);
        alert('Failed to request song. Please try again.');
    }
}

// Load queue requests
async function loadRequests() {
    const endpoint = currentView === 'pending' ? 'pending' : 'all';
    
    try {
        const response = await fetch(`${API_BASE}/api/queue/${endpoint}`);
        
        if (!response.ok) {
            throw new Error('Failed to load requests');
        }

        const requests = await response.json();
        displayRequests(requests);
    } catch (error) {
        console.error('Error loading requests:', error);
        pendingRequests.innerHTML = '<div class="empty-state"><p>Error loading requests. Please try again.</p></div>';
    }
}

// Display queue requests
function displayRequests(requests) {
    if (!requests || requests.length === 0) {
        pendingRequests.innerHTML = `
            <div class="empty-state">
                <p>No ${currentView === 'pending' ? 'pending' : ''} requests at the moment.</p>
            </div>
        `;
        return;
    }

    pendingRequests.innerHTML = requests.map(request => {
        const statusClass = `status-${getStatusText(request.status).toLowerCase()}`;
        const showActions = request.status === 0; // Pending status
        
        return `
            <div class="request-card">
                <div class="request-header">
                    <div class="request-info">
                        <h3>${escapeHtml(request.song.title)}</h3>
                        <p><strong>Artist:</strong> ${escapeHtml(request.song.artist)}</p>
                        <p><strong>Album:</strong> ${escapeHtml(request.song.album)}</p>
                        <p><strong>Requested by:</strong> ${escapeHtml(request.requestedBy)}</p>
                        <p><strong>Time:</strong> ${formatDateTime(request.requestedAt)}</p>
                        ${request.notes ? `<p><strong>Notes:</strong> ${escapeHtml(request.notes)}</p>` : ''}
                    </div>
                    <span class="request-status ${statusClass}">${getStatusText(request.status)}</span>
                </div>
                ${showActions ? `
                    <div class="request-actions">
                        <button class="btn btn-accept" onclick="updateRequestStatus('${request.id}', 1, 'Accepted')">
                            ✓ Accept
                        </button>
                        <button class="btn btn-maybe" onclick="updateRequestStatus('${request.id}', 3, 'Maybe later')">
                            ? Maybe
                        </button>
                        <button class="btn btn-refuse" onclick="updateRequestStatus('${request.id}', 2, 'Refused')">
                            ✗ Refuse
                        </button>
                    </div>
                ` : ''}
            </div>
        `;
    }).join('');
}

// Update request status
async function updateRequestStatus(requestId, status, notes) {
    try {
        const response = await fetch(`${API_BASE}/api/queue/${requestId}/status`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                status: status,
                notes: notes
            })
        });

        if (!response.ok) {
            throw new Error('Failed to update status');
        }

        loadRequests(); // Refresh the queue
    } catch (error) {
        console.error('Error updating status:', error);
        alert('Failed to update request status. Please try again.');
    }
}

// Toggle between pending and all requests
function toggleView() {
    currentView = currentView === 'pending' ? 'all' : 'pending';
    toggleViewBtn.textContent = currentView === 'pending' ? 'Show All Requests' : 'Show Pending Only';
    loadRequests();
}

// Utility functions
function formatDuration(ms) {
    const minutes = Math.floor(ms / 60000);
    const seconds = Math.floor((ms % 60000) / 1000);
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
}

function formatDateTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString();
}

function getStatusText(status) {
    const statusMap = {
        0: 'Pending',
        1: 'Accepted',
        2: 'Refused',
        3: 'Maybe'
    };
    return statusMap[status] || 'Unknown';
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}
