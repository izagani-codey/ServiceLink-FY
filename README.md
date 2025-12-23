ServiceLink

ServiceLink is a role-based service booking web application developed using ASP.NET Core MVC, Entity Framework Core, SQL Server, and ASP.NET Identity.
The system connects customers with service providers, allowing users to request services and providers to manage incoming bookings.

This project is developed as a Final Year Project (FYP) and is also intended to evolve into a production-ready platform beyond academic requirements.

🚀 Project Vision

ServiceLink aims to simplify service discovery and booking by providing:

A clear user → provider booking workflow

Role-based access control

Scalable backend architecture

A foundation for future enhancements such as approval workflows, notifications, and POS integration

The current focus is on core functionality and workflow stability, followed by UI/UX refinement.

🧱 Technology Stack

Backend: ASP.NET Core MVC (.NET 8)

Authentication: ASP.NET Core Identity

Database: SQL Server + Entity Framework Core

Frontend: Razor Views + Bootstrap (UI redesign in progress via Figma)

Architecture: MVC + ViewModels (separation of concerns)

👥 User Roles

The system currently supports multiple roles:

User (Customer):
Browse services, submit booking requests, view own bookings

Provider (Service Owner):
Create services, view incoming booking requests

Admin:
Reserved for system management (planned)

MasterDemo:
Demo/testing role with extended visibility for presentation purposes

🔄 Core Workflow (Current State)
User Flow

Register and log in

Browse available services

View service details

Submit a booking request

View booking history

Provider Flow

Log in as provider

Create and manage services

View incoming booking requests linked to their services

⚠️ Booking approval/rejection and notifications are planned for the next development phase.

✅ Implemented Features

User authentication and authorization (Identity)

Role-based access control

Service creation and listing

Booking request creation

Provider-linked booking routing

Booking visibility for users and providers

Database migrations and schema setup

ViewModels for safe UI data exposure

🛠️ Work in Progress / Planned Features

Booking approval & status updates (Accept / Reject / Complete)

Email or in-app notifications

Improved provider dashboard

UI redesign and polish (Figma-based design in progress)

Validation and error handling improvements

Admin moderation tools

Optional POS / inventory integration (future scope)

🎨 UI / UX Status

The current UI prioritizes functionality and clarity.
A redesigned UI is being prototyped in Figma to improve:

Navigation flow

Visual hierarchy

User experience for booking and management

UI integration will follow once backend workflows are fully stabilized.

📌 Project Status

Development Stage: Core functionality implemented, workflow stabilization ongoing

Focus Area: Booking workflow reliability and role-based visibility

Next Milestone: Provider actions + UI integration

This project reflects iterative development, with features being built, tested, and refined incrementally.

📂 Project Structure Overview
Controllers/
  ├── ServicesController
  ├── BookingsController
  └── HomeController

Models/
  ├── ApplicationUser
  ├── Service
  └── Booking

ViewModels/
  ├── ServiceCreateViewModel
  ├── BookingCreateViewModel
  └── BookingListItemViewModel

Views/
  ├── Services/
  ├── Bookings/
  ├── Home/
  └── Shared/

Data/
  └── ApplicationDbContext

🧠 Notes

This repository reflects active development.
Some features may be partially implemented or under refinement as the project progresses.

👤 Author

Ahmed Amru Athif
Final Year Diploma Project
ASP.NET MVC | Software Engineering | Full-stack Development
