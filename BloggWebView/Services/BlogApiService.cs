using BloggWebView.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BloggWebView.Services
{
    public class BlogApiService
    {
        private readonly HttpClient _httpClient;

        public BlogApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7091/api");
        }

        public async Task<bool> CheckTitleExistence(string title)
        {
            HttpResponseMessage titleCheckResponse = await _httpClient.GetAsync("/Auth/blogs/" + title);
            return titleCheckResponse.IsSuccessStatusCode;
        }

        public async Task<List<BlogView>> GetUserData()
        {
            HttpResponseMessage blogsResponse = await _httpClient.GetAsync("/Auth/get-user-data");
            if (blogsResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<List<BlogView>>(await blogsResponse.Content.ReadAsStringAsync());
            }
            return null;
        }

        public async Task<bool> AddNewBlog(BlogView blog)
        {
            string jsonBlog = JsonConvert.SerializeObject(blog);
            StringContent content = new StringContent(jsonBlog, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/Auth/newBlogs", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RegisterUser(UserView user)
        {
            string jsonUser = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/Auth/register", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Login(UserLoginView user)
        {
            string jsonUser = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("/Auth/login", content);
            return response.IsSuccessStatusCode;
        }
    }
}
