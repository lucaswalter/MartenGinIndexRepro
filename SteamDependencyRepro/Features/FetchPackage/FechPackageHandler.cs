using SteamDependencyRepro.Events;
using SteamDependencyRepro.Models;

namespace SteamDependencyRepro.Features.FetchPackage;

public static class FetchPackageHandler
{
    public static PackageFetched Handle(FetchPackageCommand command)
    {
        /*
         * Real application makes External API call to fetch package data
         * (see 'example_steam_key_value.json` file for a real-data example)
         */

        // Build event
        return new PackageFetched(
            SteamPackageId: command.SteamPackageId,
            ChangeNumber: 123,
            Data: new SteamKeyValue(
                Name: "root",
                Value: null,
                Children: new List<SteamKeyValue>
                {
                    new SteamKeyValue(
                        Name: "key1",
                        Value: "value1",
                        Children: new List<SteamKeyValue>()
                    ),
                    new SteamKeyValue(
                        Name: "key2",
                        Value: "value2",
                        Children: new List<SteamKeyValue>()
                    )
                }
            )
        );
    }
}