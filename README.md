# Booksy Assignment

# Project Setup

## Prerequisites
* .NET 10 SDK
* Node.js

---

## 1. Backend Setup

Go to API project in terminal
dotnet restore
dotnet ef database update
dotnet run

## 2. Frontend Setup

Go to frontend project in terminal
npm install
npm run dev

## Implementation Status & Trade-offs

### Fully Implemented
All core requirements for the Hardware Hub have been successfully delivered. The application is stable and covers the three main pillars:
* **The Management Engine:** Fully functional Admin Panel and Users Panel with CRUD implementation and toggling 'Repair' status.
* **The Rental Engine:** Complete business logic for the Rent/Return flow, including state guards.
* **The AI-Native Layer:** Semantic search is fully integrated, allowing users to find specific gear using natural language prompts rather than exact keyword matches.

### Shortcuts & "Hacks"
* **AI-Generated Frontend:** To meet the deadline, the entire frontend was scaffolded and built using AI prompts heavily based on the provided wireframe inspiration (prompts.txt file). 
* **The "Why":** For an MVP, prioritizing the backend architecture, database integrity, and AI integration was more important than frontend. Leveraging AI to generate the UI allowed me to deliver a fully working, full-stack prototype in a fraction of the time, proving out the core functionality quickly.
* **The "Future":** In a real production environment, I would perform a deep refactoring of this frontend codebase. I would break down the AI-generated code into strict, reusable components and significantly upgrade the visual design and UX to match enterprise standards.

* **Simplified Architecture & Bypassing Complex Patterns:** I intentionally avoided heavy enterprise design patterns (like CQRS, strict Repository patterns or massive multi-layer abstractions) and opted for a straightforward, cohesive API structure.
* **The "Why":** To meet the strict deadline, I strictly followed the **KISS** principle. For an MVP, over-engineering and building solutions for scaling problems that do not currently exist is a trap. I focused entirely on delivering a solid core and functional business logic rather than building a complex skeleton for a massive application.
* **The "Future":** In a real production environment, as the application's domain logic grows and the team scales, I would incrementally refactor the architecture. I would introduce more advanced design patterns and separation of concerns only when the complexity naturally demands it, actively avoiding premature optimization.

### Partial/Missing
* As of right now, there are no major missing features from the core scope, except 3 critical tests.

### Next Steps (The 24h Roadmap)
If I had one more day to work on this, my top priorities would be:
1. **Frontend Refactoring:** Manually audit the AI-generated frontend, clean up redundant CSS/logic and establish a strict component hierarchy.
2. **Domain Model Expansion:** I would introduce additional database models to support more functionality. This includes adding supporting entities like `MaintenanceLogs` to track repair histories over time or expanding properties of hardware such as `Brand` to a seperate model.
3. **Pagination:** The current Smart Dashboard loads the entire inventory at once. To ensure the application remains functional while there are thousands of devices, I would implement backend pagination combined with an optimized frontend table/infinite scroll.
4. **Rental Request Workflow:** Currently, the rental logic is direct. I would build a `RentalRequest` system to introduce a proper approval pipeline. Users would submit a request for an item and an Admin would review, approve, deny or waitlist the request based on availability and business priority.
5. **Validation:** I would strengthen data validation across both the API and frontend. I would map out and handle more complex edge cases.
6. **AI search:** I would upgrade it and make it more functional, as for right now it uses a JSON data set, instead of database.

## The AI Development Log

### Tooling
* **Gemini CLI:** Rapid frontend architecture scaffolding and initial Vue.js boilerplate generation.
* **Gemini (Web):** Primary architectural help for backend.
* **Claude:** Complex tasks.

### Data Strategy
I fed the raw JSON seed directly into the AI context. This analysis directly created the optimal C# domain models generated EF Core relationships (for example 1:N Hardware to Rental Records) and established validation logic.

### Prompt Trail (Gemini CLI)
* **Link:** [`ai.txt`](./ai.txt)

### The "Correction"
**The Error:** While setting up Data Transfer Objects (DTOs), the AI suggested installing both `AutoMapper` and `AutoMapper.Extensions.Microsoft.DependencyInjection` which created multiple problems.

**The Fix:** I identified the error and connected it to AutoMapper Program.cs config and redundant usage of AutoMapper.Extensions.Microsoft.DependencyInjection.
1.  Removed the unnecessary `AutoMapper.Extensions.Microsoft.DependencyInjection` package to avoid version conflicts.
2.  Discarded the AI's AutoMapper config and manually implemented a proven AutoMapper configuration pattern from my previous project.
