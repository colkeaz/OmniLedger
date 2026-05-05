import React, { useState } from 'react';
import './TransactionModal.css';

const currencies = [
  { symbol: '$', name: 'US Dollar' },
  { symbol: '€', name: 'Euro' },
  { symbol: '£', name: 'British Pound' },
  { symbol: '¥', name: 'Japanese Yen' },
  { symbol: '₱', name: 'Philippine Peso' },
  { symbol: '₹', name: 'Indian Rupee' }
];

const TransactionModal = ({ username, type, currentCurrency = '$', onClose, onComplete }) => {
  const [amount, setAmount] = useState('');
  const [description, setDescription] = useState('');
  const [selectedCurrency, setSelectedCurrency] = useState(currentCurrency);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!amount || isNaN(amount) || amount <= 0) {
      alert('Please enter a valid amount');
      return;
    }

    setIsSubmitting(true);
    try {
      const res = await fetch('http://localhost:8080/api/ledger/transaction', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          username,
          amount: parseFloat(amount),
          description: description || (type === 'Income' ? 'Misc Income' : 'Misc Expense'),
          type,
          currency: selectedCurrency
        })
      });
      const data = await res.json();
      if (data.success) {
        onComplete();
      } else {
        alert(data.message || 'Failed to submit transaction');
      }
    } catch (err) {
      alert('Error connecting to backend');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content transaction-modal">
        <h2>Add {type}</h2>
        <form onSubmit={handleSubmit}>
          <div className="input-group amount-group">
            <select 
              className="currency-select"
              value={selectedCurrency}
              onChange={e => setSelectedCurrency(e.target.value)}
            >
              {currencies.map(c => (
                <option key={c.symbol} value={c.symbol}>{c.symbol}</option>
              ))}
            </select>
            <input 
              type="number" 
              placeholder="Amount" 
              value={amount} 
              onChange={e => setAmount(e.target.value)} 
              step="0.01"
              required 
            />
          </div>
          <div className="input-group">
            <input 
              type="text" 
              placeholder="Description (Optional)" 
              value={description} 
              onChange={e => setDescription(e.target.value)} 
            />
          </div>
          <div className="modal-actions">
            <button type="button" className="btn-secondary" onClick={onClose} disabled={isSubmitting}>Cancel</button>
            <button type="submit" className="btn-primary" disabled={isSubmitting}>
              {isSubmitting ? 'Saving...' : 'Save'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default TransactionModal;
