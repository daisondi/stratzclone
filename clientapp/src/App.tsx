import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Main from './pages/Main';
import Matches from './pages/Matches';
// …other imports

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/main/:steam32" element={<Main />} />                           // ← carry steam32 here
        <Route path="/main/:steam32/matches" element={<Matches />} />                // ← and here, if you link “Matches” as a sub-route
      </Routes>
    </BrowserRouter>
  );
}

export default App;