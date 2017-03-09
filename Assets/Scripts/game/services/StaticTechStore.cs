using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * The current implementation of the TechStore is a staticly created list.  Each tech is created when the class is loaded.
 * This could be switched out for a config file or for the database at a future date.  For now, staticly creating them is simple
 * until the tech tree gets locked down.
 */
[System.Serializable]
public class StaticTechStore : MonoBehaviour, TechStore
{
    [SerializeField]
    private static Dictionary<string, Tech> techs = new Dictionary<string, Tech>();
    [SerializeField]
    private static Dictionary<TechCategory, List<Tech>> techsForCategory = new Dictionary<TechCategory, List<Tech>>();
    [SerializeField]
    private static List<TechEngine> engines = new List<TechEngine>();
    [SerializeField]
    private static List<TechHullComponent> armor = new List<TechHullComponent>();
    [SerializeField]
    private static List<TechHullComponent> scanners = new List<TechHullComponent>();
    [SerializeField]
    private static List<TechHullComponent> shields = new List<TechHullComponent>();
    [SerializeField]
    private static List<TechHullComponent> torpedos = new List<TechHullComponent>();
    [SerializeField]
    private static List<TechHullComponent> beamWeapons = new List<TechHullComponent>();
    [SerializeField]
    private static List<TechPlanetaryScanner> planetaryScanners = new List<TechPlanetaryScanner>();
    [SerializeField]
    private static List<TechDefence> defenses = new List<TechDefence>();
    [SerializeField]
    private static List<TechHull> hulls = new List<TechHull>();
    [SerializeField]
    private static List<TechHull> starbases = new List<TechHull>();

    [SerializeField]
    private volatile static TechStore instance;
    
    public static TechStore getInstance()
    {
        return instance;
    }

    public Tech get(string name)
    {
        return techs[name];
    }

    public TechHullComponent getHullComponent(string name)
    {
        return (TechHullComponent)techs[name];
    }

    public TechHull getHull(string name)
    {
        return (TechHull)techs[name];
    }

    public TechEngine getEngine(string name)
    {
        return (TechEngine)techs[name];
    }

    public List<Tech> getAll()
    {
        return new List<Tech>(techs.Values);
    }

    public void initShipDesign(ShipDesign design)
    {

        // fill in the TechHull
        TechHull hull = getHull(design.getHullName());
        design.setHull(hull);
        for (int slotIndex = 0; slotIndex < hull.getSlots().Count; slotIndex++)
        {
            // Fill in the TechHullSlot for this index 
            ShipDesignSlot designSlot = design.getSlots()[slotIndex];
            designSlot.setHullSlot(hull.getSlots()[slotIndex]);
            designSlot.setHullComponent(getHullComponent(designSlot.getHullComponentName()));
        }

        // fill in the aggregate engine
        design.getAggregate().setEngine(getEngine(design.getAggregate().getEngineName()));
    }

    /**
     * Initialize all the techs in the tech store
     */
    private void Awake()
    {

        DontDestroyOnLoad(this);

        // engines
        instance = this;
        TechEngine techEngine = (TechEngine)new TechEngine("Galaxy Scoop", new Cost(4, 2, 9, 12), new TechRequirements().energy(5).propulsion(20).lrtsRequired(LRT.IFE).lrtsDenied(LRT.NRSE), 130, TechCategory.Engine).mass(8).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 0;
        techEngine.getFuelUsage()[2] = 0;
        techEngine.getFuelUsage()[3] = 0;
        techEngine.getFuelUsage()[4] = 0;
        techEngine.getFuelUsage()[5] = 0;
        techEngine.getFuelUsage()[6] = 0;
        techEngine.getFuelUsage()[7] = 0;
        techEngine.getFuelUsage()[8] = 0;
        techEngine.getFuelUsage()[9] = 60;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Interspace-10", new Cost(18, 25, 10, 60), new TechRequirements().propulsion(11).lrtsRequired(LRT.NRSE), 140, TechCategory.Engine).mass(25).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 10;
        techEngine.getFuelUsage()[2] = 30;
        techEngine.getFuelUsage()[3] = 40;
        techEngine.getFuelUsage()[4] = 50;
        techEngine.getFuelUsage()[5] = 60;
        techEngine.getFuelUsage()[6] = 70;
        techEngine.getFuelUsage()[7] = 80;
        techEngine.getFuelUsage()[8] = 90;
        techEngine.getFuelUsage()[9] = 100;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Settler's Delight", new Cost(1, 0, 1, 2), new TechRequirements().prtRequired(PRT.HE), 10, TechCategory.Engine).mass(2).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 0;
        techEngine.getFuelUsage()[2] = 0;
        techEngine.getFuelUsage()[3] = 0;
        techEngine.getFuelUsage()[4] = 0;
        techEngine.getFuelUsage()[5] = 0;
        techEngine.getFuelUsage()[6] = 150;
        techEngine.getFuelUsage()[7] = 275;
        techEngine.getFuelUsage()[8] = 480;
        techEngine.getFuelUsage()[9] = 576;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Sub-Galactic Fuel Scoop", new Cost(4, 4, 7, 12), new TechRequirements().energy(2).propulsion(8).lrtsDenied(LRT.NRSE), 160, TechCategory.Engine).mass(20).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 0;
        techEngine.getFuelUsage()[2] = 0;
        techEngine.getFuelUsage()[3] = 0;
        techEngine.getFuelUsage()[4] = 0;
        techEngine.getFuelUsage()[5] = 85;
        techEngine.getFuelUsage()[6] = 105;
        techEngine.getFuelUsage()[7] = 210;
        techEngine.getFuelUsage()[8] = 380;
        techEngine.getFuelUsage()[9] = 456;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Trans-Galactic Drive", new Cost(20, 20, 9, 50), new TechRequirements().propulsion(9), 90, TechCategory.Engine).mass(25).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 15;
        techEngine.getFuelUsage()[2] = 35;
        techEngine.getFuelUsage()[3] = 45;
        techEngine.getFuelUsage()[4] = 55;
        techEngine.getFuelUsage()[5] = 70;
        techEngine.getFuelUsage()[6] = 80;
        techEngine.getFuelUsage()[7] = 90;
        techEngine.getFuelUsage()[8] = 100;
        techEngine.getFuelUsage()[9] = 120;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Trans-Galactic Fuel Scoop", new Cost(5, 4, 12, 18), new TechRequirements().energy(3).propulsion(9).lrtsDenied(LRT.NRSE), 100, TechCategory.Engine).mass(19).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 0;
        techEngine.getFuelUsage()[2] = 0;
        techEngine.getFuelUsage()[3] = 0;
        techEngine.getFuelUsage()[4] = 0;
        techEngine.getFuelUsage()[5] = 0;
        techEngine.getFuelUsage()[6] = 88;
        techEngine.getFuelUsage()[7] = 100;
        techEngine.getFuelUsage()[8] = 145;
        techEngine.getFuelUsage()[9] = 174;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Trans-Galactic Mizer Scoop", new Cost(5, 2, 13, 11), new TechRequirements().energy(4).propulsion(16).lrtsDenied(LRT.NRSE), 110, TechCategory.Engine).mass(11).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 0;
        techEngine.getFuelUsage()[2] = 0;
        techEngine.getFuelUsage()[3] = 0;
        techEngine.getFuelUsage()[4] = 0;
        techEngine.getFuelUsage()[5] = 0;
        techEngine.getFuelUsage()[6] = 0;
        techEngine.getFuelUsage()[7] = 0;
        techEngine.getFuelUsage()[8] = 70;
        techEngine.getFuelUsage()[9] = 84;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Trans-Galactic Super Scoop", new Cost(6, 4, 16, 24), new TechRequirements().energy(4).propulsion(12).lrtsDenied(LRT.NRSE), 120, TechCategory.Engine).mass(18).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 0;
        techEngine.getFuelUsage()[2] = 0;
        techEngine.getFuelUsage()[3] = 0;
        techEngine.getFuelUsage()[4] = 0;
        techEngine.getFuelUsage()[5] = 0;
        techEngine.getFuelUsage()[6] = 0;
        techEngine.getFuelUsage()[7] = 65;
        techEngine.getFuelUsage()[8] = 90;
        techEngine.getFuelUsage()[9] = 108;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Trans-Star 10", new Cost(3, 0, 3, 10), new TechRequirements().propulsion(23), 150, TechCategory.Engine).mass(5).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 5;
        techEngine.getFuelUsage()[2] = 15;
        techEngine.getFuelUsage()[3] = 20;
        techEngine.getFuelUsage()[4] = 25;
        techEngine.getFuelUsage()[5] = 30;
        techEngine.getFuelUsage()[6] = 35;
        techEngine.getFuelUsage()[7] = 40;
        techEngine.getFuelUsage()[8] = 45;
        techEngine.getFuelUsage()[9] = 50;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Quick Jump 5", new Cost(3, 0, 1, 3), new TechRequirements(), 20, TechCategory.Engine).mass(4).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 25;
        techEngine.getFuelUsage()[2] = 100;
        techEngine.getFuelUsage()[3] = 100;
        techEngine.getFuelUsage()[4] = 100;
        techEngine.getFuelUsage()[5] = 180;
        techEngine.getFuelUsage()[6] = 500;
        techEngine.getFuelUsage()[7] = 800;
        techEngine.getFuelUsage()[8] = 900;
        techEngine.getFuelUsage()[9] = 1080;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Long Hump 6", new Cost(5, 0, 1, 6), new TechRequirements().propulsion(3), 30, TechCategory.Engine).mass(9).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 20;
        techEngine.getFuelUsage()[2] = 60;
        techEngine.getFuelUsage()[3] = 100;
        techEngine.getFuelUsage()[4] = 100;
        techEngine.getFuelUsage()[5] = 105;
        techEngine.getFuelUsage()[6] = 450;
        techEngine.getFuelUsage()[7] = 750;
        techEngine.getFuelUsage()[8] = 900;
        techEngine.getFuelUsage()[9] = 1080;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Fuel Mizer", new Cost(8, 0, 0, 11), new TechRequirements().propulsion(2).lrtsRequired(LRT.IFE), 40, TechCategory.Engine).mass(6).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 0;
        techEngine.getFuelUsage()[2] = 0;
        techEngine.getFuelUsage()[3] = 0;
        techEngine.getFuelUsage()[4] = 35;
        techEngine.getFuelUsage()[5] = 120;
        techEngine.getFuelUsage()[6] = 175;
        techEngine.getFuelUsage()[7] = 235;
        techEngine.getFuelUsage()[8] = 360;
        techEngine.getFuelUsage()[9] = 420;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Radiating Hydro-Ram Scoop", new Cost(3, 2, 9, 8), new TechRequirements().energy(2).propulsion(6).lrtsDenied(LRT.NRSE), 50, TechCategory.Engine).mass(10).radiating(true).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 0;
        techEngine.getFuelUsage()[2] = 0;
        techEngine.getFuelUsage()[3] = 0;
        techEngine.getFuelUsage()[4] = 0;
        techEngine.getFuelUsage()[5] = 0;
        techEngine.getFuelUsage()[6] = 165;
        techEngine.getFuelUsage()[7] = 375;
        techEngine.getFuelUsage()[8] = 600;
        techEngine.getFuelUsage()[9] = 720;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Daddy Long Legs 7", new Cost(11, 0, 3, 12), new TechRequirements().propulsion(5), 60, TechCategory.Engine).mass(13).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 20;
        techEngine.getFuelUsage()[2] = 60;
        techEngine.getFuelUsage()[3] = 70;
        techEngine.getFuelUsage()[4] = 100;
        techEngine.getFuelUsage()[5] = 100;
        techEngine.getFuelUsage()[6] = 110;
        techEngine.getFuelUsage()[7] = 600;
        techEngine.getFuelUsage()[8] = 750;
        techEngine.getFuelUsage()[9] = 900;
        techs.Add(techEngine.getName(), techEngine);
        techEngine = (TechEngine)new TechEngine("Alpha Drive 8", new Cost(16, 0, 3, 28), new TechRequirements().propulsion(7), 70, TechCategory.Engine).mass(17).hullSlotType(HullSlotType.Engine);
        techEngine.getFuelUsage()[0] = 0;
        techEngine.getFuelUsage()[1] = 15;
        techEngine.getFuelUsage()[2] = 50;
        techEngine.getFuelUsage()[3] = 60;
        techEngine.getFuelUsage()[4] = 70;
        techEngine.getFuelUsage()[5] = 100;
        techEngine.getFuelUsage()[6] = 100;
        techEngine.getFuelUsage()[7] = 115;
        techEngine.getFuelUsage()[8] = 700;
        techEngine.getFuelUsage()[9] = 840;
        techs.Add(techEngine.getName(), techEngine);

        // enigma pulsar        [0,0,0,0,0,65,75,85,95,105],

        // Defenses
        Tech tech = new TechDefence("Missile Battery", new Cost(5, 5, 5, 15), new TechRequirements(), 0, TechCategory.PlanetaryDefense).defenseCoverage(199);
        techs.Add(tech.getName(), tech);
        tech = new TechDefence("SDI", new Cost(5, 5, 5, 15), new TechRequirements(), 100, TechCategory.PlanetaryDefense).defenseCoverage(99);
        techs.Add(tech.getName(), tech);

        // Planetary Scanners
        tech = new TechPlanetaryScanner("Viewer 50", new Cost(10, 10, 70, 100), new TechRequirements(), 0, TechCategory.PlanetaryScanner).scanRange(50).scanRangePen(0);
        techs.Add(tech.getName(), tech);
        tech = new TechPlanetaryScanner("Viewer 90", new Cost(0, 0, 0, 0), new TechRequirements().electronics(1), 10, TechCategory.PlanetaryScanner).scanRange(90).scanRangePen(0);
        techs.Add(tech.getName(), tech);
        tech = new TechPlanetaryScanner("Scoper 150", new Cost(10, 10, 70, 100), new TechRequirements().electronics(3), 30, TechCategory.PlanetaryScanner).scanRange(150).scanRangePen(0);
        techs.Add(tech.getName(), tech);
        tech = new TechPlanetaryScanner("Scoper 220", new Cost(10, 10, 70, 100), new TechRequirements().electronics(6), 40, TechCategory.PlanetaryScanner).scanRange(220).scanRangePen(0);
        techs.Add(tech.getName(), tech);
        tech = new TechPlanetaryScanner("Scoper 280", new Cost(10, 10, 70, 100), new TechRequirements().electronics(8), 50, TechCategory.PlanetaryScanner).scanRange(280).scanRangePen(0);
        techs.Add(tech.getName(), tech);
        tech = new TechPlanetaryScanner("Snooper 320X", new Cost(10, 10, 70, 100), new TechRequirements().energy(3).electronics(10).biotechnology(3), 60, TechCategory.PlanetaryScanner).scanRange(320).scanRangePen(160);
        techs.Add(tech.getName(), tech);
        tech = new TechPlanetaryScanner("Snooper 400X", new Cost(10, 10, 70, 100), new TechRequirements().energy(4).electronics(13).biotechnology(6), 70, TechCategory.PlanetaryScanner).scanRange(400).scanRangePen(200);
        techs.Add(tech.getName(), tech);
        tech = new TechPlanetaryScanner("Snooper 500X", new Cost(10, 10, 70, 100), new TechRequirements().energy(5).electronics(16).biotechnology(7), 80, TechCategory.PlanetaryScanner).scanRange(500).scanRangePen(250);
        techs.Add(tech.getName(), tech);
        tech = new TechPlanetaryScanner("Snooper 620X", new Cost(10, 10, 70, 100), new TechRequirements().energy(7).electronics(23).biotechnology(9), 90, TechCategory.PlanetaryScanner).scanRange(620).scanRangePen(310);
        techs.Add(tech.getName(), tech);

        // Hulls
        TechHull hull = new TechHull("Small Freighter", new Cost(11, 0, 15, 18), new TechRequirements(), 10, TechCategory.ShipHull).mass(25).armor(25).fuelCapacity(130);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Armor, HullSlotType.Shield), 1, false, 160, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Scanner, HullSlotType.Electrical, HullSlotType.Mechanical), 1, false, 224, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 1, true, 32, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Cargo), 70, false, 96, 96, 64, 64));
        techs.Add(hull.getName(), hull);
        hull = new TechHull("Medium Freighter", new Cost(5, 0, 5, 10), new TechRequirements(), 20, TechCategory.ShipHull).mass(60).armor(50).fuelCapacity(450);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 1, true, 32, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Cargo), 210, false, 96, 96, 128, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Armor, HullSlotType.Shield), 1, false, 224, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Scanner, HullSlotType.Electrical, HullSlotType.Mechanical), 1, false, 288, 96, 64, 64));
        techs.Add(hull.getName(), hull);
        hull = new TechHull("Large Freighter", new Cost(10, 0, 6, 28), new TechRequirements().construction(8), 30, TechCategory.ShipHull).mass(125).armor(150).fuelCapacity(2600);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 2, false, 32, 64, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Cargo), 1200, false, 96, 32, 128, 128));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Scanner, HullSlotType.Electrical, HullSlotType.Mechanical), 2, false, 224, 32, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Armor, HullSlotType.Shield), 2, false, 224, 96, 64, 64));
        techs.Add(hull.getName(), hull);

        // super freighter

        hull = new TechHull("Scout", new Cost(4, 2, 4, 9), new TechRequirements(), 50, TechCategory.ShipHull).mass(8).builtInScannerForJoaT(true).armor(20).initiative(1).fuelCapacity(50);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 1, true, 64, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Scanner), 1, false, 192, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Scanner, HullSlotType.Mechanical, HullSlotType.Electrical, HullSlotType.Shield, HullSlotType.Armor, HullSlotType.Weapon, HullSlotType.Mine), 1, false, 128, 96, 64, 64));
        techs.Add(hull.getName(), hull);
        hull = new TechHull("Frigate", new Cost(1, 1, 1, 3), new TechRequirements().construction(6), 60, TechCategory.ShipHull).mass(8).builtInScannerForJoaT(true).armor(45).fuelCapacity(125);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 1, true, 32, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Armor, HullSlotType.Shield), 2, false, 96, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Scanner, HullSlotType.Mechanical, HullSlotType.Electrical, HullSlotType.Shield, HullSlotType.Armor, HullSlotType.Weapon, HullSlotType.Mine), 3, false, 160, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Scanner), 2, false, 224, 96, 64, 64));
        techs.Add(hull.getName(), hull);

        // Destroyer
        // Cruiser
        // Battle Cruiser
        // Battleship
        hull = new TechHull("Dreadnought", new Cost(130, 40, 25, 275), new TechRequirements().construction(16).prtRequired(PRT.WM), 110, TechCategory.ShipHull).mass(250).armor(4500).fuelCapacity(4000).initiative(10);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 5, true, 32, 96, 64, 64));
        techs.Add(hull.getName(), hull);

        // Privateer
        // Rogue
        // Galleon
        hull = new TechHull("Mini-Colony Ship", new Cost(2, 0, 2, 3), new TechRequirements().prtRequired(PRT.HE), 150, TechCategory.ShipHull).mass(8).armor(10).fuelCapacity(150).initiative(0);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 1, true, 64, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Cargo), 10, false, 128, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Mechanical), 1, false, 192, 96, 64, 64));
        techs.Add(hull.getName(), hull);

        hull = new TechHull("Colony Ship", new Cost(9, 0, 13, 18), new TechRequirements(), 160, TechCategory.ShipHull).mass(20).armor(20).fuelCapacity(200);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 1, true, 64, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Cargo), 25, false, 128, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Mechanical), 1, false, 192, 96, 64, 64));
        techs.Add(hull.getName(), hull);

        hull = new TechHull("Mini Bomber", new Cost(18, 5, 9, 32), new TechRequirements().construction(1), 170, TechCategory.ShipHull).mass(28).armor(50).fuelCapacity(120);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 1, true, 64, 64, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Bomb), 2, false, 128, 64, 64, 64));
        techs.Add(hull.getName(), hull);

        hull = new TechHull("Orbital Fort", new Cost(11, 0, 15, 35), new TechRequirements(), 10, TechCategory.StarbaseHull).mass(0).armor(100).initiative(10).starbase(true);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Weapon), 12, false, 128, 32, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Armor, HullSlotType.Shield), 12, false, 64, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Orbital, HullSlotType.Electrical), 1, false, 128, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Armor, HullSlotType.Shield), 12, false, 192, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Weapon), 12, false, 128, 160, 64, 64));
        techs.Add(hull.getName(), hull);

        hull = new TechHull("Space Station", new Cost(106, 71, 220, 528), new TechRequirements(), 20, TechCategory.StarbaseHull).mass(0).armor(500).initiative(14).starbase(true);
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Engine), 1, true, 64, 64, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.SpaceDock), 0, false, 160, 128, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Orbital, HullSlotType.Electrical), 1, false, 96, 128, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Orbital, HullSlotType.Electrical), 1, false, 224, 128, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Electrical), 3, false, 160, 64, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Electrical), 3, false, 160, 192, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Shield), 16, false, 96, 32, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Weapon), 16, false, 224, 32, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Armor, HullSlotType.Shield), 16, false, 288, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Weapon), 16, false, 288, 160, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Armor, HullSlotType.Shield), 16, false, 32, 160, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Weapon), 16, false, 32, 96, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Weapon), 16, false, 96, 224, 64, 64));
        hull.getSlots().Add(new TechHullSlot(EnumSet<HullSlotType>.of(HullSlotType.Shield), 16, false, 224, 224, 64, 64));
        techs.Add(hull.getName(), hull);

        // Hull Components
        addTechHullComponent(new TechHullComponent("Mass Driver 5", new Cost(24, 20, 20, 70), new TechRequirements().energy(4).prtRequired(PRT.PP), 0, TechCategory.Orbital)).mass(0).packetSpeed(5).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Mass Driver 6", new Cost(24, 20, 20, 144), new TechRequirements().energy(7).prtRequired(PRT.PP), 0, TechCategory.Orbital)).mass(0).packetSpeed(6).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Mass Driver 7", new Cost(100, 100, 100, 512), new TechRequirements().energy(9), 0, TechCategory.Orbital)).mass(0).packetSpeed(7).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Super Driver 8", new Cost(24, 20, 20, 256), new TechRequirements().energy(11).prtRequired(PRT.PP), 0, TechCategory.Orbital)).mass(0).packetSpeed(8).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Super Driver 9", new Cost(24, 20, 20, 324), new TechRequirements().energy(13).prtRequired(PRT.PP), 0, TechCategory.Orbital)).mass(0).packetSpeed(9).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Ultra Driver 10", new Cost(100, 100, 100, 968), new TechRequirements().energy(15), 0, TechCategory.Orbital)).mass(0).packetSpeed(10).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Ultra Driver 11", new Cost(24, 20, 20, 484), new TechRequirements().energy(17).prtRequired(PRT.PP), 0, TechCategory.Orbital)).mass(0).packetSpeed(11).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Ultra Driver 12", new Cost(24, 20, 20, 576), new TechRequirements().energy(20).prtRequired(PRT.PP), 0, TechCategory.Orbital)).mass(0).packetSpeed(12).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Ultra Driver 13", new Cost(24, 20, 20, 676), new TechRequirements().energy(24).prtRequired(PRT.PP), 0, TechCategory.Orbital)).mass(0).packetSpeed(13).hullSlotType(HullSlotType.Orbital);

        // Stargates
        addTechHullComponent(new TechHullComponent("Stargate 100-250", new Cost(50, 20, 20, 200), new TechRequirements().propulsion(5).construction(5), 0, TechCategory.Orbital)).mass(0).maxRange(1250).safeHullMass(100).safeRange(250).hullSlotType(HullSlotType.Orbital).maxHullMass(500);
        addTechHullComponent(new TechHullComponent("Stargate 100-any", new Cost(50, 20, 20, 700), new TechRequirements().propulsion(16).construction(12).prtRequired(PRT.IT), 0, TechCategory.Orbital)).mass(0).safeHullMass(100).hullSlotType(HullSlotType.Orbital).maxHullMass(500);
        addTechHullComponent(new TechHullComponent("Stargate 150-600", new Cost(50, 20, 20, 500), new TechRequirements().propulsion(11).construction(7), 0, TechCategory.Orbital)).mass(0).maxRange(3000).safeHullMass(150).safeRange(600).hullSlotType(HullSlotType.Orbital).maxHullMass(750);
        addTechHullComponent(new TechHullComponent("Stargate 300-500", new Cost(50, 20, 20, 600), new TechRequirements().propulsion(9).construction(13), 0, TechCategory.Orbital)).mass(0).maxRange(2500).safeHullMass(300).safeRange(500).hullSlotType(HullSlotType.Orbital).maxHullMass(1500);
        addTechHullComponent(new TechHullComponent("Stargate any-300", new Cost(50, 20, 20, 250), new TechRequirements().propulsion(6).construction(10).prtRequired(PRT.IT), 0, TechCategory.Orbital)).mass(0).maxRange(1500).safeRange(300).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Stargate any-800", new Cost(50, 20, 20, 700), new TechRequirements().propulsion(12).construction(18).prtRequired(PRT.IT), 0, TechCategory.Orbital)).mass(0).maxRange(4000).safeRange(800).hullSlotType(HullSlotType.Orbital);
        addTechHullComponent(new TechHullComponent("Stargate any-any", new Cost(50, 20, 20, 800), new TechRequirements().propulsion(19).construction(24).prtRequired(PRT.IT), 0, TechCategory.Orbital)).mass(0).hullSlotType(HullSlotType.Orbital);

        // Miners
        addTechHullComponent(new TechHullComponent("Orbital Adjuster", new Cost(25, 25, 25, 50), new TechRequirements().biotechnology(6).prtRequired(PRT.CA), 0, TechCategory.MineRobot)).mass(80).cloak(25).terraformRate(1).hullSlotType(HullSlotType.Mining);
        addTechHullComponent(new TechHullComponent("Robo-Miner", new Cost(30, 0, 7, 100), new TechRequirements().construction(4).electronics(2).lrtsDenied(LRT.OBRM), 0, TechCategory.MineRobot)).mass(240).miningRate(12).hullSlotType(HullSlotType.Mining);
        addTechHullComponent(new TechHullComponent("Robo-Maxi-Miner", new Cost(30, 0, 7, 100), new TechRequirements().construction(7).electronics(4).lrtsDenied(LRT.OBRM), 0, TechCategory.MineRobot)).mass(240).miningRate(18).hullSlotType(HullSlotType.Mining);
        addTechHullComponent(new TechHullComponent("Robo-Midget-Miner", new Cost(12, 0, 4, 44), new TechRequirements().lrtsRequired(LRT.ARM), 0, TechCategory.MineRobot)).mass(80).miningRate(5).hullSlotType(HullSlotType.Mining);
        addTechHullComponent(new TechHullComponent("Robo-Mini-Miner", new Cost(29, 0, 7, 96), new TechRequirements().construction(2).electronics(1), 0, TechCategory.MineRobot)).mass(240).miningRate(4).hullSlotType(HullSlotType.Mining);
        addTechHullComponent(new TechHullComponent("Robo-Super-Miner", new Cost(30, 0, 7, 100), new TechRequirements().construction(12).electronics(6).lrtsDenied(LRT.OBRM), 0, TechCategory.MineRobot)).mass(240).miningRate(27).hullSlotType(HullSlotType.Mining);
        addTechHullComponent(new TechHullComponent("Robo-Ultra-Miner", new Cost(14, 0, 4, 100), new TechRequirements().construction(15).electronics(8).lrtsRequired(LRT.ARM).lrtsDenied(LRT.OBRM), 0, TechCategory.MineRobot)).mass(80).miningRate(25).hullSlotType(HullSlotType.Mining);

        // Bombs
        addTechHullComponent(new TechHullComponent("Lady Finger Bomb", new Cost(1, 19, 0, 5), new TechRequirements().weapons(2), 0, TechCategory.Bomb)).mass(40).minKillRate(300).structureKillRate(2).killRate(6).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("Black Cat Bomb", new Cost(1, 22, 0, 7), new TechRequirements().weapons(5), 10, TechCategory.Bomb)).mass(45).minKillRate(300).structureKillRate(4).killRate(9).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("M-70 Bomb", new Cost(1, 24, 0, 9), new TechRequirements().weapons(8), 20, TechCategory.Bomb)).mass(50).minKillRate(300).structureKillRate(6).killRate(12).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("M-80 Bomb", new Cost(1, 25, 0, 12), new TechRequirements().weapons(11), 30, TechCategory.Bomb)).mass(55).minKillRate(300).structureKillRate(7).killRate(17).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("Cherry Bomb", new Cost(1, 25, 0, 11), new TechRequirements().weapons(14), 40, TechCategory.Bomb)).mass(52).minKillRate(300).structureKillRate(10).killRate(25).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("LBU-17 Bomb", new Cost(1, 15, 15, 7), new TechRequirements().weapons(5).electronics(8), 50, TechCategory.Bomb)).mass(30).structureKillRate(16).killRate(2).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("LBU-32 Bomb", new Cost(1, 24, 15, 10), new TechRequirements().weapons(10).electronics(10), 60, TechCategory.Bomb)).mass(35).structureKillRate(28).killRate(3).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("LBU-74 Bomb", new Cost(1, 33, 12, 14), new TechRequirements().weapons(15).electronics(12), 70, TechCategory.Bomb)).mass(45).structureKillRate(45).killRate(4).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("Retro Bomb", new Cost(15, 15, 10, 50), new TechRequirements().weapons(10).biotechnology(12).prtRequired(PRT.CA), 80, TechCategory.Bomb)).mass(45).unterraformRate(1).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("Smart Bomb", new Cost(1, 22, 0, 27), new TechRequirements().weapons(5).biotechnology(7), 90, TechCategory.Bomb)).mass(50).smart(true).killRate(13).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("Neutron Bomb", new Cost(1, 30, 0, 30), new TechRequirements().weapons(10).biotechnology(10), 110, TechCategory.Bomb)).mass(57).smart(true).killRate(22).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("Enriched Neutron Bomb", new Cost(1, 36, 0, 25), new TechRequirements().weapons(15).biotechnology(12), 120, TechCategory.Bomb)).mass(64).smart(true).killRate(35).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("Peerless Bomb", new Cost(1, 33, 0, 32), new TechRequirements().weapons(22).biotechnology(15), 130, TechCategory.Bomb)).mass(55).smart(true).killRate(50).hullSlotType(HullSlotType.Bomb);
        addTechHullComponent(new TechHullComponent("Annihilator Bomb", new Cost(1, 30, 0, 28), new TechRequirements().weapons(26).biotechnology(17), 140, TechCategory.Bomb)).mass(50).smart(true).killRate(70).hullSlotType(HullSlotType.Bomb);

        // Scanners
        addTechHullComponent(new TechHullComponent("Bat Scanner", new Cost(1, 0, 1, 1), new TechRequirements(), 10, TechCategory.Scanner)).mass(2).scanRange(1).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Rhino Scanner", new Cost(3, 0, 2, 3), new TechRequirements().electronics(1), 20, TechCategory.Scanner)).mass(5).scanRange(50).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Mole Scanner", new Cost(2, 0, 2, 9), new TechRequirements().electronics(4), 30, TechCategory.Scanner)).mass(2).scanRange(100).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Pick Pocket Scanner", new Cost(8, 10, 6, 35), new TechRequirements().energy(4).electronics(4).biotechnology(4).prtRequired(PRT.SS), 40, TechCategory.Scanner)).mass(15).stealCargo(true).scanRange(80).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Possum Scanner", new Cost(3, 0, 3, 18), new TechRequirements().electronics(5), 50, TechCategory.Scanner)).mass(3).scanRange(150).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("DNA Scanner", new Cost(1, 1, 1, 5), new TechRequirements().propulsion(3).biotechnology(6), 60, TechCategory.Scanner)).mass(2).scanRange(125).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Chameleon Scanner", new Cost(4, 6, 4, 25), new TechRequirements().energy(3).electronics(6).prtRequired(PRT.SS), 70, TechCategory.Scanner)).mass(6).scanRange(160).cloak(2).scanRangePen(45).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Ferret Scanner", new Cost(2, 0, 8, 36), new TechRequirements().energy(3).electronics(7).biotechnology(2).lrtsDenied(LRT.NAS), 80, TechCategory.Scanner)).mass(6).scanRange(185).scanRangePen(50).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("RNA Scanner", new Cost(1, 1, 2, 20), new TechRequirements().propulsion(5).biotechnology(10), 90, TechCategory.Scanner)).mass(2).scanRange(230).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Gazelle Scanner", new Cost(4, 0, 5, 24), new TechRequirements().energy(4).electronics(8), 100, TechCategory.Scanner)).mass(5).scanRange(225).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Dolphin Scanner", new Cost(5, 5, 10, 40), new TechRequirements().energy(5).electronics(10).biotechnology(4).lrtsDenied(LRT.NAS), 110, TechCategory.Scanner)).mass(4).scanRange(220).scanRangePen(100).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Cheetah Scanner", new Cost(3, 1, 13, 50), new TechRequirements().energy(5).electronics(11), 115, TechCategory.Scanner)).mass(4).scanRange(275).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Eagle Eye Scanner", new Cost(3, 2, 21, 64), new TechRequirements().energy(6).electronics(14), 120, TechCategory.Scanner)).mass(3).scanRange(335).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Elephant Scanner", new Cost(8, 5, 14, 70), new TechRequirements().energy(6).electronics(16).biotechnology(7).lrtsDenied(LRT.NAS), 130, TechCategory.Scanner)).mass(6).scanRange(300).scanRangePen(200).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Peerless Scanner", new Cost(3, 2, 30, 90), new TechRequirements().energy(7).electronics(24), 140, TechCategory.Scanner)).mass(4).scanRange(500).hullSlotType(HullSlotType.Scanner);
        addTechHullComponent(new TechHullComponent("Robber Baron Scanner", new Cost(10, 10, 10, 90), new TechRequirements().energy(10).electronics(15).biotechnology(10).prtRequired(PRT.SS), 160, TechCategory.Scanner)).mass(20).stealCargo(true).scanRange(220).scanRangePen(120).hullSlotType(HullSlotType.Scanner);

        // Armor
        addTechHullComponent(new TechHullComponent("Tritanium", new Cost(5, 0, 0, 9), new TechRequirements(), 10, TechCategory.Armor)).mass(60).armor(50).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Crobmnium", new Cost(6, 0, 0, 13), new TechRequirements().construction(3), 20, TechCategory.Armor)).mass(56).armor(75).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Carbonic Armor", new Cost(5, 0, 0, 15), new TechRequirements().biotechnology(4), 30, TechCategory.Armor)).mass(25).armor(100).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Strobnium", new Cost(8, 0, 0, 18), new TechRequirements().construction(6), 40, TechCategory.Armor)).mass(54).armor(120).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Organic Armor", new Cost(0, 0, 6, 20), new TechRequirements().biotechnology(7), 50, TechCategory.Armor)).mass(15).armor(175).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Kelarium", new Cost(9, 1, 0, 25), new TechRequirements().construction(9), 60, TechCategory.Armor)).mass(50).armor(180).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Fielded Kelarium", new Cost(10, 0, 2, 28), new TechRequirements().energy(4).construction(10).prtRequired(PRT.IS), 70, TechCategory.Armor)).mass(50).shield(50).armor(175).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Depleted Neutronium", new Cost(10, 0, 2, 28), new TechRequirements().construction(10).electronics(3).prtRequired(PRT.SS), 80, TechCategory.Armor)).mass(50).armor(200).cloak(25).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Neutronium", new Cost(11, 2, 1, 30), new TechRequirements().construction(12), 90, TechCategory.Armor)).mass(45).armor(275).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Valanium", new Cost(15, 0, 0, 50), new TechRequirements().construction(16), 100, TechCategory.Armor)).mass(40).armor(500).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Superlatanium", new Cost(25, 0, 0, 100), new TechRequirements().construction(24), 110, TechCategory.Armor)).mass(30).armor(1500).hullSlotType(HullSlotType.Armor);

        // Beam Weapons
        addTechHullComponent(new TechHullComponent("Laser", new Cost(0, 5, 0, 4), new TechRequirements(), 0, TechCategory.BeamWeapon)).mass(1).initiative(9).power(10).hullSlotType(HullSlotType.Weapon).range(1);
        addTechHullComponent(new TechHullComponent("X-Ray Laser", new Cost(0, 6, 0, 6), new TechRequirements().weapons(3), 10, TechCategory.BeamWeapon)).mass(1).initiative(9).power(16).hullSlotType(HullSlotType.Weapon).range(1);
        addTechHullComponent(new TechHullComponent("Mini Gun", new Cost(0, 6, 0, 6), new TechRequirements().weapons(5), 20, TechCategory.BeamWeapon)).mass(3).initiative(12).mineSweep(208).power(16).hitsAllTargets(true).hullSlotType(HullSlotType.Weapon).range(2);
        addTechHullComponent(new TechHullComponent("Yakimora Light Phaser", new Cost(0, 8, 0, 7), new TechRequirements().weapons(6), 30, TechCategory.BeamWeapon)).mass(1).initiative(9).power(26).hullSlotType(HullSlotType.Weapon).range(1);
        addTechHullComponent(new TechHullComponent("Blackjack", new Cost(0, 16, 0, 7), new TechRequirements().weapons(7), 40, TechCategory.BeamWeapon)).mass(10).initiative(10).power(90).hullSlotType(HullSlotType.Weapon);
        addTechHullComponent(new TechHullComponent("Phaser Bazooka", new Cost(0, 8, 0, 11), new TechRequirements().weapons(8), 50, TechCategory.BeamWeapon)).mass(2).initiative(7).power(26).hullSlotType(HullSlotType.Weapon).range(2);
        addTechHullComponent(new TechHullComponent("Pulsed Sapper", new Cost(0, 0, 4, 12), new TechRequirements().energy(5).weapons(9), 60, TechCategory.BeamWeapon)).mass(1).initiative(14).damageShieldsOnly(true).power(82).hullSlotType(HullSlotType.Weapon).range(3);
        addTechHullComponent(new TechHullComponent("Colloidal Phaser", new Cost(0, 14, 0, 18), new TechRequirements().weapons(10), 70, TechCategory.BeamWeapon)).mass(2).initiative(5).power(26).hullSlotType(HullSlotType.Weapon).range(3);
        addTechHullComponent(new TechHullComponent("Gatling Gun", new Cost(0, 20, 0, 13), new TechRequirements().weapons(11), 80, TechCategory.BeamWeapon)).mass(3).initiative(12).mineSweep(496).power(31).hitsAllTargets(true).hullSlotType(HullSlotType.Weapon).range(2);
        addTechHullComponent(new TechHullComponent("Mini Blaster", new Cost(0, 10, 0, 9), new TechRequirements().weapons(12), 90, TechCategory.BeamWeapon)).mass(1).initiative(9).power(66).hullSlotType(HullSlotType.Weapon).range(1);
        addTechHullComponent(new TechHullComponent("Bludgeon", new Cost(0, 22, 0, 9), new TechRequirements().weapons(13), 100, TechCategory.BeamWeapon)).mass(10).initiative(10).power(231).hullSlotType(HullSlotType.Weapon);
        addTechHullComponent(new TechHullComponent("Mark IV Blaster", new Cost(0, 12, 0, 15), new TechRequirements().weapons(14), 110, TechCategory.BeamWeapon)).mass(2).initiative(7).power(66).hullSlotType(HullSlotType.Weapon).range(2);
        addTechHullComponent(new TechHullComponent("Phased Sapper", new Cost(0, 0, 6, 16), new TechRequirements().energy(8).weapons(15), 120, TechCategory.BeamWeapon)).mass(1).initiative(14).damageShieldsOnly(true).power(211).hullSlotType(HullSlotType.Weapon).range(3);
        addTechHullComponent(new TechHullComponent("Heavy Blaster", new Cost(0, 20, 0, 25), new TechRequirements().weapons(16), 130, TechCategory.BeamWeapon)).mass(2).initiative(5).power(66).hullSlotType(HullSlotType.Weapon).range(3);
        addTechHullComponent(new TechHullComponent("Gatling Neutrino Cannon", new Cost(0, 28, 0, 17), new TechRequirements().weapons(17).prtRequired(PRT.WM), 140, TechCategory.BeamWeapon)).mass(3).initiative(13).mineSweep(1280).power(80).hitsAllTargets(true).hullSlotType(HullSlotType.Weapon).range(2);
        addTechHullComponent(new TechHullComponent("Myopic Disruptor", new Cost(0, 14, 0, 12), new TechRequirements().weapons(18), 150, TechCategory.BeamWeapon)).mass(1).initiative(9).power(169).hullSlotType(HullSlotType.Weapon).range(1);
        addTechHullComponent(new TechHullComponent("Blunderbuss", new Cost(0, 30, 0, 13), new TechRequirements().weapons(19).prtRequired(PRT.WM), 160, TechCategory.BeamWeapon)).mass(10).initiative(11).power(592).hullSlotType(HullSlotType.Weapon);
        addTechHullComponent(new TechHullComponent("Disruptor", new Cost(0, 16, 0, 20), new TechRequirements().weapons(20), 170, TechCategory.BeamWeapon)).mass(2).initiative(8).power(169).hullSlotType(HullSlotType.Weapon).range(2);
        addTechHullComponent(new TechHullComponent("Syncro Sapper", new Cost(0, 0, 8, 21), new TechRequirements().energy(11).weapons(21), 180, TechCategory.BeamWeapon)).mass(1).initiative(14).damageShieldsOnly(true).power(541).hullSlotType(HullSlotType.Weapon).range(3);
        addTechHullComponent(new TechHullComponent("Mega Disruptor", new Cost(0, 30, 0, 33), new TechRequirements().weapons(22), 190, TechCategory.BeamWeapon)).mass(2).initiative(6).power(169).hullSlotType(HullSlotType.Weapon).range(3);
        addTechHullComponent(new TechHullComponent("Big Mutha Cannon", new Cost(0, 36, 0, 23), new TechRequirements().weapons(23), 200, TechCategory.BeamWeapon)).mass(3).initiative(13).mineSweep(3264).power(204).hitsAllTargets(true).hullSlotType(HullSlotType.Weapon).range(2);
        addTechHullComponent(new TechHullComponent("Streaming Pulverizer", new Cost(0, 20, 0, 16), new TechRequirements().weapons(24), 210, TechCategory.BeamWeapon)).mass(1).initiative(9).power(433).hullSlotType(HullSlotType.Weapon).range(1);
        addTechHullComponent(new TechHullComponent("Anti-Matter Pulverizer", new Cost(0, 22, 0, 27), new TechRequirements().weapons(26), 220, TechCategory.BeamWeapon)).mass(1).initiative(8).power(433).hullSlotType(HullSlotType.Weapon).range(2);
        addTechHullComponent(new TechHullComponent("Anti-Matter Generator", new Cost(8, 3, 3, 10), new TechRequirements().weapons(12).biotechnology(7).prtRequired(PRT.IT), 0, TechCategory.Electrical)).mass(10).fuelRegenerationRate(50).fuelBonus(200).hullSlotType(HullSlotType.Electrical);

        // electronics
        addTechHullComponent(new TechHullComponent("Battle Computer", new Cost(0, 0, 13, 5), new TechRequirements(), 0, TechCategory.Electrical)).mass(1).initiativeBonus(1).torpedoBonus(2).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Battle Nexus", new Cost(0, 0, 30, 15), new TechRequirements().energy(10).electronics(19), 0, TechCategory.Electrical)).mass(1).initiativeBonus(3).torpedoBonus(5).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Battle Super Computer", new Cost(0, 0, 25, 14), new TechRequirements().energy(5).electronics(11), 0, TechCategory.Electrical)).mass(1).initiativeBonus(2).torpedoBonus(3).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Energy Capacitor", new Cost(0, 0, 8, 5), new TechRequirements().energy(7).electronics(4), 0, TechCategory.Electrical)).mass(1).beamBonus(1).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Energy Dampener", new Cost(5, 10, 0, 50), new TechRequirements().energy(14).propulsion(8).prtRequired(PRT.SD), 0, TechCategory.Electrical)).mass(2).reduceMovement(1).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Flux Capacitor", new Cost(0, 0, 8, 5), new TechRequirements().energy(14).electronics(8).prtRequired(PRT.HE), 0, TechCategory.Electrical)).mass(1).beamBonus(1).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Jammer 10", new Cost(0, 0, 2, 6), new TechRequirements().energy(2).electronics(6).prtRequired(PRT.IS), 0, TechCategory.Electrical)).mass(1).torpedoJamming(1).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Jammer 20", new Cost(1, 0, 5, 20), new TechRequirements().energy(4).electronics(10), 0, TechCategory.Electrical)).mass(1).torpedoJamming(2).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Jammer 30", new Cost(1, 0, 6, 20), new TechRequirements().energy(8).electronics(16), 0, TechCategory.Electrical)).mass(1).torpedoJamming(3).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Jammer 50", new Cost(2, 0, 7, 20), new TechRequirements().energy(16).electronics(22), 0, TechCategory.Electrical)).mass(1).torpedoJamming(5).hullSlotType(HullSlotType.Electrical);

        // Cloaks
        addTechHullComponent(new TechHullComponent("Stealth Cloak", new Cost(2, 0, 2, 5), new TechRequirements().energy(2).electronics(5), 0, TechCategory.Electrical)).mass(2).cloak(35).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Super Stealth Cloak", new Cost(8, 0, 8, 15), new TechRequirements().energy(4).electronics(10), 0, TechCategory.Electrical)).mass(3).cloak(55).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Tachyon Detector", new Cost(1, 5, 0, 70), new TechRequirements().energy(8).electronics(14).prtRequired(PRT.IS), 0, TechCategory.Electrical)).mass(1).reduceCloaking(5).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Transport Cloaking", new Cost(2, 0, 2, 3), new TechRequirements().prtRequired(PRT.SS), 0, TechCategory.Electrical)).mass(1).cloakUnarmedOnly(true).cloak(75).hullSlotType(HullSlotType.Electrical);
        addTechHullComponent(new TechHullComponent("Ultra-Stealth Cloak", new Cost(10, 0, 10, 25), new TechRequirements().energy(10).electronics(12).prtRequired(PRT.SS), 0, TechCategory.Electrical)).mass(5).cloak(85).hullSlotType(HullSlotType.Electrical);

        // mine layers
        addTechHullComponent(new TechHullComponent("Heavy Dispenser 110", new Cost(2, 20, 5, 50), new TechRequirements().energy(9).biotechnology(5).prtRequired(PRT.SD), 0, TechCategory.MineLayer)).mass(15).minDamagePerFleetRS(2500).damagePerEngineRS(600).maxSpeed(6).chanceOfHit(1).hullSlotType(HullSlotType.Mine).minDamagePerFleet(2000).damagePerEngine(500);
        addTechHullComponent(new TechHullComponent("Heavy Dispenser 200", new Cost(2, 45, 5, 90), new TechRequirements().energy(14).biotechnology(7).prtRequired(PRT.SD), 0, TechCategory.MineLayer)).mass(20).minDamagePerFleetRS(2500).damagePerEngineRS(600).maxSpeed(6).chanceOfHit(1).hullSlotType(HullSlotType.Mine).minDamagePerFleet(2000).damagePerEngine(500);
        addTechHullComponent(new TechHullComponent("Heavy Dispenser 50", new Cost(2, 20, 5, 50), new TechRequirements().energy(5).biotechnology(3).prtRequired(PRT.SD), 0, TechCategory.MineLayer)).mass(10).minDamagePerFleetRS(2500).damagePerEngineRS(600).maxSpeed(6).chanceOfHit(1).hullSlotType(HullSlotType.Mine).minDamagePerFleet(2000).damagePerEngine(500);
        addTechHullComponent(new TechHullComponent("Mine Dispenser 130", new Cost(2, 18, 10, 80), new TechRequirements().energy(6).biotechnology(12).prtRequired(PRT.SD), 0, TechCategory.MineLayer)).mass(30).minDamagePerFleetRS(600).damagePerEngineRS(125).maxSpeed(4).chanceOfHit(3).hullSlotType(HullSlotType.Mine).minDamagePerFleet(500).damagePerEngine(100);
        addTechHullComponent(new TechHullComponent("Mine Dispenser 40", new Cost(2, 9, 7, 40), new TechRequirements().prtRequired(PRT.SD), 0, TechCategory.MineLayer)).mass(25).minDamagePerFleetRS(600).damagePerEngineRS(125).maxSpeed(4).chanceOfHit(3).hullSlotType(HullSlotType.Mine).minDamagePerFleet(500).damagePerEngine(100);
        addTechHullComponent(new TechHullComponent("Mine Dispenser 50", new Cost(2, 12, 10, 55), new TechRequirements().energy(2).biotechnology(4), 0, TechCategory.MineLayer)).mass(30).minDamagePerFleetRS(600).damagePerEngineRS(125).maxSpeed(4).chanceOfHit(3).hullSlotType(HullSlotType.Mine).minDamagePerFleet(500).damagePerEngine(100);
        addTechHullComponent(new TechHullComponent("Mine Dispenser 80", new Cost(2, 12, 10, 65), new TechRequirements().energy(3).biotechnology(7).prtRequired(PRT.SD), 0, TechCategory.MineLayer)).mass(30).minDamagePerFleetRS(600).damagePerEngineRS(125).maxSpeed(4).chanceOfHit(3).hullSlotType(HullSlotType.Mine).minDamagePerFleet(500).damagePerEngine(100);
        addTechHullComponent(new TechHullComponent("Speed Trap 20", new Cost(29, 0, 12, 58), new TechRequirements().propulsion(2).biotechnology(2).prtRequired(PRT.IS), 0, TechCategory.MineLayer)).mass(100).maxSpeed(5).chanceOfHit(35).hullSlotType(HullSlotType.Mine);
        addTechHullComponent(new TechHullComponent("Speed Trap 30", new Cost(32, 0, 14, 72), new TechRequirements().propulsion(3).biotechnology(6).prtRequired(PRT.IS), 0, TechCategory.MineLayer)).mass(135).maxSpeed(5).chanceOfHit(35).hullSlotType(HullSlotType.Mine);
        addTechHullComponent(new TechHullComponent("Speed Trap 50", new Cost(40, 0, 15, 80), new TechRequirements().propulsion(5).biotechnology(11).prtRequired(PRT.IS), 0, TechCategory.MineLayer)).mass(140).maxSpeed(5).chanceOfHit(35).hullSlotType(HullSlotType.Mine);

        // mechanical
        addTechHullComponent(new TechHullComponent("Beam Deflector", new Cost(0, 0, 10, 8), new TechRequirements().energy(6).weapons(6).construction(6).electronics(6), 0, TechCategory.Mechanical)).mass(1).hullSlotType(HullSlotType.Mechanical).beamDefense(1);
        addTechHullComponent(new TechHullComponent("Cargo Pod", new Cost(5, 0, 2, 10), new TechRequirements().construction(3), 0, TechCategory.Mechanical)).mass(5).cargoBonus(50).hullSlotType(HullSlotType.Mechanical);
        addTechHullComponent(new TechHullComponent("Colonization Module", new Cost(11, 9, 9, 9), new TechRequirements(), 0, TechCategory.Mechanical)).mass(32).colonizationModule(true).hullSlotType(HullSlotType.Mechanical);
        addTechHullComponent(new TechHullComponent("Fuel Tank", new Cost(5, 0, 0, 4), new TechRequirements(), 0, TechCategory.Mechanical)).mass(3).fuelBonus(250).hullSlotType(HullSlotType.Mechanical);
        addTechHullComponent(new TechHullComponent("Maneuvering Jet", new Cost(5, 0, 5, 10), new TechRequirements().energy(2).propulsion(3), 0, TechCategory.Mechanical)).mass(5).hullSlotType(HullSlotType.Mechanical);
        addTechHullComponent(new TechHullComponent("Orbital Construction Module", new Cost(18, 13, 13, 18), new TechRequirements().prtRequired(PRT.AR), 0, TechCategory.Mechanical)).mass(50).minKillRate(2000).colonizationModule(true).hullSlotType(HullSlotType.Armor);
        addTechHullComponent(new TechHullComponent("Overthruster", new Cost(10, 0, 8, 20), new TechRequirements().energy(5).propulsion(12), 0, TechCategory.Mechanical)).mass(5).hullSlotType(HullSlotType.Mechanical);
        addTechHullComponent(new TechHullComponent("Super Cargo Pod", new Cost(8, 0, 2, 15), new TechRequirements().energy(3).construction(8), 0, TechCategory.Mechanical)).mass(7).cargoBonus(100).hullSlotType(HullSlotType.Mechanical);
        addTechHullComponent(new TechHullComponent("Super Fuel Tank", new Cost(8, 0, 0, 8), new TechRequirements().energy(6).propulsion(4).construction(14), 0, TechCategory.Mechanical)).mass(8).fuelBonus(500).hullSlotType(HullSlotType.Mechanical);

        // torpedos
        addTechHullComponent(new TechHullComponent("Alpha Torpedo", new Cost(8, 3, 3, 4), new TechRequirements(), 0, TechCategory.Torpedo)).mass(25).accuracy(35).power(5).hullSlotType(HullSlotType.Weapon).range(4);
        addTechHullComponent(new TechHullComponent("Armageddon Missle", new Cost(67, 23, 16, 24), new TechRequirements().weapons(24).propulsion(10), 0, TechCategory.Torpedo)).mass(35).initiative(3).accuracy(30).capitalShipMissle(true).power(525).hullSlotType(HullSlotType.Weapon).range(6);
        addTechHullComponent(new TechHullComponent("Beta Torpedo", new Cost(18, 6, 4, 6), new TechRequirements().weapons(5).propulsion(1), 0, TechCategory.Torpedo)).mass(25).initiative(1).accuracy(45).power(12).hullSlotType(HullSlotType.Weapon).range(4);
        addTechHullComponent(new TechHullComponent("Delta Torpedo", new Cost(22, 8, 5, 8), new TechRequirements().weapons(10).propulsion(2), 0, TechCategory.Torpedo)).mass(25).initiative(1).accuracy(60).power(26).hullSlotType(HullSlotType.Weapon).range(4);
        addTechHullComponent(new TechHullComponent("Doomsday Missle", new Cost(60, 20, 13, 20), new TechRequirements().weapons(20).propulsion(10), 0, TechCategory.Torpedo)).mass(35).initiative(2).accuracy(25).capitalShipMissle(true).power(280).hullSlotType(HullSlotType.Weapon).range(6);
        addTechHullComponent(new TechHullComponent("Epsilon Torpedo", new Cost(30, 10, 6, 10), new TechRequirements().weapons(14).propulsion(3), 0, TechCategory.Torpedo)).mass(25).initiative(2).accuracy(65).power(48).hullSlotType(HullSlotType.Weapon).range(5);
        addTechHullComponent(new TechHullComponent("Jihad Missle", new Cost(37, 13, 9, 13), new TechRequirements().weapons(12).propulsion(6), 0, TechCategory.Torpedo)).mass(35).accuracy(20).capitalShipMissle(true).power(85).hullSlotType(HullSlotType.Weapon).range(5);
        addTechHullComponent(new TechHullComponent("Juggernaut Missle", new Cost(48, 16, 11, 16), new TechRequirements().weapons(16).propulsion(8), 0, TechCategory.Torpedo)).mass(35).initiative(1).accuracy(20).capitalShipMissle(true).power(150).hullSlotType(HullSlotType.Weapon).range(5);
        addTechHullComponent(new TechHullComponent("Omega Torpedo", new Cost(52, 18, 12, 18), new TechRequirements().weapons(26).propulsion(6), 0, TechCategory.Torpedo)).mass(25).initiative(4).accuracy(80).power(316).hullSlotType(HullSlotType.Weapon).range(5);
        addTechHullComponent(new TechHullComponent("Rho Torpedo", new Cost(34, 12, 8, 12), new TechRequirements().weapons(18).propulsion(4), 0, TechCategory.Torpedo)).mass(25).initiative(2).accuracy(75).power(90).hullSlotType(HullSlotType.Weapon).range(5);
        addTechHullComponent(new TechHullComponent("Upsilon Torpedo", new Cost(40, 14, 9, 15), new TechRequirements().weapons(22).propulsion(5), 0, TechCategory.Torpedo)).mass(25).initiative(3).accuracy(75).power(169).hullSlotType(HullSlotType.Weapon).range(5);

        // shields
        addTechHullComponent(new TechHullComponent("Mole-skin Shield", new Cost(1, 0, 1, 4), new TechRequirements(), 10, TechCategory.Shield)).mass(1).shield(25).hullSlotType(HullSlotType.Shield);
        addTechHullComponent(new TechHullComponent("Cow-hide Shield", new Cost(2, 0, 2, 5), new TechRequirements().energy(3), 20, TechCategory.Shield)).mass(1).shield(40).hullSlotType(HullSlotType.Shield);
        addTechHullComponent(new TechHullComponent("Wolverine Diffuse Shield", new Cost(3, 0, 3, 6), new TechRequirements().energy(6), 30, TechCategory.Shield)).mass(1).shield(60).hullSlotType(HullSlotType.Shield);
        addTechHullComponent(new TechHullComponent("Croby Sharmor", new Cost(7, 0, 4, 15), new TechRequirements().energy(7).construction(4).prtRequired(PRT.IS), 40, TechCategory.Shield)).mass(10).shield(60).armor(65).hullSlotType(HullSlotType.Shield);
        addTechHullComponent(new TechHullComponent("Shadow Shield", new Cost(3, 0, 3, 7), new TechRequirements().energy(7).electronics(3).prtRequired(PRT.SS), 50, TechCategory.Shield)).mass(2).shield(75).cloak(35).hullSlotType(HullSlotType.Shield);
        addTechHullComponent(new TechHullComponent("Bear Neutrino Barrier", new Cost(4, 0, 4, 8), new TechRequirements().energy(10), 60, TechCategory.Shield)).mass(1).shield(100).hullSlotType(HullSlotType.Shield);
        addTechHullComponent(new TechHullComponent("Gorilla Delegator", new Cost(5, 0, 6, 11), new TechRequirements().energy(14), 70, TechCategory.Shield)).mass(1).shield(175).hullSlotType(HullSlotType.Shield);
        addTechHullComponent(new TechHullComponent("Elephant Hide Fortress", new Cost(8, 0, 10, 15), new TechRequirements().energy(18), 80, TechCategory.Shield)).mass(1).shield(300).hullSlotType(HullSlotType.Shield);
        addTechHullComponent(new TechHullComponent("Complete Phase Shield", new Cost(12, 0, 15, 20), new TechRequirements().energy(22), 90, TechCategory.Shield)).mass(1).shield(500).hullSlotType(HullSlotType.Shield);

        foreach (TechCategory category in Enum.GetValues(typeof(TechCategory)))
        {
            techsForCategory.Add(category, new List<Tech>());
        }

        // sort the techs into various lists
        foreach (Tech techInList in techs.Values)
        {
            if (techInList.getCategory() == TechCategory.Engine)
            {
                engines.Add((TechEngine)techInList);
            }
            else if (techInList.getCategory() == TechCategory.Armor)
            {
                armor.Add((TechHullComponent)techInList);
            }
            else if (techInList.getCategory() == TechCategory.Shield)
            {
                shields.Add((TechHullComponent)techInList);
            }
            else if (techInList.getCategory() == TechCategory.Scanner)
            {
                scanners.Add((TechHullComponent)techInList);
            }
            else if (techInList.getCategory() == TechCategory.Torpedo)
            {
                torpedos.Add((TechHullComponent)techInList);
            }
            else if (techInList.getCategory() == TechCategory.BeamWeapon)
            {
                beamWeapons.Add((TechHullComponent)techInList);
            }
            else if (techInList.getCategory() == TechCategory.PlanetaryScanner)
            {
                planetaryScanners.Add((TechPlanetaryScanner)techInList);
            }
            else if (techInList.getCategory() == TechCategory.PlanetaryDefense)
            {
                defenses.Add((TechDefence)techInList);
            }
            else if (techInList.getCategory() == TechCategory.ShipHull)
            {
                hulls.Add((TechHull)techInList);
            }
            else if (techInList.getCategory() == TechCategory.StarbaseHull)
            {
                starbases.Add((TechHull)techInList);
            }

            techsForCategory[techInList.getCategory()].Add(techInList);
        }

        foreach (TechCategory category in Enum.GetValues(typeof(TechCategory)))
        {
            techsForCategory[category] = techsForCategory[category].OrderBy(t => t.getRanking()).ToList<Tech>();
        }

        // sort all the lists of techs.
        engines = engines.OrderBy(eng => eng.getRanking()).ToList();
        armor = armor.OrderBy(eng => eng.getRanking()).ToList();
        shields = shields.OrderBy(eng => eng.getRanking()).ToList();
        scanners = scanners.OrderBy(eng => eng.getRanking()).ToList();
        torpedos = torpedos.OrderBy(eng => eng.getRanking()).ToList();
        beamWeapons = beamWeapons.OrderBy(eng => eng.getRanking()).ToList();
        planetaryScanners = planetaryScanners.OrderBy(eng => eng.getRanking()).ToList();
        defenses = defenses.OrderBy(eng => eng.getRanking()).ToList();
        hulls = hulls.OrderBy(eng => eng.getRanking()).ToList();
        starbases = starbases.OrderBy(eng => eng.getRanking()).ToList();
    }

    /**
     * Add a new tech to the master list
     * @param tech The tech to add
     * @return The tech
     */
    private static TechHullComponent addTechHullComponent(TechHullComponent tech)
    {
        techs.Add(tech.getName(), tech);
        return tech;
    }

    public List<TechEngine> getAllEngines()
    {
        return engines;
    }

    public List<TechHullComponent> getAllScanners()
    {
        return scanners;
    }

    public List<TechHullComponent> getAllShields()
    {
        return shields;
    }

    public List<TechHullComponent> getAllArmor()
    {
        return armor;
    }

    public List<TechHull> getAllHulls()
    {
        return hulls;
    }

    public List<TechPlanetaryScanner> getAllPlanetaryScanners()
    {
        return planetaryScanners;
    }

    public List<TechDefence> getAllDefenses()
    {
        return defenses;
    }

    public List<TechHull> getAllStarbases()
    {
        return starbases;
    }

    public List<TechHullComponent> getAllTorpedos()
    {
        return torpedos;
    }

    public List<TechHullComponent> getAllBeamWeapons()
    {
        return beamWeapons;
    }

    public List<Tech> getAllForCategory(TechCategory category)
    {
        return techsForCategory[category];
    }
}

