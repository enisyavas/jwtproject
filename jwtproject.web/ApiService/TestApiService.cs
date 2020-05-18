using jwtproject.web.Models;
using jwtproject.Web.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace jwtproject.web.ApiService
{
    public class TestApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TestApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Token> SignIn(UserViewModel model)
        {
            Token token = new Token();
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Login/SignIn", stringContent);
            if (response.IsSuccessStatusCode) {
                token = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                token = null;
            }
            return token;
        }

        public async Task<string> Test()
        {
            string returnData = null;
            var stringContent = new StringContent(string.Empty);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x=>x.Type== "apitoken")?.Value);
            var response = await _httpClient.PostAsync("Test/Index", stringContent);
            if (response.IsSuccessStatusCode)
            {
                returnData  = await response.Content.ReadAsStringAsync();
            }
            return returnData;
        }
    }
}
