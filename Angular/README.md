# RKS - Project Management System

A modern, feature-rich project management system built with Angular 20.1.0. This application provides comprehensive tools for task management, user collaboration, and project tracking.

## Features

- **User Authentication & Authorization**
  - Secure login system
  - First-time user onboarding
  - Password recovery functionality
  - Profile management

- **Project Management**
  - Project selection and overview
  - Task setup and management
  - Subtask organization
  - Task history tracking
  - Resource allocation

- **Task Management**
  - Current task tracking
  - Completed task view
  - Task comments and collaboration
  - Task progress monitoring
  - ETA tracking

- **User Management**
  - User registration
  - Access control
  - User permissions management
  - Role-based authorization

- **Additional Features**
  - Useful links management
  - Custom time display pipes
  - Theme customization
  - Rich text editing with CKEditor 5
  - Alert system
  - Encrypted data handling

## Prerequisites

- Node.js (Latest LTS version recommended)
- npm (comes with Node.js)
- Angular CLI version 20.1.4

## Installation

1. Clone the repository:
   ```bash
   git clone [repository-url]
   cd rks
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

## Development server

To start a local development server, run:

```bash
ng serve
```

Navigate to `http://localhost:4200/`. The application will automatically reload when you change any of the source files.

## Build

To build the project for production:

```bash
ng build
```

The build artifacts will be stored in the `dist/` directory.

## Running Tests

To execute the unit tests:

```bash
ng test
```

## Project Structure

```
src/
├── app/
│   ├── components/
│   │   ├── feature/          # Feature components
│   │   │   ├── current-task
│   │   │   ├── done-task
│   │   │   ├── login
│   │   │   ├── profile
│   │   │   └── ...
│   │   └── shared/          # Shared components
│   ├── constants/           # Application constants
│   ├── models/             # Data models
│   ├── pipes/              # Custom pipes
│   └── services/           # Application services
```

## Key Technologies

- Angular 20.1.0
- CKEditor 5
- TypeScript
- SCSS
- Angular Router
- Angular Forms

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

