/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; */
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using ToDoList.DTO;

// controlador en el API que maneja operacions CRUD

// BBDD context que s'inicialitza cada cop que s'inicia l'appweb,
// al ser a la memoria RAM. Te que existir un element en la BBDD abans
// d'usar un PUT, que actualitza, que no POST. usa GET per assegurar d'elements
// existents en la BBDD abans de cridar el PUT
// 204, sense contingut, no es lo mateix que id inexistent, sino que si existeix

/* actualitzem el controlador TDController, per usar el nou
 * model TodoItemDTO: */
namespace TodoApi.Controllers
{
    // normalment es substitueix "controller" pel nom del 
    // controlador "TodoItemsController" menys el sufix Controller"
    // per tant, seria "TodoItems" la classe en plural, nom del contextDB
    [Route("api/[controller]")] // comença amb una template/plantilla string en la Route de controller's
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;
        
        // ara per la referencia i lus del mapeig fem IMapper mapper
        public TodoItemsController(TodoContext context, IMapper mapper)
        {
            _context = context;
            // afegim un camp mes del mappeig
            _mapper = mapper;
        }

        // atribut HTTPGet, mètode que respon a una petició/solicitud HTTP tipus GET
        // RUTA URL: per a cada mètode { cadena de pantilla/template en 
        // l'atribut ruta del controlador}: "(per el host des d'on s'escolta https://localhost:7014/)api/controlador:TodoItems"
        // 404 Error, not found, codi d'estat de petició acció metode get
        // GET: api/TodoItems
        // en el path del atribut httpget, se li pot afegir HttpGet("alguna clau/etiqueta") en el path
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            // * primer canviem el tipus de TASCA Async a retornar, tasca, tipus RESULTATD'ACCIO
            // de petició retornat, de tipus enumerats, del model tipus (TIDTO)
            // * l'actualtzació pel model TI.DTO, enlloc de cridar directament ToListAsync()
            // desde TodoItems, contextBBDD, cridem .select( lambda/funció sense nom que obté el valor retornat pel mètode ItemToDTO(x)) i després cridar .ToListAsync();
            /*return await _context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();*/
                //.ToListAsync();
                // l'item, classe TodoItem en nova instància TIDTO.

            return Ok(_mapper.Map<IEnumerable<TodoTask>>(await _context.TodoItems.ToListAsync()));
            // retorna un 200, estem retornant un accioResult i per aixo fem un Ok(metide amb argument);
            // actionResult, classe de SPN, resultat de la peticio, tingui contingut o no, sigui la peticio exitos, o cas alternatius, o ERROR's
            // aquest Task, tipus ActionResult, ara es torna una llista de todoTask.
            // .Select() també es un mappeig, amb linkqiu, i filtre amb WHERE, però s'hauria creat un model en el DTO ("ItemToDTO" amb una lambda).
        }

        // 200: petició amb recurs de retorn
        // a la URL es proporciona al metode en el paràmetre l'id, al ser invocat, "{id}"
        // GET: api/TodoItems/5
        // <snippet_GetByID>
        [HttpGet("{id}")]

        // novament canviem TodoItem, a TodoItemDTO
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            // context del todoitem BBDD, troba paralelament per ID, fins trobar guardar
            // a la variable todoItem item de todo, posteriorment
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                // mètode de controlladors notfound()
                return NotFound();
            }

            // ItemToDTO retornat amb l'argument todoItem, passat per la crida
            // return todoItem; enlloc de retornar-la classe todoItem tal cual, la retorna passat pel metode ItemToDTO
            return ItemToDTO(todoItem);
        }
        // </snippet_GetByID>

        // funcions GETTERS retornen valors tipus ActionResult, la tasca async que sempre usara await
        // l'objecte de JSON és serialitzat per ASP.NET, i ho escriu en un cos de missatge de resposta.
        // codi d'estat de petició = 200 si existeix el recurs.
        // EXCEPCIONS no controlats = ERROR's "5XX" HTTP

        // el getter per ID: 404 = no coincident. 200 = coincident, amb un body de resposta, retornant una resposta d'objectes/item resultats 


        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // <snippet_Update>
        [HttpPut("{id}")]

        // ara l'element a modificar/actualitzar per l'ID, i per l'element TodoItemDTO.
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
        {
            // el PUT requereix, no només les especificacions a actualitzar, sinó de l'entitat/objecte json sencer,
            // per fer-ho parcial seria PATCH 
            // a més de donar l'ID com a paràmetre, li passem la classe que conté els mètodes accessors de la llista.

            // passa el todoItem que guardar en que ara es una variable todoDTO, rebut pel paràmetre
            if (id != todoDTO.Id)
            {
                // objecte no trobat 204
                return BadRequest();
            }
            
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoDTO.Name;
            todoItem.IsComplete = todoDTO.IsComplete;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }
            
            /*// IMPORTANT MANTENIR EL todoItem AQUÍ en el Entry,
            var todoItem = _mapper.Map<TodoItem>(todoDTO);

            // Bloquejar pel CORS
            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    // si no existeix l'id d'objecte, no trobat
                    return NotFound();
                }
                else
                {
                    // si nó, tira excepció
                    throw;
                }
            }*/

            // 204 codi d'estat de peticio, no hi ha contingut, pero existent
            return NoContent();
        }
        // </snippet_Update>

        // mètode actualitzat per a la creació d'un amb un POST
        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // <snippet_Create>
        [HttpPost]
        // TodoItem -> TodoItemDTO, perque posteja un element TodoItemDTO
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
        {
            // al crear-se el nou model TodoItemDTO, en el mètode POST
            // ara ens interessa usar una nova instància de tipus todoItem
            // en que la variable contindrà els mateixos camps, que del 
            // tipus TodoItemDTO, pel siEsCompletat i pel Nom.
            /*var todoItem = new TodoItem
            {
                IsComplete = todoDTO.IsComplete,
                Name = todoDTO.Name
            };*/
            
            var todoItem = _mapper.Map<TodoItem>(todoDTO);
            // mètode POST indicat amb l'atribut [HttpPost] que obté pel paràmetre el valor de TodoItem de la solicitud HTTP
            // PostTodoItem -> crea Location header's URI. nameof keyword, usat per hard-coding action name a CreatedAtAction
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            //    return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);**
            //    return CreatedAtAction("PostTodoItem", new { id = todoItem.Id }, todoItem); actualitzat per l'operador nameof()
            return CreatedAtAction(
                /*"GetTodoItem", 
                new { id = todoItem.Id },
                todoItem*/
                nameof(PostTodoItem), 
                new { id = todoItem.Id }, 
                ItemToDTO(todoItem));
            // (nameof) evita codificar nom d'acció en la crida de CREATE AT ACTION
        }
        // </snippet_Create>
        /* HTTP 201 status code(codi d'estat de la petició) : SUCCESSFUL. Generant un nou recurs al server 
         * Bases de Datos en MemoriaRAM. Si la app, es deté i re iniciar un altra vegada,
         * les peticions GET's anteriors, no retorna ningún recurs/dada.
         * Si no retorna dades, POSTeja dades en l'applicatiu API*/

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            // eliminar element de llista per id, troba per id, espera fins acabar
            // la tasca paralel async, guarda la resposta, error, resultat, a la variable
            // todoItem.
            var todoItem = await _context.TodoItems.FindAsync(id);
            // si es null, es que no s'ha trobat ID coincident/existent
            if (todoItem == null)
            {
                return NotFound();
            }

            // si no ha tancat el proces de la funció, elimina l'element retornat per l'id
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            // espera pel metode que guarda canvis Asyncroment

            return NoContent();
            // sempre es retornarà, codi d'estat 204, si la petició es correcte
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }

        // nou mètode creat a pel model TI.DTO, funcio que retorna un TodoItemDTO
        // que rep pel paràmetre un TodoItem, amb la creació d'instanciar i inicialitzar
        // un nou TodoItemDTO
        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                // classe TodoItem que té camps rebuts per la varable de la classe TodoItem
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };  // nova instància creada de TodoItemDTO
    }
}
// probar API web, per testejar, eines com: 
/* Visual Studio Endpoints Explorer + .http files(fitxers) 
 * http-repl, resposta http
 * curl, el swagger que usa curl i mostra comandes curl enviats
 * Fiddler (també per testejar)
 * instalar i testeja API's amb http-repl 
 *
 * la app actual, mostra l'objecte TodoItem sencer, apps de producció 
 * solen limitar dades que s'ingressen i retornen per mitja d'un subconjunt del MODEL
 * Raons com la SEGURETAT important, el subconjunt/subset, d'un MODEL,
 * usualment es referenciat/denominat com a objectes de transferencia de dades
 * objectes, que conté dades, sense procediments/funcions, que seràn transferides
 * (DTO), input model/d'entrada, o view model/model de vista.
 *
 * DTO s'usa per evitar POST, publicacions excessius, 
 * ocultar propietats que als clients no els cal veure.
 * omitir algunes propietats per reduir tamany de carga.
 * aplana grafics d'objectes que conté objectes aniuats.
 * grafics d'objectes aplanats resulten més convenients pels clients
 * Enfoque DTO, actualitzar la classe TodoItem, per incloure el camp tipus String nom Secret
 * public string? Secret { get; set; } que pot ser null, nou camp de la classe TodoItem
 */