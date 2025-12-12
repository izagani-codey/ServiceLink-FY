# ServiceLink — Service Marketplace Platform  
Final Year Project (FYP) — 2025  
Built with **ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server**, and **Identity**

---

## 📌 Overview

**ServiceLink** is a web-based platform designed to connect customers with service providers (e.g., electricians, mechanics, cleaners, tutors).  
The system supports multiple roles, secure authentication, service listing management, and a full booking workflow.

This project demonstrates a real-world service marketplace with:

- User registration & login  
- Role-based access (User, Provider, Admin, MasterDemo)  
- Service listing creation & management  
- Customer booking workflow  
- Provider booking approval dashboard  
- Clean UI with responsive Bootstrap layout  

The goal is to deliver a functional, scalable, and user-friendly service platform suitable for business adoption.

---

## 🚀 Tech Stack

### **Backend**
- ASP.NET Core MVC (LTS)
- Entity Framework Core (Code First)
- ASP.NET Core Identity (Custom ApplicationUser)
- C#

### **Frontend**
- Bootstrap 5  
- Razor Views  
- jQuery (minimal usage)

### **Database**
- SQL Server (LocalDB)

---

## 🔐 Authentication & Roles

Implemented using **ASP.NET Identity** with a customized ApplicationUser model.

Current roles supported:

- **User** — can browse & book services  
- **Provider** — can create/manage services & incoming bookings  
- **Admin** — system-level actions (TBD)  
- **MasterDemo** — full-access demo role for showcasing features  

Role seeding is included during application startup.

---

## 🧩 Features Implemented (✔ Completed)

### ✅ **1. User Authentication**
- Registration  
- Login  
- Logout  
- Identity scaffolding integrated  
- Custom ApplicationUser model (FullName, Provider link)  
- Role seeding

---

### ✅ **2. Role-Based Navigation**
Navbar dynamically adapts based on:
- Role  
- Authentication status  
- Current route (active highlighting)

Sticky navbar and layout polish completed.

---

### ✅ **3. Service Management (Provider)**
- Create Service  
- View all services (public)  
- View provider's own services (`MyServices`)  

Model supports:
- Title  
- Description  
- Category  
- Price  
- ProviderId (FK to ApplicationUser)

---

### ✅ **4. Booking Workflow**
**Customer side:**
- Book a service  
- View bookings (`MyBookings`)  
- Cancel pending bookings

**Provider side:**
- View incoming bookings  
- Accept/Reject booking requests  

---

### ✅ **5. Layout & UI Enhancements**
- Updated `_Layout.cshtml`  
- RenderBody + RenderSection fixed  
- Mobile-friendly role-aware navigation  
- Flash messages support  
- Basic hero/landing structure ready  

---

## 🛠 Features in Progress (🔄 Ongoing)

### 🔄 **1. Service Edit/Delete**
To complete provider-side CRUD.

### 🔄 **2. Admin Panel**
Minimal dashboard for:
- User management  
- Service moderation  
- Booking oversight  

### 🔄 **3. Improved Home Page**
Need a hero section and platform introduction for SV demo.

---

## 📅 Planned Features (📝 Upcoming)

### 📝 **1. Provider Dashboard**
Analytics + quick overview:
- Service count  
- Pending bookings  
- Today’s tasks  

### 📝 **2. POS CSV Import (Optional High-Impact Feature)**
Allow providers to upload a CSV file representing inventory/POS data.  
Useful for business integration.

### 📝 **3. Search & Filtering**
Search services by:
- Category  
- Price  
- Keywords  

### 📝 **4. MasterDemo Account Menu**
Toggle between roles for demonstration purposes.

---

## 📁 Project Structure (Current)

ServiceLink/
│
├── Areas/
│ └── Identity/Pages/Account/... # Identity UI
│
├── Controllers/
│ ├── HomeController.cs
│ ├── ServicesController.cs
│ └── BookingsController.cs
│
├── Data/
│ ├── ApplicationDbContext.cs
│ └── DesignTimeDbContextFactory.cs
│
├── Models/
│ ├── ApplicationUser.cs
│ ├── Service.cs
│ └── Booking.cs
│
├── Views/
│ ├── Home/
│ ├── Services/
│ ├── Bookings/
│ └── Shared/
│ ├── _Layout.cshtml
│ └── _FlashMessages.cshtml
│
└── Migrations/



---

## 🔧 How to Run the Project

1. Clone repository  
2. Open solution in **Visual Studio 2022**  
3. Ensure packages restore  
4. Update database:
```bash
dotnet ef database update
Run the project (IIS Express or Kestrel)

Master Demo account (auto seeded):

makefile
Copy code
Email: master@servicelink.test
Password: MasterPass123!
📌 Progress Status (Today)
Category	Status
Authentication	✔ Done
Role System	✔ Done
Service Creation	✔ Done
Service Listing	✔ Done
Booking Workflow	✔ 80%
Provider Dashboard	❌ Not Started
Admin Panel	❌ Not Started
POS Integration	❌ Optional
UI Polish	🔄 Ongoing

🏁 Conclusion
ServiceLink is now functionally stable with user roles, services, and full booking workflow.
Upcoming work focuses on admin functionality, UI polish, and optional advanced features like POS integration.
