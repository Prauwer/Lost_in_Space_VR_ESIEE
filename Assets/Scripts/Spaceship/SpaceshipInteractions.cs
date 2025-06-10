using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipInteraction : MonoBehaviour
{
    public int score = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void IncrementScore()
    {
        score++;
        Debug.Log("Score augmenté : " + score);
    }

    public void DecrementScore()
    {
        score = Mathf.Max(0, score - 1); // Évite les valeurs négatives
        Debug.Log("Score diminué : " + score);
    }
}
