using BookingSystem.Core.Interfaces;
using MediatR;
using BookingSystem.Application.Commands;

namespace BookingSystem.Application.Handlers
{
    public class ImportInventoryCommandHandler : IRequestHandler<ImportInventoryCommand, bool>
    {
        private readonly IFileImporter _fileImporter;

        public ImportInventoryCommandHandler(IFileImporter fileImporter)
        {
            _fileImporter = fileImporter;
        }

        public async Task<bool> Handle(ImportInventoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var stream = request.File.OpenReadStream();
                await _fileImporter.ImportInventoryAsync(stream);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
