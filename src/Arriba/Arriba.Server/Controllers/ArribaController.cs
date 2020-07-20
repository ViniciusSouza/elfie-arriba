using Arriba.Communication.Server.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


    }
}
