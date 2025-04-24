import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
// …other imports

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />
        {/* <Route path="/matches" element={<Matches />} /> */}
      </Routes>
    </BrowserRouter>
  );
}

export default App;
