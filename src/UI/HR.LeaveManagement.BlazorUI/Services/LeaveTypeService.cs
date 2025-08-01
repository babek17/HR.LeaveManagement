using System;
using AutoMapper;
using HR.LeaveManagement.BlazorUI.Contracts;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;

namespace HR.LeaveManagement.BlazorUI.Services.Base;

public class LeaveTypeService : BaseHttpService, ILeaveTypeService
{
    private readonly IMapper _mapper;
    public LeaveTypeService(IClient client, IMapper mapper) : base(client)
    {
        _mapper = mapper;
    }

    public async Task<Response<Guid>> CreateLeaveType(LeaveTypeVM leaveType)
    {
        try
        {
            var createLeaveTypeCommand = _mapper.Map<CreateLeaveTypeCommand>(leaveType);
            await _client.LeaveTypePOSTAsync(createLeaveTypeCommand);
            return new Response<Guid>
            {
                Success = true
            };
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<Guid>(ex);
        }
    }

    public async Task<Response<Guid>> DeleteLeaveType(int id)
    {
        try
        {
            await _client.LeaveTypeDELETEAsync(id);
            return new Response<Guid>
            {
                Success = true
            };
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<Guid>(ex);
        }
    }

    public async Task<LeaveTypeVM> GetLeaveTypeDetails(int id)
    {
        var leaveType = await _client.LeaveTypeGETAsync(id);
        return _mapper.Map<LeaveTypeVM>(leaveType);
    }

    public async Task<List<LeaveTypeVM>> GetLeaveTypes()
    {
        var leaveTypes = await _client.LeaveTypeAllAsync();
        return _mapper.Map<List<LeaveTypeVM>>(leaveTypes);
    }

    public async Task<Response<Guid>> UpdateLeaveType(int id, LeaveTypeVM leaveType)
    {
        try
        {
            var updateLeaveTypeCommand = _mapper.Map<UpdateLeaveTypeCommand>(leaveType);
            await _client.LeaveTypePUTAsync(id, updateLeaveTypeCommand);
            return new Response<Guid>
            {
                Success = true
            };
        }
        catch (ApiException ex)
        {
            return ConvertApiExceptions<Guid>(ex);
        }
    }
}
