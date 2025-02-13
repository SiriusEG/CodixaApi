using System.Collections.Generic;
using System.Threading.Tasks;
using Codixa.Core.Custom_Exceptions;
using Codixa.Core.Dtos.AccountDtos.Request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.request;
using Codixa.Core.Dtos.adminDashDtos.InstructorOperations.response;
using Codixa.Core.Interfaces;
using Codixa.Core.Models.UserModels;
using CodixaApi.Services;
using Codxia.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;
namespace Codixa.Api.Tests.ServicesTests
{
    public class AdmindashbordServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IAdminDashboardService _AdminDashboardService;

        public AdmindashbordServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _AdminDashboardService = new AdminDashboardService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetAllInstructors_ShouldReturnListOfInstructors()
        {
            // Arrange
            var expectedInstructors = new List<ReturnAllInstructorsReqDto>
            {
                new ReturnAllInstructorsReqDto
                {
                    RequestId = 1,
                    UserName = "john_doe",
                    FullName = "John Doe",
                    Specialty = "Mathematics",
                    Status = "Approved",
                    PhoneNumber = "123-456-7890",
                    SubmittedAt = DateTime.UtcNow,
                    Email = "john.doe@example.com",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    AdminRemarks = "No remarks",
                    Gender = "Male",
                    FilePath = "/files/john_doe.pdf"
                },
                new ReturnAllInstructorsReqDto
                {
                    RequestId = 2,
                    UserName = "jane_smith",
                    FullName = "Jane Smith",
                    Specialty = "Physics",
                    Status = "Pending",
                    PhoneNumber = "987-654-3210",
                    SubmittedAt = DateTime.UtcNow,
                    Email = "jane.smith@example.com",
                    DateOfBirth = new DateTime(1990, 8, 25),
                    AdminRemarks = null,
                    Gender = "Female",
                    FilePath = "/files/jane_smith.pdf"
                }
            };

            _mockUnitOfWork
                .Setup(uow => uow.ExecuteStoredProcedureAsync<ReturnAllInstructorsReqDto>("ShowAllInstructorRequest"))
                .ReturnsAsync(expectedInstructors);

            // Act
            var result = await _AdminDashboardService.GetAllInstructors();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedInstructors.Count, result.Count);

            // Verify the properties of the first instructor
            Assert.Equal(expectedInstructors[0].RequestId, result[0].RequestId);
            Assert.Equal(expectedInstructors[0].UserName, result[0].UserName);
            Assert.Equal(expectedInstructors[0].FullName, result[0].FullName);
            Assert.Equal(expectedInstructors[0].Specialty, result[0].Specialty);
            Assert.Equal(expectedInstructors[0].Status, result[0].Status);
            Assert.Equal(expectedInstructors[0].PhoneNumber, result[0].PhoneNumber);
            Assert.Equal(expectedInstructors[0].SubmittedAt, result[0].SubmittedAt);
            Assert.Equal(expectedInstructors[0].Email, result[0].Email);
            Assert.Equal(expectedInstructors[0].DateOfBirth, result[0].DateOfBirth);
            Assert.Equal(expectedInstructors[0].AdminRemarks, result[0].AdminRemarks);
            Assert.Equal(expectedInstructors[0].Gender, result[0].Gender);
            Assert.Equal(expectedInstructors[0].FilePath, result[0].FilePath);

            // Verify the properties of the second instructor
            Assert.Equal(expectedInstructors[1].RequestId, result[1].RequestId);
            Assert.Equal(expectedInstructors[1].UserName, result[1].UserName);
            Assert.Equal(expectedInstructors[1].FullName, result[1].FullName);
            Assert.Equal(expectedInstructors[1].Specialty, result[1].Specialty);
            Assert.Equal(expectedInstructors[1].Status, result[1].Status);
            Assert.Equal(expectedInstructors[1].PhoneNumber, result[1].PhoneNumber);
            Assert.Equal(expectedInstructors[1].SubmittedAt, result[1].SubmittedAt);
            Assert.Equal(expectedInstructors[1].Email, result[1].Email);
            Assert.Equal(expectedInstructors[1].DateOfBirth, result[1].DateOfBirth);
            Assert.Equal(expectedInstructors[1].AdminRemarks, result[1].AdminRemarks);
            Assert.Equal(expectedInstructors[1].Gender, result[1].Gender);
            Assert.Equal(expectedInstructors[1].FilePath, result[1].FilePath);
        }

        [Fact]
        public async Task GetAllInstructors_ShouldReturnEmptyList_WhenNoInstructorsExist()
        {
            // Arrange
            var expectedInstructors = new List<ReturnAllInstructorsReqDto>();

            _mockUnitOfWork
                .Setup(uow => uow.ExecuteStoredProcedureAsync<ReturnAllInstructorsReqDto>("ShowAllInstructorRequest"))
                .ReturnsAsync(expectedInstructors);

            // Act
            var result = await _AdminDashboardService.GetAllInstructors();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ChangeInstructorRequestStatus_ShouldReturnRowsAffected_WhenStatusIsValid()
        {
            // Arrange
            var requestStatusDto = new ChangeInstructorRequestStatusDto
            {
                RequestId = 1,
                NewStatus = "Approved"
            };

            _mockUnitOfWork
                .Setup(uow => uow.ExecuteStoredProcedureAsyncIntReturn(
                    "EXEC ChangeInstructorRequestStatus @InstructorId, @NewStatus",
                    It.Is<SqlParameter>(p => p.ParameterName == "@InstructorId" && (int)p.Value == requestStatusDto.RequestId),
                    It.Is<SqlParameter>(p => p.ParameterName == "@NewStatus" && (string)p.Value == requestStatusDto.NewStatus)
                ))
                .ReturnsAsync(1); // Simulate 1 row affected

            // Act
            var result = await _AdminDashboardService.ChangeInstructorRequestStatus(requestStatusDto);

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task ChangeInstructorRequestStatus_ShouldThrowArgumentException_WhenStatusIsInvalid()
        {
            // Arrange
            var requestStatusDto = new ChangeInstructorRequestStatusDto
            {
                RequestId = 1,
                NewStatus = "InvalidStatus"
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _AdminDashboardService.ChangeInstructorRequestStatus(requestStatusDto)
            );

            Assert.Equal("Invalid status. Allowed values are: Pending, Approved, Rejected.", exception.Message);
        }

        [Fact]
        public async Task ChangeInstructorRequestStatus_ShouldThrowRequestIdNotFoundException_WhenRequestIdIsNotFound()
        {
            // Arrange
            var requestStatusDto = new ChangeInstructorRequestStatusDto
            {
                RequestId = 999, // Non-existent RequestId
                NewStatus = "Approved"
            };

            _mockUnitOfWork
                .Setup(uow => uow.ExecuteStoredProcedureAsyncIntReturn(
                    "EXEC ChangeInstructorRequestStatus @InstructorId, @NewStatus",
                    It.Is<SqlParameter>(p => p.ParameterName == "@InstructorId" && (int)p.Value == requestStatusDto.RequestId),
                    It.Is<SqlParameter>(p => p.ParameterName == "@NewStatus" && (string)p.Value == requestStatusDto.NewStatus)
                ))
                .ReturnsAsync(-1); // Simulate RequestId not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<RequestIdnotFoundInInstructorJoinRequestsException>(() =>
                _AdminDashboardService.ChangeInstructorRequestStatus(requestStatusDto)
            );

            Assert.Equal("RequestId not found!", exception.Message);
        }

        [Fact]
        public async Task ChangeInstructorRequestStatus_ShouldCallStoredProcedureWithCorrectParameters()
        {
            // Arrange
            var requestStatusDto = new ChangeInstructorRequestStatusDto
            {
                RequestId = 1,
                NewStatus = "Rejected"
            };

            _mockUnitOfWork
                .Setup(uow => uow.ExecuteStoredProcedureAsyncIntReturn(
                    "EXEC ChangeInstructorRequestStatus @InstructorId, @NewStatus",
                    It.Is<SqlParameter>(p => p.ParameterName == "@InstructorId" && (int)p.Value == requestStatusDto.RequestId),
                    It.Is<SqlParameter>(p => p.ParameterName == "@NewStatus" && (string)p.Value == requestStatusDto.NewStatus)
                ))
                .ReturnsAsync(1); // Simulate 1 row affected

            // Act
            await _AdminDashboardService.ChangeInstructorRequestStatus(requestStatusDto);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.ExecuteStoredProcedureAsyncIntReturn(
                "EXEC ChangeInstructorRequestStatus @InstructorId, @NewStatus",
                It.Is<SqlParameter>(p => p.ParameterName == "@InstructorId" && (int)p.Value == requestStatusDto.RequestId),
                It.Is<SqlParameter>(p => p.ParameterName == "@NewStatus" && (string)p.Value == requestStatusDto.NewStatus)
            ), Times.Once);
        }

        [Fact]
        public async Task RegisterAdminAsync_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var registerAdminRequestDto = new registerAdminRequestDto
            {
                UserName = "testAdmin",
                Email = "testAdmin@example.com",
                PhoneNumber = "1234567890",
                Gender = true,
                DateOfBirth = new System.DateTime(1990, 1, 1),
                Password = "Password123!"
            };

            var user = new AppUser
            {
                UserName = registerAdminRequestDto.UserName,
                Email = registerAdminRequestDto.Email,
                PhoneNumber = registerAdminRequestDto.PhoneNumber,
                Gender = registerAdminRequestDto.Gender,
                DateOfBirth = registerAdminRequestDto.DateOfBirth
            };

            // Mock the behavior of UsersManger.CreateAsync
            _mockUnitOfWork.Setup(u => u.UsersManger.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Mock the behavior of UsersManger.AddToRoleAsync
            _mockUnitOfWork.Setup(u => u.UsersManger.AddToRoleAsync(It.IsAny<AppUser>(), "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            // Mock the behavior of UnitOfWork.Complete
            _mockUnitOfWork.Setup(u => u.Complete()).ReturnsAsync(1); // Ensure it returns Task<int>

            // Act
            var result = await _AdminDashboardService.RegisterAdminAsync(registerAdminRequestDto);

            // Assert
            Assert.True(result.Succeeded); // Ensure the result is successful
            _mockUnitOfWork.Verify(u => u.UsersManger.CreateAsync(
                It.Is<AppUser>(u => u.UserName == "testAdmin"), "Password123!"), Times.Once);
            _mockUnitOfWork.Verify(u => u.UsersManger.AddToRoleAsync(
                It.Is<AppUser>(u => u.UserName == "testAdmin"), "Admin"), Times.Once);
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
        }

        [Fact]
        public async Task RegisterAdminAsync_InvalidRequest_ReturnsFailure()
        {
            // Arrange
            var registerAdminRequestDto = new registerAdminRequestDto
            {
                UserName = "testAdmin",
                Email = "testAdmin@example.com",
                PhoneNumber = "1234567890",
                Gender = true,
                DateOfBirth = new System.DateTime(1990, 1, 1),
                Password = "Password123!"
            };

            var identityError = IdentityResult.Failed(new IdentityError { Code = "Error", Description = "Test Error" });

            // Mock the behavior of UsersManger.CreateAsync to return a failure
            _mockUnitOfWork.Setup(u => u.UsersManger.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(identityError);

            // Act
            var result = await _AdminDashboardService.RegisterAdminAsync(registerAdminRequestDto);

            // Assert
            Assert.False(result.Succeeded); // Ensure the result is not successful
            Assert.Single(result.Errors); // Ensure there is one error

            // Use First() to access the first error in the IEnumerable
            var firstError = result.Errors.First();
            Assert.Equal("Test Error", firstError.Description); // Verify the error description

            _mockUnitOfWork.Verify(u => u.UsersManger.AddToRoleAsync(It.IsAny<AppUser>(), "Admin"), Times.Never); // Ensure AddToRoleAsync is not called
            _mockUnitOfWork.Verify(u => u.Complete(), Times.Never); // Ensure Complete is not called
        }

        [Fact]
        public async Task GetAllApprovedInstructors_ReturnsCorrectData()
        {
            // Arrange
            var expectedInstructors = new List<ReturnAllApprovedInstructorsDto>
            {
                new ReturnAllApprovedInstructorsDto
                {
                    InstructorId = 1,
                    UserName = "johndoe",
                    InstructorFullName = "John Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "1234567890",
                    Specialty = "Mathematics",
                    DateOfBirth = new DateTime(1980, 5, 15),
                    Gender = "Male",
                    FilePath = "path/to/john.jpg"
                },
                new ReturnAllApprovedInstructorsDto
                {
                    InstructorId = 2,
                    UserName = "janesmith",
                    InstructorFullName = "Jane Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "0987654321",
                    Specialty = "Physics",
                    DateOfBirth = new DateTime(1985, 8, 25),
                    Gender = "Female",
                    FilePath = "path/to/jane.jpg"
                }
            };

            _mockUnitOfWork.Setup(u => u.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors"))
                .ReturnsAsync(expectedInstructors);

            // Act
            var result = await _AdminDashboardService.GetAllApprovedInstructors();

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Equal(expectedInstructors.Count, result.Count); // Ensure the count matches

            // Validate the first instructor's properties
            Assert.Equal(expectedInstructors[0].InstructorId, result[0].InstructorId);
            Assert.Equal(expectedInstructors[0].UserName, result[0].UserName);
            Assert.Equal(expectedInstructors[0].InstructorFullName, result[0].InstructorFullName);
            Assert.Equal(expectedInstructors[0].Email, result[0].Email);
            Assert.Equal(expectedInstructors[0].PhoneNumber, result[0].PhoneNumber);
            Assert.Equal(expectedInstructors[0].Specialty, result[0].Specialty);
            Assert.Equal(expectedInstructors[0].DateOfBirth, result[0].DateOfBirth);
            Assert.Equal(expectedInstructors[0].Gender, result[0].Gender);
            Assert.Equal(expectedInstructors[0].FilePath, result[0].FilePath);

            _mockUnitOfWork.Verify(u => u.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors"), Times.Once);
        }

        [Fact]
        public async Task GetAllApprovedInstructors_ReturnsEmptyList_WhenNoData()
        {
            // Arrange
            var expectedInstructors = new List<ReturnAllApprovedInstructorsDto>(); // Empty list

            _mockUnitOfWork.Setup(u => u.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors"))
                .ReturnsAsync(expectedInstructors);

            // Act
            var result = await _AdminDashboardService.GetAllApprovedInstructors();

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Empty(result); // Ensure the result is an empty list

            _mockUnitOfWork.Verify(u => u.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors"), Times.Once);
        }

        [Fact]
        public async Task GetAllApprovedInstructors_ReturnsNull_WhenStoredProcReturnsNull()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors"))
                .ReturnsAsync((List<ReturnAllApprovedInstructorsDto>)null);

            // Act
            var result = await _AdminDashboardService.GetAllApprovedInstructors();

            // Assert
            Assert.Null(result); // Ensure the result is null

            _mockUnitOfWork.Verify(u => u.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors"), Times.Once);
        }

        [Fact]
        public async Task GetAllApprovedInstructors_ThrowsException_WhenStoredProcFails()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors"))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _AdminDashboardService.GetAllApprovedInstructors());

            _mockUnitOfWork.Verify(u => u.ExecuteStoredProcedureAsync<ReturnAllApprovedInstructorsDto>("GetAllApprovedInstructors"), Times.Once);
        }
    }
}
