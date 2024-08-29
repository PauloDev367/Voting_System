Aqui está uma versão aprimorada do seu `README.md`:

---

# Voting System

A robust real-time voting application built with .NET and Vue.js, leveraging SignalR for seamless communication. This project allows you to efficiently manage a voting system, enabling features such as adding new agents, pausing and resuming votes, and displaying the winner to all users in real-time.

## System Requirements

To run this project, ensure your machine meets the following requirements:

- .NET 8.0
- SQL Server
- Node.js 18.12.0

After installing these dependencies, you’ll need to set up the project dependencies for both the backend (`VotingSystem`) and frontend (`frontend`). If you open the `VotingSystem` in an IDE, the dependencies will be automatically downloaded. For the frontend, navigate to the `frontend` directory in your terminal and run:

```bash
npm install
```

Next, run the migrations to initialize the database. Open the `VotingSystem` folder in your terminal and execute:

```bash
dotnet ef database update
```

After initializing the database, insert the base roles required by the system by running the following SQL script:

```sql
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
    VALUES (1, 'ADMIN', 'ADMIN', 'ADMIN');

INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
    VALUES (2, 'CLIENT', 'CLIENT', 'CLIENT');
```

## Running the Project

To start the application, follow these steps:

1. Open the `VotingSystem` directory in your terminal and run the backend:

    ```bash
    dotnet run
    ```

2. In a new terminal tab, navigate to the `frontend` directory and start the frontend server:

    ```bash
    npm run server
    ```

3. The application will be accessible at [http://localhost:8080](http://localhost:8080). You can also explore the Swagger documentation at [http://localhost:5293/swagger/index.html](http://localhost:5293/swagger/index.html) to manage system users and interact with the API.

## Conventional Commits

This project follows the Conventional Commits specification to maintain consistent and meaningful commit messages. Below is the convention used:

| Type     | Emoji                 | Code                    |
|:---------|:----------------------|:------------------------|
| feat     | :sparkles:            | `:sparkles:`            |
| fix      | :bug:                 | `:bug:`                 |
| docs     | :books:               | `:books:`               |
| style    | :gem:                 | `:gem:`                 |
| refactor | :hammer:              | `:hammer:`              |
| perf     | :rocket:              | `:rocket:`              |
| test     | :rotating_light:      | `:rotating_light:`      |
| build    | :package:             | `:package:`             |
| ci       | :construction_worker: | `:construction_worker:` |
| chore    | :wrench:              | `:wrench:`              |

---

Essa versão é mais clara e organizada, além de seguir boas práticas de documentação para projetos de software.