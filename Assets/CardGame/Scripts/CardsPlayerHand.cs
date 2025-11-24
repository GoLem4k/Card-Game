using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CardsPlayerHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardManager cardManager;
    public GameObject selectedCard;
    public List<GameObject> Cards = new List<GameObject>();

    private void Update()
    {
        if (cardManager.selectedCard)
            selectedCard = cardManager.selectedCard; 
        else selectedCard = null;

        if (selectedCard)
        {
            for (int i = 0; i < Cards.Count; i++)
            {
                if (selectedCard.transform.position.x > Cards[i].transform.position.x)
                {
                    if (selectedCard.transform.parent.GetSiblingIndex() < Cards[i].transform.parent.GetSiblingIndex())
                    {
                        SwapCards(selectedCard, Cards[i]);
                        break;
                    }
                }
                if (selectedCard.transform.position.x < Cards[i].transform.position.x)
                {
                    if (selectedCard.transform.parent.GetSiblingIndex() > Cards[i].transform.parent.GetSiblingIndex())
                    {
                        SwapCards(selectedCard, Cards[i]);
                        break;
                    }
                }
            }
        }
    }

    public void SwapCards(GameObject currentCard, GameObject targetCard)
    {
        Transform currentCardParent = currentCard.transform.parent;
        Transform targetCardParent = targetCard.transform.parent;
        
        currentCard.transform.SetParent(targetCardParent);
        targetCard.transform.SetParent(currentCardParent);

        if (!currentCard.transform.GetComponent<Card>().IsDragging)
        {
            currentCard.transform.localPosition = Vector2.zero;
        }

        targetCard.transform.localPosition = Vector2.zero;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        cardManager.hoveringMenu = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardManager.hoveringMenu = null;
    }
}
