using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public int count;
    public TextMeshProUGUI countText;

    public GameObject winTextObject;

    public RawImage frontScreenImage;
    public GameObject dialogueBox;
    public GameObject instructionBox;
    public SolitudeBar solitudePanel;


    void Start()
    {
        winTextObject.gameObject.SetActive(false);
        count = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("familyPhoto"))
        {
            frontScreenImage.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(true);
            StartCoroutine(DisableUIAfterDelay(5f));

            other.gameObject.SetActive(false);

            Renderer objectRenderer = other.gameObject.GetComponent<Renderer>();
            if (objectRenderer != null && objectRenderer.material.mainTexture != null)
            {
                frontScreenImage.texture = objectRenderer.material.mainTexture;
            }

            solitudePanel.solitude = Math.Min(solitudePanel.solitude + 35, 100);
        }
    }

    void Update()
    {
        if (count >= 5)
        {
            winTextObject.gameObject.SetActive(true);
        }
    }

    public void IncrementScore()
    {
        count++;
    }

    public void DecrementScore()
    {
        count = Mathf.Max(0, count - 1);
    }

    private IEnumerator DisableUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        frontScreenImage.gameObject.SetActive(false);
        dialogueBox.gameObject.SetActive(false);
        instructionBox.gameObject.SetActive(false);
    }
}