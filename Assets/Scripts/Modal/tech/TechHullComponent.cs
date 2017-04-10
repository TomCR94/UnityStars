using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TechHullComponent : Tech {

    [SerializeField]
    private HullSlotType _hullSlotType;
    [SerializeField]
    private int _mass;
    [SerializeField]
    private int _armor;
    [SerializeField]
    private int _shield;
    [SerializeField]
    private int _cloak;
    [SerializeField]
    private bool _cloakUnarmedOnly;
    [SerializeField]
    private int _torpedoBonus;
    [SerializeField]
    private int _initiativeBonus;
    [SerializeField]
    private int _torpedoJamming;
    [SerializeField]
    private int _reduceMovement;
    [SerializeField]
    private int _reduceCloaking;
    [SerializeField]
    private int _fuelBonus;
    [SerializeField]
    private int _fuelRegenerationRate;
    [SerializeField]
    private bool _colonizationModule;
    [SerializeField]
    private int _cargoBonus;
    [SerializeField]
    private int _movementBonus;
    [SerializeField]
    private int _beamDefense;
    [SerializeField]
    private int _beamBonus;
    [SerializeField]
    private int _scanRange;
    [SerializeField]
    private int _scanRangePen;
    [SerializeField]
    private bool _stealCargo;
    [SerializeField]
    private bool _radiating;
    [SerializeField]
    private bool _smart;
    [SerializeField]
    private int _killRate;
    [SerializeField]
    private int _minKillRate;
    [SerializeField]
    private int _structureKillRate;
    [SerializeField]
    private int _unterraformRate;
    [SerializeField]
    private int _power;
    [SerializeField]
    private int _range;
    [SerializeField]
    private int _initiative;
    [SerializeField]
    private int _accuracy;
    [SerializeField]
    private int _mineSweep;
    [SerializeField]
    private bool _hitsAllTargets;
    [SerializeField]
    private bool _damageShieldsOnly;
    [SerializeField]
    private bool _capitalShipMissle;
    [SerializeField]
    private int _packetSpeed;
    [SerializeField]
    private int _safeHullMass;
    [SerializeField]
    private int _safeRange;
    [SerializeField]
    private int _maxHullMass;
    [SerializeField]
    private int _maxRange;
    [SerializeField]
    private int _miningRate;
    [SerializeField]
    private int _terraformRate;
    [SerializeField]
    private int _mineLayingRate;
    [SerializeField]
    private int _maxSpeed;
    [SerializeField]
    private int _chanceOfHit;
    [SerializeField]
    private int _damagePerEngine;
    [SerializeField]
    private int _damagePerEngineRS;
    [SerializeField]
    private int _minDamagePerFleet;
    [SerializeField]
    private int _minDamagePerFleetRS;

    public TechHullComponent() : base()
    {
    }

    public TechHullComponent(string name, Cost cost, TechRequirements techRequirements, int ranking, TechCategory category) : base(name, cost, techRequirements, ranking, category)
    {
    }

    override public string ToString()
    {
        return "TechHullComponent [name=" + getName() + ", hullSlotType=" + _hullSlotType + ", mass=" + _mass + ", getCost()=" + getCost()
               + ", getCategory()=" + getCategory() + "]";
    }

    public HullSlotType getHullSlotType()
    {
        return _hullSlotType;
    }

    public void setHullSlotType(HullSlotType hullSlotType)
    {
        this._hullSlotType = hullSlotType;
    }

    public int getMass()
    {
        return _mass;
    }

    public void setMass(int mass)
    {
        this._mass = mass;
    }

    public int getArmor()
    {
        return _armor;
    }

    public void setArmor(int armor)
    {
        this._armor = armor;
    }

    public int getShield()
    {
        return _shield;
    }

    public void setShield(int shield)
    {
        this._shield = shield;
    }

    public int getCloak()
    {
        return _cloak;
    }

    public void setCloak(int cloak)
    {
        this._cloak = cloak;
    }

    public bool isCloakUnarmedOnly()
    {
        return _cloakUnarmedOnly;
    }

    public void setCloakUnarmedOnly(bool cloakUnarmedOnly)
    {
        this._cloakUnarmedOnly = cloakUnarmedOnly;
    }

    public int getTorpedoBonus()
    {
        return _torpedoBonus;
    }

    public void setTorpedoBonus(int torpedoBonus)
    {
        this._torpedoBonus = torpedoBonus;
    }

    public int getInitiativeBonus()
    {
        return _initiativeBonus;
    }

    public void setInitiativeBonus(int initiativeBonus)
    {
        this._initiativeBonus = initiativeBonus;
    }

    public int getTorpedoJamming()
    {
        return _torpedoJamming;
    }

    public void setTorpedoJamming(int torpedoJamming)
    {
        this._torpedoJamming = torpedoJamming;
    }

    public int getReduceMovement()
    {
        return _reduceMovement;
    }

    public void setReduceMovement(int reduceMovement)
    {
        this._reduceMovement = reduceMovement;
    }

    public int getReduceCloaking()
    {
        return _reduceCloaking;
    }

    public void setReduceCloaking(int reduceCloaking)
    {
        this._reduceCloaking = reduceCloaking;
    }

    public int getFuelBonus()
    {
        return _fuelBonus;
    }

    public void setFuelBonus(int fuelBonus)
    {
        this._fuelBonus = fuelBonus;
    }

    public int getFuelRegenerationRate()
    {
        return _fuelRegenerationRate;
    }

    public void setFuelRegenerationRate(int fuelRegenerationRate)
    {
        this._fuelRegenerationRate = fuelRegenerationRate;
    }

    public bool isColonisationModule()
    {
        return _colonizationModule;
    }

    public void setColonizationModule(bool colonizationModule)
    {
        this._colonizationModule = colonizationModule;
    }

    public int getCargoBonus()
    {
        return _cargoBonus;
    }

    public void setCargoBonus(int cargoBonus)
    {
        this._cargoBonus = cargoBonus;
    }

    public int getMovementBonus()
    {
        return _movementBonus;
    }

    public void setMovementBonus(int movementBonus)
    {
        this._movementBonus = movementBonus;
    }

    public int getBeamDefense()
    {
        return _beamDefense;
    }

    public void setBeamDefense(int beamDefense)
    {
        this._beamDefense = beamDefense;
    }

    public int getBeamBonus()
    {
        return _beamBonus;
    }

    public void setBeamBonus(int beamBonus)
    {
        this._beamBonus = beamBonus;
    }

    public int getScanRange()
    {
        return _scanRange;
    }

    public void setScanRange(int scanRange)
    {
        this._scanRange = scanRange;
    }

    public int getScanRangePen()
    {
        return _scanRangePen;
    }

    public void setScanRangePen(int scanRangePen)
    {
        this._scanRangePen = scanRangePen;
    }

    public bool isStealCargo()
    {
        return _stealCargo;
    }

    public void setStealCargo(bool stealCargo)
    {
        this._stealCargo = stealCargo;
    }

    public bool isRadiating()
    {
        return _radiating;
    }

    public void setRadiating(bool radiating)
    {
        this._radiating = radiating;
    }

    public bool isSmart()
    {
        return _smart;
    }

    public void setSmart(bool smart)
    {
        this._smart = smart;
    }

    public int getKillRate()
    {
        return _killRate;
    }

    public void setKillRate(int killRate)
    {
        this._killRate = killRate;
    }

    public int getMinKillRate()
    {
        return _minKillRate;
    }

    public void setMinKillRate(int minKillRate)
    {
        this._minKillRate = minKillRate;
    }

    public int getStructureKillRate()
    {
        return _structureKillRate;
    }

    public void setStructureKillRate(int structureKillRate)
    {
        this._structureKillRate = structureKillRate;
    }

    public int getUnterraformRate()
    {
        return _unterraformRate;
    }

    public void setUnterraformRate(int unterraformRate)
    {
        this._unterraformRate = unterraformRate;
    }

    public int getPower()
    {
        return _power;
    }

    public void setPower(int power)
    {
        this._power = power;
    }

    public int getRange()
    {
        return _range;
    }

    public void setRange(int range)
    {
        this._range = range;
    }

    public int getInitiative()
    {
        return _initiative;
    }

    public void setInitiative(int initiative)
    {
        this._initiative = initiative;
    }

    public int getAccuracy()
    {
        return _accuracy;
    }

    public void setAccuracy(int accuracy)
    {
        this._accuracy = accuracy;
    }

    public int getMineSweep()
    {
        return _mineSweep;
    }

    public void setMineSweep(int mineSweep)
    {
        this._mineSweep = mineSweep;
    }

    public bool isHitsAllTargets()
    {
        return _hitsAllTargets;
    }

    public void setHitsAllTargets(bool hitsAllTargets)
    {
        this._hitsAllTargets = hitsAllTargets;
    }

    public bool isDamageShieldsOnly()
    {
        return _damageShieldsOnly;
    }

    public void setDamageShieldsOnly(bool damageShieldsOnly)
    {
        this._damageShieldsOnly = damageShieldsOnly;
    }

    public bool isCapitalShipMissle()
    {
        return _capitalShipMissle;
    }

    public void setCapitalShipMissle(bool capitalShipMissle)
    {
        this._capitalShipMissle = capitalShipMissle;
    }

    public int getPacketSpeed()
    {
        return _packetSpeed;
    }

    public void setPacketSpeed(int packetSpeed)
    {
        this._packetSpeed = packetSpeed;
    }

    public int getSafeHullMass()
    {
        return _safeHullMass;
    }

    public void setSafeHullMass(int safeHullMass)
    {
        this._safeHullMass = safeHullMass;
    }

    public int getSafeRange()
    {
        return _safeRange;
    }

    public void setSafeRange(int safeRange)
    {
        this._safeRange = safeRange;
    }

    public int getMaxHullMass()
    {
        return _maxHullMass;
    }

    public void setMaxHullMass(int maxHullMass)
    {
        this._maxHullMass = maxHullMass;
    }

    public int getMaxRange()
    {
        return _maxRange;
    }

    public void setMaxRange(int maxRange)
    {
        this._maxRange = maxRange;
    }

    public int getMiningRate()
    {
        return _miningRate;
    }

    public void setMiningRate(int miningRate)
    {
        this._miningRate = miningRate;
    }

    public int getTerraformRate()
    {
        return _terraformRate;
    }

    public void setTerraformRate(int terraformRate)
    {
        this._terraformRate = terraformRate;
    }

    public int getMineLayingRate()
    {
        return _mineLayingRate;
    }

    public void setMineLayingRate(int mineLayingRate)
    {
        this._mineLayingRate = mineLayingRate;
    }

    public int getMaxSpeed()
    {
        return _maxSpeed;
    }

    public void setMaxSpeed(int maxSpeed)
    {
        this._maxSpeed = maxSpeed;
    }

    public int getChanceOfHit()
    {
        return _chanceOfHit;
    }

    public void setChanceOfHit(int chanceOfHit)
    {
        this._chanceOfHit = chanceOfHit;
    }

    public int getDamagePerEngine()
    {
        return _damagePerEngine;
    }

    public void setDamagePerEngine(int damagePerEngine)
    {
        this._damagePerEngine = damagePerEngine;
    }

    public int getDamagePerEngineRS()
    {
        return _damagePerEngineRS;
    }

    public void setDamagePerEngineRS(int damagePerEngineRs)
    {
        this._damagePerEngineRS = damagePerEngineRs;
    }

    public int getMinDamagePerFleet()
    {
        return _minDamagePerFleet;
    }

    public void setMinDamagePerFleet(int minDamagePerFleet)
    {
        this._minDamagePerFleet = minDamagePerFleet;
    }

    public int getMinDamagePerFleetRS()
    {
        return _minDamagePerFleetRS;
    }

    public void setMinDamagePerFleetRS(int minDamagePerFleetRs)
    {
        this._minDamagePerFleetRS = minDamagePerFleetRs;
    }

    public TechHullComponent hullSlotType(HullSlotType hullSlotType)
    {
        this._hullSlotType = hullSlotType;
        return this;
    }

    public TechHullComponent mass(int mass)
    {
        this._mass = mass;
        return this;
    }

    public TechHullComponent armor(int armor)
    {
        this._armor = armor;
        return this;
    }

    public TechHullComponent shield(int shield)
    {
        this._shield = shield;
        return this;
    }

    public TechHullComponent cloak(int cloak)
    {
        this._cloak = cloak;
        return this;
    }

    public TechHullComponent cloakUnarmedOnly(bool cloakUnarmedOnly)
    {
        this._cloakUnarmedOnly = cloakUnarmedOnly;
        return this;
    }

    public TechHullComponent torpedoBonus(int torpedoBonus)
    {
        this._torpedoBonus = torpedoBonus;
        return this;
    }

    public TechHullComponent initiativeBonus(int initiativeBonus)
    {
        this._initiativeBonus = initiativeBonus;
        return this;
    }

    public TechHullComponent torpedoJamming(int torpedoJamming)
    {
        this._torpedoJamming = torpedoJamming;
        return this;
    }

    public TechHullComponent reduceMovement(int reduceMovement)
    {
        this._reduceMovement = reduceMovement;
        return this;
    }

    public TechHullComponent reduceCloaking(int reduceCloaking)
    {
        this._reduceCloaking = reduceCloaking;
        return this;
    }

    public TechHullComponent fuelBonus(int fuelBonus)
    {
        this._fuelBonus = fuelBonus;
        return this;
    }

    public TechHullComponent fuelRegenerationRate(int fuelRegenerationRate)
    {
        this._fuelRegenerationRate = fuelRegenerationRate;
        return this;
    }

    public TechHullComponent colonizationModule(bool colonizationModule)
    {
        this._colonizationModule = colonizationModule;
        return this;
    }

    public TechHullComponent cargoBonus(int cargoBonus)
    {
        this._cargoBonus = cargoBonus;
        return this;
    }

    public TechHullComponent movementBonus(int movementBonus)
    {
        this._movementBonus = movementBonus;
        return this;
    }

    public TechHullComponent beamDefense(int beamDefense)
    {
        this._beamDefense = beamDefense;
        return this;
    }

    public TechHullComponent beamBonus(int beamBonus)
    {
        this._beamBonus = beamBonus;
        return this;
    }

    public TechHullComponent scanRange(int scanRange)
    {
        this._scanRange = scanRange;
        return this;
    }

    public TechHullComponent scanRangePen(int scanRangePen)
    {
        this._scanRangePen = scanRangePen;
        return this;
    }

    public TechHullComponent stealCargo(bool stealCargo)
    {
        this._stealCargo = stealCargo;
        return this;
    }

    public TechHullComponent radiating(bool radiating)
    {
        this._radiating = radiating;
        return this;
    }

    public TechHullComponent smart(bool smart)
    {
        this._smart = smart;
        return this;
    }

    public TechHullComponent killRate(int killRate)
    {
        this._killRate = killRate;
        return this;
    }

    public TechHullComponent minKillRate(int minKillRate)
    {
        this._minKillRate = minKillRate;
        return this;
    }

    public TechHullComponent structureKillRate(int structureKillRate)
    {
        this._structureKillRate = structureKillRate;
        return this;
    }

    public TechHullComponent unterraformRate(int unterraformRate)
    {
        this._unterraformRate = unterraformRate;
        return this;
    }

    public TechHullComponent power(int power)
    {
        this._power = power;
        return this;
    }

    public TechHullComponent range(int range)
    {
        this._range = range;
        return this;
    }

    public TechHullComponent initiative(int initiative)
    {
        this._initiative = initiative;
        return this;
    }

    public TechHullComponent accuracy(int accuracy)
    {
        this._accuracy = accuracy;
        return this;
    }

    public TechHullComponent mineSweep(int mineSweep)
    {
        this._mineSweep = mineSweep;
        return this;
    }

    public TechHullComponent hitsAllTargets(bool hitsAllTargets)
    {
        this._hitsAllTargets = hitsAllTargets;
        return this;
    }

    public TechHullComponent damageShieldsOnly(bool damageShieldsOnly)
    {
        this._damageShieldsOnly = damageShieldsOnly;
        return this;
    }

    public TechHullComponent capitalShipMissle(bool capitalShipMissle)
    {
        this._capitalShipMissle = capitalShipMissle;
        return this;
    }

    public TechHullComponent packetSpeed(int packetSpeed)
    {
        this._packetSpeed = packetSpeed;
        return this;
    }

    public TechHullComponent safeHullMass(int safeHullMass)
    {
        this._safeHullMass = safeHullMass;
        return this;
    }

    public TechHullComponent safeRange(int safeRange)
    {
        this._safeRange = safeRange;
        return this;
    }

    public TechHullComponent maxHullMass(int maxHullMass)
    {
        this._maxHullMass = maxHullMass;
        return this;
    }

    public TechHullComponent maxRange(int maxRange)
    {
        this._maxRange = maxRange;
        return this;
    }

    public TechHullComponent miningRate(int miningRate)
    {
        this._miningRate = miningRate;
        return this;
    }

    public TechHullComponent terraformRate(int terraformRate)
    {
        this._terraformRate = terraformRate;
        return this;
    }

    public TechHullComponent mineLayingRate(int mineLayingRate)
    {
        this._mineLayingRate = mineLayingRate;
        return this;
    }

    public TechHullComponent maxSpeed(int maxSpeed)
    {
        this._maxSpeed = maxSpeed;
        return this;
    }

    public TechHullComponent chanceOfHit(int chanceOfHit)
    {
        this._chanceOfHit = chanceOfHit;
        return this;
    }

    public TechHullComponent damagePerEngine(int damagePerEngine)
    {
        this._damagePerEngine = damagePerEngine;
        return this;
    }

    public TechHullComponent damagePerEngineRS(int damagePerEngineRS)
    {
        this._damagePerEngineRS = damagePerEngineRS;
        return this;
    }

    public TechHullComponent minDamagePerFleet(int minDamagePerFleet)
    {
        this._minDamagePerFleet = minDamagePerFleet;
        return this;
    }

    public TechHullComponent minDamagePerFleetRS(int minDamagePerFleetRS)
    {
        this._minDamagePerFleetRS = minDamagePerFleetRS;
        return this;
    }
}
