using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ticaretix.Application.Commands;
using ticaretix.Application.Commands.KullaniciCommands;
using ticaretix.Application.Queries;
using ticaretix.Application.Queries.KullanıcıQueries;
using ticaretix.Core.Entities;

namespace ticaretix.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        private readonly ISender _sender;

        public KullaniciController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet("GetAllKullanici")]
        public async Task<IActionResult> GetAlKullaniciAsync()
        {
            var command = new GetAllKullaniciQuery();
            var result = await _sender.Send(command);
            return Ok(result);
        }


        [HttpPost("AddKullanici")]
        public async Task<IActionResult> AddKullaniciAsync([FromBody] KullaniciEntity kullaniciEntity)
        {
            var command = new AddKullaniciCommand(kullaniciEntity);
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpGet("GetKullaniciByEmail/{email}")]
        public async Task<IActionResult> GetKullaniciByEmailAsync([FromRoute] string email)
        {
            var command = new GetKullaniciByIdQuery(email);
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateKullanici/{email}")]
        public async Task<IActionResult> UpdateKullaniciAsync([FromRoute] string email, [FromBody] KullaniciEntity kullaniciEntity)
        {
            var command = new UpdateKullaniciCommand(email, kullaniciEntity);
            var result = await _sender.Send(command);
            return Ok(result);
        }
        [HttpDelete("DeleteKullaniciAsync/{email}")]
        public async Task<IActionResult> DeleteKullaniciAsync([FromRoute] string email)
        {
            var command = new DeleteKullaniciCommand(email);
            var result = await _sender.Send(command);
            return Ok(result);
        }

    }
}
