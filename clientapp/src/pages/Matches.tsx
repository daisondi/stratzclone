// src/pages/Matches.tsx
import React, { useEffect, useState } from "react";
import { useParams } from 'react-router-dom';
import styles from "./Matches.module.css";

// ---------- Types ---------- //
interface Item {
  id: number;
  name: string;
  url_image: string; // CDN url
}

interface Hero {
  heroId: number;
  name: string;
  localizedName: string;
  pictureUrl: string;
}

export interface Match {
  matchId: number;
  startTimeUtc: string; // ISO string
  duration: string;     // "hh:mm:ss" from API
  hero: Hero;
  isWin: boolean;
  kills: number;
  deaths: number;
  assists: number;
  item0?: Item;
  item1?: Item;
  item2?: Item;
  item3?: Item;
  item4?: Item;
  item5?: Item;
}

interface Paged<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

// ---------- Helpers ---------- //
const formatDuration = (iso: string) => {
  const [h, m, s] = iso.split(":");
  return h === "00" ? `${m}:${s}` : `${h}:${m}:${s}`;
};

// ---------- Component ---------- //
export default function Matches() {
  const { steam32 } = useParams<{ steam32: string }>();
  const [page, setPage] = useState(1);
  const [paged, setPaged] = useState<Paged<Match> | null>(null);
  const [loading, setLoading] = useState(false);
  const pageSize = 20;

  useEffect(() => {
    if (!steam32) return; // no id, skip
    let cancelled = false;
    const fetchPage = async () => {
      setLoading(true);
      try {
        const res = await fetch(
          `/api/viewmatches/${steam32}?page=${page}&pageSize=${pageSize}`,
          { credentials: 'include' }
        );
        if (!res.ok) throw new Error(await res.text());
        const data: Paged<Match> = await res.json();
        if (!cancelled) setPaged(data);
      } catch (err) {
        console.error(err);
        if (!cancelled) setPaged(null);
      } finally {
        if (!cancelled) setLoading(false);
      }
    };

    fetchPage();
    return () => { cancelled = true; };
  }, [page, steam32]);

  if (loading && !paged) return <div className={styles.container}>Loading…</div>;
  if (!paged) return <div className={styles.container}>No matches found.</div>;

  const { items, totalCount } = paged;
  const maxPage = Math.ceil(totalCount / pageSize);

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>Matches</h1>

      <div className={styles.tableWrapper}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th className={styles.th}>Результат</th>
              <th className={styles.th}>Герой</th>
              <th className={styles.th}>K / D / A</th>
              <th className={styles.th}>Тривалість</th>
              <th className={styles.th}>Дата</th>
              <th className={styles.th}>Предмети</th>
            </tr>
          </thead>
          <tbody>
            {items.map(m => (
              <tr key={m.matchId} className={m.isWin ? styles.winRow : styles.lossRow}>
                <td className={styles.td}>
                  <span
                    className={`${styles.resultIcon} ${m.isWin ? styles.winIcon : styles.lossIcon
                      }`}
                  >
                    {m.isWin ? 'W' : 'L'}
                  </span>
                </td>
                <td className={`${styles.td} ${styles.heroCell}`}>
                  <img
                    src={m.hero.pictureUrl}
                    alt={m.hero.localizedName}
                    className={styles.heroIcon}
                  />
                  <span>{m.hero.localizedName}</span>
                </td>
                <td className={styles.td}>{m.kills} / {m.deaths} / {m.assists}</td>
                <td className={styles.td}>{formatDuration(m.duration)}</td>
                <td className={styles.td}>{new Date(m.startTimeUtc).toLocaleDateString()}</td>
                <td className={styles.td}>
                  <div className={styles.items}>
                    {[m.item0, m.item1, m.item2, m.item3, m.item4, m.item5]
                      .filter(Boolean)
                      .map(it => (
                        <img
                          key={it!.id}
                          src={it!.url_image}
                          alt={it!.name}
                          className={styles.itemIcon}
                        />
                      ))}
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className={styles.pager}>
        <button
          onClick={() => setPage(p => Math.max(1, p - 1))}
          disabled={page === 1}
          className={styles.button}
        >
          « Prev
        </button>
        <span className={styles.pageInfo}>
          Page {page} of {maxPage}
        </span>
        <button
          onClick={() => setPage(p => Math.min(maxPage, p + 1))}
          disabled={page === maxPage}
          className={styles.button}
        >
          Next »
        </button>
      </div>
    </div>
  );
}
