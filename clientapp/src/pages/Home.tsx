// src/pages/Home.tsx
import React, { useEffect, useState } from 'react';
import styles from './Home.module.css';
import { ReactComponent as SteamIcon } from '../assets/steam.svg';
import { useNavigate } from 'react-router-dom';
interface Player {
  steamId:   string;
  steamId32: string; 
  displayName: string;
  username: string;
}

const Home: React.FC = () => {
  const [player, setPlayer] = useState<Player | null>(null);
  const navigate = useNavigate();
  // Try to load the logged-in user on mount
  useEffect(() => {
    fetch('/api/auth/steam/me', { credentials: 'include' })
      .then(r => r.ok ? r.json() : Promise.reject())
      .then(player => {
        // redirect to /main/STEAM32
        navigate(`/main/${player.steamId32}`);
      })
      .catch(() => setPlayer(null));
  }, []);

  const handleSteamLogin = () => {
    const ret = encodeURIComponent(window.location.origin + '/');
    window.location.href =
      'https://localhost:7065/api/auth/steam/login?returnUrl=' + ret;
  };
  
  
  
  if (player) {
    // Show saved info
    return (
      <div className={styles.hero}>
        <h2>Welcome, {player.displayName}!</h2>
        <p><strong>Steam ID:</strong> {player.steamId}</p>
        <p><strong>Username:</strong> {player.username}</p>
      </div>
    );
  }

  // Not logged in → show Steam button
  return (
    <div className={styles.hero}>
      <button className={styles.btnSteam} onClick={handleSteamLogin}>
        <SteamIcon width={20} height={20} />
        Увійти через Steam
      </button>
    </div>
  );
};

export default Home;
