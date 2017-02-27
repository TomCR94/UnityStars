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

    public static void colonizeNonPlanet(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has attempted to colonize a waypoint with no Planet.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.ColonizeNonPlanet, text));

    }

    public static void colonizeOwnedPlanet(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has attempted to colonize a planet that is already inhabited.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.ColonizeOwnedPlanet, text));

    }

    public static void colonizeWithNoModule(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has attempted to colonize a planet without a colonization module.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.ColonizeWithNoColonizationModule, text));

    }

    public static void colonizeWithNoColonists(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has attempted to colonize a planet without bringing any colonists.", fleet.getName());
        player.getMessages().Add(new Message(MessageType.ColonizeWithNoColonists, text));
    }

    public static void planetColonized(Player player, Planet planet)
    {
        string text = string.Format("Your colonists are now in control of {0}", planet.getName());
        player.getMessages().Add(new Message(MessageType.PlanetColonized, text, planet));
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
                text = string.Format("You have found a new habitable planet.  Your colonists will grow by up {0}% per year if you colonize {1}", growth,
                                     planet.getName());
            }
            else
            {
                text = string.Format("You have found a new planet which unfortunately is not habitable by you.  {0}% of your colonists will die per year if you colonize {1}",
                                     -growth, planet.getName());
            }
        }

        player.getMessages().Add(new Message(MessageType.PlanetDiscovery, text, planet));
    }

    public static void fleetCompletedAssignedOrders(Player player, Fleet fleet)
    {
        string text = string.Format("{0} has completed its assigned orders", fleet.getName());
        player.getMessages().Add(new Message(MessageType.FleetOrdersComplete, text, fleet));
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
