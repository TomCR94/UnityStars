using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TechStore
{

    /**
     * Get a tech by a name
     * @param name
     * @return
     */
    Tech get(string name);

    /**
     * Get all the hull component techs
     * @param name
     * @return
     */
    TechHullComponent getHullComponent(string name);

    /**
     * Get a Hull tech
     * @param name The name of the tech
     * @return The Hull tech, or null if not found
     */
    TechHull getHull(string name);

    /**
     * Get an Engine tech by name
     * @param name The name of the tech
     * @return The TechEngine tech, or null if not found
     */
    TechEngine getEngine(string name);

    /**
     * Get all the techs
     * @return All the techs
     */
    List<Tech> getAll();

    /**
     * @return All the engines, sorted by ranking
     */
    List<TechEngine> getAllEngines();

    /**
     * @return All the Scanners, sorted by ranking
     */
    List<TechHullComponent> getAllScanners();


    /**
     * @return All the Shields, sorted by ranking
     */
    List<TechHullComponent> getAllShields();

    /**
     * @return All the Armor, sorted by ranking
     */
    List<TechHullComponent> getAllArmor();

    /**
     * @return All the Torpedos, sorted by ranking
     */
    List<TechHullComponent> getAllTorpedos();

    /**
     * @return All the Beam Weapons, sorted by ranking
     */
    List<TechHullComponent> getAllBeamWeapons();

    /**
     * @return All the hulls, sorted by ranking
     */
    List<TechHull> getAllHulls();

    /**
     * @return All the starbases, sorted by ranking
     */
    List<TechHull> getAllStarbases();

    /**
     * @return All the planetary scanners, sorted by ranking
     */
    List<TechPlanetaryScanner> getAllPlanetaryScanners();

    /**
     * @return All the planetary defenses, sorted by ranking
     */
    List<TechDefence> getAllDefenses();

    /**
     * Get all the techs for a category, sorted by ranking
     * @param category The category to get techs for
     * @return A list of techs in the category
     */
    List<Tech> getAllForCategory(TechCategory category);

    /**
     * Initialize a ship design's techs with techs from this TechStore 
     * @param design The design to initialize
     */
    void initShipDesign(ShipDesign design);
}