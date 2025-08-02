using UnityEngine;
using UnityEngine.UI;  // Needed for Buttons
using System;
using System.Collections.Generic;
using System.Collections; // This is required
using TMPro; // needed for text mesh pro text input
using Mirror;



public class GameLoopHandler
{

    public Table table;
    int startFlop = 0;
    public List<Card> deck;
    public bool handRunning;


    public GameLoopHandler()
    {

    }

    public GameLoopHandler(Table table, int startFlop, List<Card> deck, bool handRunning)
    {
        this.table = table;
        this.startFlop = startFlop;
        this.deck = deck;
        this.handRunning = handRunning;
    }



    public void startGameLoop(int handPhase)
    {
        Debug.Log("Starting game loop with " + table.players.Count + " players. And with the flop:" + deck[(startFlop + 2)].cardObject + " " + deck[(startFlop + 3)].cardObject + " " + deck[(startFlop + 4)].cardObject);

        /*while (handRunning)
        {


            switch (handPhase)
            {
                case 1:

                    Debug.Log("PREFLOP");
                    break;

                case 2:

                    Debug.Log("FLOP");

                    break;

                case 3:

                    Debug.Log("TURN");

                    break;

                default:

                    Debug.Log("RIVER");

                    break;

            }
        }*/
        // Initialize game state, players, etc.


        //Debug.Log("GAME LOOP ENDED");



    }

    public bool checkifRoundRunning(Table table)
    {
        foreach (Player p in table.players)
        {
            if (p.madeAction == false)
            {
                return true;
            }
        }
        return false;
    }

    public void setAllPlayerActionsToFalse(Table table, Player cur)
    {
        foreach (Player p in table.players)
        {
            if (p != cur)
            {
                Debug.Log("Setting madeAction to false for player at pos: " + p.tablepos + ", instance ID: " + p.GetHashCode());

                p.madeAction = false;
            }

        }
        Debug.Log("All player actions set to false.");
    }


    public void printAllPlayerDetails(Table table)
    {
        Debug.Log("=== Player Details ===");

        for (int i = 0; i < table.players.Count; i++)
        {
            Player p = table.players[i];

            Debug.Log($"Player {i + 1}:");
            Debug.Log($"  Table Position: {p.tablepos}");
            Debug.Log($"  Made Action: {p.madeAction}");

            string card1Str = p.card1 != null ? p.card1.ToString() : "None";
            string card2Str = p.card2 != null ? p.card2.ToString() : "None";
            Debug.Log($"  Card 1: {card1Str}");
            Debug.Log($"  Card 2: {card2Str}");
        }

        Debug.Log("=======================");
    }



    public void progressGame()
    {

    }
}