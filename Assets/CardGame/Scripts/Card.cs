using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsDragging;
    public bool CanDrag;
    public bool Played;
    private Canvas canvas;
    public CardManager cardManager;
    public GameObject target;

    public int value;
    public Color color;
    
    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        cardManager.cardsPlayerHand.Cards.Add(gameObject);
    }

    private void Update()
    {
        if (Played || !CanDrag)
        {
            StartCoroutine(WaitAndDestroy(10));
        }
    }

    private IEnumerator WaitAndDestroy(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Destroy(target);
            Destroy(transform.parent.gameObject);
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if (CanDrag)
        {
            cardManager.selectedCard = gameObject;
            target.transform.SetAsLastSibling();
            IsDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData){
        Debug.Log("OnDrag");
        if (CanDrag)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,
                Input.mousePosition, canvas.worldCamera, out position);
            transform.position = canvas.transform.TransformPoint(position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        // Если мышь не над зоной меню — возвращаем карту
        if (cardManager.hoveringMenu == null)
        {
            cardManager.selectedCard = null;
            transform.position = transform.parent.position;
            IsDragging = false;
            return;
        }

        string menuName = cardManager.hoveringMenu.name;

        if (menuName.Contains("Discard"))
        {
            ProcessDiscard();
        }
        else if (menuName.Contains("Table"))
        {
            ProcessTable();
        }
        else
        {
            cardManager.selectedCard = null;
            transform.position = transform.parent.position;
        }

        IsDragging = false;
    }
    
    private void ProcessDiscard()
    {
        CanDrag = false;
        cardManager.selectedCard = null;

        transform.parent.position = cardManager.hoveringMenu.transform.position;
        transform.parent.SetParent(cardManager.hoveringMenu.transform);

        transform.position = transform.parent.position;

        cardManager.cardsPlayerHand.Cards.Remove(gameObject);
    }

    private void ProcessTable()
    {
        CanDrag = false;
        Played = true;
        cardManager.selectedCard = null;

        transform.parent.position = cardManager.hoveringMenu.transform.position;
        transform.parent.SetParent(cardManager.hoveringMenu.transform);

        transform.position = transform.parent.position;

        cardManager.cardsPlayerHand.Cards.Remove(gameObject);
    }


}
