using BookingSystem.Core.Interfaces;
using BookingSystem.Core.Models;
using BookingSystem.Infrastructure.Data;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BookingSystem.Infrastructure.Services
{
    public class FileImporter : IFileImporter
    {
        private readonly ApplicationDbContext _context;

        public FileImporter(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ImportMembersAsync(Stream fileStream)
        {
            try
            {
                using var reader = new StreamReader(fileStream);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                });

                csv.Context.RegisterClassMap<MemberCsvMap>();

                var records = csv.GetRecords<MemberCsvModel>().ToList();

                var existingEmails = new HashSet<string>(
                    await _context.Members.Select(m => m.Email).ToListAsync(),
                    StringComparer.OrdinalIgnoreCase);

                var emailSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var record in records)
                {
                    try
                    {
                        string baseEmail = $"{record.FirstName}.{record.LastName}".ToLower().Replace(" ", "") + "@example.com";
                        string uniqueEmail = baseEmail;
                        int suffix = 1;

                        while (existingEmails.Contains(uniqueEmail) || emailSet.Contains(uniqueEmail))
                        {
                            uniqueEmail = $"{record.FirstName}.{record.LastName}{suffix}@example.com".ToLower().Replace(" ", "");
                            suffix++;
                        }

                        emailSet.Add(uniqueEmail);

                        var member = new Member
                        {
                            FirstName = record.FirstName,
                            LastName = record.LastName,
                            Email = uniqueEmail,
                            DateJoined = record.DateJoined,
                            BookingCount = record.BookingCount
                        };

                        await _context.Members.AddAsync(member);
                    }
                    catch (Exception ex)
                    {
                        // Log individual row failure and continue
                        Console.WriteLine($"Error processing member row: {ex.Message}");
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (CsvHelperException ex)
            {
                throw new InvalidOperationException("CSV parsing failed for members import.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException("File read error during members import.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred during members import.", ex);
            }
        }

        public async Task ImportInventoryAsync(Stream fileStream)
        {
            try
            {
                using var reader = new StreamReader(fileStream);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                });

                csv.Context.RegisterClassMap<InventoryCsvMap>();

                var records = csv.GetRecords<InventoryCsvModel>().ToList();

                foreach (var record in records)
                {
                    try
                    {
                        var existingItem = await _context.InventoryItems
                            .FirstOrDefaultAsync(i => i.Name == record.Name);

                        if (existingItem == null)
                        {
                            var item = new InventoryItem
                            {
                                Name = record.Name,
                                Description = record.Description ?? string.Empty,
                                RemainingCount = record.RemainingCount,
                                ExpirationDate = record.ExpirationDate
                            };

                            await _context.InventoryItems.AddAsync(item);
                        }
                        else
                        {
                            existingItem.Description = record.Description ?? existingItem.Description;
                            existingItem.RemainingCount = record.RemainingCount;
                            _context.InventoryItems.Update(existingItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log and skip this inventory row
                        Console.WriteLine($"Error processing inventory row: {ex.Message}");
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (CsvHelperException ex)
            {
                throw new InvalidOperationException("CSV parsing failed for inventory import.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException("File read error during inventory import.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error occurred during inventory import.", ex);
            }
        }

        // CSV Models
        private class MemberCsvModel
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public DateTime DateJoined { get; set; }
            public int BookingCount { get; set; }
        }

        private class InventoryCsvModel
        {
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
            public int RemainingCount { get; set; }
            public DateTime ExpirationDate { get; set; }
        }

        private sealed class MemberCsvMap : ClassMap<MemberCsvModel>
        {
            public MemberCsvMap()
            {
                Map(m => m.FirstName).Name("name");
                Map(m => m.LastName).Name("surname");
                Map(m => m.BookingCount).Name("booking_count");
                Map(m => m.DateJoined).Name("date_joined");
            }
        }
        private sealed class InventoryCsvMap : ClassMap<InventoryCsvModel>
        {
            public InventoryCsvMap()
            {
                Map(m => m.Name).Name("title");
                Map(m => m.Description).Name("description");
                Map(m => m.RemainingCount).Name("remaining_count");
                Map(m => m.ExpirationDate)
                    .Name("expiration_date")
                    .TypeConverterOption
                    .Format("dd-MM-yyyy", "dd/MM/yyyy", "yyyy-MM-dd");
            }
        }

    }


}
