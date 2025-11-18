# ServiceLink-FY

**ServiceLink** — a two-sided service marketplace (Provider ↔ Customer) built with ASP.NET Core (MVC + Razor Pages), EF Core and Identity.

This repo contains the FYP prototype: authentication (Identity), service management (Providers), booking flow (Customers), and an admin seeding script.

---

## Quick summary
- Framework: **.NET 8** (project target `net8.0`)
- Identity: ASP.NET Core Identity (custom `ApplicationUser`)
- ORM: Entity Framework Core (SQL Server LocalDB)
- Main features implemented:
  - User registration & login (Identity UI scaffolded)
  - Role seeding (Admin / Provider / Customer)
  - `Service` model + `Booking` model
  - EF Migrations + database created (`ServiceLinkDb`)
  - Basic Provider CRUD & Customer booking flow (in progress)

---

## Quick start (local)
1. Clone repo:
   ```bash
   git clone https://github.com/<your-username>/ServiceLink-FY.git
   cd ServiceLink-FY
