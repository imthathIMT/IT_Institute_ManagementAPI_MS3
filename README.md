# IT Institute Management API

A comprehensive .NET 8 Web API for managing an IT institute's operations, including student enrollment, course management, payment processing, and administrative functions.

## 🚀 Features

### Core Modules
- **Student Management**: Complete student lifecycle management with profile, enrollment tracking, and academic records
- **Course Management**: Course catalog, scheduling, and academic content management
- **Enrollment System**: Student course enrollment with payment plan integration
- **Payment Processing**: Financial transaction handling and payment tracking
- **Admin Dashboard**: Administrative controls and institute management
- **Authentication & Authorization**: JWT-based secure authentication with role-based access
- **Notification System**: Real-time notifications for students and administrators
- **Communication**: Student messaging and announcement broadcasting
- **Contact Management**: Inquiry handling and support ticket management

### Additional Features
- **Image Management**: Profile pictures and course images with file upload handling
- **Email Integration**: Automated email notifications using SMTP and SendGrid
- **Social Media Links**: Student and institute social media profile management
- **Reporting & Analytics**: Summary dashboards and revenue tracking
- **Account Security**: Account lockout protection and password security

## 🛠️ Technology Stack

### Backend Framework
- **.NET 8.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API development
- **Entity Framework Core 8.0** - Object-relational mapping
- **SQL Server** - Primary database (LocalDB for development)

### Authentication & Security
- **JWT Bearer Authentication** - Secure token-based authentication
- **BCrypt.Net** - Password hashing and security
- **ASP.NET Core Identity** - User management framework

### Communication & Notifications
- **SendGrid** - Email delivery service
- **NETCore.MailKit** - SMTP email handling

### Documentation & Development
- **Swagger/OpenAPI** - API documentation and testing interface
- **Microsoft.EntityFrameworkCore.Tools** - Database migration tools

### Architecture Pattern
- **Repository Pattern** - Data access abstraction
- **Service Layer Pattern** - Business logic separation
- **Dependency Injection** - IoC container for loose coupling
- **DTO Pattern** - Data transfer objects for API communication

## 📁 Project Structure

```
IT_Institute_Management/
├── Controllers/              # API endpoint controllers
│   ├── AdminController.cs
│   ├── AuthController.cs
│   ├── CourseController.cs
│   ├── StudentsController.cs
│   ├── EnrollmentController.cs
│   ├── PaymentController.cs
│   ├── NotificationController.cs
│   └── ...
├── Entity/                   # Domain models/entities
│   ├── Student.cs
│   ├── Course.cs
│   ├── Enrollment.cs
│   ├── Payment.cs
│   └── ...
├── DTO/                      # Data Transfer Objects
│   ├── RequestDTO/
│   └── ResponseDTO/
├── IRepositories/            # Repository interfaces
├── Repositories/             # Repository implementations
├── IServices/                # Service interfaces
├── Services/                 # Service implementations
├── Database/                 # Database context and configurations
│   ├── InstituteDbContext.cs
│   ├── SampleDataIntializer.cs
│   └── EmailTemplateInitializer.cs
├── EmailSection/             # Email service implementation
├── ImageService/             # Image handling service
├── PasswordService/          # Password management
├── Migrations/               # EF Core database migrations
└── wwwroot/                  # Static files and images
    └── images/
        ├── students/
        ├── courses/
        └── admins/
```

## 🔧 Installation & Setup

### Prerequisites
- **.NET 8.0 SDK** or later
- **SQL Server** or **SQL Server LocalDB**
- **Visual Studio 2022** or **Visual Studio Code**
- **Git** for version control

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd IT_Institute_ManagementAPI_MS3
   ```

2. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

3. **Configure Database Connection**

   Update `appsettings.json` with your database connection string:
   ```json
   {
     "ConnectionStrings": {
       "DevHubDB": "Server=(localdb)\\MSSQLLocalDB;Database=DevHub;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

4. **Configure Email Settings**

   Update email configuration in `appsettings.json`:
   ```json
   {
     "EMAIL_CONFIGURATION": {
       "FromName": "Your Institute Name",
       "HOST": "smtp.gmail.com",
       "PORT": 465,
       "EMAIL": "your-email@gmail.com",
       "Username": "your-email@gmail.com",
       "PASSWORD": "your-app-password"
     }
   }
   ```

5. **Run Database Migrations**
   ```bash
   dotnet ef database update
   ```

6. **Build and Run the Application**
   ```bash
   dotnet build
   dotnet run
   ```

7. **Access the Application**
   - API: `https://localhost:7xxx` (port will be displayed in console)
   - Swagger Documentation: `https://localhost:7xxx/swagger`

## 📊 Database Schema

### Main Entities

- **Students**: Student profiles with NIC-based identification
- **Courses**: Course catalog with pricing and duration
- **Enrollments**: Student-course relationships with payment plans
- **Payments**: Financial transactions and payment tracking
- **Admins**: Administrative user accounts
- **Users**: Base user entity for authentication
- **Notifications**: System-wide notification management
- **Announcements**: Institute-wide announcements
- **ContactUs**: Customer inquiry management

### Key Relationships
- Student → Enrollments (One-to-Many)
- Course → Enrollments (One-to-Many)
- Enrollment → Payments (One-to-Many)
- Student → Address (One-to-One)
- Student → SocialMediaLinks (One-to-One)

## 🔐 Authentication

The API uses JWT Bearer token authentication:

1. **Login**: POST `/api/Auth/login` with credentials
2. **Receive JWT Token**: Use token in Authorization header
3. **Access Protected Endpoints**: `Authorization: Bearer <token>`

### Default Admin Account
- Username: Set during sample data initialization
- Password: Set during sample data initialization

## 📡 API Endpoints

### Authentication
- `POST /api/Auth/login` - User authentication
- `POST /api/Auth/register` - User registration

### Students
- `GET /api/Students` - Get all students
- `GET /api/Students/{nic}` - Get student by NIC
- `POST /api/Students` - Create new student
- `PUT /api/Students/{nic}` - Update student
- `DELETE /api/Students/{nic}` - Delete student

### Courses
- `GET /api/Course` - Get all courses
- `GET /api/Course/{id}` - Get course by ID
- `POST /api/Course` - Create new course
- `PUT /api/Course/{id}` - Update course
- `DELETE /api/Course/{id}` - Delete course

### Enrollments
- `GET /api/Enrollment` - Get all enrollments
- `POST /api/Enrollment` - Create new enrollment
- `PUT /api/Enrollment/{id}` - Update enrollment

### Payments
- `GET /api/Payment` - Get all payments
- `POST /api/Payment` - Process payment
- `GET /api/Payment/enrollment/{enrollmentId}` - Get payments by enrollment

*[Additional endpoints available - see Swagger documentation for complete API reference]*

## 🎯 Sample Data

The application includes sample data initialization for:
- Master admin account
- Sample administrators
- Demo students
- Course catalog
- Enrollment records
- Payment records
- Announcements
- Contact inquiries
- Notifications

## 🔧 Configuration

### JWT Settings
```json
{
  "Jwt": {
    "Issuer": "DevHub",
    "Audience": "Users",
    "Key": "your-secret-key-here"
  }
}
```

### CORS Policy
- Currently configured for open access (`*`)
- Modify in `Program.cs` for production deployment

### File Upload
- Student images: `wwwroot/images/students/`
- Course images: `wwwroot/images/courses/`
- Admin images: `wwwroot/images/admins/`

## 🚀 Deployment

### Development
```bash
dotnet run --environment Development
```

### Production
1. Update `appsettings.Production.json` with production settings
2. Build for release:
   ```bash
   dotnet publish -c Release
   ```
3. Deploy to your preferred hosting platform (IIS, Azure, AWS, etc.)

## 🧪 Testing

Access the interactive API documentation:
- **Swagger UI**: Available at `/swagger` endpoint
- **Test Authentication**: Use login endpoint to get JWT token
- **Authorize**: Click 'Authorize' button in Swagger and enter: `Bearer <your-token>`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 📞 Support

For support and queries, please contact the development team or create an issue in the repository.

## 🔄 Version History

- **v1.0.0** - Initial release with core functionality
- **v1.1.0** - Added course image support and CORS policy updates

---

**Developed by**: Unicom TIC Development Team
**Last Updated**: December 2024