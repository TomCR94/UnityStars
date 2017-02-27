using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class PlayerTechs
{
    [SerializeField]
    private TechEngine bestEngine;
    [SerializeField]
    private TechHullComponent bestScanner;
    [SerializeField]
    private TechPlanetaryScanner bestPlanetaryScanner;
    [SerializeField]
    private TechDefence bestDefense;
    [SerializeField]
    private TechHullComponent bestWeapon;
    [SerializeField]
    private TechHullComponent bestArmor;
    [SerializeField]
    private TechHullComponent bestShield;
    [SerializeField]
    private TechHullComponent bestTorpedo;
    [SerializeField]
    private TechHullComponent bestBeamWeapon;
    [SerializeField]
    private TechHull bestHull;
    [SerializeField]
    private TechHull bestStarbase;


    [SerializeField]
    private List<TechEngine> engines = new List<TechEngine>();
    [SerializeField]
    private List<TechHullComponent> scanners = new List<TechHullComponent>();
    [SerializeField]
    private List<TechHullComponent> shields = new List<TechHullComponent>();
    [SerializeField]
    private List<TechHullComponent> armor = new List<TechHullComponent>();
    [SerializeField]
    private List<TechHullComponent> torpedos = new List<TechHullComponent>();
    [SerializeField]
    private List<TechHullComponent> beamWeapons = new List<TechHullComponent>();
    [SerializeField]
    private List<TechHull> hulls = new List<TechHull>();
    [SerializeField]
    private List<TechHull> starbases = new List<TechHull>();
    [SerializeField]
    private List<TechPlanetaryScanner> planetaryScanners = new List<TechPlanetaryScanner>();
    [SerializeField]
    private List<TechDefence> defenses = new List<TechDefence>();
    [SerializeField]
    private Dictionary<TechCategory, List<Tech>> techsForCategory = new Dictionary<TechCategory, List<Tech>>();
    [SerializeField]
    private List<TechHullComponent> hullComponents = new List<TechHullComponent>();

    /**
     * Initialize the Player techs with the tech store
     * 
     * @param player The player to create a PlayerTechs list for
     * @param techStore The techstore to get techs from
     */
    public void init(Player player, TechStore techStore)
    {

        engines = new List<TechEngine>();
        scanners = new List<TechHullComponent>();
        shields = new List<TechHullComponent>();
        armor = new List<TechHullComponent>();
        torpedos = new List<TechHullComponent>();
        beamWeapons = new List<TechHullComponent>();
        hulls = new List<TechHull>();
        starbases = new List<TechHull>();
        planetaryScanners = new List<TechPlanetaryScanner>();
        defenses = new List<TechDefence>();
        techsForCategory = new Dictionary<TechCategory, List<Tech>>();
        hullComponents = new List<TechHullComponent>();

        bestEngine = addPlayerTechs(player, techStore.getAllEngines(), engines);
        Debug.Log(bestEngine);
        bestScanner = addPlayerTechs(player, techStore.getAllScanners(), scanners);
        bestShield = addPlayerTechs(player, techStore.getAllShields(), shields);
        bestArmor = addPlayerTechs(player, techStore.getAllArmor(), armor);
        bestTorpedo = addPlayerTechs(player, techStore.getAllTorpedos(), torpedos);
        bestBeamWeapon = addPlayerTechs(player, techStore.getAllBeamWeapons(), beamWeapons);
        bestHull = addPlayerTechs(player, techStore.getAllHulls(), hulls);
        bestStarbase = addPlayerTechs(player, techStore.getAllStarbases(), starbases);
        bestPlanetaryScanner = addPlayerTechs(player, techStore.getAllPlanetaryScanners(), planetaryScanners);
        bestDefense = addPlayerTechs(player, techStore.getAllDefenses(), defenses);

        foreach (TechCategory category in Enum.GetValues(typeof(TechCategory)))
        {
            techsForCategory.Add(category, new List<Tech>());
        }

        foreach (Tech tech in techStore.getAll())
        {
            if (player.hasTech(tech))
            {
                List<Tech> t;
                techsForCategory.TryGetValue(tech.getCategory(), out t);
                t.Add(tech);
                if (tech is TechHullComponent) {
            hullComponents.Add((TechHullComponent)tech);
        }
    }
}
        
        // sort the category techs
        foreach (TechCategory category in Enum.GetValues(typeof(TechCategory))) {
            //Collections.sort(techsForCategory[category], new TechComparator<Tech>());
            //techsForCategory[category].Sort();
            techsForCategory[category] = techsForCategory[category].OrderBy(t => t.getRanking()).ToList<Tech>();
        }
        
        // sort the hull components by category and ranking
        //Collections.sort(hullComponents, new TechCategoryRankingComparator<Tech>());
        //hullComponents.Sort();
        hullComponents = hullComponents.OrderBy(t => t.getRanking()).ToList<TechHullComponent>();

    }

    /**
     * Get all the techs for a category, sorted by ranking
     * @param category The category to get techs for
     * @return A list of techs in the category
     */
    public List<Tech> getAllForCategory(TechCategory category)
{
    return techsForCategory[category];
}

/**
 * Add techs to an internal list, if the player has them, returning the best tech
 * @param <T> The type of tech
 * @param player The player
 * @param techs The master list of techs
 * @param internalList The internal list to update
 * @return The best tech of the list
 */
private T addPlayerTechs<T>(Player player, List<T> techs, List<T> internalList) where T : Tech
{
    foreach (T tech in techs)
    {
        Debug.Log("tech: " + tech);
        if (player.hasTech(tech))
        {
            internalList.Add(tech);

                Debug.Log("tech: " + tech + " added");
            }
    }

    if (internalList.Count > 0)
    {
        return internalList[internalList.Count - 1];
    }
    else
    {
        return null;
    }
}

public TechEngine getBestEngine()
{
    return bestEngine;
}

public void setBestEngine(TechEngine bestEngine)
{
    this.bestEngine = bestEngine;
}

public TechHullComponent getBestScanner()
{
    return bestScanner;
}

public void setBestScanner(TechHullComponent bestScanner)
{
    this.bestScanner = bestScanner;
}

public TechPlanetaryScanner getBestPlanetaryScanner()
{
    return bestPlanetaryScanner;
}

public void setBestPlanetaryScanner(TechPlanetaryScanner bestPlanetaryScanner)
{
    this.bestPlanetaryScanner = bestPlanetaryScanner;
}

public TechDefence getBestDefense()
{
    return bestDefense;
}

public void setBestDefense(TechDefence bestDefense)
{
    this.bestDefense = bestDefense;
}

public TechHullComponent getBestWeapon()
{
    return bestWeapon;
}

public void setBestWeapon(TechHullComponent bestWeapon)
{
    this.bestWeapon = bestWeapon;
}

public TechHullComponent getBestArmor()
{
    return bestArmor;
}

public void setBestArmor(TechHullComponent bestArmor)
{
    this.bestArmor = bestArmor;
}

public TechHullComponent getBestShield()
{
    return bestShield;
}

public void setBestShield(TechHullComponent bestShield)
{
    this.bestShield = bestShield;
}

public TechHull getBestHull()
{
    return bestHull;
}

public void setBestHull(TechHull bestHull)
{
    this.bestHull = bestHull;
}

public TechHull getBestStarbase()
{
    return bestStarbase;
}

public void setBestStarbase(TechHull bestStarbase)
{
    this.bestStarbase = bestStarbase;
}

public List<TechEngine> getEngines()
{
    return engines;
}

public void setEngines(List<TechEngine> engines)
{
    this.engines = engines;
}

public List<TechHullComponent> getScanners()
{
    return scanners;
}

public void setScanners(List<TechHullComponent> scanners)
{
    this.scanners = scanners;
}

public List<TechHullComponent> getShields()
{
    return shields;
}

public void setShields(List<TechHullComponent> shields)
{
    this.shields = shields;
}

public List<TechHullComponent> getArmor()
{
    return armor;
}

public void setArmor(List<TechHullComponent> armor)
{
    this.armor = armor;
}

public List<TechHull> getHulls()
{
    return hulls;
}

public void setHulls(List<TechHull> hulls)
{
    this.hulls = hulls;
}

public List<TechHull> getStarbases()
{
    return starbases;
}

public void setStarbases(List<TechHull> starbases)
{
    this.starbases = starbases;
}

public List<TechPlanetaryScanner> getPlanetaryScanners()
{
    return planetaryScanners;
}

public void setPlanetaryScanners(List<TechPlanetaryScanner> planetaryScanners)
{
    this.planetaryScanners = planetaryScanners;
}

public List<TechDefence> getDefenses()
{
    return defenses;
}

public void setDefenses(List<TechDefence> defenses)
{
    this.defenses = defenses;
}


public TechHullComponent getBestTorpedo()
{
    return bestTorpedo;
}


public void setBestTorpedo(TechHullComponent bestTorpedo)
{
    this.bestTorpedo = bestTorpedo;
}


public TechHullComponent getBestBeamWeapon()
{
    return bestBeamWeapon;
}


public void setBestBeamWeapon(TechHullComponent bestBeamWeapon)
{
    this.bestBeamWeapon = bestBeamWeapon;
}


public List<TechHullComponent> getTorpedos()
{
    return torpedos;
}


public void setTorpedos(List<TechHullComponent> torpedos)
{
    this.torpedos = torpedos;
}


public List<TechHullComponent> getBeamWeapons()
{
    return beamWeapons;
}


public void setBeamWeapons(List<TechHullComponent> beamWeapons)
{
    this.beamWeapons = beamWeapons;
}

public void setHullComponents(List<TechHullComponent> hullComponents)
{
    this.hullComponents = hullComponents;
}

public List<TechHullComponent> getHullComponents()
{
    return hullComponents;
}
}
