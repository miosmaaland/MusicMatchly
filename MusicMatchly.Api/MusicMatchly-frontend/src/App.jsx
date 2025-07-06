import React, { useEffect, useState } from 'react';
import API from './utils/api';
import Login from './components/Login';
import Profile from './components/Profile';
import MusicMatchly from './components/MusicMatchly';

export default function App() {
  const [user, setUser] = useState(null);

  // On mount, check if user is logged in by fetching profile
  useEffect(() => {
    API.get('/profile')
      .then(res => setUser(res.data))
      .catch(() => setUser(null));
  }, []);

  if (!user) return <Login />;

  return (
    <>
      <Profile user={user} />
      <MusicMatchly user={user} />
    </>
  );
}
