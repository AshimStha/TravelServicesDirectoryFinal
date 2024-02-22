# Travel Services Directory

Travel Services directory is a data logging web application made to provide users with comprehensive information about their travels. It has been built to aid travel companies to keep record of the bookings made by customers and also
provide them the travel services that includes accommodation, expense details, and the purpose for travel. This application aims to make travel planning easier by acting as a single hub for all the requirements. 

## Features

- CRUD Operations: There are 3 tables in this application and all of them have been provided with the major CRUD functions. The features to create, read, update and delete the entries for each entities have been added in the system.
- Tabular relationship: There exists a many to many relationship between the major tables which are Customer and Package which led to the creation of a third bridging table calle 'Booking' that joins the major tables.
- ViewModels: In order to make the application more convenient and practical, the concept of viewModels have been used for efficient data sharing among the model classes.
- Views: The application has a neat and proper user interface for ease of use to the users.

## Technologies Used

- ASP.NET MVC Framework: The web application is developed using the ASP.NET MVC framework, providing a robust and scalable architecture for building dynamic web applications.
- C# Programming Language: C# is used for server-side scripting and backend logic, handling data processing and other server-side functionalities.
- Entity Framework & LINQ: Entity Framework and Language Integration Query are utilized for data access and management, providing seamless integration with the underlying database and simplifying CRUD operations.
- HTML/Bootstrap: Frontend development is done using HTML and Bootstrap, ensuring a visually appealing and interactive user interface.
- SQL Server Object Explorer: SQL SOE is used as the relational database management system for storing and managing application data efficiently.


## Installation

Follow these steps to install the project:

1. Step 1: Clone the repository
   ```bash
   git clone https://github.com/username/project.git

2. Step 2: Update the framework version
   Project > ZooApplication Properties > Change target framework to 4.7.1 -> Change back to 4.7.2
   
3. Step 3: Run the migrations
   ```bash
   enable migrations

4. Step 4: Add the migrations
   ```bash
   add-migration

5. Step 5: Update database
   ```bash
   update-database

6. Step 6: Run the application

## Contribution Guidelines

Contributions to the Travel Services Directory project are welcome! If you'd like to contribute, please fork the repository, make your changes, and submit a pull request. Be sure to follow the project's coding standards and guidelines.
