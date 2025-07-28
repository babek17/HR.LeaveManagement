using System;
using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Logging;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.Features.LeaveType.Queries.GetAllLeaveTypes;
using HR.LeaveManagement.Application.MappingProfiles;
using HR.LeaveManagement.Application.UnitTest.Mocks;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shouldly;

namespace HR.LeaveManagement.Application.UnitTest.Features.LeaveTypes.Queries;

public class GetLeaveTypeListQueryHandlerTest
{
    private readonly Mock<ILeaveTypeRepository> _mockLeaveTypeRepository;
    private readonly IMapper _mapper;
    private readonly Mock<IAppLogger<GetLeaveTypesQueryHandler>> _appLogger;

    public GetLeaveTypeListQueryHandlerTest()
    {
        _mockLeaveTypeRepository = MockLeaveTypeRepository.GetMockLeaveTypeRepository();

        var loggerFactory = NullLoggerFactory.Instance;
        var configExpression = new MapperConfigurationExpression();
        configExpression.AddProfile<LeaveTypeProfile>();
        var configuration = new MapperConfiguration(configExpression, loggerFactory);
        _mapper = new Mapper(configuration);

        _appLogger = new Mock<IAppLogger<GetLeaveTypesQueryHandler>>();
    }

    [Fact]
    public async Task GetLeaveTypeListTest()
    {
        // Given
        var handler = new GetLeaveTypesQueryHandler(_mapper, _mockLeaveTypeRepository.Object, _appLogger.Object);
        // When
        var result = await handler.Handle(new GetLeaveTypesQuery(), CancellationToken.None);
        // Then
        result.ShouldBeOfType<List<LeaveTypeDto>>();
        result.Count.ShouldBe(3);
    }
}
