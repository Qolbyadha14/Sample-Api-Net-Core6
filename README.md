## README: .NET Core 6 Web API

### Project Overview
This project is a .NET Core 6 Web API that leverages several key packages to streamline development and enhance functionality.

### Prerequisites
- [.NET Core 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or your preferred IDE

### Packages Used
1. **AutoMapper**: A convention-based object-object mapper.
2. **AutoMapper.Extensions.Microsoft.DependencyInjection**: Integration for AutoMapper with Microsoft.Extensions.DependencyInjection.
3. **FluentValidation**: A popular library for building strongly-typed validation rules.
4. **Microsoft.EntityFrameworkCore**: Entity Framework Core for database operations.
5. **Microsoft.EntityFrameworkCore.Design**: Design-time support for EF Core tools.
6. **Microsoft.EntityFrameworkCore.Sqlite**: EF Core provider for SQLite.
7. **Microsoft.EntityFrameworkCore.Tools**: Additional tools for EF Core.
8. **Newtonsoft.Json**: A high-performance JSON framework.
9. **Swashbuckle.AspNetCore**: Swagger tooling for API documentation.

### Getting Started

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/Qolbyadha14/Sample-Api-Net-Core6
   cd Sample-Api-Net-Core6
   ```

2. **Restore Packages:**
   ```bash
   dotnet restore
   ```

3. **Database Setup:**
   - this use sqlite.
   - Run migrations:
     ```bash
     dotnet ef database update
     ```

4. **Run the Application:**
   ```bash
   dotnet run
   ```
   The API will be accessible at `https://localhost:{port}`.

### Configuration

- **AutoMapper:** Profiles are configured in `program.cs`. Modify the profiles in the `ConfigureAutoMapper` method.
- **FluentValidation:** Validation rules are defined in the `Validators` folder. Customize these based on your model.

### Swagger Documentation

Swagger UI is available at `https://localhost:{port}/swagger`. Explore and test your API endpoints interactively.

### Contributing

Feel free to contribute by opening issues or submitting pull requests. Follow the standard coding conventions and documentation practices.

### License

This project is licensed under the [MIT License](LICENSE).