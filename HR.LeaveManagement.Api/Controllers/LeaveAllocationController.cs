using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.CreateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.DeleteLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Commands.UpdateLeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Query.GetLeaveAllocationDetails;
using HR.LeaveManagement.Application.Features.LeaveAllocation.Query.GetLeaveAllocations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveAllocationController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveAllocationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<LeaveAllocationDto>>> Get(bool isLoggedInUser = false)
    {
        var leaveAllocations = await _mediator.Send(new GetLeaveAllocationListQuery());
        return Ok(leaveAllocations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveAllocationDto>> Get(int id)
    {
        var leaveAllocation = await _mediator.Send(new GetLeaveAllocationDetailQuery { Id = id });
        if (leaveAllocation == null)
        {
            return NotFound();
        }
        return Ok(leaveAllocation);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Post(CreateLeaveAllocationCommand leaveAllocation)
    {
        var response = await _mediator.Send(leaveAllocation);
        return CreatedAtAction(nameof(Get), new { Id = response });
    }

    [HttpPut]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(UpdateLeaveAllocationCommand leaveAllocation)
    {
        await _mediator.Send(leaveAllocation);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteLeaveAllocationCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}