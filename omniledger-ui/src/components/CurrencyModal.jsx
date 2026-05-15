import React, { useState } from 'react';
import './CurrencyModal.css';
import { apiFetch } from '../utils/api';

const currencies = [
  { symbol: '$', name: 'US Dollar (USD)' },
  { symbol: '€', name: 'Euro (EUR)' },
  { symbol: '£', name: 'British Pound (GBP)' },
  { symbol: '¥', name: 'Japanese Yen (JPY)' },
  { symbol: '₱', name: 'Philippine Peso (PHP)' },
  { symbol: '₹', name: 'Indian Rupee (INR)' }
];

const CurrencyModal = ({ username, currentCurrency, onClose, onComplete }) => {
  const [selectedCurrency, setSelectedCurrency] = useState(currentCurrency || '$');
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (selectedCurrency === currentCurrency) {
      onClose(); // No change needed
      return;
    }

    setIsSubmitting(true);
    try {
      const res = await apiFetch('/api/ledger/currency', {
        method: 'POST',
        body: JSON.stringify({
          username,
          currency: selectedCurrency
        })
      });
      const data = await res.json();
      if (data.success) {
        onComplete();
      } else {
        alert(data.message || 'Failed to change currency');
      }
    } catch (err) {
      alert('Error connecting to backend');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content currency-modal">
        <h2>Change Currency</h2>
        <p>Select your preferred display currency. This will convert all your existing transactions.</p>
        <form onSubmit={handleSubmit}>
          <div className="input-group">
            <select 
              value={selectedCurrency} 
              onChange={e => setSelectedCurrency(e.target.value)}
              disabled={isSubmitting}
            >
              {currencies.map(c => (
                <option key={c.symbol} value={c.symbol}>
                  {c.name} ({c.symbol})
                </option>
              ))}
            </select>
          </div>
          <div className="modal-actions">
            <button type="button" className="btn-secondary" onClick={onClose} disabled={isSubmitting}>Cancel</button>
            <button type="submit" className="btn-primary" disabled={isSubmitting}>
              {isSubmitting ? 'Converting...' : 'Save'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CurrencyModal;
