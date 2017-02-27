using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType
{
    Info,
    HomePlanet,
    PlanetDiscovery,
    PlanetProductionQueueEmpty,
    PlanetProductionQueueComplete,
    BuiltMine,
    BuiltFactory,
    BuiltDefense,
    BuiltShip,
    BuiltStarbase,
    FleetOrdersComplete,
    FleetScrapped,
    ColonizeOwnedPlanet,
    ColonizeNonPlanet,
    ColonizeWithNoColonizationModule,
    ColonizeWithNoColonists,
    PlanetColonized,
    GainTechLevel
}