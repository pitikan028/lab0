# Simple Web Crawler - Lab 0 CPE433

## Overview
A .NET 8.0 console application that recursively crawls web pages starting from a given URL and downloads linked pages.

## Project Structure
- `Program.cs` - Main application code containing the Crawler class
- `simple_crawler.csproj` - .NET 8.0 project file

## Implementation Status ✅

### Main Requirement: Recursive Operation
**Status: COMPLETED**
- Implemented recursive page traversal in the `GetPage()` method (lines 89-92)
- Crawler now follows links from downloaded pages and recursively downloads linked content
- Added level check `if (level > 0)` to prevent infinite recursion
- Each recursive call decrements the level until reaching 0

### Extra Task: Improve Main Method
**Status: COMPLETED**
- Modified `Main()` method to accept command-line arguments:
  - `args[0]` - Starting URL (default: https://dandadan.net/)
  - `args[1]` - Recursion depth level (default: 2)
- Changed to `async Task Main()` to support async operations
- Improved from hardcoded values to configurable parameters
- Created output folder "./pages" to store downloaded HTML files

## Running the Crawler

**Basic usage (with defaults):**
```bash
dotnet run --project simple_crawler.csproj
```

**With custom URL and depth level:**
```bash
dotnet run --project simple_crawler.csproj "https://example.com" 3
```

## Key Features
- **Recursive Crawling**: Follows links in each page up to specified depth level
- **Link Limiting**: Restricts to 5 links per page (configurable via `SetMaxLinksPerPage()`)
- **HTML Extraction**: Uses regex pattern to extract all href links
- **File Storage**: Saves downloaded HTML with reformatted filenames
- **Error Handling**: Catches HTTP request exceptions and reports status codes

## Configuration Options
In code:
- `SetBasedFolder()` - Directory to store retrieved HTML files
- `SetMaxLinksPerPage()` - Maximum number of links to follow per page (default: 5)
- `GetPage(url, level)` - Starting URL and recursion depth level

## Recent Changes
- 2025-12-26: Implemented recursive operation and improved Main method
