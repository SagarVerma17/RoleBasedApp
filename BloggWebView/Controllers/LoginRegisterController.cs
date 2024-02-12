using BloggWebView.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BloggWebView.Controllers
{
    public class LoginRegisterController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7091/api");
        private readonly HttpClient _httpClient;

        public LoginRegisterController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginView user)
        {
            string jsonEmployee = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                // Retrieve username from cookies
                var username = User?.Identity?.Name;

                // Fetch all blogs
                HttpResponseMessage blogsResponse = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Auth/get-user-data");

                if (blogsResponse.IsSuccessStatusCode)
                {
                    //var blogs = JsonConvert.DeserializeObject<List<BlogView>>(await blogsResponse.Content.ReadAsStringAsync());

                    var blogs = JsonConvert.DeserializeObject<List<BlogView>>(await blogsResponse.Content.ReadAsStringAsync());
                    blogs = blogs.OrderByDescending(blog => blog.TimeStamp).ToList();

                    // Pass the blogs and username to the view
                    ViewBag.Blogs = blogs;
                    ViewBag.UserName = user.Username;

                    Console.WriteLine($"Username from cookies: {user.Username}");

                    // Redirect to the feed page or display the feed view
                    return View(blogs);
                }
                else
                {
                    // Show error message for blog retrieval
                    ViewBag.ErrorMessage = "Error retrieving blogs. Please try again.";
                    return RedirectToAction("Index");
                }
            }

            else
            {
                // Show error message in dialog box using JavaScript
                ViewBag.ErrorMessage = "Error saving Employee data. Please try again.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Register()
        { return View(); }

        public async Task<IActionResult> Edittitle(string title)
        {
            try
            {
                // Send GET request to the API endpoint to fetch the blog details by title
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Auth/blogg/" + title);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to get the blog details
                    var blogJson = await response.Content.ReadAsStringAsync();
                    var blog = JsonConvert.DeserializeObject<BlogView>(blogJson);

                    // Return the view with the blog details for editing
                    return View(blog);
                }
                else
                {
                    // If the blog is not found, display an error message
                    ViewBag.ErrorMessage = "Blog not found.";
                    return RedirectToAction("AddNewBlog"); // Redirect to a suitable action
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
                return RedirectToAction("AddNewBlog"); // Redirect to a suitable action
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BlogView blog)
        {
            try
            {
                blog.TimeStamp = DateTime.Now;
                // Serialize the updated blog object to JSON
                string jsonBlog = JsonConvert.SerializeObject(blog);
                StringContent content = new StringContent(jsonBlog, Encoding.UTF8, "application/json");

                // Send PUT request to the API endpoint for updating the blog
                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Auth/newBlogs", content);

                if (response.IsSuccessStatusCode)
                {
                    // Blog updated successfully
                    ViewBag.SuccessMessage = "Blog updated successfully!";
                    return RedirectToAction("Index"); // Redirect to a suitable action
                }
                else
                {
                    // Error updating the blog
                    ViewBag.ErrorMessage = "Error updating blog. Please try again.";
                    return View(blog); // Return to the edit view with the blog details
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
                return View(blog); // Return to the edit view with the blog details
            }
        }



        public async Task<ActionResult> Delete(string title)
        {
            try
            {

                // Send DELETE request to the API endpoint for deleting the blog with the specified title
                HttpResponseMessage response = await _httpClient.DeleteAsync(_httpClient.BaseAddress+"/Auth/blogs/" + title);

                if (response.IsSuccessStatusCode)
                {
                    // Blog deleted successfully
                    ViewBag.SuccessMessage = "Blog deleted successfully!";
                }
                else
                {
                    // Error deleting the blog
                    ViewBag.ErrorMessage = "Error deleting blog. Please try again.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                ViewBag.ErrorMessage = $"Error: {ex.Message}";
                return RedirectToAction("AddNewBlog");
            }
        }

        public async Task<ActionResult> Registers(UserView user)
        {
            string jsonEmployee = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(jsonEmployee, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Auth/register", content).Result;
            if (response.IsSuccessStatusCode)
            {
                // Show success message in dialog box using JavaScript
                ViewBag.SuccessMessage = "Employee data saved successfully!";
            }
            else
            {
                // Show error message in dialog box using JavaScript
                ViewBag.ErrorMessage = "Error saving Employee data. Please try again.";
            }
            return RedirectToAction("Index");
        }



        public IActionResult AddBlog(string username)
        {
            ViewBag.UserName = username;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewBlog(BlogView blog)
        {
            // Set the username and timestamp for the blog
            blog.Username = blog.Username;
            blog.TimeStamp = DateTime.Now;

            // Serialize the blog object to JSON
            string jsonBlog = JsonConvert.SerializeObject(blog);
            StringContent content = new StringContent(jsonBlog, Encoding.UTF8, "application/json");

            // Call the API to add the new blog entry
            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress+"/Auth/newBlogs", content);

            if (response.IsSuccessStatusCode)
            {
                UserView user = new UserView();
                user.Username = blog.Username;
                var username = User?.Identity?.Name;

                // Fetch all blogs
                HttpResponseMessage blogsResponse = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Auth/get-user-data");

                if (blogsResponse.IsSuccessStatusCode)
                {
                    //var blogs = JsonConvert.DeserializeObject<List<BlogView>>(await blogsResponse.Content.ReadAsStringAsync());

                    var blogs = JsonConvert.DeserializeObject<List<BlogView>>(await blogsResponse.Content.ReadAsStringAsync());
                    blogs = blogs.OrderByDescending(blog => blog.TimeStamp).ToList();

                    // Pass the blogs and username to the view
                    ViewBag.Blogs = blogs;
                    ViewBag.UserName = user.Username;

                    Console.WriteLine($"Username from cookies: {user.Username}");

                    // Redirect to the feed page or display the feed view
                    return View(blogs);
                }
                else
                {
                    // Show error message for blog retrieval
                    ViewBag.ErrorMessage = "Error retrieving blogs. Please try again.";
                    ViewBag.UserName = user.Username;
                    return RedirectToAction("AddBlog");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Error adding blog entry. Please try again.";
                return RedirectToAction("AddBlog");
            }
        }

        
    }
}