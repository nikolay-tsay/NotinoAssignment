using DocumentCommon.Enums;

namespace DocumentCommon.Options;

public sealed class StorageOptions
{
    public StorageTypes Type { get; init; }
    public string? HddStoragePath { get; init; }
}