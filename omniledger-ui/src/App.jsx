import React, { useState, useEffect } from 'react';
import SplashScreen from './components/SplashScreen';
import AuthModal from './components/AuthModal';
import Dashboard from './components/Dashboard';
import './App.css';

function App() {
  const [showSplash, setShowSplash] = useState(true);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [username, setUsername] = useState('');

  useEffect(() => {
    // Simulate loading time, then hide splash screen
    const timer = setTimeout(() => {
      setShowSplash(false);
    }, 2500); // 2.5 seconds
    
    return () => clearTimeout(timer);
  }, []);

  const handleAuthComplete = (user) => {
    setUsername(user);
    setIsAuthenticated(true);
  };

  return (
    <>
      {showSplash && <SplashScreen />}
      {!showSplash && !isAuthenticated && <AuthModal onComplete={handleAuthComplete} />}
      {!showSplash && isAuthenticated && <Dashboard username={username} onLogout={() => setIsAuthenticated(false)} />}
    </>
  );
}

export default App;
