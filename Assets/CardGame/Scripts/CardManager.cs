using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public GameObject catImage;
    public GameObject selectedCard;
    public GameObject hoveringMenu;
    public GameObject lastDiscard;

    public CardsPlayerHand cardsPlayerHand;

    public GameObject cardsParent;
    public GameObject CardFace;

    public List<Color> Colors = new List<Color>();
    public List<string> Letters = new List<string>();
    public int deckMultiplier = 1;
    private int deckSize;
    public List<int> LettersCounters = new List<int>();
    public List<int> ColorsCounters = new List<int>();
    
    public int discardCount;
    public TextMeshProUGUI discardText;

    public List<GameObject> cardSlots = new List<GameObject>(3);

    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI deckText;

    public ResultManager resManager;
    
    private void Start()
    {
        for (int i = 0; i < LettersCounters.Count; i++)
        {
            LettersCounters[i] = 4 * deckMultiplier;
        }
        for (int i = 0; i < ColorsCounters.Count; i++)
        {
            ColorsCounters[i] = 3 * deckMultiplier;
        }
        discardCount = deckMultiplier;
        discardText.text = "ОСТАЛОСЬ СБРОСОВ: " + discardCount.ToString();
        deckSize = 3 * 4 * deckMultiplier;
        deckText.text = "КАРТ В КОЛОДЕ: " + deckSize.ToString();
        AddCard();
        AddCard();
        AddCard();
        AddCard();
        AddCard();
    }

    public void ReduceDeck()
    {
        deckSize -= 1;
        deckText.text = "КАРТ В КОЛОДЕ: " + deckSize.ToString();
    }

    private string GetRandomLetter()
    {
        // Собираем индексы доступных букв
        List<int> available = new List<int>();
        for (int i = 0; i < LettersCounters.Count; i++)
        {
            if (LettersCounters[i] > 0)
                available.Add(i);
        }

        // Если нет доступных — возвращаем заглушку
        if (available.Count == 0)
            return "?";

        // Выбираем случайный из доступных
        int randomIndex = available[Random.Range(0, available.Count)];
        LettersCounters[randomIndex]--;

        return Letters[randomIndex];
    }

    
    private Color GetRandomColor()
    {
        List<int> available = new List<int>();
        for (int i = 0; i < ColorsCounters.Count; i++)
        {
            if (ColorsCounters[i] > 0)
                available.Add(i);
        }

        if (available.Count == 0)
            return Color.clear;

        int randomIndex = available[Random.Range(0, available.Count)];
        ColorsCounters[randomIndex]--;

        return Colors[randomIndex];
    }

    private void Update()
    {
        if (cardSlots[0] != null && cardSlots[1] != null && cardSlots[2] != null)
        {
            Debug.Log(cardSlots[0].GetComponent<Card>().letter + cardSlots[1].GetComponent<Card>().letter + cardSlots[2].GetComponent<Card>().letter);
            if (cardSlots[0].GetComponent<Card>().letter == Letters[0] && cardSlots[1].GetComponent<Card>().letter == Letters[1] &&
                cardSlots[2].GetComponent<Card>().letter == Letters[2])
            {
                AddScore(10);
                if (cardSlots[0].GetComponent<Card>().color == cardSlots[1].GetComponent<Card>().color &&
                    cardSlots[1].GetComponent<Card>().color == cardSlots[2].GetComponent<Card>().color)
                {
                    AddScore(40);
                }

                if (cardSlots[0].GetComponent<Card>().color != cardSlots[1].GetComponent<Card>().color &&
                    cardSlots[1].GetComponent<Card>().color != cardSlots[2].GetComponent<Card>().color &&
                    cardSlots[0].GetComponent<Card>().color != cardSlots[2].GetComponent<Card>().color)
                {
                    AddScore(15);
                }
                Instantiate(catImage);
            }
            else
            {
                AddScore(-25);
            }
            foreach (var card in cardSlots)
            {
                card.GetComponent<Card>().DestroyAfter(0.5f);
            }
            cardSlots[0] = null;
            cardSlots[1] = null;
            cardSlots[2] = null;
        }

        if (deckSize == 0 && cardsPlayerHand.Cards.Count < 1)
        {
            StartCoroutine(WaitAndShowResults(2));
        }
    }
    
    private IEnumerator WaitAndShowResults(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            resManager.ShowResults(score);
        }
    }

    public void AddScore(int s)
    {
        score += s;
        scoreText.text = score.ToString();
    }

    public void AddCard()
    {
        if (deckSize == 0) return;
        if (cardsPlayerHand.transform.childCount < 5)
        {
            GameObject card = Instantiate(cardsParent, cardsPlayerHand.transform);

            string randomLetter = GetRandomLetter();
            Color randomColor = GetRandomColor();
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
                if (img != null) img.color = randomColor;
            }
            else
            {
                Debug.LogError("Не найден дочерний объект 'icon' в CardFace!");
            }
            TextMeshProUGUI tmp = cardFace.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = randomLetter;


            cardComp.color = randomColor;
            cardComp.letter = randomLetter;
            ReduceDeck();
        }
    }

    public void OnDiscard()
    {
        discardCount -= 1;
        discardText.text = "ОСТАЛОСЬ СБРОСОВ: " + discardCount.ToString();
    }
}
