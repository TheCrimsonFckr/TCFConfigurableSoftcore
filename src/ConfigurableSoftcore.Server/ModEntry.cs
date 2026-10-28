using SPTarkov.Server.Core.Models.Utils;
using ConfigurableSoftcore.Server.Config;

namespace ConfigurableSoftcore.Server;

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader)]
public class ModEntry : IOnLoad
{
    private readonly ISptLogger<ModEntry> _logger;
    private readonly SoftcoreConfigService _configService;

    public ModEntry(ISptLogger<ModEntry> logger, SoftcoreConfigService configService)
    {
        _logger = logger;
        _configService = configService;
    }

    public Task OnLoad()
    {
        var cfg = _configService.LoadOrCreate();

        if (_configService.LoadError is { } error)
        {
            _logger.Warning(
                $"[ConfigurableSoftcore] Could not read {_configService.ConfigPath} ({error}). " +
                "Using default settings until the file is fixed and the server is restarted.");
        }

        _logger.Success(
            $"[ConfigurableSoftcore] Loaded. Enabled={cfg.Enabled} DefaultMode={cfg.DefaultMode} " +
            $"ProfileOverrides={cfg.ProfileOverrides.Count}");
        return Task.CompletedTask;
    }
}
