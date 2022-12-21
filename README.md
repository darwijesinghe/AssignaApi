# AssignaApi

## Project Purpose

This simple task management API application was made to showcase my coding ability to those who would like to hire me as a .NET developer.

This app was mainly developed to demonstrate basic CRUD operations using .NET Core REST API. This full app was designed and developed by myself. AssignaApi can use it as a back-end project to develop another small task management application.

## Contributors

- Darshana Wijesinghe

## Application Structure

### Back-end design

- .NET Core 3.1
  - C #

### Database

- Microsoft SQL Server EF Core

### Security

- Secure with JWT Bearer token

## App Features

- Add a new task
  - This can do within only team lead account
- Tasks are categorized
  - Pending tasks
  - Complete tasks
  - High priority
  - Medium priority
  - Low priority
- Role based access
  - team-lead
  - team-member
- Tasks are prioritized
  - High
  - Medium
  - Low
- View task information
  - Assigner can Edit the task if the task is still pending
  - Assigner can Delete a task whether it is completed or not
  - Assignee can Add a Note for the task if the task is still not done
- Send warning email
  - Team lead can send a task completion warning to the assignee via email

## Security

- Secure with Tokens
  - JWT Bearer token
  - Random generated refresh token
  - Password reset token

## Usage

### Base URL of the application

- `https://assignaapi.azurewebsites.net/`

## Login / Register / Reset Password / Refresh Token

- ### User registration

```json5
// [POST]
// [base url]/user/register

// Team lead account
// Request

{
  "user_name": "manudi@test",
  "first_name": "manudi",
  "email": "manudi@example.com",
  "password": "manudi@123",
  "role": "team-lead"
}

// Team member account
// Request

{
  "user_name": "manudi@test",
  "first_name": "manudi",
  "email": "manudi@example.com",
  "password": "manudi@123",
  "role": "team-member"
}

// Response

{
    "message": "Ok",
    "success": true
}
```

- ### User login

```json5
// [POST]
// [base url]/user/login

// Request

{
  "user_name": "manudi@test",
  "password": "manudi@123"
}

// Response

{
    "message": "Ok",
    "success": true,
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refresh_token": "4vZkirs9ZxD7kx5IgjQ9HcG4aze6RoEv..."
}
```

- ### Forgot password

```json5
// [POST]
// [base url]/user/forgot-password

// Request

{
  "email": "manudi@example.com"
}

// Response

{
    "message": "Ok",
    "success": true,
    "reset_token": "4vZkirs9ZxD7kx5IgjQ9HcG4aze6RoEv4CwFdzrd5wt..."
}
```

- ### Reset password

```json5
// [POST]
// [base url]/user/reset-password

// Request

{
  "password": "manudi@123",
  "con_password": "manudi@123",
  "reset_token": "4vZkirs9ZxD7kx5IgjQ9HcG4aze6RoEv..."
}

// Response

{
    "message": "Ok",
    "success": true
}
```

- ### Refresh token

```json5
// [POST]
// [base url]/user/refresh-token

// Request

{
  "refresh_token": "4vZkirs9ZxD7kx5IgjQ9HcG4aze6RoEv..."
}

// Response

{
    "message": "Ok",
    "success": true,
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refresh_token": "4vZkirs9ZxD7kx5IgjQ9HcG4aze6RoEv..."
}
```

## Team Lead

- ### Task categories

```
// [GET]
https://assignaapi.azurewebsites.net/category/categories
```

- ### Task priorities

```
// [GET]
https://assignaapi.azurewebsites.net/priority/priorities
```

- ### Team members

```
// [GET]
https://assignaapi.azurewebsites.net/user/members
```

- ### All tasks

```
// [GET]
https://assignaapi.azurewebsites.net/leadtasks/tasks
```

- ### Pending tasks

```
// [GET]
https://assignaapi.azurewebsites.net/leadtasks/pendings
```

- ### Completed tasks

```
// [GET]
https://assignaapi.azurewebsites.net/leadtasks/completes
```

- ### High priority tasks

```
// [GET]
https://assignaapi.azurewebsites.net/leadtasks/high-priority
```

- ### Medium priority tasks

```
// [GET]
https://assignaapi.azurewebsites.net/leadtasks/medium-priority
```

- ### Low priority tasks

```
// [GET]
https://assignaapi.azurewebsites.net/leadtasks/low-priority
```

- ### Add a new task

```json5
// [POST]
// [base url]/leadtasks/add-task

// Request

{
  "tsk_title": "Test task",
  "tsk_category": 1,
  "deadline": "2022-12-18",
  "priority": "High",
  "member": 2,
  "tsk_note": "Test note for the test task"
}

// Response

{
  "message" = "Ok",
  "success" = true
}
```

- ### Edit a task

```json5
// [POST]
// [base url]/leadtasks/edit-task

// Request

{
  "tsk_id": 1,
  "tsk_title": "Test task edit",
  "tsk_category": 1,
  "deadline": "2022-12-19",
  "priority": "Low",
  "member": 2,
  "tsk_note": "Edit test note for the test task"
}

// Response

{
  "message" = "Ok",
  "success" = true
}
```

- ### Delete a task

```json5
// [POST]
// [base url]/leadtasks/delete-task

// Request

{
  "tsk_id": 1
}

// Response

{
  "message" = "Ok",
  "success" = true
}
```

- ### Send an email remind to assignee

```json5
// [POST]
// [base url]/leadtasks/send-remind

// Request

{
  "tsk_id": 1,
  "message": "Please do this task as soon as possible"
}

// Response

{
  "message" = "Ok",
  "success" = true
}
```

- ### Task info

```
// [GET]
https://assignaapi.azurewebsites.net/leadtasks/task-info?taskid=1
```

## Team Member

- ### All tasks

```
// [GET]
https://assignaapi.azurewebsites.net/membertasks/tasks
```

- ### Pending tasks

```
// [GET]
https://assignaapi.azurewebsites.net/membertasks/pendings
```

- ### Completed tasks

```
// [GET]
https://assignaapi.azurewebsites.net/membertasks/completes
```

- ### High priority tasks

```
// [GET]
https://assignaapi.azurewebsites.net/membertasks/high-priority
```

- ### Medium priority tasks

```
// [GET]
https://assignaapi.azurewebsites.net/membertasks/medium-priority
```

- ### Low priority tasks

```
// [GET]
https://assignaapi.azurewebsites.net/membertasks/low-priority
```

- ### Add a note for task

```json5
// [POST]
// [base url]/membertasks/write-note

// Request

{
  "tsk_id": 1,
  "user_note": "This will be take two more days"
}

// Response

{
  "message" = "Ok",
  "success" = true
}
```

- ### Mark as task done

```json5
// [POST]
// [base url]/membertasks/mark-done

// Request

{
  "tsk_id": 1
}

// Response

{
  "message" = "Ok",
  "success" = true
}
```

## Support

Darshana Wijesinghe  
Email address - [dar.mail.work@gmail.com](mailto:dar.mail.work@gmail.com)  
Linkedin - [darwijesinghe](https://www.linkedin.com/in/darwijesinghe/)  
GitHub - [darwijesinghe](https://github.com/darwijesinghe)

## License

This project is licensed under the terms of the **MIT** license.
