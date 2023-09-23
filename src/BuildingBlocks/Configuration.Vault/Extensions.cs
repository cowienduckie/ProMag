using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Hosting;
using Promag.Shared;

namespace Configuration.Vault;

public static class Extensions
{
    public static IHostBuilder UseVault(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((_, cfg) =>
        {
            var options = cfg.Build().GetOptions<VaultOptions>("Vault");
            var enabled = options.Enabled;
            var vaultEnabled = Environment.GetEnvironmentVariable("VAULT_ENABLED")?.ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(vaultEnabled))
            {
                enabled = vaultEnabled is "true" or "1";
            }

            if (!enabled)
            {
                return;
            }

            var client = new VaultStore(options);
            var secret = client.GetDefaultAsync().GetAwaiter().GetResult();

            var source = new MemoryConfigurationSource
            {
                InitialData = secret.ToDictionary(k => k.Key, p => p.Value.ToString())
            };

            cfg.Add(source);
        });

        return hostBuilder;
    }
}