using UnityEngine;
using UnityEngine.UI;  // Needed for Buttons
using System;
using System.Collections.Generic;
using System.Collections; // This is required
using TMPro; // needed for text mesh pro text input
using Mirror;



public class GameLoopHandler
{

    Table table;

    public GameLoopHandler(Table table)
    {
        this.table = table;
    }


    public void startGameLoop()
    {
        Debug.Log("Game loop started.");
        // Initialize game state, players, etc.
    }

}
