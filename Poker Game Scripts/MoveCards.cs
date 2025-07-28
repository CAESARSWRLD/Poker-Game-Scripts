using UnityEngine;
using UnityEngine.UI;  // Needed for Buttons
using System;
using System.Collections.Generic;
using System.Collections; // This is required
using TMPro; // needed for text mesh pro text input
using Mirror;

public class MoveCards : MonoBehaviour
{
    public Table table;



    public void highlightCards(Player p)
    {
        if (p == null)
        {
            Debug.LogError("highlightCards: Player is null");
            return;
        }

        if (p.card1 == null || p.card2 == null)
        {
            Debug.LogError("highlightCards: One or both cards are null (card1 or card2)");
            return;
        }

        float offset = .2f;
        float duration = 0.1f;

        Vector3 upOffset = new Vector3(0, offset, 0);

        Debug.Log(p.card1.cardObject + " and " + p.card2.cardObject + " are being highlighted for player");

        // Start both highlight coroutines
        StartCoroutine(HighlightBounce(p.card1.cardObject, upOffset, duration));
        StartCoroutine(HighlightBounce(p.card2.cardObject, upOffset, duration));
    }


    private IEnumerator HighlightBounce(GameObject cardObject, Vector3 offset, float duration)
    {
        if (cardObject == null)
        {
            Debug.LogError("HighlightBounce: cardObject is null");
            yield break;
        }

        Vector3 originalPos = cardObject.transform.position;
        Vector3 targetPos = originalPos + offset;

        // Move up
        yield return StartCoroutine(MoveCard(cardObject, targetPos, duration));

        // Optional pause at top
        yield return new WaitForSeconds(0.05f);

        // Move back down
        yield return StartCoroutine(MoveCard(cardObject, originalPos, duration));
    }



    private IEnumerator MoveCard(GameObject cardObject, Vector3 targetPosition, float duration)
    {
        if (cardObject == null)
        {
            Debug.LogError("MoveCard: cardObject is null");
            yield break;
        }

        Vector3 startPosition = cardObject.transform.position;
        float elapsed = 0;

        while (elapsed < duration)
        {
            cardObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final position is set
        cardObject.transform.position = targetPosition;
    }

}
