using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CardFace : MonoBehaviour
{
    public GameObject target;
    
    public float rotationSpeed;
    public float rotationAmount;

    private Vector3 _rotation;
    private Vector3 _movement;

    
    private float randomRot;

    private void Start()
    {
        randomRot = Random.Range(-rotationAmount, rotationAmount);
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, target.transform.position, Time.deltaTime * 25);

        if (!target.GetComponent<Card>().Played)
        {
            Vector3 pos = transform.position - target.transform.position;
            Vector3 movementRotation;

            _movement = Vector3.Lerp(_movement, pos, Time.deltaTime * 25);
            
            movementRotation = _movement;

            _rotation = Vector3.Lerp(_rotation, movementRotation, rotationSpeed * Time.deltaTime);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                Math.Clamp(movementRotation.x, -rotationAmount, rotationAmount));
        }
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, randomRot);
        }
    }
}
