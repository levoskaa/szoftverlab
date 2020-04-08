using api.DAL;
using api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksRepository repository;

        public TasksController(ITasksRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<Task> List()
        {
            return repository.List();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Task> Get(int id)
        {
            var value = repository.FindById(id);
            if (value == null)
                return NotFound();
            return Ok(value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Task> Create([FromBody] Dto.CreateTask value)
        {
            try
            {
                var created = repository.Insert(value);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(new { error = ae.Message });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Task> Delete(int id)
        {
            var task = repository.FindById(id);
            if (task == null)
                return NotFound();
            repository.Delete(id);
            return NoContent();
        }

        [HttpPatch("{id}/done")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Task> MarkDone(int id)
        {
            var task = repository.MarkDone(id);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpPatch("{id}/move")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Task> MoveToStatus(int id, string newStatusName)
        {
            if (String.IsNullOrEmpty(newStatusName))
                return NotFound();
            var task = repository.MoveToStatus(id, newStatusName);
            if (task == null)
                return NotFound();
            return Ok(task);
        }
    }
}
