# W01 Assignment Notes
**Course:** CSE 325 | **Assignment:** Build .NET Applications with C#  
**SDK:** .NET 8.0 (LTS) — confirmed with `dotnet --version`

---

## Projects

| Project | Type | Location |
|---------|------|----------|
| `PizzaStore` | ASP.NET Core Web API (controllers) | `W01/PizzaStore/` |
| `FileProcessor` | .NET 8 Console App | `W01/FileProcessor/` |

---

## 1 — Pizza API Evidence

### Seed Data (all pizzas, including additional pizza)

`GET http://localhost:5100/pizza` → **200 OK**

```json
[
  {
    "id": 1,
    "name": "Classic Italian",
    "description": "Fresh tomatoes, mozzarella, basil, and olive oil on a traditional thin crust.",
    "isGlutenFree": false
  },
  {
    "id": 2,
    "name": "The Carnivore",
    "description": "Loaded with pepperoni, sausage, bacon, and ham on a thick crust with extra cheese.",
    "isGlutenFree": false
  },
  {
    "id": 3,
    "name": "Garden Fresh",
    "description": "Bell peppers, mushrooms, onions, olives, and spinach on a gluten-free crust.",
    "isGlutenFree": true
  },
  {
    "id": 4,
    "name": "BBQ Chicken",
    "description": "Grilled chicken, red onions, cilantro, and smoked gouda on a tangy BBQ sauce base.",
    "isGlutenFree": false
  },
  {
    "id": 5,
    "name": "Hawaiian Luau",
    "description": "Canadian bacon, fresh pineapple chunks, jalapeños, and a blend of mozzarella and cheddar on a honey-glazed crust.",
    "isGlutenFree": false
  }
]
```

> **Pizza #5 "Hawaiian Luau"** is the additional pizza added per assignment requirements.

---

## 2 — API CRUD Operations

### GET — Retrieve a single pizza

**Request:**
```
GET http://localhost:5100/pizza/3
```

**Response body:**
```json
{
  "id": 3,
  "name": "Garden Fresh",
  "description": "Bell peppers, mushrooms, onions, olives, and spinach on a gluten-free crust.",
  "isGlutenFree": true
}
```
**Status Code:** `200 OK`

---

### POST — Create a new pizza

**Request:**
```
POST http://localhost:5100/pizza
Content-Type: application/json

{
  "name": "Truffle Mushroom",
  "description": "Wild mushrooms, truffle oil, parmesan, arugula on a sourdough crust.",
  "isGlutenFree": false
}
```

**Response body:**
```json
{
  "id": 6,
  "name": "Truffle Mushroom",
  "description": "Wild mushrooms, truffle oil, parmesan, arugula on a sourdough crust.",
  "isGlutenFree": false
}
```
**Status Code:** `201 Created`

---

### PUT — Update an existing pizza

**Request:**
```
PUT http://localhost:5100/pizza/6
Content-Type: application/json

{
  "name": "Truffle Mushroom Deluxe",
  "description": "Wild mushrooms, black truffle oil, aged parmesan, fresh arugula, and balsamic drizzle on a sourdough crust.",
  "isGlutenFree": false
}
```

**Response body:** *(empty — 204 No Content)*  
**Status Code:** `204 No Content`

---

### DELETE — Remove a pizza

**Request:**
```
DELETE http://localhost:5100/pizza/6
```

**Response body:** *(empty — 204 No Content)*  
**Status Code:** `204 No Content`

---

## 3 — Sales Summary Function (FileProcessor)

The function `GenerateSalesSummary` was added to `FileProcessor/Program.cs`. It reads all `.json` sales files found recursively under the `stores/` directory, computes per-file and grand totals, then writes a formatted report using a `StringBuilder`.

### Source Code

```csharp
/// <summary>
/// Reads each sales file, computes per-file and grand totals, and writes a
/// formatted summary report to <paramref name="summaryFilePath"/>.
/// </summary>
/// <param name="salesFiles">Collection of paths to individual sales files.</param>
/// <param name="summaryFilePath">Output path for the generated report.</param>
static void GenerateSalesSummary(IEnumerable<string> salesFiles, string summaryFilePath)
{
    double grandTotal = 0;
    var fileTotals = new Dictionary<string, double>();

    foreach (string file in salesFiles)
    {
        double fileTotal = 0;
        string[] lines = File.ReadAllLines(file);

        foreach (string line in lines)
        {
            // Each line is "date,amount" – parse the amount in the second column
            string[] parts = line.Split(',');
            if (parts.Length >= 2 && double.TryParse(parts[1].Trim(), out double amount))
            {
                fileTotal += amount;
            }
        }

        grandTotal += fileTotal;
        // Use the relative path from the stores folder for readability
        string relativeName = Path.GetRelativePath(Directory.GetCurrentDirectory(), file);
        fileTotals[relativeName] = fileTotal;
    }

    // Build the report with a StringBuilder
    var sb = new StringBuilder();
    sb.AppendLine("Sales Summary");
    sb.AppendLine("----------------------------");
    sb.AppendLine($"Total Sales: {grandTotal:C}");
    sb.AppendLine();
    sb.AppendLine("Details:");

    foreach (var kvp in fileTotals)
    {
        sb.AppendLine($"    {kvp.Key}: {kvp.Value:C}");
    }

    File.WriteAllText(summaryFilePath, sb.ToString());
}
```

### Generated Output (`sales-summary.txt`)

```
Sales Summary
----------------------------
Total Sales: $78,272.65

Details:
    stores/201/sales.json: $17,283.80
    stores/202/sales.json: $33,827.15
    stores/203/sales.json: $27,161.70
```

> Note: Currency format is determined by the system locale. The `:C` format specifier and `ToString("C")` are used as required by the assignment.

