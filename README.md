# 🌟 Codixa API

![Codixa API](https://img.shields.io/badge/ASP.NET-Core-blue.svg)
![Codixa API](https://img.shields.io/badge/SQL-Server-red.svg)
![Codixa API](https://img.shields.io/badge/Status-Under%20Development-yellow.svg)

🚀 **Codixa API** is a robust and scalable API built with **ASP.NET Core** and **SQL Server** to power the Codixa platform. It is designed to handle user authentication, course management, and advanced features with efficiency and security.

---

## ✨ Features
✔️ **User Authentication & Authorization** (JWT-based)  
✔️ **Course Management** (Create, Update, Delete, Enroll)  
✔️ **Role-Based Access Control** (Admin, Instructor, Student)  
✔️ **Pagination & Filtering** for efficient data retrieval  
✔️ **Stored Procedures for optimized database performance**  
✔️ **Secure API Endpoints** with validation & error handling  

---

## 🏗️ Tech Stack
- **Backend**: ASP.NET Core 7  
- **Database**: SQL Server  
- **ORM**: Entity Framework Core  
- **Authentication**: JWT (JSON Web Token)  
- **Caching**: Redis (Planned)  
- **Deployment**: Docker & Azure (Planned)  

---

## 📦 Installation & Setup

### 🔹 1. Clone the Repository
```bash
git clone https://github.com/SiriusEG/CodixaApi.git
cd CodixaApi
```

### 🔹 2. Configure the Database
- Update **appsettings.json** with your SQL Server connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=CodixaDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
}
```
- Apply migrations:
```bash
dotnet ef database update
```

### 🔹 3. Run the API
```bash
dotnet run
```
The API will be available at: **`http://localhost:5000`**

---

## 📜 API Endpoints

### 🔐 Authentication
- `POST /api/Account/RegisterNewStudent` → Register a new student
- `POST /api/Account/RegisterNewInstructor` → Register a new instructor
- `POST /api/Account/Login` → Login and receive a JWT token
- `POST /api/Account/RefreshToken` → Refresh authentication token

### 👨‍🏫 Admin
- `GET /api/Admin/GetInstructorsRequests` → Get pending instructor requests
- `GET /api/Admin/GetApprovedInstructors` → Get approved instructors
- `PUT /api/Admin/ChangeInstructorStatus` → Change instructor approval status
- `POST /api/Admin/RegisterAdmin` → Register a new admin

### 📚 Courses
- `GET /api/Courses/GetCoursesByUserToken/{PageNumber}` → Get courses for authenticated users
- `POST /api/Courses/AddNewCourse` → Add a new course
- `DELETE /api/Courses/Delete/{CourseId}` → Delete a course
- `PUT /api/Courses/Update` → Update course details

### 📂 Course Categories
- `GET /api/CourseCategory` → Get all course categories
- `POST /api/CourseCategory` → Add a new category
- `PUT /api/CourseCategory` → Update a category
- `DELETE /api/CourseCategory` → Delete a category

### 🎓 Student
- `POST /api/Student/StudentRequestToEnrollCourse/{CourseId}` → Request enrollment in a course

### 📖 Sections
- `GET /api/Section/GetAllSections/{CourseId}` → Get all sections of a course
- `POST /api/Section/AddNewSection` → Add a new section
- `PUT /api/Section/UpdateSectionName` → Update section name
- `DELETE /api/Section/Delete` → Delete a section
- `PUT /api/Section/UpdateSectionsLessons` → Update section lessons

### 🎥 Lessons
- `POST /api/Lesson/AddNewLesson` → Add a new lesson
- `DELETE /api/Lesson/Delete` → Delete a lesson

---

## 🛠️ Contributing
Contributions are welcome! Follow these steps:
1. Fork the repository  
2. Create a new branch: `git checkout -b feature-name`  
3. Commit your changes: `git commit -m "Add new feature"`  
4. Push to your fork: `git push origin feature-name`  
5. Open a pull request 🚀  

---

## 📄 License
This project is licensed under the **MIT License**.

📢 **Follow for updates!** ⭐ If you like this project, consider **starring** the repo! 😊

