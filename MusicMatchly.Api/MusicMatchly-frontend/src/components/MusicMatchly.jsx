import React, { useEffect, useState } from 'react';
import API from '../utils/api';

export default function MusicMatchly({ user }) {
  const [topArtists, setTopArtists] = useState([]);
  const [matches, setMatches] = useState([]);

  useEffect(() => {
    // Backend route to get top artists for logged in user (you'll add this later)
    API.get('/spotify/top-artists')
      .then(res => setTopArtists(res.data.items))
      .catch(() => setTopArtists([]));

    // Mock: fetch other users or match data from backend or local
    // This should be replaced with real match logic or API call
    setMatches([
      { name: 'Alice', sharedArtists: ['Artist A', 'Artist B'] },
      { name: 'Bob', sharedArtists: ['Artist C'] },
    ]);
  }, []);

  return (
    <div>
      <h3>Your Top Artists</h3>
      <ul>
        {topArtists.map((artist) => (
          <li key={artist.id}>{artist.name}</li>
        ))}
      </ul>

      <h3>Matched Vibes</h3>
      {matches.length === 0 ? (
        <p>No matches found yet.</p>
      ) : (
        matches.map((match, idx) => (
          <div key={idx}>
            <h4>{match.name}</h4>
            <p>Shared artists: {match.sharedArtists.join(', ')}</p>
          </div>
        ))
      )}
    </div>
  );
}
