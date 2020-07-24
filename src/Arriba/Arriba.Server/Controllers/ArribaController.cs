using Arriba.Communication.Server.Application;
using Arriba.Model.Column;
using Arriba.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arriba.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArribaController : ControllerBase
    {
       private readonly IArribaManagementService _arribaManagement;

        public ArribaController(IArribaManagementService arribaManagement)
        {
            _arribaManagement = arribaManagement;
        }

        [HttpGet]
        public IActionResult GetTables ()
        {
            return Ok(_arribaManagement.GetTables());
        }

        [HttpGet("allBasics")]
        public IActionResult GetAllBasics()
        {
            return Ok(_arribaManagement.GetTablesForUser(this.User));
        }

        [HttpGet("unloadAll")]
        public IActionResult GetUnloadAll()
        {
            if (!_arribaManagement.UnloadAllTableForUser(this.User))
                return new ForbidResult();

            return Ok($"All tables unloaded");
        }

        [HttpGet("{tableName}/unload")]
        public IActionResult GetUnloadTable(string tableName)
        {
            if (!_arribaManagement.UnloadTableForUser(tableName, this.User))
                return new ForbidResult();

            return Ok($"Table {tableName} unloaded");
        }

        [HttpPost]
        public IActionResult PostCreateNewTable([Required]CreateTableRequest table)
        {
            try
            {
                _arribaManagement.CreateTableForUser(table, this.User);
            }catch(Exception ex)
            {
                if (ex is ArribaAccessForbiddenException)
                    return Forbid();

                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(PostCreateNewTable), null);
        }

        [HttpPost("table/{tableName}/addcolumns")]
        public IActionResult PostAddColumn(string tableName, [FromBody, Required] IList<ColumnDetails> columnDetails)
        {

            try
            {
                _arribaManagement.AddColumnsToTableForUser(tableName, columnDetails, this.User);
            }
            catch (Exception ex)
            {
                if (ex is ArribaAccessForbiddenException)
                    return Forbid();

                if (ex is TableNotFoundException)
                    return NotFound(ex.Message);

                return BadRequest(ex.Message);

            }

            return CreatedAtAction(nameof(PostAddColumn), "Columns Added");

        }
    }
}
