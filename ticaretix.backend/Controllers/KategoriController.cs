using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ticaretix.Application.Commands.KategoriCommands;
using ticaretix.Application.Queries;
using ticaretix.Application.Queries.KategoriQueries;
using ticaretix.Core.Entities;

namespace ticaretix.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategoriController : ControllerBase
    {
        private readonly ISender _sender;
        public KategoriController(ISender sender)
        {
            _sender = sender;
        }


        [HttpGet("GetAllKategori")]
        public async Task<IActionResult> GetAlKategoriAsync()
        {
            var command = new GetAllKategoriQuery();
            var result = await _sender.Send(command);
            return Ok(result);
        }
        [HttpPost("AddKategori")]
        public async Task<IActionResult> AddlKategoriAsync([FromBody] CategoryEntity categoryEntity)
        {
            var command = new AddKategoriCommand(categoryEntity);
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpGet("GetKategoriByName/{name}")]
        public async Task<IActionResult> GetKategoriByNameAsync([FromRoute] string name)
        {
            var command = new GetKategoriByNameQuery(name);
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateKategoriByName/{name}")]
        public async Task<IActionResult> UpdateKategoriByNameAsync([FromRoute] string name, [FromBody] CategoryEntity categoryEntity)
        {
            var command = new UpdateKategoriCommand(name,categoryEntity);
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteKategoriByName/{name}")]
        public async Task<IActionResult> DeleteKategoriByNameAsync([FromRoute] string name)
        {
            var command = new DeleteKategoriCommand(name);
            var result = await _sender.Send(command);
            return Ok(result);
        }
    }
}
