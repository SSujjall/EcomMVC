# EcomMVC

EcomMVC is a feature-rich e-commerce platform built using ASP.NET Core 8 MVC with Razor Pages. This project implements modern web development practices to deliver a complete e-commerce solution with both customer and administrative functionalities.

![Project Preview](https://via.placeholder.com/800x400?text=EcomMVC+Preview) <!-- Add actual screenshot later -->

## Key Features

### Core Functionality
- **User Authentication & Authorization**
  - Secure registration/login with ASP.NET Identity
  - Role-based access control (Admin/Customer)
  - Email verification system
  - Password reset functionality
- **Cloud Image Storage**
  - Integration with Cloudinary for image storage
- **Admin Panel**
  - Product catalog management
  - Order management system
  - User management interface
  - Sales analytics dashboard

### Customer Experience
- **Product Catalog**
  - Product browsing with filters
  - Search functionality
  - Product details with image gallery
- **Shopping Cart**
  - Persistent cart functionality
  - Guest checkout option
- **Order Management**
  - Order history tracking
  - Order status updates
- **Payment Integration**
  - Khalti payment gateway implementation
  - Secure transaction processing

## Technologies Used

### Backend
- ASP.NET Core 8 MVC
- Entity Framework Core (Code-First Approach)
- ASP.NET Identity for authentication
- MailKit for email services
- Cloudinary SDK for image management

### Frontend
- Razor Pages with Bootstrap 5
- jQuery/AJAX for dynamic interactions
- HTML5/CSS3 with responsive design

### Database
- Microsoft SQL Server

### Third-Party Services
- Khalti Payment Gateway
- Cloudinary Image CDN
- SMTP Email Service

## Installation

### Prerequisites
- .NET 8 SDK
- Microsoft SQL Server
- Visual Studio 2022 or VS Code
- Accounts with Khalti and Cloudinary

### Setup Steps
1. Clone the repository:
   git clone https://github.com/yourusername/EcomMVC.git

2. Install required NuGet packages:
   dotnet restore

3. Configure app settings:
   update appsettings.json with actual values.

4. Apply database migration:
   (using package manager): update-database
   (using cli): dotnet ef database update

5. Run the application:
   dotnet run