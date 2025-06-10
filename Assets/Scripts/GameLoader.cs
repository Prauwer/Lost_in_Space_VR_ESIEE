using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public GameObject objectToPersist;
    public string sceneToLoad; // Le nom de la scène à charger

    void Start()
    {
        if (objectToPersist != null)
        {
            DontDestroyOnLoad(objectToPersist);
        }
        else
        {
            Debug.LogError("Aucun objet à rendre persistant n'a été assigné.");
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Aucune scène à charger n'a été spécifiée.");
        }
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        Debug.Log("HELLO, nous sommes dans " + currentScene);
    }

    void Update()
    {
        // Votre logique de mise à jour ici
    }
}
