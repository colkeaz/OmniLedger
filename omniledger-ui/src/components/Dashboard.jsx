import React, { useState, useEffect } from 'react';
import './Dashboard.css';
import TransactionModal from './TransactionModal';
import CurrencyModal from './CurrencyModal';

const Dashboard = ({ username, onLogout }) => {
  const [data, setData] = useState({
    balance: 0,
    currency: '$',
    totalIncome: 0,
    totalExpenses: 0,
    history: []
  });
  
  const [modalType, setModalType] = useState(null); // 'Income' or 'Expense'
  const [showCurrencyModal, setShowCurrencyModal] = useState(false);
  const [sortOrder, setSortOrder] = useState('latest'); // 'latest' or 'oldest'
  const [trackerView, setTrackerView] = useState('day'); // 'year', 'month', 'day'
  const [hoveredBar, setHoveredBar] = useState(null);
  
  const [historyPage, setHistoryPage] = useState(1);
  const [trackerOffset, setTrackerOffset] = useState(0);

  const fetchData = () => {
    fetch(`http://localhost:8080/api/ledger/dashboard?username=${username}`)
      .then(res => res.json())
      .then(json => {
        if (!json.error) {
          setData(json);
        }
      })
      .catch(err => console.error(err));
  };
  
  useEffect(() => {
    fetchData();
  }, [username]);

  // Derived state for History table
  const sortedHistory = [...(data.history || [])].sort((a, b) => {
    const timeA = new Date(a.date).getTime();
    const timeB = new Date(b.date).getTime();
    return sortOrder === 'latest' ? timeB - timeA : timeA - timeB;
  });

  const historyPerPage = 6;
  const totalPages = Math.ceil(sortedHistory.length / historyPerPage) || 1;
  const paginatedHistory = sortedHistory.slice((historyPage - 1) * historyPerPage, historyPage * historyPerPage);

  // Handle resets
  useEffect(() => {
    setHistoryPage(1);
  }, [sortOrder, data.history]);

  useEffect(() => {
    setTrackerOffset(0);
  }, [trackerView]);

  // Derived state for Tracker chart
  const computeChartData = () => {
    if (!data.history || data.history.length === 0) {
      return { labels: [], incomePercentages: [], expensePercentages: [], rawData: [] };
    }

    const expenses = data.history.filter(t => !t.isPositive);
    const incomes = data.history.filter(t => t.isPositive);
    const now = new Date();
    
    let labels = [];
    let incValues = [];
    let expValues = [];

    if (trackerView === 'year') {
      const currentYear = now.getFullYear() + (trackerOffset * 5);
      for (let i = 4; i >= 0; i--) {
        labels.push((currentYear - i).toString());
        incValues.push(0);
        expValues.push(0);
      }
      const mapVal = (list, valuesArr) => {
        list.forEach(t => {
          const d = new Date(t.date);
          const diff = currentYear - d.getFullYear();
          if (diff >= 0 && diff <= 4) {
            valuesArr[4 - diff] += t.amount;
          }
        });
      };
      mapVal(incomes, incValues);
      mapVal(expenses, expValues);
    } else if (trackerView === 'month') {
      const targetYear = now.getFullYear() + trackerOffset;
      labels = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
      incValues = new Array(12).fill(0);
      expValues = new Array(12).fill(0);
      const mapVal = (list, valuesArr) => {
        list.forEach(t => {
          const d = new Date(t.date);
          if (d.getFullYear() === targetYear) {
            valuesArr[d.getMonth()] += t.amount;
          }
        });
      };
      mapVal(incomes, incValues);
      mapVal(expenses, expValues);
    } else if (trackerView === 'day') {
      const targetDate = new Date(now);
      targetDate.setDate(now.getDate() + (trackerOffset * 7));
      
      // Find Sunday of that week
      const dayOfWeek = targetDate.getDay();
      const sundayDate = new Date(targetDate);
      sundayDate.setDate(targetDate.getDate() - dayOfWeek);
      
      const shortDays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
      
      for (let i = 0; i < 7; i++) {
        const iterDate = new Date(sundayDate);
        iterDate.setDate(sundayDate.getDate() + i);
        labels.push(`${shortDays[i]} ${iterDate.getMonth() + 1}/${iterDate.getDate()}`);
        incValues.push(0);
        expValues.push(0);
      }
      
      const mapVal = (list, valuesArr) => {
        list.forEach(t => {
          const d = new Date(t.date);
          for (let i = 0; i < 7; i++) {
            const iterDate = new Date(sundayDate);
            iterDate.setDate(sundayDate.getDate() + i);
            if (d.getFullYear() === iterDate.getFullYear() && d.getMonth() === iterDate.getMonth() && d.getDate() === iterDate.getDate()) {
              valuesArr[i] += t.amount;
            }
          }
        });
      };
      mapVal(incomes, incValues);
      mapVal(expenses, expValues);
    }

    const maxInc = Math.max(...incValues, 1);
    const maxExp = Math.max(...expValues, 1);
    const globalMax = Math.max(maxInc, maxExp); 
    
    // Calculate percentages relative to the global max so the scales match
    const incomePercentages = incValues.map(v => (v / globalMax) * 100);
    const expensePercentages = expValues.map(v => (v / globalMax) * 100);

    const rawData = labels.map((lbl, idx) => ({
      label: lbl,
      income: incValues[idx],
      expense: expValues[idx]
    }));

    return { labels, incomePercentages, expensePercentages, rawData };
  };

  const chartInfo = computeChartData();

  return (
    <div className="dashboard-container">
      {/* Sidebar */}
      <aside className="sidebar">
        <div className="sidebar-top">
          <div className="sidebar-logo">OmniLedger</div>
          <div className="sidebar-nav">
            <button className="btn-outline" onClick={() => setShowCurrencyModal(true)}>CHANGE CURRENCY</button>
            <button className="btn-outline" onClick={() => window.open(`http://localhost:8080/api/ledger/export?username=${username}`)}>EXPORT REPORT</button>
          </div>
        </div>
        <div className="sidebar-bottom">
          <span className="logout-link" onClick={onLogout}>Logout</span>
        </div>
      </aside>

      {/* Main Content */}
      <main className="main-content">
        <header className="dashboard-header">
          <h1>Dashboard</h1>
          <div className="header-icon">
            {/* Branding Icon */}
            <div className="mini-pulse-icon">
               <div className="leaf leaf-1"></div>
               <div className="leaf leaf-2"></div>
               <div className="leaf leaf-3"></div>
               <div className="leaf leaf-4"></div>
            </div>
          </div>
        </header>

        {/* Summary Cards */}
        <section className="summary-cards">
          <div className="card card-primary">
            <h3>Total Balance</h3>
            <div className="card-value">{data.currency}{data.balance.toFixed(2)}</div>
            <div className="card-subtext">Current standing</div>
          </div>
          <div className="card card-dark">
            <h3>Total Expenses</h3>
            <div className="card-value">{data.currency}{data.totalExpenses.toFixed(2)}</div>
            <div className="card-subtext">Lifetime</div>
          </div>
          <div className="card card-dark">
            <h3>Total Income</h3>
            <div className="card-value">{data.currency}{data.totalIncome.toFixed(2)}</div>
            <div className="card-subtext">Lifetime</div>
          </div>
        </section>

        {/* Quick Actions */}
        <section className="quick-actions">
          <button className="btn-primary action-btn" onClick={() => setModalType('Income')}>+ Income</button>
          <button className="btn-primary action-btn" onClick={() => setModalType('Expense')}>- Expense</button>
        </section>

        {/* Widgets Row */}
        <section className="widgets-row">
          {/* History Widget */}
          <div className="widget history-widget">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '16px' }}>
              <h3 style={{ margin: 0 }}>History</h3>
              <select 
                style={{ background: 'transparent', color: '#a0a0a0', border: 'none', cursor: 'pointer', outline: 'none' }}
                value={sortOrder} 
                onChange={e => setSortOrder(e.target.value)}
              >
                <option value="latest">Latest first</option>
                <option value="oldest">Oldest first</option>
              </select>
            </div>
            <table className="history-table">
              <thead>
                <tr>
                  <th>Date</th>
                  <th>Source</th>
                  <th>Sessions</th>
                  <th>Change</th>
                </tr>
              </thead>
              <tbody>
                {paginatedHistory.map(row => (
                  <tr key={row.id}>
                    <td style={{ color: '#a0a0a0' }}>{new Date(row.date).toLocaleDateString()}</td>
                    <td>{row.source}</td>
                    <td>{data.currency}{Number(row.amount).toFixed(2)}</td>
                    <td className={row.isPositive ? 'text-positive' : 'text-negative'}>
                      {row.isPositive ? '+' : '-'}{data.currency}{Number(row.amount).toFixed(2)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            
            {/* Pagination Controls */}
            {totalPages > 1 && (
              <div className="pagination">
                <button 
                  onClick={() => setHistoryPage(p => Math.max(1, p - 1))} 
                  disabled={historyPage === 1}
                >&lt;</button>
                <div className="page-numbers">
                  {Array.from({length: totalPages}, (_, i) => i + 1).map(p => (
                    <span 
                      key={p} 
                      className={p === historyPage ? 'active' : ''}
                      onClick={() => setHistoryPage(p)}
                    >
                      {p}
                    </span>
                  ))}
                </div>
                <button 
                  onClick={() => setHistoryPage(p => Math.min(totalPages, p + 1))} 
                  disabled={historyPage === totalPages}
                >&gt;</button>
              </div>
            )}
          </div>

          {/* Tracker Widget */}
          <div className="widget tracker-widget">
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '16px' }}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                <h3 style={{ margin: 0 }}>Tracker (Net Flow)</h3>
                <div className="tracker-nav">
                  <button onClick={() => setTrackerOffset(o => o - 1)}>&lt;</button>
                  <button onClick={() => setTrackerOffset(o => o + 1)} disabled={trackerOffset === 0}>&gt;</button>
                </div>
              </div>
              <div style={{ display: 'flex', gap: '8px' }}>
                <button 
                  style={{ background: trackerView === 'year' ? 'var(--primary-color)' : 'transparent', color: 'white', border: '1px solid var(--primary-color)', borderRadius: '4px', padding: '4px 8px', cursor: 'pointer' }}
                  onClick={() => setTrackerView('year')}
                >Year</button>
                <button 
                  style={{ background: trackerView === 'month' ? 'var(--primary-color)' : 'transparent', color: 'white', border: '1px solid var(--primary-color)', borderRadius: '4px', padding: '4px 8px', cursor: 'pointer' }}
                  onClick={() => setTrackerView('month')}
                >Month</button>
                <button 
                  style={{ background: trackerView === 'day' ? 'var(--primary-color)' : 'transparent', color: 'white', border: '1px solid var(--primary-color)', borderRadius: '4px', padding: '4px 8px', cursor: 'pointer' }}
                  onClick={() => setTrackerView('day')}
                >Day</button>
              </div>
            </div>
            <div className="chart-container split-chart">
              <div className="y-axis split-y-axis">
                <span>Max</span>
                <span></span>
                <span>0</span>
                <span></span>
                <span>Max</span>
              </div>
              <div className="chart-area dual-axis">
                {/* Center zero-line */}
                <div className="zero-line"></div>
                
                {chartInfo.labels.map((lbl, idx) => (
                  <div 
                    key={idx} 
                    className="bar-column dual"
                    onMouseEnter={() => setHoveredBar(idx)}
                    onMouseLeave={() => setHoveredBar(null)}
                  >
                    <div className="income-area">
                      <div className="bar positive" style={{ height: `${chartInfo.incomePercentages[idx]}%` }}></div>
                    </div>
                    <div className="expense-area">
                      <div className="bar negative" style={{ height: `${chartInfo.expensePercentages[idx]}%` }}></div>
                    </div>

                    {/* Tooltip */}
                    {hoveredBar === idx && (
                      <div className="chart-tooltip">
                        <div className="tooltip-date">{trackerView === 'year' ? lbl : trackerView === 'month' ? `${lbl} ${new Date().getFullYear()}` : `${lbl} ${new Date().toLocaleString('default', { month: 'short' })}`}</div>
                        <div className="tooltip-row">
                          <span className="dot positive-dot"></span> Income: {data.currency}{chartInfo.rawData[idx].income.toFixed(2)}
                        </div>
                        <div className="tooltip-row">
                          <span className="dot negative-dot"></span> Expense: {data.currency}{chartInfo.rawData[idx].expense.toFixed(2)}
                        </div>
                      </div>
                    )}
                  </div>
                ))}
              </div>
            </div>
            <div className="x-axis">
              {chartInfo.labels.map(lbl => <span key={lbl}>{lbl}</span>)}
            </div>
          </div>
        </section>
      </main>
      
      {showCurrencyModal && (
        <CurrencyModal
          username={username}
          currentCurrency={data.currency}
          onClose={() => setShowCurrencyModal(false)}
          onComplete={() => {
            setShowCurrencyModal(false);
            fetchData();
          }}
        />
      )}
      
      {modalType && (
        <TransactionModal
          username={username}
          type={modalType}
          currentCurrency={data.currency}
          onClose={() => setModalType(null)}
          onComplete={() => {
            setModalType(null);
            fetchData();
          }}
        />
      )}
    </div>
  );
};

export default Dashboard;
