import React from 'react';
import './SplashScreen.css';

const SplashScreen = () => {
  return (
    <div className="splash-screen">
      <div className="splash-content">
        <div className="logo-icon">
          {/* A simple representation of a four-leaf blue gradient icon using CSS */}
          <div className="leaf leaf-1"></div>
          <div className="leaf leaf-2"></div>
          <div className="leaf leaf-3"></div>
          <div className="leaf leaf-4"></div>
        </div>
        <h1 className="logo-text">OmniLedger</h1>
      </div>
    </div>
  );
};

export default SplashScreen;
