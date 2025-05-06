// src/pages/Graphics.tsx
import React from 'react';
import {
  ResponsiveContainer,
  LineChart,
  Line,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  PieChart,
  Pie,
  Cell
} from 'recharts';
// ---------- Sample Data ---------- //
const gpmXpmData = Array.from({ length: 20 }, (_, i) => ({
  match: i + 1,
  gpm: Math.floor(300 + Math.random() * 400),
  xpm: Math.floor(250 + Math.random() * 350),
}));

const kdaData = gpmXpmData.map(d => ({
  match: d.match,
  kills: Math.floor(1 + Math.random() * 15),
  deaths: Math.floor(1 + Math.random() * 10),
  assists: Math.floor(5 + Math.random() * 30),
}));

const winRateData = [
  { name: 'Wins', value: 12 },
  { name: 'Losses', value: 8 },
];

const heroPickData = [
  { name: 'Phantom Assassin', picks: 5 },
  { name: 'Invoker', picks: 4 },
  { name: 'Crystal Maiden', picks: 3 },
  { name: 'Juggernaut', picks: 2 },
  { name: 'Earthshaker', picks: 1 },
];

const COLORS = ['#1db954', '#e63946'];
const HERO_COLORS = ['#8884d8', '#82ca9d', '#ffc658', '#d0ed57', '#a4de6c'];

export default function Graphics() {
  return (
    <div style={{ padding: '1rem', background: '#121212', color: '#eee' }}>
      {/* 1) GPM & XPM за останні 20 матчів */}
      <h2>GPM &amp; XPM за останні 20 матчів</h2>
      <ResponsiveContainer width="100%" height={250}>
        <LineChart data={gpmXpmData} margin={{ top: 10, right: 30, left: 0, bottom: 0 }}>
          <CartesianGrid stroke="#333" />
          <XAxis dataKey="match" stroke="#aaa" />
          <YAxis stroke="#aaa" />
          <Tooltip contentStyle={{ backgroundColor: '#222', borderColor: '#555' }} />
          <Legend />
          <Line
            type="monotone"
            dataKey="gpm"
            name="Золото за хвилину (GPM)"
            stroke="#82ca9d"
            dot={{ r: 3 }}
          />
          <Line
            type="monotone"
            dataKey="xpm"
            name="Досвід за хвилину (XPM)"
            stroke="#8884d8"
            dot={{ r: 3 }}
          />
        </LineChart>
      </ResponsiveContainer>
      <p style={{ fontSize: '0.9rem', color: '#bbb' }}>
        Лінійний графік, що показує тенденцію золота за хвилину (GPM) та досвіду за хвилину (XPM) у ваших останніх 20 іграх.
      </p>

      {/* 2) Вбивства / Смерті / Асисти за матч */}
      <h2>Вбивства / Смерті / Асисти за матч</h2>
      <ResponsiveContainer width="100%" height={250}>
        <LineChart data={kdaData} margin={{ top: 10, right: 30, left: 0, bottom: 0 }}>
          <CartesianGrid stroke="#333" />
          <XAxis dataKey="match" stroke="#aaa" />
          <YAxis stroke="#aaa" />
          <Tooltip contentStyle={{ backgroundColor: '#222', borderColor: '#555' }} />
          <Legend />
          <Line type="monotone" dataKey="kills" name="Вбивства" stroke="#ff6961" dot={{ r: 3 }} />
          <Line type="monotone" dataKey="deaths" name="Смерті" stroke="#77dd77" dot={{ r: 3 }} />
          <Line type="monotone" dataKey="assists" name="Асисти" stroke="#aec6cf" dot={{ r: 3 }} />
        </LineChart>
      </ResponsiveContainer>
      <p style={{ fontSize: '0.9rem', color: '#bbb' }}>
        Тенденція ваших вбивств, смертей і асистів у кожному з останніх 20 матчів.
      </p>

      {/* 3) Розподіл перемог і поразок */}
      <h2>Розподіл перемог і поразок</h2>
      <ResponsiveContainer width={300} height={200}>
        <PieChart>
          <Pie
            data={winRateData}
            dataKey="value"
            nameKey="name"
            cx="50%"
            cy="50%"
            outerRadius={80}
            label
          >
            {winRateData.map((entry, index) => (
              <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
            ))}
          </Pie>
          <Tooltip contentStyle={{ backgroundColor: '#222', borderColor: '#555' }} />
        </PieChart>
      </ResponsiveContainer>
      <p style={{ fontSize: '0.9rem', color: '#bbb' }}>
        Кругова діаграма, що показує співвідношення перемог до поразок у ваших останніх 20 іграх.
      </p>

      {/* 4) Топ-5 обраних героїв */}
      <h2>Топ-5 обраних героїв</h2>
      <ResponsiveContainer width="100%" height={250}>
        <BarChart data={heroPickData} layout="vertical" margin={{ top: 5, right: 30, left: 80, bottom: 5 }}>
          <CartesianGrid stroke="#333" horizontal={false} />
          <XAxis type="number" stroke="#aaa" />
          <YAxis dataKey="name" type="category" stroke="#aaa" width={120} />
          <Tooltip contentStyle={{ backgroundColor: '#222', borderColor: '#555' }} />
          <Bar dataKey="picks" name="Кількість виборів" fill="#1db954">
            {heroPickData.map((entry, index) => (
              <Cell key={`cell-hero-${index}`} fill={HERO_COLORS[index % HERO_COLORS.length]} />
            ))}
          </Bar>
        </BarChart>
      </ResponsiveContainer>
      <p style={{ fontSize: '0.9rem', color: '#bbb' }}>
        Стовпчиковий графік, що відображає частоту вибору ваших топ-5 героїв у нещодавніх матчах.
      </p>
    </div>
  );
}