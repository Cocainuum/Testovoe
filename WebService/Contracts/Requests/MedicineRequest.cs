﻿namespace WebService.Contracts.Requests;

public class MedicineRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
}