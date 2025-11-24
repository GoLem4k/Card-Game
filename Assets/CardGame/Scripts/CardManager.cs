using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public GameObject selectedCard;
    public GameObject hoveringMenu;

    public CardsPlayerHand cardsPlayerHand;

    public GameObject cardsParent;
    public GameObject CardFace;

    public List<Color> Colors = new List<Color>();
    public int maxValue = 5;

    private void Start()
    {
        AddCard();
    }

    public void AddCard()
    {
        if (cardsPlayerHand.transform.childCount < 5)
        {
            GameObject card = Instantiate(cardsParent, cardsPlayerHand.transform);

            int randomValue = Random.Range(1, maxValue + 1);
            int randomColor = Random.Range(0, Colors.Count);
            Transform visualsParent = GameObject.Find("CardVisuals")?.transform;

            if (visualsParent == null)
            {
                Debug.LogError("Не найден объект CardVisuals!");
                return;
            }

            GameObject cardFace = Instantiate(CardFace, visualsParent);

            CardFace cardFaceComp = cardFace.GetComponent<CardFace>();
            if (cardFaceComp == null)
            {
                Debug.LogError("На карточке нет компонента CardFace!");
                return;
            }

            Card cardComp = card.GetComponentInChildren<Card>();
            if (cardComp == null)
            {
                Debug.LogError("На карточке нет компонента Card!");
                return;
            }

            cardFaceComp.target = cardComp.gameObject;
            cardComp.target = cardFaceComp.gameObject;

            Transform child = cardFace.transform.Find("icon");
            if (child != null)
            {
                Image img = child.GetComponent<Image>();
                if (img != null) img.color = Colors[randomColor];
            }
            else
            {
                Debug.LogError("Не найден дочерний объект 'icon' в CardFace!");
            }
            TextMeshProUGUI tmp = cardFace.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = randomValue.ToString();


            cardComp.color = Colors[randomColor];
            cardComp.value = randomValue;

        }
    }
}
