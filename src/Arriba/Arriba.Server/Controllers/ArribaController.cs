using Arriba.Communication.Server.Application;
using Arriba.Model;
using Arriba.Model.Column;
using Arriba.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Arriba.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArribaController : ControllerBase
    {
        private readonly IArribaManagementService _arribaManagement;
        private readonly IArribaServerConfiguration _arribaServerConfiguration;

        public ArribaController(IArribaManagementService arribaManagement, IArribaServerConfiguration arribaServerConfiguration)
        {
            _arribaManagement = arribaManagement;
            _arribaServerConfiguration = arribaServerConfiguration;
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
                return ExceptionToActionResult(ex);
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
                return ExceptionToActionResult(ex);
            }

            return CreatedAtAction(nameof(PostAddColumn), "Columns Added");

        }

        private IActionResult ExceptionToActionResult(Exception ex)
        {
            if (ex is ArribaAccessForbiddenException)
                if(_arribaServerConfiguration.EnabledAuthentication) 
                    return Forbid();
                else
                    return new StatusCodeResult((int)HttpStatusCode.Forbidden);

            if (ex is TableNotFoundException)
                return NotFound(ex.Message);

            return BadRequest(ex.Message);
        }

        [HttpGet("/table/{tableName}/save")]
        public IActionResult GetSaveTable(string tableName)
        {
            try
            {
                _arribaManagement.SaveTableForUser(tableName, this.User, VerificationLevel.Normal);
            }
            catch (Exception ex)
            {
                return ExceptionToActionResult(ex);
            }
            return Ok("Saved");
        }

        [HttpGet("/table/{tableName}/reload")]
        public IActionResult GetReloadTable(string tableName)
        {
            try
            {
                _arribaManagement.ReloadTableForUser(tableName, this.User);
            }
            catch (Exception ex)
            {
                return ExceptionToActionResult(ex);
            }
            return Ok("Reloaded");
        }

        [HttpDelete("/table/{tableName}")]
        [HttpGet("/table/{tableName}/delete")]
        public IActionResult DeleteTable(string tableName)
        {
            try
            {
                _arribaManagement.DeleteTableForUser(tableName, this.User);
            }
            catch (Exception ex)
            {
                return ExceptionToActionResult(ex);
            }
            return Ok("Deleted");
        }

    }
}
