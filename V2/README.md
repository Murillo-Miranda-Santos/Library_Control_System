# Library Control System - Version 2.0

Console application developed in C# using MySQL for database management.

This version is an evolution of the initial system, now applying Object-Oriented Programming (OOP) and a layered architecture to improve organization, scalability, and maintainability.

## Features
Register books  
Register users  
Borrow books  
Return books  
List all books  
Generate system reports  

## Technologies
C#  
.NET  
MySQL  
DotNetEnv  

## Project Structure

This version introduces a layered architecture:

- **Program** → Entry point and user interaction (console)
- **Service Layer** → Business logic and system rules
- **Repository Layer** → Data access and SQL operations
- **Database Layer** → Connection management using environment variables

## Improvements Over Version 1

- Application of Object-Oriented Programming (OOP)
- Clear separation of responsibilities
- Better code organization and readability
- Easier maintenance and scalability
- Use of environment variables for secure configuration

## Environment Setup

Create a `.env` file in the root directory based on `.env.example`:

DB_SERVER=
DB_DATABASE=
DB_USER=
DB_PASSWORD=

## 🔧 Database Setup

A SQL script is available in the `MySQL/` folder.

To set up the database:

1. Open your MySQL client (MySQL Workbench, DBeaver, etc.)
2. Run the script located at:

MySQL/Banco library.sql

3. This will create the required database structure and tables

Make sure your `.env` file matches the database configuration.


## Purpose of This Version

The goal of this version is to:

- Apply OOP concepts in a real project  
- Introduce layered architecture  
- Improve code maintainability  
- Simulate a more realistic backend structure  

This version represents a transition from a procedural approach to a more professional and scalable design.

## Author
Murillo Miranda Santos