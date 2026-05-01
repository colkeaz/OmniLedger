# OmniLedger

## Project Description and Purpose
OmniLedger is a modern, offline financial ledger management system designed to provide a secure and straightforward way for users to track their personal or business finances. The purpose of this project is to create a multi-profile environment where individuals can seamlessly log their income and expenses, review their current financial standing through a clear dashboard, and automatically manage multiple global currencies. It features a sleek Dark Mode UI and robust data persistence to ensure financial tracking is both beautiful and reliable.

---

## UML Diagram
*You can paste your own exported UML image below, or use the interactive Mermaid diagram provided.*

```
<img width="8192" height="3811" alt="NewUMLDiagram" src="https://github.com/user-attachments/assets/89d5c56c-0eb4-485b-9f98-f83690567137" />

```

---

## Features and Functionalities of the System
- **Multi-Profile Authentication:** Secure Login and Sign Up system utilizing SHA256 password hashing. Every user has a strictly separate profile and data file.
- **Modern Dark Mode UI:** A custom borderless window design featuring draggable headers, styled flat controls, and an anti-aliased dynamic bar chart.
- **Persistent Data Storage:** Income and Expense transactions are automatically saved locally into user-specific CSV files, preventing data loss between sessions.
- **Smart Currency Conversion:** Includes a dynamic offline currency converter. Users can input transactions in various currencies (USD, EUR, GBP, JPY, PHP, INR) which immediately convert to the user's saved, preferred display currency.
- **Data Exporting:** The ability to export the entire transaction ledger and current balance into an Excel-compatible (.csv) file for external processing or backup.

---

## Explanation of How the Program Works
1. **Authentication:** Upon launch, the `UserManager` loads all known user credentials from a local file (`users.txt`). The user is presented with `Form1` where they can either Register or Login.
2. **Initialization:** Once authenticated, `Form1` initializes a `LedgerManager` context specific to the user and launches the Dashboard (`Form2`).
3. **Data Loading:** The `LedgerManager` utilizes the `DataStore` to read the user's dedicated financial CSV file, parsing all historical transactions into memory and calculating the `CurrentBalance`.
4. **Transaction Processing:** When a user clicks "+ Income" or "- Expense", they are prompted for an amount, a category/source, and the currency of the transaction. The `CurrencyConverter` scales the amount to the user's stored `PreferredCurrency`. The `LedgerManager` then registers the `IncomeRecord` or `BusinessExpense` and immediately calls `DataStore` to save the updated state to the hard drive.
5. **UI Rendering:** The Dashboard seamlessly updates the balance text, populates the datagrid with new rows, and recalculates the relative heights for the Income vs. Expense Bar Chart graphics.

---

## Instructions on How to Run the Application
### Prerequisites:
- A Windows Operating System.
- .NET Framework 4.7.2 (or newer) installed on your machine.

### Running the App:
1. Navigate to the project directory: `OmniLedger\bin\Debug\`.
2. Locate and double-click the `OmniLedger.exe` executable file.
3. If this is your first time, use the toggle at the bottom to switch to the **Sign Up** screen to create an account.
4. Log in using your newly created credentials to access your personal dashboard.

### Building from Source:
1. Open a command prompt or PowerShell inside the root `OmniLedger` directory.
2. Run the command: `dotnet build OmniLedger.csproj`
3. The newly compiled executable will be generated in `bin\Debug\`.

---

## Development Team

- **Luke Andre V. Paala** - Project Head/Leader
- **Amber Dadap** - UI/UX Designer
- **Hans Gadiel P. Caraig** - Logic Tester
