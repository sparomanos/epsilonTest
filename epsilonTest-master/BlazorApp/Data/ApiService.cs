using Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp.Data
{
    public class ApiService : IApiService
    {
        public HttpClient _httpClient;

        public ApiService(HttpClient client, HttpContext context) 
        {
            _httpClient = client;
           
            var accesToken = Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.GetTokenAsync(context, "access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accesToken.Result);
        }

        public async Task Delete(string id, string route)
        {
            var response = await _httpClient.DeleteAsync("api/Customer/" + id);
            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                // handle error
            }
        }

        public async Task<PaginationList<T>> Get<T>(int? pageNumber, string route)
        {
            var response = await _httpClient.GetAsync($"api/{route}/GetItems?pageNumber={pageNumber}");
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var results = JsonSerializer.Deserialize<PaginationList<T>>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return results;
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync();
                if(response.ReasonPhrase.Contains("Unauthorized"))
                {
                    throw new Exception("Unauthorized");
                }
                else
                {
                    throw new Exception(responseString);
                }
            }
        }

        public async Task<T> Get<T>(string id, string route)
        {
            var response = await _httpClient.GetAsync($"api/{route}/" + id);
            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                var results = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return results;
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync();
                throw new Exception(responseString);
            }
        }

        public async Task<T> Post<T>(T obj, string route)
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(obj);
                var stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/{route}", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var results = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return results;
                }
                else
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    throw new Exception(responseString);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> Put<T>(T obj, string route)
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(obj);
                var stringContent = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/{route}", stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var results = JsonSerializer.Deserialize<T>(responseString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    return results;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
