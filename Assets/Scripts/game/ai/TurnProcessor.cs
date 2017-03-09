using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TurnProcessor
{

    /**
     * Initialize a turn processor with a player instance
     * @param player The player to process turns for
     */
    void init(Player player, Game game);

    /**
     * Called to process a player's turn
     */
    void process();


}