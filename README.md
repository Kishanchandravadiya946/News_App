# News App

This is a simple *News Web Application* built with *ASP.NET Core MVC* that allows users to browse the latest news articles. This project demonstrates the use of ASP.NET Core MVC with Entity Framework and SQL Server.

## Table of Contents

- [Purpose](#purpose)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Running the Project](#running-the-project)
- [Database Setup](#database-setup)

## Purpose

The purpose of this project is to create a simple and functional news application that allows users to view news articles in a structured format. It showcases my understanding of ASP.NET Core MVC, Entity Framework, and how to work with databases in a web application. 

## Features

- Display a list of news articles.
- Ability to create, read, update, and delete (CRUD) articles (for admin users).
- Structured article details page.
- Responsive UI for a better user experience across devices.
- Integration with SQL Server for persistent data storage.

## Technologies Used

- **ASP.NET Core MVC**: Framework for building web applications.
- **Entity Framework Core**: ORM for managing database interactions.
- **SQL Server**: Relational database for storing articles.
- **Bootstrap**: CSS framework for responsive UI.
- **HTML/CSS/JavaScript**: Frontend for building the user interface.

## Getting Started

To get a local copy of the project up and running, follow these steps.

### Prerequisites

- Visual Studio (or Visual Studio Code with .NET support)
- SQL Server (for the database)
- .NET SDK installed on your machine

### Clone the repository

```
git clone https://github.com/Kishanchandravadiya946/News_App.git
```

## Running the Project

### 1. Open the Project

- Open the project in *Visual Studio* by double-clicking the .sln file in the repository.

### 2. Restore Dependencies

- Visual Studio will automatically restore all the necessary NuGet packages. If not, right-click on the solution in the *Solution Explorer* and select *Restore NuGet Packages*.

### 3. Configure Database Connection

- Make sure that the connection string in the appsettings.json file points to your local or cloud-based SQL Server. You may need to update the Server and Database values to match your environment:

```json
"ConnectionStrings": {
  "ConnectionName": "server=SERVERNAME;database=DATABASENAME;Integrated Security=true;"
},
```


### 4. Do migration and Update the Database

- Open *Package Manager Console* in Visual Studio and run the following commands to apply any pending migrations and create the database:

```
add-migration "message"
update-database
```


### 5. Run the Application

- Press F5 in Visual Studio to build and run the project. It will launch the web application in your default browser, and you can begin browsing news articles.

## Database Setup

The project uses *SQL Server* as its database. Follow the steps below to set up the database:

1. Ensure that SQL Server is installed and running.
2. Update the connection string in the appsettings.json file to point to your SQL Server instance.
3. Run the Update-Database command in the *Package Manager Console* to create the required tables and seed the database with sample data.

