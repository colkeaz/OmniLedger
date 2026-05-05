# OmniLedger

## Project Description and Purpose
OmniLedger is a modern, offline financial ledger management system designed to provide a secure and straightforward way for users to track their personal or business finances. The purpose of this project is to create a multi-profile environment where individuals can seamlessly log their income and expenses, review their current financial standing through an interactive dashboard, and automatically manage multiple global currencies. It features a sleek Dark Mode UI built with React and a robust C# REST API backend to ensure financial tracking is both beautiful and reliable.

---

## Architecture Overview

OmniLedger follows a decoupled **client-server** architecture:

| Layer | Technology | Description |
|-------|-----------|-------------|
| **Frontend** | React + Vite | Single-page application with component-based UI |
| **Backend** | C# .NET Framework 4.7.2 | Headless REST API server using `System.Net.HttpListener` |
| **Data** | CSV flat-files | Per-user transaction logs and profile storage |

### API Endpoints

| Method | Route | Purpose |
|--------|-------|---------|
| `POST` | `/api/auth/login` | Authenticate a user |
| `POST` | `/api/auth/register` | Create a new user account |
| `GET` | `/api/ledger/dashboard?username=` | Fetch balance, currency, and transaction history |
| `POST` | `/api/ledger/transaction` | Add an income or expense (with multi-currency conversion) |
| `POST` | `/api/ledger/currency` | Change the user's preferred currency and convert all records |
| `GET` | `/api/ledger/export?username=` | Download the full transaction ledger as a CSV file |

---

## UML Diagram



<img width="8192" height="3811" alt="NewUMLDiagram" src="https://github.com/user-attachments/assets/89d5c56c-0eb4-485b-9f98-f83690567137" />



---

## Features and Functionalities

- **Multi-Profile Authentication:** Secure Login and Sign Up system utilizing SHA256 password hashing. Every user has a strictly separate profile and data file.
- **Modern Dark Mode UI:** A custom React-based interface featuring glassmorphism effects, smooth animations, and a premium branded splash screen.
- **Interactive Dashboard:**
  - **Summary Cards** displaying Total Balance, Total Expenses, and Total Income formatted to 2 decimal places.
  - **History Table** with a Date column, sortable by Latest/Oldest, and paginated at 6 transactions per page with clickable page numbers.
  - **Dual-Axis Tracker Chart** showing Income (green bars, upward) and Expenses (red bars, downward) with interactive hover tooltips displaying per-period financial totals.
  - **Time Navigation** with Year, Month, and Day (7-day weekly) view toggles, plus `<` / `>` slider arrows to browse through historical periods.
- **Multi-Currency Transaction Input:** When adding Income or Expenses, users can select any supported currency from a dropdown. The backend automatically converts the amount into the user's preferred display currency before saving.
- **Smart Currency Conversion:** Includes a dynamic offline currency converter supporting USD ($), EUR (€), GBP (£), JPY (¥), PHP (₱), and INR (₹). Changing the dashboard currency converts all existing transactions in real time.
- **Persistent Data Storage:** Income and Expense transactions are automatically saved locally into user-specific CSV files with ISO 8601 timestamps, preventing data loss between sessions.
- **Data Exporting:** Export the entire transaction ledger into a downloadable CSV file directly from the browser.
- **Thread Safety:** All financial operations use `lock` synchronization to prevent data corruption during concurrent API calls.

---

## How the Program Works

1. **Startup:** The C# backend launches an `HttpServer` listening on `http://localhost:8080/`. The React frontend is served separately via the Vite development server on `http://localhost:5173/`.
2. **Authentication:** The user is presented with a branded splash screen. Clicking "Get Started" opens the Auth modal where they can Register or Login. Credentials are validated against the `UserManager`, which reads from a local `users.txt` file with SHA256-hashed passwords.
3. **Dashboard Loading:** Once authenticated, the React frontend calls `GET /api/ledger/dashboard` to fetch the user's balance, preferred currency, and full transaction history. The dashboard renders summary cards, a paginated history table, and the dual-axis tracker chart.
4. **Transaction Processing:** When the user clicks "+ Income" or "- Expense", a modal opens with a currency dropdown (defaulting to their preferred currency). The frontend sends a `POST /api/ledger/transaction` with the amount, description, type, and selected currency. The backend auto-converts via `CurrencyConverter` if needed, then the `LedgerManager` processes and persists the transaction.
5. **Currency Switching:** The "Change Currency" button in the sidebar opens a modal. Selecting a new currency triggers `POST /api/ledger/currency`, which converts every historical transaction and updates the user's profile preference.
6. **Data Export:** The "Export Report" button opens `GET /api/ledger/export` in a new browser tab, triggering a CSV file download.

---

## Instructions on How to Run the Application

### Prerequisites
- **Windows** Operating System
- **.NET Framework 4.7.2** (or newer) installed
- **Node.js** (v18 or newer) and **npm** installed — [Download Node.js](https://nodejs.org/)

### Step 1: Start the Backend (C# API Server)

#### Option A: Run from pre-built binary
1. Navigate to `OmniLedger\bin\Debug\`.
2. Run `OmniLedger.exe` from a terminal or double-click it.
3. You should see: `Server started on http://localhost:8080/`

#### Option B: Build from source
1. Open a terminal in the root `OmniLedger` directory.
2. Run:
   ```
   dotnet build OmniLedger/OmniLedger.csproj
   ```
3. Then run the compiled executable:
   ```
   OmniLedger\bin\Debug\OmniLedger.exe
   ```

### Step 2: Start the Frontend (React Dev Server)
1. Open a **new** terminal window.
2. Navigate to the `omniledger-ui` directory:
   ```
   cd omniledger-ui
   ```
3. Install dependencies (first time only):
   ```
   npm install
   ```
4. Start the development server:
   ```
   npm run dev
   ```
5. The terminal will display: `Local: http://localhost:5173/`

### Step 3: Open the App
1. Open your browser and navigate to **http://localhost:5173/**.
2. Click "Get Started" on the splash screen.
3. Register a new account or log in with existing credentials.
4. You're in! Start tracking your finances.

> **Important:** Both the C# backend and the React dev server must be running simultaneously for the application to function.

---

## Project Structure

```
OmniLedger/
├── OmniLedger/                  # C# Backend
│   ├── Logic/
│   │   ├── HttpServer.cs        # REST API controller & routing
│   │   ├── LedgerManager.cs     # Thread-safe financial operations
│   │   ├── UserManager.cs       # User authentication & profiles
│   │   ├── DataStore.cs         # CSV persistence layer
│   │   ├── CurrencyConverter.cs # Offline exchange rate conversion
│   │   ├── Transaction.cs       # Base transaction model
│   │   ├── Income.cs            # IncomeRecord subclass
│   │   ├── Expense.cs           # BusinessExpense subclass
│   │   ├── User.cs              # User profile model
│   │   ├── IReportGenerator.cs  # Report generation interface
│   │   ├── ExcelExporter.cs     # CSV export implementation
│   │   └── PdfExporter.cs       # PDF export (placeholder)
│   ├── Program.cs               # Entry point (starts HttpServer)
│   └── OmniLedger.csproj        # Project configuration
│
├── omniledger-ui/               # React Frontend
│   ├── src/
│   │   ├── components/
│   │   │   ├── SplashScreen.jsx # Animated landing page
│   │   │   ├── AuthModal.jsx    # Login / Sign Up modal
│   │   │   ├── Dashboard.jsx    # Main dashboard with charts
│   │   │   ├── TransactionModal.jsx  # Income/Expense input
│   │   │   └── CurrencyModal.jsx     # Currency switcher
│   │   ├── App.jsx              # Root component & routing
│   │   └── main.jsx             # React entry point
│   ├── package.json
│   └── vite.config.js
│
├── .gitignore
└── README.md
```

---

## Development Team

- **Luke Andre V. Paala** - Project Head/Leader
- **Amber Dadap** - UI/UX Designer
- **Hans Gadiel P. Caraig** - Logic Tester
