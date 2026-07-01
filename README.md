# Quiz Application

A dynamic, cross-platform solution for knowledge assessment, featuring a Web Portal (ASP.NET Core), Desktop App (WPF), and Web API.

## Overview

This application serves as a dynamic quiz generator. Users receive a fresh, randomly generated quiz from a question database every time. After completion, the user gets a performance report based on the answers they give.

Key features include:
- **Quiz Generation**: Random question selection with state management and validation.
- **Scoring**: Calculation of statistics and scoring after quiz completion.
- **Cross-Platform Access**: Support for Web, Desktop (WPF), and API interactions.
- **Security & Reliability**: API Rate limiting, health checks, and CI pipeline SAST scanning.

## Technical Architecture

The solution (`QuizSolution.sln`) uses an updated version of Layered Architecture where the Persistence Layer is positioned between the Application and Presentation layers. The main projects include:

1. **Quiz.DomainLayer**: The core of the system containing entities, models (e.g., `Question`), and domain logic.
2. **Quiz.ApplicationLayer**: Application business logic, maintaining 100% code coverage.
3. **Quiz.PersistenceLayer**: Infrastructure and data access layer, utilizing Entity Framework Core and the Repository pattern.
4. **Quiz.Shared**: Shared resources and utilities used across different layers.
5. **Presentation**:
   - **Quiz.AspNetUI**: Responsive web portal.
   - **Quiz.WPFUI**: Native desktop client.
   - **Quiz.WebApi**: RESTful API for external or decoupled consumption.

### Tech Stack
- **Framework**: .NET 9.0
- **Database**: SQL Server
- **ORM**: Entity Framework Core 9
- **Architecture**: Layered Architecture, Repository Pattern
- **Testing**: xUnit, FluentAssertions, Moq, Coverlet
- **CI Pipeline**: GitHub Actions (Build, Test, Snyk SAST Scan)

## Setup Instructions

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server
- A preferred IDE (Visual Studio 2022, JetBrains Rider, or VS Code)

### Installation
1.  **Clone the repository**:
    ```bash
    git clone https://github.com/your-username/Quiz.git
    cd Quiz
    ```

2.  **Configure Database**:
    Update the connection string in the `appsettings.json` file of the API/UI projects.

3.  **Apply Migrations**:
    Open a terminal at the solution root and update the database:
    ```powershell
    dotnet ef database update --project "Quiz\Quiz.PersistenceLayer" --startup-project "Quiz\Quiz.WebApi"
    ```

4.  **Run the Application**:
    - **Web API**: `dotnet run --project "Quiz\Quiz.WebApi"`
    - **Web Portal**: `dotnet run --project "Quiz\Quiz.AspNetUI"`
    - **Desktop**: Open the solution in Visual Studio and set `Quiz.WPFUI` as the startup project.

## Database Design

The project is designed to work with SQL databases, but can easily be adapted to other storage methods due to the Repository Pattern. It primarily relies on a `Question` table which includes:

- **ID** (int): Primary key.
- **Text** (nvarchar): The main question text.
- **TextSecond** (nvarchar, nullable): Secondary text (e.g., text coming after an empty space in fill-in-the-blanks).
- **Options** (nvarchar): Available options for the question.
- **CorrectOption** (nvarchar): Indices of the correct options.
- **Type** (int): Question type identifier.
  - `0`: **Choice** (Standard multiple-choice question).
  - `1`: **Complete Empty Space** (Options must match the text exactly).

## Testing

The solution emphasizes strong testing coverage, achieving 100% coverage for business logic in the Application Layer.

- **Unit Tests**: `dotnet test "Quiz\Quiz.UnitTests"`
- **Integration/API Tests**: `dotnet test "Quiz\Quiz.ApiTests"`

### Code Coverage
- **Run Coverage**:
  ```powershell
  dotnet test --collect:"XPlat Code Coverage"
  ```

## CI Pipeline

The project uses GitHub Actions for Continuous Integration:
- Triggers on `push` and `pull_request` to `main`.
- Builds the solution across `Debug` and `Release` configurations.
- Runs the full test suite.
- Scans for vulnerabilities using **Snyk** (SAST).
