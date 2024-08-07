# Student Management Application

This is a student project application built with .NET for the backend and Angular for the frontend. The application allows an administrator to manage students, enroll them in courses, and change user roles.

## Administrator Login
- **Email:** admin@admin.com
- **Password:** Admin@123

## Features
- **User Authentication:**
  - Login and registration functionality for users.
- **Manage Students:**
  - Add, delete, and update student information.
- **Course Enrollment:**
  - Enroll students in various courses.
- **User Roles:**
  - Change roles of users within the system.

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/)

### Installation and Running

1. **Backend (API)**
    - Navigate to the `api` folder:
      ```sh
      cd api
      ```
    - Run the .NET application:
      ```sh
      dotnet run
      ```

2. **Frontend (Client)**
    - Navigate to the `client` folder:
      ```sh
      cd client
      ```
    - Install the dependencies:
      ```sh
      npm install
      ```
    - Run the Angular application:
      ```sh
      ng serve
      ```

### How It Works

Once the application is running, you can log in or register as a new user. The administrator can log in using the provided credentials. The administrator has the following capabilities:

- **User Authentication:**
  - Users can register for a new account.
  - Users can log in to access the system.

- **Manage Students:**
  - Add new students to the system.
  - Update existing student information.
  - Delete students from the system.

- **Course Enrollment:**
  - Add new course to the system.
  - Enroll students in different courses.
  - View enrolled courses for each student.

- **User Roles:**
  - Change the roles of users, assigning them different permissions within the system.

This project demonstrates basic CRUD operations, user authentication, and role management, providing a solid foundation for a student management system.
