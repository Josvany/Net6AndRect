import React from 'react';
import HouseList from '../house/HouseList';
import './App.css';
import Header from './Header';

function App() {
  return (
    <div className='container'>
      <Header subtitle='test'></Header>
      <HouseList></HouseList>
    </div>
  );
}

export default App;
