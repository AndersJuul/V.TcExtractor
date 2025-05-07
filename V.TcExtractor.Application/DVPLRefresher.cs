using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application;

public class DVPLRefresher : IDVPLRefresher
{
    private readonly IFolderScanner _folderScanner;
    private readonly IDvplItemRepository _dvplItemRepository;
    private readonly ILogger<DVPLRefresher> _logger;

    public DVPLRefresher(IFolderScanner folderScanner, IDvplItemRepository dvplItemRepository,
        ILogger<DVPLRefresher> logger)
    {
        _folderScanner = folderScanner;
        _dvplItemRepository = dvplItemRepository;
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing DVPL Items.");

        var dvplItems = _folderScanner
            .GetDvplItems()
            .ToArray();

        _dvplItemRepository.DeleteAll();
        _dvplItemRepository.AddRange(dvplItems);

        _logger.LogInformation("Done Refreshing DVPL Items: " + dvplItems.Length);
    }
}