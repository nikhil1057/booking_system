using BookingSystem.Core.Interfaces;
using MediatR;
using BookingSystem.Application.Commands;

namespace BookingSystem.Application.Handlers
{
    public class ImportMembersCommandHandler : IRequestHandler<ImportMembersCommand, bool>
    {
        private readonly IFileImporter _fileImporter;

        public ImportMembersCommandHandler(IFileImporter fileImporter)
        {
            _fileImporter = fileImporter;
        }

        public async Task<bool> Handle(ImportMembersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var stream = request.File.OpenReadStream();
                await _fileImporter.ImportMembersAsync(stream);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
