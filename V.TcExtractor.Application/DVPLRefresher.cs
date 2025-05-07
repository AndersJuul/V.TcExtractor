using Microsoft.Extensions.Logging;
using V.TcExtractor.Domain.Refreshers;
using V.TcExtractor.Domain.Repositories;
using V.TcExtractor.Infrastructure.OfficeDocuments;

namespace V.TcExtractor.Application;

public class DVPLRefresher : IDVPLRefresher
{
    private readonly IFolderScanner _folderScanner;
    private readonly IDvplRepository _dvplRepository;
    private readonly ILogger<DVPLRefresher> _logger;

    public DVPLRefresher(IFolderScanner folderScanner, IDvplRepository dvplRepository, ILogger<DVPLRefresher> logger)
    {
        _folderScanner = folderScanner;
        _dvplRepository = dvplRepository;
        _logger = logger;
    }

    public void Execute()
    {
        _logger.LogInformation("Refreshing DVPL Items.");

        var dvplItems = _folderScanner
            .GetDvplItems()
            .ToArray();

        _dvplRepository.DeleteAll();
        _dvplRepository.AddRange(dvplItems);

        _logger.LogInformation("Done Refreshing DVPL Items: " + dvplItems.Length);
    }
}