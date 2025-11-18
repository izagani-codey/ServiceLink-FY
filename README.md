# ServiceLink — Service Link, For Everyone

**ServiceLink** is a modern web-based service marketplace designed to seamlessly connect **customers** with **local service providers** in a secure, scalable, and user-friendly platform.  
Developed using **ASP.NET Core MVC**, **Razor Pages (Identity)**, and **Entity Framework Core**, this system integrates real-world service workflows with academic software engineering practices.

> **ServiceLink — connecting services and people, effortlessly.**

---

# 📌 Project Overview

ServiceLink simplifies the process of discovering, requesting, and managing services.  
The platform supports two primary user groups:

- **Customers** — browse available services and request bookings  
- **Service Providers** — create and manage service offerings, and respond to booking requests  

The system also includes an **Admin** role for supervising user accounts and system integrity.

---

# 🧩 Key Features (Completed So Far)

### ✅ **1. ASP.NET Core MVC Project Setup**
- Created solution using Visual Studio 2022  
- Clean folder structure (Controllers, Views, Models, Areas, Data)

### ✅ **2. Database Setup (EF Core)**
- Configured `ApplicationDbContext`
- Added `Service` and `Booking` models  
- Installed EF Core packages matched to .NET 8  
- Created and applied migrations  
- Database successfully generated (`ServiceLinkDb`)

### ✅ **3. Authentication & Identity Integration**
- Integrated ASP.NET Core Identity into existing MVC project  
- Added custom `ApplicationUser` model with `FullName` property  
- Fully scaffolded Identity UI:
  - Register / Login / Logout  
  - AccessDenied  
  - RegisterConfirmation  
- Added Razor Pages routing (`AddRazorPages()` + `MapRazorPages()`)

### ✅ **4. Bug Fixes During Identity Setup**
- Resolved package version mismatch (NET 10 vs NET 8)  
- Implemented design-time DbContext creation fixes  
- Added `NoopEmailSender` to prevent email-sender crashes  
- Resolved SQL error: `FullName` not nullable → migration added  
- Successfully registered users in Identity system

### ✅ **5. GitHub Integration**
- Project pushed to GitHub repository  
- Commit history recorded  
- Documentation folder planned  

---

# 🏗️ In Progress / Pending Features (TBD / TBC)

These represent your current stopping point and what comes next:

### 🔄 **TBD (To Be Done)**
- Service CRUD (Create, Edit, Delete, View) for providers  
- Public marketplace listing of available services  
- Booking workflow (Customer requests → Provider approves/declines)  
- Provider dashboard (manage services + bookings)  
- Customer dashboard (track bookings and statuses)  

### 🧪 **Under Construction**
- Role-based UI filtering (Provider-only pages, etc.)  
- Adding required input fields to Identity (FullName on Register page)  
- Implementing basic search/filter for services (category, price, keywords)

### 📝 **TBC (To Be Continued)**
- Integrating image uploads for services  
- Implementing notifications (email/SMS)  
- Admin management panel  
- Frontend improvements with Bootstrap  
- Deployment documentation (Azure / IIS)

---

# 🗂️ Project Structure

ServiceLink/
│
├── Areas/
│ └── Identity/
│ └── Pages/Account/ ← Scaffolded Register/Login/Logout/etc.
│
├── Controllers/
│ └── HomeController.cs
│ └── (ServicesController.cs) ← To be added
│
├── Data/
│ └── ApplicationDbContext.cs
│ └── Migrations/
│
├── Models/
│ ├── ApplicationUser.cs
│ ├── Service.cs
│ └── Booking.cs
│
├── Views/
│ ├── Home/
│ ├── Shared/
│ └── (Service views) ← To be added
│
├── Services/
│ └── NoopEmailSender.cs
│
├── Program.cs
├── appsettings.json
├── ServiceLink.csproj
└── README.md

yaml
Copy code

---

# 🧪 Demo Instructions (Current Capabilities)

1. Launch the application (`F5` in Visual Studio).
2. Navigate to:
   - Register → `/Identity/Account/Register`
   - Login → `/Identity/Account/Login`
3. Inspect created users in SQL Server Object Explorer under:
ServiceLinkDb → Tables → dbo.AspNetUsers

yaml
Copy code

> Booking and service management not completed yet — planned next.

---

# 🛠️ Technologies Used

- **ASP.NET Core MVC 8**
- **ASP.NET Core Identity**
- **Entity Framework Core**
- **SQL Server LocalDB**
- **Bootstrap 5 (for UI)**
- **C# 12**
- **Visual Studio 2022**
- **GitHub for version control**

---

# 🚧 Running the Project (Local Setup)

### 1. Restore dependencies
```bash
dotnet restore
2. Apply database migrations
bash
Copy code
dotnet ef database update -c ApplicationDbContext
3. Run the project
bash
Copy code
dotnet run
4. Access authentication pages
pgsql
Copy code
/Identity/Account/Register  
/Identity/Account/Login
