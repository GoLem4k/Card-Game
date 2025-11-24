using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool IsDragging;
    public bool CanDrag;
    public bool Played;
    public bool Discarded;
    private Canvas canvas;
    public CardManager cardManager;
    public GameObject target;

    public string letter;
    public Color color;
    
    
    
    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        cardManager = GameObject.Find("CardManager").GetComponent<CardManager>();
        cardManager.cardsPlayerHand.Cards.Add(gameObject);
    }

    private void Update()
    {
        if (Discarded && gameObject != cardManager.lastDiscard)
        {
            DestroyAfter(1);
        }
    }

    public void DestroyAfter(float time)
    {
        StartCoroutine(WaitAndDestroy(time));
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
        //Debug.Log("OnBeginDrag");
        if (CanDrag)
        {
            cardManager.selectedCard = gameObject;
            target.transform.SetAsLastSibling();
            IsDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData){
        //Debug.Log("OnDrag");
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
        //Debug.Log("OnEndDrag");
        if (!CanDrag) return;
        // Если мышь не над зоной меню — возвращаем карту
        if (cardManager.hoveringMenu == null)
        {
            cardManager.selectedCard = null;
            transform.position = transform.parent.position;
            IsDragging = false;
            return;
        }

        string menuName = cardManager.hoveringMenu.name;

        if (menuName.Contains("Discard") && cardManager.discardCount > 0)
        {
            ProcessDiscard();
        }
        else if (menuName.Contains("CardSlot"))
        {
            switch (menuName)
            {
                case "CardSlot1":
                    CheckSlot(0);
                    break;
                case "CardSlot2":
                    CheckSlot(1);
                    break;
                case "CardSlot3":
                    CheckSlot(2);
                    break;
            }
        }
        else
        {
            cardManager.selectedCard = null;
            transform.position = transform.parent.position;
        }

        IsDragging = false;
    }

    public void CheckSlot(int i)
    {
        if (cardManager.cardSlots[i] == null)
        {
            cardManager.cardSlots[i] = gameObject;
            ProcessTable();
        }
        else
        {
            cardManager.selectedCard = null;
            transform.position = transform.parent.position;
        }
    }
    
    private void ProcessDiscard()
    {
        cardManager.lastDiscard = gameObject;
        Discarded = true;
        CanDrag = false;
        cardManager.selectedCard = null;

        transform.parent.position = cardManager.hoveringMenu.transform.position;
        transform.parent.SetParent(cardManager.hoveringMenu.transform);

        transform.position = transform.parent.position;

        cardManager.cardsPlayerHand.Cards.Remove(gameObject);

        cardManager.OnDiscard();
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
