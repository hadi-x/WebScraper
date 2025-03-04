WebScraper
Overview
WebScraper is a .NET Core API project designed to analyze a user-supplied URL, extract details from the web page, and return structured data. The extracted details include:
- The count and total byte size of each type of image (e.g., .jpg, .png, etc.).
- All nested links categorized into internal and external links.
- The ability for users to request details for extracted links.
This project follows a Clean Architecture approach, ensuring modularity, maintainability, and separation of concerns.
Technologies Used
- Backend: ASP.NET Core Web API
- Architecture: Clean Architecture
- Language: C#
- Libraries:
  - HtmlAgilityPack for parsing HTML.
  - Moq and xUnit for unit testing.
  - HttpClient for making HTTP requests.
Project Structure
This project is structured based on Clean Architecture, which separates concerns into distinct layers:

WebScraper/
│── src/
│   ├── WebScraper.API/                # API Layer - Handles HTTP requests/responses
│   │   ├── Controllers/               # Web API Controllers
│   │   │   ├── WebScraperController.cs
│   │   ├── Program.cs                 # Configures DI and services
│   │   ├── appsettings.json
│   │
│   ├── WebScraper.Application/        # Business Logic Layer
│   │   ├── Interfaces/                # Interfaces for services
│   │   │   ├── IWebScraperService.cs
│   │   ├── Services/                  # Implementation of business logic
│   │   │   ├── WebScraperService.cs
│   │   ├── DTOs/                      # Data Transfer Objects
│   │
│   ├── WebScraper.Domain/             # Core Entities & Business Rules
│   │   ├── Entities/                   
│   │   │   ├── ImageInfo.cs            # Represents an image and its properties
│   │   │   ├── LinkData.cs             # Represents extracted links
│   │
│   ├── WebScraper.Infrastructure/     # External dependencies (optional for DB/storage)
│   │
│── tests/                             # Unit Tests
│   ├── WebScraper.Tests/
│   │   ├── WebScraperServiceTests.cs
│
│── WebScraper.sln                     # Solution File
│── README.md

Features & Functionality
1. Extracting Images with Size
   - The API extracts all images from a given webpage.
   - It retrieves the file extension (e.g., .jpg, .png) and calculates the total file size.
   - Uses HttpClient to fetch image sizes asynchronously.
2. Extracting Internal & External Links
   - Internal links: Links that belong to the same domain as the provided URL.
   - External links: Links pointing to other domains.
   - All extracted links are cleaned and standardized.
3. Ability to Recurse into Extracted Links
   - Users can click on extracted links and submit them for further analysis.
How to Run the Project
1. Install .NET SDK
   Ensure that you have .NET 7.0 or .NET 8.0 installed on your system.
   You can check your installation by running:
   dotnet --version
2. Navigate to the Project Directory
   cd WebScraper
3. Restore Dependencies
   dotnet restore
4. Run the API
   dotnet run --project src/WebScraper.API
The API will be available at https://localhost:5001/swagger/index.html.
API Endpoints
**Extract Data from a Web Page**
- **Endpoint:** POST /api/WebScraper/scrape
- **Request Body:**

{
  "url": "https://example.com"
}

- **Response:**

{
  "Url": "https://example.com",
  "Images": [
    { "Url": "https://example.com/image1.jpg", "Extension": ".jpg", "Size": 12345 }
  ],
  "Links": {
    "Internal": ["https://example.com/about"],
    "External": ["https://external.com"]
  }
}

Testing the Project
This project includes unit tests using xUnit and Moq.
To run tests:
   dotnet test
Why This Architecture?
This project follows Clean Architecture to ensure:
✅ Separation of concerns (API, Business Logic, Domain, Infrastructure).
✅ Testability - Business logic is decoupled from external dependencies.
✅ Scalability - Easy to extend by adding databases, caching, or additional processing.
Next Steps & Improvements
- ✅ Add caching for repeated requests.
- ✅ Optimize performance for large webpages.
- ✅ Implement logging for better debugging.
