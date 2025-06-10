using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton : MonoBehaviour
{
    // Instance Singleton
    private static Singleton _instance;
    // Exemple de donnée persistant


    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            // Ne pas détruire ce GameObject au chargement d'une nouvelle scène
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Un autre Singleton existe déjà : on supprime ce doublon
            Destroy(gameObject);
        }
    }


    void Start()
    {
    }

    void Update()
    {
    }
}