import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import HouseDetail from '../house/HouseDetail';
import HouseList from '../house/HouseList';
import './App.css';
import Header from './Header';

function App() {
  return (
    <BrowserRouter>
      <div className='container'>
        <Header subtitle='test'></Header>
        <Routes>
          <Route path="/" element={<HouseList/>}></Route>
          <Route path="/house/:id" element={<HouseDetail />}></Route>
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
