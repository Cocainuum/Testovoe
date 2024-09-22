using Microsoft.AspNetCore.Mvc;
using WebService.Contracts.Requests;
using WebService.Contracts.Responses;
using WebService.Persistence.Entities;
using WebService.Services;

namespace WebService.Controllers;

[Route("api/[controller]")]
public class MedicineController(IMedicineService medicineService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllMedicinesAsync()
    {
        var result = await medicineService.GetAllMedicinesAsync();

        return Ok(new MultipleDataResponse<Medicine> {Data = result});
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMedicineByIdAsync([FromQuery] long id)
    {
        var result = await medicineService.GetMedicineByIdAsync(id);

        return Ok(new SingleDataResponse<Medicine> {Data = result});
    }

    [HttpPost]
    public async Task<IActionResult> AddNewMedicineAsync([FromBody] MedicineRequest request)
    {
        var result = await medicineService.AddMedicineAsync(request);

        return Ok(new SingleDataResponse<Medicine> {Data = result});
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMedicineAsync([FromRoute] long id,
        [FromBody] MedicineRequest request)
    {
        await medicineService.UpdateMedicineAsync(id, request);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedicineAsync([FromRoute] long id)
    {
        await medicineService.DeleteMedicineAsync(id);

        return Ok();
    }
}