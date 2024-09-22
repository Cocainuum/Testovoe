namespace ClientApp.Configuration
{
    public static class MedicineClientOptions
    {
        public const string ClientName = "Medicine";
        public const string AddressField = "ServiceBaseAddress";

        public const string GetAllMedicinesRequest = "api/medicine";
        public static string GetMedicineByIdRequest(long id) => $"api/medicine/{id}";
        public const string AddMedicineRequest = "api/medicine";
        public static string UpdateMedicineRequest(long id) => $"api/medicine/{id}";
        public static string DeleteMedicineRequest(long id) => $"api/medicine/{id}";
    }
}