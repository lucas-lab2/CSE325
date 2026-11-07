using System.Text;

// ─── Paths ───────────────────────────────────────────────────────────────────
string currentDirectory = Directory.GetCurrentDirectory();
string storesDirectory   = Path.Combine(currentDirectory, "stores");
string summaryFile       = Path.Combine(currentDirectory, "sales-summary.txt");

// ─── 1. Create the stores directory structure if it doesn't exist ─────────────
if (!Directory.Exists(storesDirectory))
{
    Directory.CreateDirectory(storesDirectory);
    Console.WriteLine($"Created: {storesDirectory}");
}

// ─── 2. Find all sales files recursively ─────────────────────────────────────
IEnumerable<string> salesFiles = FindFiles(storesDirectory);

foreach (var file in salesFiles)
{
    Console.WriteLine($"Found: {file}");
}

// ─── 3. Generate the sales summary report ────────────────────────────────────
GenerateSalesSummary(salesFiles, summaryFile);

Console.WriteLine($"\nSales summary written to: {summaryFile}");
Console.WriteLine("\n--- Preview ---");
Console.WriteLine(File.ReadAllText(summaryFile));

// ─────────────────────────────────────────────────────────────────────────────
// FUNCTIONS
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Recursively finds all .json sales files under the given root directory.
/// </summary>
static IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new();

    var foundFiles = Directory.EnumerateFiles(folderName, "*.json", SearchOption.AllDirectories);
    salesFiles.AddRange(foundFiles);

    return salesFiles;
}

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
