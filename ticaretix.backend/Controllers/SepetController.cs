using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ticaretix.Application.Commands.SepetCommands;
using ticaretix.Application.Queries.SepetQueries;
using ticaretix.Application.Queries.UrunQueries;
using ticaretix.Core.Entities;

namespace ticaretix.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SepetController : ControllerBase
    {
        private readonly ISender _sender;
        public SepetController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("GetSepet")]
        public async Task<IActionResult> GetSepetAsync()
        {
            var command = new GetAllSepetQuery();
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpGet("GetSepetDetay")]
        public async Task<IActionResult> GetSepetDetay()
        {
            var command = new GetAllSepetDetayQuery();
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpGet("GetSepetDetayById{sepetId}")]
        public async Task<IActionResult> GetSepetDetayByIdAsync([FromRoute] int sepetId)
        {
            var command = new GetSepetDetayByIdQuery(sepetId);
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpPost("AddSepetUrun")]
        public async Task<IActionResult> AddSepetUrunAsync([FromBody] SepetDetaylariEntity sepetDetaylariEntity)
        {
            var command = new AddSepetUrunCommand(sepetDetaylariEntity);
            var result = await _sender.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteSepetUrun")]
        public async Task<IActionResult> DeleteSepetUrunAsync([FromRoute] int urunId, [FromRoute] int sepetId) { 
            var command = new DeleteSepetUrunCommand(urunId, sepetId);
            var result = await _sender.Send(command);
            return Ok(result);
        }
    }
}
