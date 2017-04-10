using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Message : AbstractStarsObject_NonMono {


    [SerializeField]
    private MessageType type;

    [SerializeField]
    private string text;


    [SerializeField]
    private MapObject target;

    public Message()
    {
    }

    public Message(MessageType type, string text)
    {
        this.type = type;
        this.text = text;
    }

    public Message(MessageType type, string text, MapObject target)
    {
        this.type = type;
        this.text = text;
        this.target = target;
    }

    public static void info(Player player, string text, params object[] args)
    {
        player.getMessages().Add(new Message(MessageType.Info, string.Format(text, args)));
    }

    public static void homePlanet(Player player, Planet planet)
    {
        string text = string.Format("Your home planet is {0}.  Your people are ready to leave the nest and explore the universe.  Good luck", planet.getName());
        player.getMessages().Add(new Message(MessageType.HomePlanet, text, planet));

    }

    public static void mine(Player player, Planet planet, int num_mines)
    {
        string text = string.Format("You have built {0} mine(s) on {1}.", num_mines, planet.getName());
        player.getMessages().Add(new Message(MessageType.BuiltMine, text, planet));

    }

    public static void factory(Player player, Planet planet, int num_factories)
    {
        string text = string.Format("You have built {0} factory(s) on {1}.", num_factories, planet.getName());
        player.getMessages().Add(new Message(MessageType.BuiltFactory, text, planet));

    }

    public static void defense(Player player, Planet planet, int num_defenses)
    {
        string text = string.Format("You have built {0} defense(s) on {1}.", num_defenses, planet.getName());
        player.getMessages().Add(new Message(MessageType.BuiltDefense, text, planet));

    }

    public static void techLevel(Player player, TechField field, int level, TechField next_field)
    {
        string text = "Your scientists have completed research into Tech Level " + level + " for " + field.ToString() + ".  They will continue their efforts in the " + next_field.ToString() + " field.";
        player.getMessages().Add(new Message(MessageType.GainTechLevel, text));

    }

    public static void coloniseNonPlanet(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has attempted to colonise a waypoint with no Planet.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.ColoniseNonPlanet, text));

    }

    public static void coloniseOwnedPlanet(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has attempted to colonise a planet that is already inhabited.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.ColoniseOwnedPlanet, text));

    }

    public static void coloniseWithNoModule(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has attempted to colonise a planet without a colonization module.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.ColoniseWithNoColonizationModule, text));

    }

    public static void coloniseWithNoColonists(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has attempted to colonise a planet without bringing any colonists.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.ColoniseWithNoColonists, text));
    }

    public static void planetColonised(Player player, Planet planet)
    {
        string text = string.Format("Your colonists are now in control of {0}", planet.getName());
        player.getMessages().Add(new Message(MessageType.PlanetColonised, text, planet));
    }

    public static void fleetScrapped(Player player, Fleet fleet, int num_minerals, Planet planet)
    {
        string text = string.Format("{0} has been dismantled for {1}kT of minerals which have been deposited on {2}.", fleet.getName(), num_minerals,
                                    planet.getName());
        player.getMessages().Add(new Message(MessageType.FleetScrapped, text, planet));
    }

    public static void planetDiscovered(Player player, Planet planet)
    {
        long habValue = player.getRace().getPlanetHabitability(planet.getHab());
        string text;
        if (planet.getOwner() != null && !(planet.getOwner().getID() == player.getID()))
        {
            text = string.Format("You have found a planet occupied by someone else. {0} is currently owned by the {1}", planet.getName(), planet.getOwner()
                                                                                                                                              .getRace()
                                                                                                                                              .getPluralName());
        }
        else
        {
            double growth = (habValue / 100.0) * player.getRace().getGrowthRate();
            if (habValue > 0)
            {
                text = string.Format("You have found a new habitable planet.  Your colonists will grow by up {0}% per year if you colonise {1}", growth,
                                     planet.getName());
            }
            else
            {
                text = string.Format("You have found a new planet which unfortunately is not habitable by you.  {0}% of your colonists will die per year if you colonise {1}",
                                     -growth, planet.getName());
            }
        }

        player.getMessages().Add(new Message(MessageType.PlanetDiscovery, text, planet));
    }

    internal static void Warped(Fleet fleet, Player player, Wormhole wormhole, Wormhole twin)
    {
        string text = string.Format("{0} has warped from {1} to {2}", fleet.getName(), wormhole.getName(), twin.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void fleetCompletedAssignedOrders(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has completed its assigned orders", fleet.getName());
        player.getMessages().Add(new Message(MessageType.FleetOrdersComplete, text, fleet));
    }

    public static void unloadNotInOrbit(Player player, Fleet fleet)
    {
        string text = string.Format("{0} attempted to unload cargo while not in orbit.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void unloadInOrbit(Player player, Fleet fleet, Planet target)
    {
        string text = string.Format("{0} has unloaded its cargo at {1}.", fleet.getName(), target.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void InvadeNotOrbiting(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has orders to invade but the waypoint is not a planet.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void NotAWormHole(Fleet fleet, Player player, MapObject mapObject)
    {
        string text = string.Format("{0} has waypoint orders to stabilize {1} but it is not a wormhole.", fleet.getName(), mapObject.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void AlreadyStabilized(Fleet fleet, Player player, MapObject mapObject)
    {
        string text = string.Format("{0} has waypoint orders to stabilize {1} but it is already stabilized.", fleet.getName(), mapObject.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void Stabilized(Fleet fleet, Player player, MapObject mapObject)
    {
        string text = string.Format("{0} has stabilized {1}.", fleet.getName(), mapObject.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void CannotAffordStabilize(Fleet fleet, Player player, MapObject mapObject)
    {
        string text = string.Format("{0} cannot stabilize {1} as it does not have the required 10000 Ironium.", fleet.getName(), mapObject.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void InvadeNoTroops(Player player, Fleet fleet, Planet target)
    {
        string text = string.Format("{0} has waypoint orders to invade {1} but there are no troops on board.", fleet.getName(), target.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void InvadeAlreadyOwned(Player player, Fleet fleet, Planet target)
    {
        string text = string.Format("{0} has waypoint orders to invade {1} but it is already ours. Troops have joined the local populace.", fleet.getName(), target.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void InvadeStarBase(Player player, Fleet fleet, Planet target)
    {
        string text = string.Format("{0} has waypoint orders to invade {1} but the starbase at {1} would kill all invading troops. Order has been cancelled.", fleet.getName(), target.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void InvadeAttackersSlain(Player player, Planet target, int defendersKilled)
    {
        string text = string.Format("The attackers were slain but {0} colonists were killed in the attack.", defendersKilled);
        player.getMessages().Add(new Message(MessageType.Info, text, target));
        target.getOwner().getMessages().Add(new Message(MessageType.Info, text, target));
    }

    public static void InvadeDefendersSlain(Player player, Planet target, int attackersKilled)
    {
        string text = string.Format("The defenders were slain but {0} troops were killed in the attack.", attackersKilled);
        player.getMessages().Add(new Message(MessageType.Info, text, target));
        target.getOwner().getMessages().Add(new Message(MessageType.Info, text, target));
    }

    public static void terraform(Player player, Planet planet, int amount, int index)
    {
        string[] names = new string[] { "gravity", "temperature", "radiation" };
        string text = string.Format("{0} has been terraformed for {1} by {2} points.", planet.getName(), names[index], amount);
        player.getMessages().Add(new Message(MessageType.Info, text, planet));
    }

    public static void noNeedToTerraform(Player player, Planet planet, int index)
    {
        string[] names = new string[] { "gravity", "temperature", "radiation" };
        string text = string.Format("{0} does not need to be terraformed for {1}.", planet.getName(), names[index]);
        player.getMessages().Add(new Message(MessageType.Info, text, planet));
    }

    public static void InvadeDraw(Player player, Planet target)
    {
        string text = string.Format("Both sides fought to the last and none were left to claim {0}!", target.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, target));
        target.getOwner().getMessages().Add(new Message(MessageType.Info, text, target));
    }

    public static void BombInvalidTarget(Player player, Fleet fleet)
    {
        string text = string.Format("Fleet {0} cannot bomb as it is not targeting a planet", fleet.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void BombNotBomber(Player player, Fleet fleet)
    {
        string text = string.Format("Fleet {0} cannot bomb as it is not a bomber", fleet.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void BombNoOne(Player player, Fleet fleet, Planet target)
    {
        string text = string.Format("Fleet {0} cannot bomb {1} as there are no colonists or there is a Space Station protecting it", fleet.getName(), target.getName());
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void BombKillAll(Player player, Fleet fleet, Planet target)
    {
        string text = string.Format("Fleet {0} has bombed {1} killing all of the colonists", fleet.getName(), target.getName());
        player.getMessages().Add(new Message(MessageType.Info, text));
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public static void BombKillSome(Player player, Fleet fleet, Planet target, int colonists, int defences, int factories, int mines)
    {
        string text = string.Format("Fleet {0} has bombed {1} killing {2} of the colonists and destroying {3} defenses, {4} factories, and {5} mines.", fleet.getName(), target.getName(), colonists, defences, factories, mines);
        player.getMessages().Add(new Message(MessageType.Info, text));
        player.getMessages().Add(new Message(MessageType.Info, text, fleet));
    }

    public MessageType getType()
    {
        return type;
    }

    public void setType(MessageType type)
    {
        this.type = type;
    }

    public string getText()
    {
        return text;
    }

    public void setText(string text)
    {
        this.text = text;
    }

    public MapObject getTarget()
    {
        return target;
    }

    public void setTarget(MapObject target)
    {
        this.target = target;
    }
}
