import React, { useState } from 'react';
import './AuthModal.css';
import { apiFetch } from '../utils/api';

const AuthModal = ({ onComplete }) => {
  const [view, setView] = useState('login'); // 'login', 'register', 'success'
  const [isTransitioning, setIsTransitioning] = useState(false);

  const switchView = (newView) => {
    setIsTransitioning(true);
    setTimeout(() => {
      setView(newView);
      setIsTransitioning(false);
    }, 300); // 300ms for smooth fade out before switching
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const res = await apiFetch('/api/auth/login', {
        method: 'POST',
        body: JSON.stringify({ username: e.target[0].value, password: e.target[1].value })
      });
      const data = await res.json();
      if (data.success) {
        if (onComplete) onComplete(data.username);
      } else {
        alert(data.message || 'Login failed');
      }
    } catch (err) {
      alert('Error connecting to backend');
    }
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const res = await apiFetch('/api/auth/register', {
        method: 'POST',
        body: JSON.stringify({ username: e.target[0].value, password: e.target[2].value })
      });
      const data = await res.json();
      if (data.success) {
        switchView('success');
      } else {
        alert(data.message || 'Registration failed');
      }
    } catch (err) {
      alert('Error connecting to backend');
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-modal">
        <div className="modal-header">
          <div className="header-logo">OmniLedger</div>
          <div className="window-controls">
            <span className="control minimize"></span>
            <span className="control maximize"></span>
            <span className="control close"></span>
          </div>
        </div>

        <div className={`modal-content ${isTransitioning ? 'fade-out' : 'fade-in'}`}>
          {view === 'login' && (
            <div className="auth-view">
              <div className="view-header">
                <h2>LOG IN</h2>
              </div>
              <form onSubmit={handleLogin} className="auth-form">
                <input type="text" placeholder="Username" required />
                <input type="password" placeholder="Password" required />
                <button type="submit" className="btn-primary">LOGIN</button>
              </form>
              <div className="auth-footer">
                <p>Don't have an account? <span className="link" onClick={() => switchView('register')}>Sign up</span></p>
              </div>
            </div>
          )}

          {view === 'register' && (
            <div className="auth-view">
              <div className="view-header">
                <h2>SIGN UP</h2>
                <p className="helper-text">Create your personal ledger.</p>
              </div>
              <form onSubmit={handleRegister} className="auth-form">
                <input type="text" placeholder="Username" required />
                <input type="email" placeholder="Email" required />
                <input type="password" placeholder="Password" required />
                <button type="submit" className="btn-primary">SIGN UP</button>
              </form>
              <div className="auth-footer">
                <p>Already have an account? <span className="link" onClick={() => switchView('login')}>Log in</span></p>
              </div>
            </div>
          )}

          {view === 'success' && (
            <div className="auth-view success-view">
              <div className="success-icon">
                {/* Blue checkmark icon representation */}
                <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3">
                  <path strokeLinecap="round" strokeLinejoin="round" d="M5 13l4 4L19 7" />
                </svg>
              </div>
              <h2>Registration Success</h2>
              <p className="helper-text">Your account has been created.</p>
              <button className="btn-primary" onClick={() => switchView('login')}>CONTINUE TO LOGIN</button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AuthModal;
