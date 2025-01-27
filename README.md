# Diary Entries REST API

A RESTful API for managing diary entries, built with **ASP.NET Core** and **Entity Framework Core**. This API supports full CRUD operations and includes **Swagger documentation** for easy testing.

---

## Features

- **CRUD Operations**: Create, read, update, and delete diary entries.
- **Automatic Timestamping**: Each diary entry is automatically assigned a `DateTime` when created.
- **Swagger Documentation**: Interactive API documentation for testing endpoints.
- **SQL Server Integration**: Uses Entity Framework Core for seamless database interactions.

---

## Code Overview

### 1. **DiaryEntry Model**
The `DiaryEntry` class represents a diary entry in the database.

```csharp
public class DiaryEntry
{
    public int Id { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string Content { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
```
- **Id**: Auto-generated unique identifier.
- **Title**: Required field for the entry title.
- **Content**: Required field for the entry content.
- **DateCreated**: Automatically set to the current date and time when the entry is created.

---

### 2. **ApplicationDbContext**
The `ApplicationDbContext` class manages the database context and interactions.

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<DiaryEntry> DiaryEntries { get; set; }
}
```
- **DiaryEntries**: A `DbSet` representing the collection of diary entries in the database.

---

### 3. **DiaryEntriesController**
The `DiaryEntriesController` handles all CRUD operations for diary entries.

#### **GET All Entries**
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<DiaryEntry>>> GetDiaryEntries()
{
    return await _context.DiaryEntries.ToListAsync();
}
```

#### **GET Entry by ID**
```csharp
[HttpGet("{id}")]
public async Task<ActionResult<DiaryEntry>> GetDiaryEntryById(int id)
{
    var diaryEntry = await _context.DiaryEntries.FindAsync(id);
    if (diaryEntry == null) return NotFound();
    return diaryEntry;
}
```

#### **POST Create Entry**
```csharp
[HttpPost]
public async Task<ActionResult<DiaryEntry>> PostDiaryEntry(DiaryEntry diaryEntry)
{
    diaryEntry.Id = 0; // Ensure ID is auto-generated
    _context.DiaryEntries.Add(diaryEntry);
    await _context.SaveChangesAsync();
    return CreatedAtAction(nameof(GetDiaryEntryById), new { id = diaryEntry.Id }, diaryEntry);
}
```

#### **PUT Update Entry**
```csharp
[HttpPut("{id}")]
public async Task<IActionResult> PutDiaryEntry(int id, DiaryEntry diaryEntry)
{
    if (id != diaryEntry.Id) return BadRequest();
    _context.Entry(diaryEntry).State = EntityState.Modified;
    await _context.SaveChangesAsync();
    return NoContent();
}
```

#### **DELETE Entry**
```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteDiaryEntry(int id)
{
    var entry = await _context.DiaryEntries.FindAsync(id);
    if (entry == null) return NotFound();
    _context.DiaryEntries.Remove(entry);
    await _context.SaveChangesAsync();
    return NoContent();
}
```

---

### 4. **Configuration (Program.cs)**
The `Program.cs` file configures the application and services.

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

- **SQL Server Setup**: Configures the database connection using `appsettings.json`.
- **Swagger**: Enabled in development mode for API testing.

---

## API Endpoints

| Method | Endpoint                   | Description                        |
|--------|----------------------------|------------------------------------|
| GET    | /api/DiaryEntries          | Get all diary entries             |
| GET    | /api/DiaryEntries/{id}     | Get a diary entry by ID           |
| POST   | /api/DiaryEntries          | Create a new diary entry          |
| PUT    | /api/DiaryEntries/{id}     | Update an existing diary entry    |
| DELETE | /api/DiaryEntries/{id}     | Delete a diary entry by ID        |

---

## Technologies Used

- **ASP.NET Core**
- **Entity Framework Core**
- **SQL Server**
- **Swagger/OpenAPI**

---

## Example Requests

### Create a Diary Entry
```http
POST /api/DiaryEntries
Content-Type: application/json

{
  "title": "My First Entry",
  "content": "This is the content of my diary entry."
}
```

### Get All Diary Entries
```http
GET /api/DiaryEntries
```

### Get a Diary Entry by ID
```http
GET /api/DiaryEntries/1
```

### Update a Diary Entry
```http
PUT /api/DiaryEntries/1
Content-Type: application/json

{
  "id": 1,
  "title": "Updated Title",
  "content": "Updated content."
}
```

### Delete a Diary Entry
```http
DELETE /api/DiaryEntries/1
