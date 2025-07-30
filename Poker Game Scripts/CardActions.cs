/*
  7-28-25
Here is the poker program so far. It's currently pretty unintuitive and 
annoying to work with but I'm working on organizing the game logic to 
make it more wieldy. Initially my thought process was that the main game loop
could be somewhat messy and I can have fun and try not to be a perfectionist, but as
I make more progress I need to consider how difficult it'll be to add networking on 
top of what I currently have. 

I've included too much code in this CardActions.cs file so I'll have to re-organize 
at some point to make it more readable and easy to work with for others or myself in 
the future of course. However, I don't plan on outsourcing any work.

So far I have buttons and players take turns making their move using the buttons. After that the 
next player at the table gets to make their move. There is not yet a board 
of cards that come out after players make their actions.


Features to add:
-Vote to kick
-"Show me one card?"
-"Run it once or twice?"
-Chat
-Voice chat
-Player stats
-Player notes
-Percentage to win
-
*/


using UnityEngine;
using UnityEngine.UI;  // Needed for Buttons
using System;
using System.Collections.Generic;
using System.Collections; // This is required
using TMPro; // needed for text mesh pro text input
using Mirror;


public class Card
{
    public char suit { get; set; }         //'S', 'H', 'D', 'C'
    public int rank { get; set; }          //2-14
    public bool isAce { get; set; }
    public int positionInDeck { get; set; }
    public GameObject cardObject { get; set; }

    public Card(char suit, int rank, int positionInDeck, GameObject cardObject)
    {
        this.suit = suit;
        this.rank = rank;
        this.isAce = (rank == 14 || rank == 1);
        this.positionInDeck = positionInDeck;
        this.cardObject = cardObject;
    }
}

public class Player
{
    public Card card1 { get; set; }
    public Card card2 { get; set; }

    public bool madeAction { get; set; }

    public float stackSize { get; set; }

    public int tablepos { get; set; } //(position)

    public Player(Card firstCard, Card secondCard, int pos, bool madeAction)
    {
        card1 = firstCard;
        card2 = secondCard;
        tablepos = pos;
        this.madeAction = madeAction;
    }
}

public class Board
{

    Card flop1 { get; set; }
    Card flop2 { get; set; }
    Card flop3 { get; set; }

    Card turn { get; set; }

    Card river { get; set; }

}

public class Table
{

    public int playerCount { get; set; }

    public List<Player> players { get; set; }

    int postion { get; set; }

    public Table(List<Player> playersAtTable)
    {
        this.players = playersAtTable;

        this.playerCount = players.Count;
    }

}

public class Action
{
    string[] facingAction { get; set; }




}


public class CardActions : MonoBehaviour
{

    //buttons and other inputs:
    public Button dealButton;
    public Button foldButton;
    public Button checkButton;
    public Button callButton;
    public Button raiseButton;
    public Button betButton;

    bool handBegun = false;


    public TMP_InputField playerCountInput;


    List<Card> deck = new List<Card>();//list for cards in the deck


    Table table;

    public GameLoopHandler gameLoop;

    int playerCount = 7;

    void Start()
    {




        hideButtons();






















        Debug.Log("Creating cards...");

        string[] suits = { "spades", "hearts", "diamonds", "clubs" };
        char[] suitChars = { 'S', 'H', 'D', 'C' };

        string[] ranks = {
                "ace", "2", "3", "4", "5", "6", "7",
                "8", "9", "10", "jack", "queen", "king"
                };

        int position = 1;

        for (int s = 0; s < suits.Length; s++)
        {
            for (int r = 0; r < ranks.Length; r++)
            {
                string objectName = $"{ranks[r]}_of_{suits[s]}_0";
                GameObject obj = GameObject.Find(objectName);

                if (obj == null)
                {
                    Debug.LogError($"GameObject '{objectName}' not found!");
                    continue;
                }

                int rankValue = r + 1;

                Card newCard = new Card(suitChars[s], rankValue, position, obj);

                deck.Add(newCard);



                obj.GetComponent<Renderer>().enabled = false; // Makes cards invisible


                position++;
            }
        }



        dealButton = GameObject.Find("DealButton").GetComponent<Button>();

        dealButton.onClick.AddListener(() => beginHand(playerCount));


        Camera.main.transform.position = new Vector3(0, 0, -10);
        Camera.main.orthographic = true;

        //clientFacingBet();

    }


    void Update()
    {

        //STAGES:
        //-preflop(1)
        //-flop(2)
        //-turn(3)
        //-river(4)

        // Check if the player count input field has changed


        //int stage = 1;


        /*if (handBegun)
        {
            Debug.Log("Hand Running!");

            foreach(Player p in table.players)
            {
                Debug.Log("Player " + p.tablepos + " has cards: " + p.card1.cardObject.name + " and " + p.card2.cardObject.name);
            }

        }
        else
        {
            Debug.Log("NOPE");
        }*/

        /*foreach (Player p in table.players)
        {
            if (p.madeAction)
            {
                Debug.Log("Player " + p.tablepos + " has made an action.");
            }
            else
            {
                Debug.Log("Player " + p.tablepos + " has NOT made an action.");
            }

        }*/
    }




    void ShuffleDeck()
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int randIndex = UnityEngine.Random.Range(0, i + 1); // inclusive of i by adding 1
            (deck[i], deck[randIndex]) = (deck[randIndex], deck[i]);
        }
    }




    void beginHand(int numberOfPlayers)
    {
        handBegun = true;



        Debug.Log("Deal button was clicked!");

        foreach (Card c in deck)
        {
            c.cardObject.GetComponent<Renderer>().enabled = false;
        }



        /*foreach (Card c in deck)
        {
            Debug.Log("Card: " + c.cardObject.name + ", Position: " + c.positionInDeck);

        }*/





        ShuffleDeck();//shuffle the deck every hand


        List<Player> players = new List<Player>();


        int startOf_flop = 0;//track number of cards dealt to know when the flop starts afterwards

        //create players and give them cards from the deck based on how many players are in the game:
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Player p = new Player(deck[i], deck[i + numberOfPlayers], i, false);
            renderCardstoPlayers(p, i + 1);
            startOf_flop += 2; // each player gets 2 cards
            players.Add(p);
        }


        //DEBUG
        //Debug.Log("Dealt " + cardsDealt + " cards to " + numberOfPlayers + " players.");

        this.table = new Table(players);






        showAllButtons();










        table.players.ForEach(p => p.madeAction = false);




        foreach (Player p in table.players)
        {
            Debug.Log("ALL PLAYERS:" + p.card1.cardObject);
            Debug.Log("ALL PLAYERS:" + p.card2.cardObject);

        }




        this.gameLoop = new GameLoopHandler(table, startOf_flop, deck);

        gameLoop.startGameLoop();



        addButtonListener(players, table);

        showPlayerInfo(players, table);


        return;
    }


    void renderBoardCards(List<Card> deck)
    {

    }

    public void renderCardstoPlayers(Player player, int position)
    {

        float x, y, z;

        float offset = 1.2f; // Offset for card 2 from card 1

        //creates coordinates for hands based on player position
        if (position == 1)
        {
            x = -0.6f; y = -2f; z = 0;
        }
        else if (position == 2)
        {
            x = -4.5f; y = -2f; z = 0;
        }
        else if (position == 3)
        {
            x = -6.2f; y = 1f; z = 0;
        }
        else if (position == 4)
        {
            x = -2.8f; y = 2f; z = 0;
        }
        else if (position == 5)
        {
            x = 1f; y = 2f; z = 0;
        }
        else if (position == 6)
        {
            x = 4.8f; y = .8f; z = 0;
        }
        else // position == 7
        {
            x = 3.3f; y = -2f; z = 0;
        }




        player.card1.cardObject.SetActive(true);
        player.card1.cardObject.GetComponent<Renderer>().enabled = true;

        SpriteRenderer sr1 = player.card1.cardObject.GetComponent<SpriteRenderer>();
        sr1.sortingLayerName = "Cards";
        sr1.sortingOrder = 2;

        player.card1.cardObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        player.card1.cardObject.transform.position = new Vector3(x, y, z);


        // CARD 2
        player.card2.cardObject.SetActive(true);
        player.card2.cardObject.GetComponent<Renderer>().enabled = true;

        SpriteRenderer sr2 = player.card2.cardObject.GetComponent<SpriteRenderer>();
        sr2.sortingLayerName = "Cards";
        sr2.sortingOrder = 2;

        player.card2.cardObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        player.card2.cardObject.transform.position = new Vector3(x + offset, y, z);
    }

    // checks if the card is visible in the camera view after one frame
    private IEnumerator CheckVisibilityAfterFrame(GameObject card)
    {
        yield return new WaitForEndOfFrame(); // Wait for one frame to pass

        bool isVisible = card.GetComponent<Renderer>().isVisible;

        if (!isVisible)
            Debug.LogWarning(card.name + " is NOT visible in camera view.");
        else
            Debug.Log(card.name + " is visible in camera view.");
    }


    public void showPlayerInfo(List<Player> players, Table table)
    {
        Debug.Log("Table created with " + table.playerCount + " players.");

        foreach (Player p in players)
        {
            Debug.Log("Player " + p.tablepos + " has cards: " + p.card1.cardObject.name + " and " + p.card2.cardObject.name);
        }
    }

    public void renderButtonstoPlayer(int tablePosition, string previousAction)//the player faces a bet so their options are fold, call and raise. this functions renders those buttons
    {
        renderButtonsAbstraction(previousAction);

    }

    public void renderButtonsAbstraction(string previousAction)
    {
        switch (previousAction)
        {
            case "check":
                checkButton.image.enabled = true;
                checkButton.GetComponentInChildren<Text>().enabled = true;

                betButton.image.enabled = true;
                betButton.GetComponentInChildren<Text>().enabled = true;

                break;
            case "bet":
                foldButton.image.enabled = true;
                foldButton.GetComponentInChildren<Text>().enabled = true;

                callButton.image.enabled = true;
                callButton.GetComponentInChildren<Text>().enabled = true;

                raiseButton.image.enabled = true;
                raiseButton.GetComponentInChildren<Text>().enabled = true;

                break;
            case "fold":
                foldButton.image.enabled = true;
                foldButton.GetComponentInChildren<Text>().enabled = true;

                callButton.image.enabled = true;
                callButton.GetComponentInChildren<Text>().enabled = true;

                raiseButton.image.enabled = true;
                raiseButton.GetComponentInChildren<Text>().enabled = true;
                break;
            case "call":
                foldButton.image.enabled = true;
                foldButton.GetComponentInChildren<Text>().enabled = true;

                callButton.image.enabled = true;
                callButton.GetComponentInChildren<Text>().enabled = true;

                raiseButton.image.enabled = true;
                raiseButton.GetComponentInChildren<Text>().enabled = true;
                break;
            case "raise":
                foldButton.image.enabled = true;
                foldButton.GetComponentInChildren<Text>().enabled = true;

                callButton.image.enabled = true;
                callButton.GetComponentInChildren<Text>().enabled = true;

                raiseButton.image.enabled = true;
                raiseButton.GetComponentInChildren<Text>().enabled = true;
                break;
            default:

                break;

        }
    }


    public void hideButtons()
    {
        foldButton = GameObject.Find("foldButton").GetComponent<Button>();
        foldButton.image.enabled = false;
        foldButton.GetComponentInChildren<Text>().enabled = false;

        callButton = GameObject.Find("callButton").GetComponent<Button>();
        callButton.image.enabled = false;
        callButton.GetComponentInChildren<Text>().enabled = false;

        raiseButton = GameObject.Find("raiseButton").GetComponent<Button>();
        raiseButton.image.enabled = false;
        raiseButton.GetComponentInChildren<Text>().enabled = false;

        checkButton = GameObject.Find("checkButton").GetComponent<Button>();
        checkButton.image.enabled = false; ;
        checkButton.GetComponentInChildren<Text>().enabled = false;

        betButton = GameObject.Find("betButton").GetComponent<Button>();
        betButton.image.enabled = false;
        betButton.GetComponentInChildren<Text>().enabled = false;
    }

    public void showAllButtons()
    {
        betButton.image.enabled = true;
        betButton.GetComponentInChildren<Text>().enabled = true;

        checkButton.image.enabled = true;
        checkButton.GetComponentInChildren<Text>().enabled = true;

        foldButton.image.enabled = true;
        foldButton.GetComponentInChildren<Text>().enabled = true;

        callButton.image.enabled = true;
        callButton.GetComponentInChildren<Text>().enabled = true;

        raiseButton.image.enabled = true;
        raiseButton.GetComponentInChildren<Text>().enabled = true;
    }


    public void addButtonListener(List<Player> players, Table table)
    {

        MoveCards move = gameObject.AddComponent<MoveCards>();
        move.table = table;

        int whoseTurn = 1;



        gameLoop.printAllPlayerDetails(table);







        checkButton.onClick.AddListener(() =>
        {


            hideButtons();
            renderButtonstoPlayer(whoseTurn, "check");

            Player p = findCorrespondingPlayer(whoseTurn, table);
            move.highlightCards(p);
            table.players[whoseTurn - 1].madeAction = true;

            if (whoseTurn == table.players.Count)
            {
                whoseTurn = 1;
            }
            else
            {
                whoseTurn++;
            }

        });

        betButton.onClick.AddListener(() =>
        {


            hideButtons();

            renderButtonstoPlayer(whoseTurn, "bet");

            Player p = findCorrespondingPlayer(whoseTurn, table);

            move.highlightCards(p);
            table.players[whoseTurn - 1].madeAction = true;




            if (whoseTurn == table.players.Count)
            {
                whoseTurn = 1;
            }
            else
            {
                whoseTurn++;
            }

            //Debug.Log("RUNNING!!!!!!!");
            gameLoop.setAllPlayerActionsToFalse(table);


        });

        foldButton.onClick.AddListener(() =>
        {



            hideButtons();

            renderButtonstoPlayer(whoseTurn, "fold");

            Player p = findCorrespondingPlayer(whoseTurn, table);

            move.highlightCards(p);
            table.players[whoseTurn - 1].madeAction = true;



            if (whoseTurn == table.players.Count)
            {
                whoseTurn = 1;
            }
            else
            {
                whoseTurn++;
            }



        });

        callButton.onClick.AddListener(() =>
        {




            hideButtons();

            renderButtonstoPlayer(whoseTurn, "call");

            Player p = findCorrespondingPlayer(whoseTurn, table);

            move.highlightCards(p);
            table.players[whoseTurn - 1].madeAction = true;



            if (whoseTurn == table.players.Count)
            {
                whoseTurn = 1;
            }
            else
            {
                whoseTurn++;
            }


        });

        raiseButton.onClick.AddListener(() =>
        {


            hideButtons();

            renderButtonstoPlayer(whoseTurn, "raise");

            Player p = findCorrespondingPlayer(whoseTurn, table);

            move.highlightCards(p);
            table.players[whoseTurn - 1].madeAction = true;



            if (whoseTurn == table.players.Count)
            {
                whoseTurn = 1;
            }
            else
            {
                whoseTurn++;
            }


            //Debug.Log("RUNNING!!!!!!!");

            gameLoop.setAllPlayerActionsToFalse(table);

        });
    }


    Player findCorrespondingPlayer(int tablePosition, Table table)
    {
        foreach (Player p in table.players)
        {
            //Debug.Log("Comparing player at position " + (p.tablepos + 1) + " with requested position " + tablePosition + ". Found player: " + p.card1.cardObject.name + " and " + p.card2.cardObject.name);

            if (p.tablepos + 1 == tablePosition)
            {
                //Debug.Log("PLAYER FOUND" + "returning player with: " + p.card1.cardObject + " and " + p.card2.cardObject);
                return p;
            }
        }

        //Debug.LogError("No player found at position " + tablePosition);
        return null; // or throw an exception
    }





}