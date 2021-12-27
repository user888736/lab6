using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Attributes;


namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }



        // GET: api/TodoItems   notice: we don't use method name
        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation("Zwraca wszystkie zadania.", "Używa EF")]
        [SwaggerResponse(200, "Sukces", Type = typeof(List<TodoItem>))]        
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();  //http 200
        }



        // GET: api/TodoItems/5
        [HttpGet("{id}")]        
        [Produces("application/json")]
        [SwaggerOperation("Zwraca zadanie o podanym {id}.", "Używa EF FindAsync()")]
        [SwaggerResponse(200, "Znaleziono zadanie o podanym {id}", Type = typeof(TodoItem))]
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz odczytać", Required = true)]
            int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound(); //http 404
            }

            return todoItem;    //http 200
        }


        // PUT: api/TodoItems/5        
        [HttpPut("{id}")]
        [Produces("application/json")]
        [SwaggerOperation("Aktualizuje zadanie o podanym {id}.", "Używa EF")]
        [SwaggerResponse(204, "Zaktualizowano zadanie o podanym {id}")]
        [SwaggerResponse(400, "Nie rozpoznano danych wejściowych")]
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<IActionResult> PutTodoItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz zaktualizować", Required = true)]
            int id,
            [SwaggerParameter("Definicja zadania", Required = true)]
            TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest(); //http 400
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();  //http 404
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); //http 204
        }


        // POST: api/TodoItems        
        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation("Tworzy nowe zadanie.", "Używa EF")]
        [SwaggerResponse(201, "Zapis z sukcesem.", Type = typeof(TodoItem))]
        public async Task<ActionResult<TodoItem>> PostTodoItem(
            [SwaggerParameter("Definicja zadania", Required = true)]
            TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);  //http 201, add Location header
        }



        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [SwaggerOperation("Usuwa zadanie o podanym {id}.", "Używa EF")]
        [SwaggerResponse(204, "Usunięto zadanie o podanym {id}")]        
        [SwaggerResponse(404, "Nie znaleziono zadania o podanym {id}")]
        public async Task<IActionResult> DeleteTodoItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz usunąć", Required = true)]
            int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();  //http 404
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent(); //http 204
        }



        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
