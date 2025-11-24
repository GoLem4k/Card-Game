using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsTable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardManager cardsManager;
    public void OnPointerEnter(PointerEventData eventData)
    {
        cardsManager.hoveringMenu = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardsManager.hoveringMenu = null;
    }
}