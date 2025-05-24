using BookingSystem.API.Controllers;
using BookingSystem.Application.Commands;
using BookingSystem.Application.Queries;
using BookingSystem.Core.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Proto = BookingSystem.API.Protos;

namespace BookingSystem.API.Services
{
    public class BookingGrpcService : Proto.BookingService.BookingServiceBase
    {
        private readonly IMediator _mediator;

        public BookingGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Proto.BookResponse> Book(Proto.BookRequest request, ServerCallContext context)
        {
            var command = new CreateBookingCommand(request.MemberEmail, request.InventoryItemName);
            var result = await _mediator.Send(command);

            return new Proto.BookResponse
            {
                Success = result.Success,
                BookingReference = result.BookingReference ?? string.Empty,
                ErrorMessage = result.ErrorMessage ?? string.Empty
            };
        }

        public override async Task<Proto.CancelResponse> Cancel(Proto.CancelRequest request, ServerCallContext context)
        {
            var command = new CancelBookingCommand(request.ReferenceNumber);
            var result = await _mediator.Send(command);

            return new Proto.CancelResponse
            {
                Success = result.Success,
                ErrorMessage = result.ErrorMessage ?? string.Empty
            };
        }

        public override async Task<Proto.ImportResponse> ImportMembers(Proto.ImportRequest request, ServerCallContext context)
        {
            try
            {
                // Convert protobuf bytes to IFormFile
                var memoryStream = new MemoryStream(request.FileContent.ToByteArray());
                var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", request.FileName);

                var result = await _mediator.Send(new ImportMembersCommand(formFile));

                return new Proto.ImportResponse
                {
                    Success = result,
                    Message = result ? "Members imported successfully." : "Failed to import members."
                };
            }
            catch (Exception ex)
            {
                return new Proto.ImportResponse
                {
                    Success = false,
                    Message = $"Error importing members: {ex.Message}"
                };
            }
        }

        public override async Task<Proto.ImportResponse> ImportInventory(Proto.ImportRequest request, ServerCallContext context)
        {
            try
            {
                // Convert protobuf bytes to IFormFile
                var memoryStream = new MemoryStream(request.FileContent.ToByteArray());
                var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", request.FileName);

                var result = await _mediator.Send(new ImportInventoryCommand(formFile));

                return new Proto.ImportResponse
                {
                    Success = result,
                    Message = result ? "Inventory items imported successfully." : "Failed to import inventory items."
                };
            }
            catch (Exception ex)
            {
                return new Proto.ImportResponse
                {
                    Success = false,
                    Message = $"Error importing inventory: {ex.Message}"
                };
            }
        }

        public override async Task<Proto.GetMembersResponse> GetMembers(Proto.GetMembersRequest request, ServerCallContext context)
        {
            var members = await _mediator.Send(new GetMembersQuery());

            var response = new Proto.GetMembersResponse();
            foreach (var member in members)
            {
                response.Members.Add(new Proto.Member
                {
                    Id = member.Id,
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    Email = member.Email,
                    DateJoined = Timestamp.FromDateTime(member.DateJoined.ToUniversalTime()),
                    BookingCount = member.BookingCount,
                    ActiveBookingsCount = member.ActiveBookingsCount
                });
            }

            return response;
        }

        public override async Task<Proto.GetInventoryResponse> GetInventory(Proto.GetInventoryRequest request, ServerCallContext context)
        {
            var items = await _mediator.Send(new GetInventoryItemsQuery());

            var response = new Proto.GetInventoryResponse();
            foreach (var item in items)
            {
                response.Items.Add(new Proto.InventoryItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    ExpirationDate = Timestamp.FromDateTime(item.ExpirationDate.ToUniversalTime()),
                    RemainingCount = item.RemainingCount
                });
            }

            return response;
        }
    }
}
