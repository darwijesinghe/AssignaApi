# AssignaApi

## Project Purpose

This simple task management API was created to showcase my coding skills to potential employers interested in hiring me as a .NET developer.

The application primarily demonstrates basic CRUD operations using a .NET Core REST API. I designed and developed the entire project myself. AssignaApi can be used as the back-end for developing a small task management application, serving as a foundation for further expansion.

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

### Login / Register / Reset Password / Refresh Token

- ### User registration

```json5
// [POST]
// [base url]/user/register

// Team lead account
// Request

{
  "FirstName": "Harshi",
  "UserName": "harshi@lead",
  "Email": "Any email address",
  "Password": "team@lead123",
  "Role": "team-lead"
}

// Team member account
// Request

{
  "FirstName": "Nadeesha",
  "UserName": "nadeesha@work",
  "Email": "Any email address",
  "Password": "team@member123",
  "Role": "team-member"
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
  "UserName": "harshi@lead",
  "Password": "team@lead123"
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
  "Email": "harshi@example.com"
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
  "Password": "harshi@123Pw",
  "ConPassword": "harshi@123Pw",
  "ResetToken": "4vZkirs9ZxD7kx5IgjQ9HcG4aze6RoEv..."
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
  "RefreshToken": "4vZkirs9ZxD7kx5IgjQ9HcG4aze6RoEv..."
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
https://[base-url]/category/categories
```

- ### Task priorities

```
// [GET]
https://[base-url]/priority/priorities
```

- ### Team members

```
// [GET]
https://[base-url]/user/members
```

- ### All tasks

```
// [GET]
https://[base-url]/leadtasks/tasks
```

- ### Pending tasks

```
// [GET]
https://[base-url]/leadtasks/pendings
```

- ### Completed tasks

```
// [GET]
https://[base-url]/leadtasks/completes
```

- ### High priority tasks

```
// [GET]
https://[base-url]/leadtasks/high-priority
```

- ### Medium priority tasks

```
// [GET]
https://[base-url]/leadtasks/medium-priority
```

- ### Low priority tasks

```
// [GET]
https://[base-url]/leadtasks/low-priority
```

- ### Add a new task

```json5
// [POST]
// [base url]/leadtasks/add-task

// Request

{
  "TskTitle": "Test task",
  "TskCategory": 1,
  "Deadline": "2022-12-18",
  "Priority": "High",
  "Member": 2,
  "TskNote": "Test note for the test task."
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
  "TskId": 1,
  "TskTitle": "Test task edit",
  "TskCategory": 1,
  "Deadline": "2022-12-19",
  "Priority": "Low",
  "Member": 2,
  "TskNote": "Edit test note for the test task."
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
  "TskId": 1
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
  "TskId": 1,
  "Message": "Please do this task as soon as possible."
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
https://[base-url]/leadtasks/task-info?taskid=1
```

## Team Member

- ### All tasks

```
// [GET]
https://[base-url]/membertasks/tasks
```

- ### Pending tasks

```
// [GET]
https://[base-url]/membertasks/pendings
```

- ### Completed tasks

```
// [GET]
https://[base-url]/membertasks/completes
```

- ### High priority tasks

```
// [GET]
https://[base-url]/membertasks/high-priority
```

- ### Medium priority tasks

```
// [GET]
https://[base-url]/membertasks/medium-priority
```

- ### Low priority tasks

```
// [GET]
https://[base-url]/membertasks/low-priority
```

- ### Add a note for task

```json5
// [POST]
// [base url]/membertasks/write-note

// Request

{
  "TskId": 1,
  "UserNote": "This will take two more days."
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
  "TskId": 1
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
https://[base-url]/membertasks/task-info?taskid=1
```

## Support

Darshana Wijesinghe  
Email address - [dar.mail.work@gmail.com](mailto:dar.mail.work@gmail.com)  
Linkedin - [darwijesinghe](https://www.linkedin.com/in/darwijesinghe/)  
GitHub - [darwijesinghe](https://github.com/darwijesinghe)

## License

This project is licensed under the terms of the **MIT** license.
