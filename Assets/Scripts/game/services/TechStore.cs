using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TechStore
{

    /**
     * Get a tech by a name
     */
    Tech get(string name);

    /**
     * Get all the hull component techs
     */
    TechHullComponent getHullComponent(string name);

    /**
     * Get a Hull tech
     */
    TechHull getHull(string name);

    /**
     * Get an Engine tech by name
     */
    TechEngine getEngine(string name);

    /**
     * Get all the techs
     */
    List<Tech> getAll();
    
    List<TechEngine> getAllEngines();
    
    List<TechHullComponent> getAllScanners();

    
    List<TechHullComponent> getAllShields();
    
    List<TechHullComponent> getAllArmor();
    
    List<TechHullComponent> getAllTorpedos();
    
    List<TechHullComponent> getAllBeamWeapons();
    
    List<TechHull> getAllHulls();
    
    List<TechHull> getAllStarbases();
    
    List<TechPlanetaryScanner> getAllPlanetaryScanners();
    
    List<TechDefence> getAllDefenses();
    
    List<Tech> getAllForCategory(TechCategory category);
    
    void initShipDesign(ShipDesign design);
}