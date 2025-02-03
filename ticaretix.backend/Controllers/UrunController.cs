using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ticaretix.Application.Commands;
using ticaretix.Application.Commands.KullaniciCommands;
using ticaretix.Application.Commands.UrunCommands;
using ticaretix.Application.Queries;
using ticaretix.Application.Queries.KullanıcıQueries;
using ticaretix.Application.Queries.UrunQueries;
using ticaretix.Core.Entities;

namespace ticaretix.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrunController : ControllerBase
    {
        private readonly ISender _sender;

        public UrunController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("GetAllUrunler")]
        public async Task<IActionResult> GetAllUrunlerAsync()
        {
            var command = new GetAllUrunQuery();
            var result = await _sender.Send(command);
            return Ok(result);
        }


        [HttpPost("AddUrun")]
        public async Task<IActionResult> AddUrunAsync([FromBody] UrunlerEntity urunlerEntity)
        {
            var command = new AddUrunCommand(urunlerEntity);
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpGet("GetUrunById/{id}")]
        public async Task<IActionResult> GetUrunByIdAsync([FromRoute] int id)
        {
            var command = new GetUrunByIdQuery(id);
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateUrun/{id}")]
        public async Task<IActionResult> UpdateKullaniciAsync([FromRoute] int id, [FromBody] UrunlerEntity urunlerEntity)
        {
            var command = new UpdateUrunCommand(id, urunlerEntity);
            var result = await _sender.Send(command);
            return Ok(result);
        }
        [HttpDelete("DeleteUrunAsync/{id}")]
        public async Task<IActionResult> DeleteKullaniciAsync([FromRoute] int id)
        {
            var command = new DeleteUrunCommand(id);
            var result = await _sender.Send(command);
            return Ok(result);
        }
    }
}
