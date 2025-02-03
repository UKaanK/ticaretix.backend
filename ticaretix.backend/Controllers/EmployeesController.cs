using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ticaretix.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ISender _sender;

        public EmployeesController(ISender sender)
        {
            _sender = sender;
        }
        //[HttpPost("")]
        //public async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeEntity employee)
        //{
        //    var command = new AddEmployeeCommand(employee);
        //    var result = await _sender.Send(command);
        //    return Ok(result);
        //}
        //[HttpGet("")]
        //public async Task<IActionResult> GetAllEmployeeAsync() {
        //    var command = new GetAllEmployeesQuery();
        //    var result = await _sender.Send(command);
        //    return Ok(result);
        //}
        //[HttpGet("{employeeId}")]
        //public async Task<IActionResult> GetEmployeeByIdAsync([FromRoute] Guid employeeId)
        //{
        //    var command = new GetEmployeeByIdQuery(employeeId);
        //    var result = await _sender.Send(command);
        //    return Ok(result);
        //}
        //[HttpPut("{employeeId}")]
        //public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] Guid employeeId, [FromBody] EmployeeEntity employeeEntity)
        //{
        //    var command = new UpdateEmployeeCommand(employeeId, employeeEntity);
        //    var result = await _sender.Send(command);
        //    return Ok(result);
        //}
        //[HttpDelete("{employeeId}")]
        //public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] Guid employeeId)
        //{
        //    var command = new DeleteEmployeeCommand(employeeId);
        //    var result = await _sender.Send(command);
        //    return Ok(result);
        //}
    }
}
