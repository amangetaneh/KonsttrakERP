using System.Text;

namespace ERP.Configuration
{
    public sealed record RuntimeSettings(string ConnectionString, string SigningKey, string[] AllowedOrigins)
    {
        public static RuntimeSettings FromConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("CHANGE_ME", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Set ConnectionStrings__DefaultConnection to a valid SQL Server connection string.");

            var signingKey = configuration["AppSettings:Token"];
            if (string.IsNullOrWhiteSpace(signingKey) || Encoding.UTF8.GetByteCount(signingKey) < 64 || signingKey.Contains("CHANGE_ME", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Set AppSettings__Token to a private signing key of at least 64 UTF-8 bytes.");

            var origins = (configuration["Cors:AllowedOrigins"] ?? "")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (origins.Length == 0 || origins.Any(origin => !Uri.TryCreate(origin, UriKind.Absolute, out var uri) ||
                (uri.Scheme != "http" && uri.Scheme != "https") || uri.AbsolutePath != "/" || origin.EndsWith('/') || uri.Query.Length != 0 || uri.Fragment.Length != 0 || uri.UserInfo.Length != 0))
                throw new InvalidOperationException("Set Cors__AllowedOrigins to comma-separated HTTP(S) origins without trailing slashes.");

            return new RuntimeSettings(connectionString, signingKey, origins);
        }
    }
}
