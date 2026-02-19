# Chatbot

A multi\-project chatbot solution built with C\# \(\.NET\). It includes a backend Web API, a Blazor frontend, a shared library for common models, and an automated test project.

## Solution Structure

- `Chatbot.sln` \- Solution file
- `Backend/` \- ASP\.NET Core Web API (controllers, services, EF Core)
- `Frontend/` \- Blazor frontend (UI, pages, auth state, services)
- `Shared/` \- Shared models/entities used by Backend and Frontend
- `TestProject/` \- Automated tests

## Tech Stack

- C\# / \.NET
- ASP\.NET Core Web API \(`Backend/`\)
- Blazor \(`Frontend/`\)
- Entity Framework Core \(`Backend/EFCData/`, `Backend/Migrations/`\)
- xUnit/NUnit/MSTest \(\*depends on `TestProject/` config\)

## Key Features (High Level)

- Authentication \(`Backend/Controllers/AuthenticationController/`, `Frontend/Authentication/`\)
- Chat sessions and conversations \(`Backend/Controllers/ChatSessionController/`, `Backend/Controllers/ConversationController/`\)
- PDF upload/processing \(`Backend/Controllers/PDFController/`, `Backend/DataAccessObjects/PdfDAO/`\)
- Feedback collection \(`Backend/Controllers/FeedbackController/`\)
- Embeddings / LLM chain integration \(`Backend/Services/EmbeddingProvider.cs`, `Backend/Services/LlmChainProvider.cs`\)
- Shared domain models \(`Shared/`\)

## Prerequisites

- Windows 10/11
