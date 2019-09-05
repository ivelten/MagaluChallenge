# Magalu Challenge

This project was made as a challenge for Magazine Luiza's hiring process.

## Building this project

### Requirements

1. Accessible instance of MySQL Server. The account used to access the server needs permissions to create and drop databases, as well as defining a database schema and change it's data.
2. .NET Core 2.2 (you can download it [here](https://dotnet.microsoft.com/download)).

### Build steps

1. Clone this project into some directory of your preference.
2. Build the project using an IDE, code editor or the terminal (by running `dotnet build` inside project folder).

### Running the application and executing tests

You will need to setup MySQL database access for the project before being able to run the API and/or test it.

2. Open [appsettings.json](Magalu.Challenge.Web.Api/appsettings.json) and locate the `MagaluDatabase` connection string, under the section `ConnectionStrings`. Change the connection string to match a connection string used to access the needed MySQL instance.
3. Run integration tests of project `Magalu.Challenge.Web.Api.IntegrationTests` using an IDE, code editor or the terminal (by running `dotnet test`  inside the test project folder).