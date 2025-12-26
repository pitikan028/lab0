# Simple Crawler

## Overview
A .NET 8.0 console application that crawls web pages starting from a given URL and recursively retrieves linked pages.

## Project Structure
- `Program.cs` - Main application code containing the Crawler class
- `simple_crawler.csproj` - .NET 8.0 project file

## Running
Execute the crawler using:
```bash
dotnet run --project simple_crawler.csproj
```

## Configuration
The crawler has configurable options in `Program.cs`:
- `SetBasedFolder()` - Directory to store retrieved HTML files
- `SetMaxLinksPerPage()` - Maximum number of links to follow per page
- Starting URL passed to `GetPage()` method

## Recent Changes
- 2025-12-26: Initial setup in Replit environment with .NET 8.0
