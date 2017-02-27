using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStatus
{
    /**
     * Waiting for players to join the game or accept invitations
     */
    WaitingForPlayers,

    /**
     * Generating a turn (no actions allowed)
     */
    GeneratingTurn,

    /**
     * Game is created or turn is generated, now waiting for players to submit their turns  
     */
    WaitingForSubmit
}