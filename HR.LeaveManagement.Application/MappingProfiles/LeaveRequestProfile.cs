using System;
using AutoMapper;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands;
using HR.LeaveManagement.Application.Features.LeaveRequest.Commands.UpdateLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequest.Queries.GetLeaveRequestDetails;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.MappingProfiles;

public class LeaveRequestProfile : Profile
{
    public LeaveRequestProfile()
    {
        CreateMap<LeaveRequest, LeaveRequestDetailsDto>();
        CreateMap<LeaveRequestDto, LeaveRequest>();
        CreateMap<CreateLeaveRequestCommand, LeaveRequest>();
        CreateMap<UpdateLeaveRequestCommand, LeaveRequest>();
    }
}
