using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PerfectChannel.WebApi.Models;
using Newtonsoft.Json;

namespace PerfectChannel.WebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ToDoContext _context;

        public TaskController(ToDoContext context)
        {
            _context = context;
        }

        // GET: api/task/GetToDoItems
        // Return a group with the completed tasks and another group with pending tasks
        [HttpGet]
        [Route("GetToDoItems")]
        public async Task<ActionResult<IEnumerable<ToDoItemModel>>> GetToDoItems()
        {
            var todos = await _context.ToDoItems.ToListAsync();

            if(todos == null)
            {
                return Ok();
            }

            var completed = todos.Select(t => new ToDoItemModel
            {
                ItemId = t.ItemId,
                ItemName = t.ItemName,
                ItemDescription = t.ItemDescription,
                ItemStatusCompleted = t.ItemStatusCompleted
            }).Where(b => b.ItemStatusCompleted);

            var pending = todos.Select(t => new ToDoItemModel
            {
                ItemId = t.ItemId,
                ItemName = t.ItemName,
                ItemDescription = t.ItemDescription,
                ItemStatusCompleted = t.ItemStatusCompleted
            }).Where(b => !b.ItemStatusCompleted);

            var content = JsonConvert.SerializeObject(new { Completed = completed, Pending = pending });

            return Ok(content);
        }

        // POST: api/task/PostToDoItem
        // Add a new task, pending by default
        [HttpPost]
        [Route("PostToDoItem")]
        public async Task<ActionResult<ToDoItemModel>> PostToDoItem(ToDoItemModel toDoItem)
        {
            //toDoItem.ItemStatusCompleted = false; // pending by default
            _context.ToDoItems.Add(toDoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostToDoItem", new { id = toDoItem.ItemId }, toDoItem);
        }

        // PUT: api/task/PutToDoItem/1
        // Update the status of a task and return the updated item
        [HttpPut("{id}")]
        [Route("PutToDoItem/{id}")]
        public async Task<IActionResult> PutTodoItem(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);

            if (toDoItem == null)
            {
                return BadRequest();
            }

            if (id != toDoItem.ItemId)
            {
                return BadRequest();
            }

            toDoItem.ItemStatusCompleted = !toDoItem.ItemStatusCompleted; //change status
            _context.Entry(toDoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("PutTodoItem", toDoItem);
        }

        // Method to check if an ID exists
        private bool TodoItemExists(int id)
        {
            return _context.ToDoItems.Any(e => e.ItemId == id);
        }
    }
}