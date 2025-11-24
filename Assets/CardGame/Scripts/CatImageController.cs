using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatImageController : MonoBehaviour
{
    private Canvas canvas;
    private Image img;
    
    public List<Sprite> CatSprites = new List<Sprite>();
    public float fadeInTime = 0.15f;
    public float fadeOutTime = 0.4f;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        transform.SetParent(canvas.transform, false); // Убирает предупреждение
    }

    private void Start()
    {
        img = GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f); // прозрачная
        img.sprite = CatSprites[Random.Range(0, CatSprites.Count)];
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        // Плавное появление
        float t = 0;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            float alpha = t / fadeInTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }

        // Плавное исчезновение
        t = 0;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            float alpha = 1 - (t / fadeOutTime);
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}