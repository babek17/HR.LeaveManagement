using System;
using AutoMapper;
using HR.LeaveManagement.BlazorUI.Models.LeaveTypes;
using HR.LeaveManagement.BlazorUI.Services.Base;

namespace HR.LeaveManagement.BlazorUI.MappingProfiles;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<LeaveTypeDto, LeaveTypeVM>().ReverseMap();
        CreateMap<CreateLeaveAllocationCommand, LeaveTypeVM>().ReverseMap();
        CreateMap<UpdateLeaveAllocationCommand, LeaveTypeVM>().ReverseMap();
    }
}
