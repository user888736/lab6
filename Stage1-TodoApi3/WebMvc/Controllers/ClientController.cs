using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; //IConfiguration
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebMvc.Controllers
{
    [Authorize]

    public class ClientController : Controller
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;
        private readonly IConfiguration _configuration;

        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
            WebApiPath = _configuration["TodoApiConfig:Url"];   //read from appsettings.json
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", _configuration["TodoApiConfig:ApiKey"]);   //use on any http calls      
        }


        // GET: ClientController
        public async Task<ActionResult> Index()
        {
            List<TodoItem> todos = null;
            HttpResponseMessage response = await client.GetAsync(WebApiPath);
            if (response.IsSuccessStatusCode)
            {
                todos = await response.Content.ReadAsAsync<List<TodoItem>>();  //requires System.Net.Http.Formatting.Extension
            }
            return View(todos);
        }



        // GET: ClientController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + id);
            if (response.IsSuccessStatusCode)
            {
                TodoItem todo = await response.Content.ReadAsAsync<TodoItem>();
                return View(todo);
            }
            return NotFound();
        }


        // GET: ClientController/Create
        public ActionResult Create()
        {
            return View();
        }



        // POST: ClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]  //todo add deoc to slides
        public async Task<ActionResult> Create([Bind("Id,Name,IsComplete")] TodoItem todo)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(WebApiPath, todo);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }



        // GET: ClientController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + id);
            if (response.IsSuccessStatusCode)
            {
                TodoItem todo = await response.Content.ReadAsAsync<TodoItem>();
                return View(todo);
            }
            return NotFound();
        }


        // POST: ClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,IsComplete")] TodoItem todo)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(WebApiPath + id, todo);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }




        // GET: ClientController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + id);
            if (response.IsSuccessStatusCode)
            {
                TodoItem todo = await response.Content.ReadAsAsync<TodoItem>();
                return View(todo);
            }
            return NotFound();
        }


        // POST: ClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, int notUsed = 0)
        {
            HttpResponseMessage response = await client.DeleteAsync(WebApiPath + id);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
    }
}

