// src/pages/Main.tsx
import React, { useEffect, useState } from 'react';
import styles from './Main.module.css';
import Matches from './Matches';
type Tab = 'matches' | 'recommendations' | 'graphics';

interface Player {
  steamId: string;
  steamId32?: string;
  displayName: string;
  profilePictureUrl?: string;
}

const TABS: { key: Tab; label: string }[] = [
  { key: 'matches', label: 'Matches' },
  { key: 'recommendations', label: 'Recommendations' },
  { key: 'graphics', label: 'Graphics' },
];

const Main: React.FC = () => {
  const [player, setPlayer] = useState<Player | null>(null);
  const [activeTab, setActiveTab] = useState<Tab>('matches');

  useEffect(() => {
    fetch('/api/auth/steam/me', { credentials: 'include' })
      .then(r => {
        if (!r.ok) throw new Error();
        return r.json();
      })
      .then(setPlayer)
      .catch(() => setPlayer(null));
  }, []);

  if (!player) {
    return <div className={styles.loading}>Loading...</div>;
  }

  const renderContent = () => {
    switch (activeTab) {
      case 'matches':
        return <Matches />;
      case 'recommendations':
        return <div className={styles.content}>[Recommendations will go here]</div>;
      case 'graphics':
        return <div className={styles.content}>[Graphics will go here]</div>;
    }
  };

  return (
    <div className={styles.page}>
      <div className={styles.header}>
        <img
          className={styles.avatar}
          src={player.profilePictureUrl}
          alt={player.displayName}
        />
        <h2 className={styles.name}>{player.displayName}</h2>
      </div>

      <div className={styles.tabsWrapper}>
        <div className={styles.tabs}>
          {TABS.map((tab, idx) => (
            <button
              key={tab.key}
              className={`${styles.tab} ${
                activeTab === tab.key ? styles.active : ''
              }`}
              onClick={() => setActiveTab(tab.key)}
            >
              {tab.label}
            </button>
          ))}
          <div
            className={styles.underline}
            style={{
              transform: `translateX(${TABS.findIndex(t => t.key === activeTab) * 100}%)`
            }}
          />
        </div>
      </div>

      <div className={styles.contentWrapper}>{renderContent()}</div>
    </div>
  );
};

export default Main;
