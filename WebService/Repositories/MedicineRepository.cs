using Microsoft.EntityFrameworkCore;
using WebService.Persistence.Contexts;
using WebService.Persistence.Entities;

namespace WebService.Repositories;

public interface IMedicineRepository
{
    Task<List<Medicine>> GetAllMedicinesAsync();
    Task<Medicine?> GetMedicineByIdAsync(long id, bool withTracking = false);
    Task AddNewMedicineAsync(Medicine medicine);
    Task DeleteMedicineAsync(Medicine medicine);
    Task<bool> IsMedicineExistsAsync(string name);
    Task SaveChangesAsync();
}

public class MedicineRepository(DatabaseContext context)
    : IMedicineRepository
{
    public async Task<List<Medicine>> GetAllMedicinesAsync()
    {
        return await context.Medicines.ToListAsync();
    }

    public async Task<Medicine?> GetMedicineByIdAsync(long id, bool withTracking = false)
    {
        var query = context.Medicines.Where(x => x.Id == id);

        if (withTracking)
            query = query.AsTracking();

        return await query.FirstOrDefaultAsync();
    }

    public async Task AddNewMedicineAsync(Medicine medicine)
    {
        await context.Medicines.AddAsync(medicine);
        await context.SaveChangesAsync();
    }

    public async Task DeleteMedicineAsync(Medicine medicine)
    {
        context.Medicines.Remove(medicine);
        await context.SaveChangesAsync();
    }
    
    public async Task<bool> IsMedicineExistsAsync(string name)
    {
        return await context.Medicines.AnyAsync(x => x.Name == name);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}