// src/pages/Home.tsx
import React, { useEffect, useState } from 'react';
import styles from './Home.module.css';
import { ReactComponent as SteamIcon } from '../assets/steam.svg';

interface Player {
  steamId: string;
  displayName: string;
  username: string;
}

const Home: React.FC = () => {
  const [player, setPlayer] = useState<Player | null>(null);

  // Try to load the logged-in user on mount
  useEffect(() => {
    fetch('/api/auth/steam/me', { credentials: 'include' })
      .then(res => {
        if (!res.ok) throw new Error('Not logged in');
        return res.json() as Promise<Player>;
      })
      .then(setPlayer)
      .catch(() => setPlayer(null));
  }, []);

  const handleSteamLogin = () => {
    const returnUrl = encodeURIComponent(window.location.origin + '/');
    window.location.href =
      `http://localhost:5179/api/auth/steam/login?returnUrl=${returnUrl}`;
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
        Войти через Steam
      </button>
    </div>
  );
};

export default Home;
