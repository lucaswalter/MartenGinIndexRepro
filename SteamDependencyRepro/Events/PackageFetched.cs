using SteamDependencyRepro.Models;

namespace SteamDependencyRepro.Events;

public record PackageFetched(
    long SteamPackageId,
    uint ChangeNumber,
    SteamKeyValue Data);