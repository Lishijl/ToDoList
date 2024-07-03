using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using ToDoList.DTO;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TodoListsController : ControllerBase
    {
        private readonly TodoContext _context;

        // injectem un TodoContext (Context de BBDD) al controlador de la app (ASP .NET Core) 
        // i ho utilitzarem per les conexions i OPERACIONS del CRUD que li farem a aquest TodoContext
        public TodoListsController(TodoContext context) {

            // assignem a la propieat de la classe TodoListsController, el context injectat pel paràmetre
            // que farà d'intermediari entre l'app i la BBDD, per fer operacions de dades ESTRUCTURADES i segures
            _context = context;
        }
        [HttpGet]
        [HttpPut("{id}")]
        [HttpPost]
        [HttpDelete("{id}")]
        private bool TodoListExists(long id)
        {
            return _context.TodoLists.Any(l => l.Id == id);
        }
    }
}