using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Exceptions;
using MediatR;

namespace HR.LeaveManagement.Application.Features.LeaveType.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler :  IRequestHandler<CreateLeaveTypeCommand, int>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly IMapper _mapper;

    public CreateLeaveTypeCommandHandler(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
    {
        _leaveTypeRepository = leaveTypeRepository;
        _mapper = mapper;
    }
    
    public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        //Validate incoming data
        var validator = new CreateLeaveTypeCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
        {
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine($"Property: {error.PropertyName}, Error: {error.ErrorMessage}");
            }
            throw new BadRequestException("Invalid LeaveType.", validationResult);
        }
        //Convert to domain enity type
        var leaveType = _mapper.Map<Domain.LeaveType>(request);
        //Add to database
        await _leaveTypeRepository.CreateAsync(leaveType);
        //Return record id
        return leaveType.Id;
    }
}