﻿using System;
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
    }
}