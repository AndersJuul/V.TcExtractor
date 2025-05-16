using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application;

public class FileItemRefresher : IFileItemRefresher
{
    private readonly IFolderScanner _folderScanner;
    private readonly ILogger<FileItemRefresher> _logger;
    private readonly IFileItemRepository _fileItemRepository;

    public FileItemRefresher(IFolderScanner folderScanner,
        IFileItemRepository fileItemRepository, ILogger<FileItemRefresher> logger)
    {
        _folderScanner = folderScanner;
        _logger = logger;
        _fileItemRepository = fileItemRepository;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing File Items.");

        var fileItems = _folderScanner
            .GetFileItems()
            .ToArray();

        _fileItemRepository.DeleteAll();
        _fileItemRepository.AddRange(fileItems);

        _logger.LogInformation("Done Refreshing File Items: " + fileItems.Length);
    }
}