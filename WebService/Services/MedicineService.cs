using WebService.Contracts.Requests;
using WebService.Exceptions;
using WebService.Persistence.Entities;
using WebService.Repositories;

namespace WebService.Services;

public interface IMedicineService
{
    Task<List<Medicine>> GetAllMedicinesAsync();
    Task<Medicine> GetMedicineByIdAsync(long id);
    Task<Medicine> AddMedicineAsync(MedicineRequest request);
    Task UpdateMedicineAsync(long id, MedicineRequest request);
    Task DeleteMedicineAsync(long id);
}

public class MedicineService(IMedicineRepository medicineRepository)
    : IMedicineService
{
    public async Task<List<Medicine>> GetAllMedicinesAsync()
    {
        return await medicineRepository.GetAllMedicinesAsync();
    }

    public async Task<Medicine> GetMedicineByIdAsync(long id)
    {
        var result = await medicineRepository.GetMedicineByIdAsync(id);

        if (result == null)
            throw new CommonError(404, "Not found");

        return result;
    }

    public async Task<Medicine> AddMedicineAsync(MedicineRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
            throw new CommonError(400, "Name not valid");

        if (!request.Price.HasValue || request.Price <= 0)
            throw new CommonError(400, "Price not valid");

        if (await medicineRepository.IsMedicineExistsAsync(request.Name))
            throw new CommonError(400, "Same medicine exists");

        var newMedicine = new Medicine
        {
            Name = request.Name,
            Price = request.Price.Value,
            Description = request.Description
        };

        await medicineRepository.AddNewMedicineAsync(newMedicine);

        return newMedicine;
    }

    public async Task UpdateMedicineAsync(long id, MedicineRequest request)
    {
        var medicine = await medicineRepository.GetMedicineByIdAsync(id, true);
        if (medicine == null)
            throw new CommonError(404, "Not found");

        if (!string.IsNullOrEmpty(request.Name))
        {
            if (!request.Name.Equals(medicine.Name, StringComparison.InvariantCultureIgnoreCase) &&
                await medicineRepository.IsMedicineExistsAsync(request.Name))
                throw new CommonError(400, "Same medicine exists");
            
            medicine.Name = request.Name;
        }

        if (request.Price.HasValue)
        {
            if (request.Price <= 0)
                throw new CommonError(400, "Price not valid");

            medicine.Price = request.Price.Value;
        }

        if (!string.IsNullOrEmpty(request.Description))
        {
            medicine.Description = request.Description;
        }

        await medicineRepository.SaveChangesAsync();
    }

    public async Task DeleteMedicineAsync(long id)
    {
        var medicine = await medicineRepository.GetMedicineByIdAsync(id);
        if (medicine == null)
            throw new CommonError(404, "Not found");

        await medicineRepository.DeleteMedicineAsync(medicine);
    }
}