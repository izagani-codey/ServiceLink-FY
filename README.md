# ServiceLink — FYP Progress

This repository contains the current development progress of **ServiceLink**, a web platform for connecting customers and service providers.  
Below is what has been completed so far.

---

## ✅ Project Setup (Completed)
- Created ASP.NET Core MVC project using Visual Studio 2022  
- Set up project structure (Controllers, Models, Views, Areas)

---

## ✅ Authentication System (Completed)
- Integrated **ASP.NET Core Identity** into the MVC project
- Scaffolded Identity UI (Register, Login, Logout)
- Added **ApplicationUser** model (custom user with FullName)
- Configured Identity routing and authentication services in `Program.cs`
- Implemented a development-friendly `IEmailSender` (Noop Email Sender)

---

## ✅ Database & Entity Framework Core (Completed)
- Configured **ApplicationDbContext**
- Added models: `Service`, `Booking`, `ApplicationUser`
- Set up SQL Server LocalDB
- Created and applied EF Core migrations
- Database tables successfully generated (Identity tables + custom models)

---

## ✅ Debugging & Fixes (Completed)
- Fixed package version mismatches
- Resolved `FullName` nullability error
- Fixed Identity routing and missing Razor Pages mappings
- Handled DbContext design-time issues
- Corrected scaffolding and email sender dependency errors

---

## ✅ Version Control (Completed)
- Initialized Git repository
- Connected project to GitHub
- Added initial documentation and commits

---

## 📌 Current Status
The system now has:
- Working authentication (Register + Login)
- Functional database
- Identity + custom user model integrated
- Backend foundation ready for next features

---

## 📁 Repository Structure (Current)
ServiceLink/
├── Areas/Identity/Pages/Account
├── Controllers/
├── Data/ApplicationDbContext.cs
├── Models/
├── Migrations/
├── Views/
└── Program.cs


---

## 📌 Next Steps (Not Yet Started)
- Service CRUD (Provider)
- Booking System (Customer → Provider)
- Dashboards
- Admin panel
- UI improvements
