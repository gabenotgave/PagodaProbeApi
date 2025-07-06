# PagodaProbe API

[![.NET](https://github.com/gabenotgave/PagodaProbeApi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/gabenotgave/PagodaProbeApi/actions/workflows/dotnet.yml)

This repository contains the source code for the PagodaProbe API, the backend service that powers the [PagodaProbeWeb](https://github.com/gabenotgave/PagodaProbeWeb) application. The API is responsible for fetching, processing, and serving public record data related to people, property parcels, and court dockets.

It is built with .NET 8 using Clean Architecture principles to ensure a decoupled, maintainable, and testable codebase.

---

## Table of Contents

- [Architecture](#architecture)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation & Setup](#installation--setup)
  - [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

---

## Architecture

This solution follows the principles of **Clean Architecture**, separating concerns into distinct layers:

-   **`Domain`**: Contains the core business logic and entities of the application (e.g., `Person`, `DocketCase`, `Parcel`). It has no dependencies on any other layer.
-   **`Application`**: Orchestrates the data flow and implements the application's use cases (Commands and Queries). It depends only on the `Domain` layer and defines interfaces for infrastructure concerns (like repositories or API clients).
-   **`Infrastructure`**: Provides implementations for the interfaces defined in the `Application` layer. This includes the database context (using Entity Framework Core), API clients for external services (e.g., Pennsylvania Docket Search, Berks County Parcel Search), and other concrete implementations.
-   **`Presentation`**: The entry point of the application. This is an ASP.NET Core Web API project that exposes the application's functionality via RESTful endpoints. It depends on the `Application` and `Infrastructure` layers for dependency injection.

This architectural style makes the system easier to understand, maintain, and extend over time.

---

## Features

-   **Person Search**: Aggregates data about individuals from various public sources.
-   **Pennsylvania Docket Search**: Fetches and parses court docket information from the Pennsylvania Unified Judicial System portal.
-   **Berks County Parcel Search**: Retrieves property parcel information for Berks County.
-   **Contact Form Submission**: Allows users to send inquiries through the web application.
-   **Google ReCAPTCHA Integration**: Protects public-facing endpoints from bots and abuse.

---

## Technologies Used

-   **Backend**: .NET 8, ASP.NET Core Web API
-   **Architecture**: Clean Architecture, Command Query Responsibility Segregation (CQRS) with MediatR
-   **Data Access**: Entity Framework Core 8
-   **Database**: SQL Server (or any other EF Core compatible database)
-   **API Clients**: `HttpClient` for consuming external services.

---

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development and testing.

### Prerequisites

-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   A SQL Server instance (like the free [SQL Server Developer Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads))
-   A code editor like [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/).

### Installation & Setup

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/gabenotgave/PagodaProbeApi.git](https://github.com/gabenotgave/PagodaProbeApi.git)
    cd PagodaProbeApi
    ```

2.  **Configure User Secrets:**
    This project uses User Secrets to store sensitive information like database connection strings and API keys during development.

    Navigate to the `Presentation` directory:
    ```bash
    cd Presentation
    ```

    Initialize user secrets for the project:
    ```bash
    dotnet user-secrets init
    ```

    Set the database connection string:
    ```bash
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=your_server;Database=PagodaProbe;Trusted_Connection=True;TrustServerCertificate=True;"
    ```
    > **Note:** Replace the connection string with the one for your local SQL Server instance.

    Set the Google ReCAPTCHA keys (if you plan to test features that use it):
    ```bash
    dotnet user-secrets set "GoogleRecaptcha:SiteKey" "YOUR_RECAPTCHA_SITE_KEY"
    dotnet user-secrets set "GoogleRecaptcha:SecretKey" "YOUR_RECAPTCHA_SECRET_KEY"
    ```

3.  **Apply Database Migrations:**
    The database schema is managed using EF Core Migrations. To create and update your local database, run the following command from the root directory of the repository:
    ```bash
    dotnet ef database update --project Infrastructure --startup-project Presentation
    ```
    This will apply all existing migrations and create the necessary tables.

### Running the Application

You can run the API directly from your IDE (Visual Studio or Rider) or by using the .NET CLI.

From the `Presentation` directory, run:
```bash
dotnet run
```

The API will start and listen on the URLs specified in `Presentation/Properties/launchSettings.json` (typically `https://localhost:7035` and `http://localhost:5035`).

---

## API Endpoints

Once the application is running, you can explore the available API endpoints and interact with them using the built-in Swagger UI.

Navigate to **`https://localhost:7035/swagger`** in your browser.

The main controllers include:
-   `PersonController`: For handling person-related data requests.
-   `DocketCaseController`: For searching and retrieving court docket information.
-   `ContactController`: For managing contact inquiries.
-   `RecaptchaController`: For validating ReCAPTCHA tokens.

---

## Contributing

Contributions are what make the open-source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".

1.  Fork the Project
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the Branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request
