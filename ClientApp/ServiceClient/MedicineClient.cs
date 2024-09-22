using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Configuration;
using ClientApp.Contracts;
using Newtonsoft.Json;

namespace ClientApp.ServiceClient
{
    public class MedicineClient
    {
        private readonly IHttpClientFactory _clientFactory;

        public MedicineClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<MedicineDto>> GetAllMedicinesAsync()
        {
            var client = _clientFactory.CreateClient(MedicineClientOptions.ClientName);

            var request = new HttpRequestMessage(HttpMethod.Get, MedicineClientOptions.GetAllMedicinesRequest);

            string content;
            try
            {
                var response = await client.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Connection error");
            }

            var result = JsonConvert.DeserializeObject<MultipleDataResponse<MedicineDto>>(content);

            if (result.Code.HasValue)
                throw new Exception(result.Text);

            return result.Data;
        }

        public async Task<MedicineDto> GetMedicineByIdAsync(long id)
        {
            var client = _clientFactory.CreateClient(MedicineClientOptions.ClientName);

            var request = new HttpRequestMessage(HttpMethod.Get, MedicineClientOptions.GetMedicineByIdRequest(id));

            string content;
            try
            {
                var response = await client.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Connection error");
            }

            var result = JsonConvert.DeserializeObject<SingleDataResponse<MedicineDto>>(content);

            if (result.Code.HasValue)
                throw new Exception(result.Text);

            return result.Data;
        }

        public async Task<MedicineDto> AddMedicineAsync(MedicineRequest newMedicine)
        {
            var client = _clientFactory.CreateClient(MedicineClientOptions.ClientName);

            var request = new HttpRequestMessage(HttpMethod.Post, MedicineClientOptions.AddMedicineRequest)
            {
                Content = new StringContent(JsonConvert.SerializeObject(newMedicine), Encoding.UTF8, "application/json")
            };

            string content;
            try
            {
                var response = await client.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Connection error");
            }

            var result = JsonConvert.DeserializeObject<SingleDataResponse<MedicineDto>>(content);

            if (result.Code.HasValue)
                throw new Exception(result.Text);

            return result.Data;
        }

        public async Task UpdateMedicineAsync(long id, MedicineRequest medicine)
        {
            var client = _clientFactory.CreateClient(MedicineClientOptions.ClientName);

            var request = new HttpRequestMessage(HttpMethod.Put, MedicineClientOptions.UpdateMedicineRequest(id))
            {
                Content = new StringContent(JsonConvert.SerializeObject(medicine), Encoding.UTF8, "application/json")
            };

            string content;
            try
            {
                var response = await client.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Connection error");
            }

            var result = JsonConvert.DeserializeObject<SingleDataResponse<BaseResponse>>(content);

            if (result != null && result.Code.HasValue)
                throw new Exception(result.Text);
        }

        public async Task DeleteMedicineAsync(long id)
        {
            var client = _clientFactory.CreateClient(MedicineClientOptions.ClientName);

            var request = new HttpRequestMessage(HttpMethod.Delete, MedicineClientOptions.DeleteMedicineRequest(id));

            string content;
            try
            {
                var response = await client.SendAsync(request);
                content = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Connection error");
            }

            var result = JsonConvert.DeserializeObject<SingleDataResponse<BaseResponse>>(content);

            if (result != null && result.Code.HasValue)
                throw new Exception(result.Text);
        }
    }
}