using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Classe pour un événement
    [System.Serializable]
    public class GameEvent
    {
        public float TriggerTime; // Temps relatif au lancement du jeu
        [TextArea] public string Description; // Description de l'événement
        [HideInInspector] public System.Action Action; // L'action à exécuter

        public GameEvent(float triggerTime, string description, System.Action action)
        {
            TriggerTime = triggerTime;
            Description = description;
            Action = action;
        }
    }

    // Classe pour un mail
    [System.Serializable]
    public class Mail
    {
        public string Title; // Titre du mail
        [TextArea] public string Content; // Contenu du mail
        public bool IsOpened; // Indique si le mail a été ouvert
        public Texture2D Image; // Image associée au mail

        public Mail(string title, string content, Texture2D image = null)
        {
            Title = title;
            Content = content;
            IsOpened = false;
            Image = image; // L'image est optionnelle
        }
    }

    [Header("Drain de solitude")]
    public float drainRate = 0.5f; // Nombre d'unités de solitude drainées par seconde

    [Header("Liste des événements à venir")]
    [SerializeField] private List<GameEvent> eventQueue = new List<GameEvent>();

    [Header("Liste des mails")]
    public List<Mail> mailList = new List<Mail>();

    private float elapsedTime = 0f; // Temps écoulé depuis le début du jeu
    private bool isTimerActive = false; // Détermine si le timer est actif ou non

    [Header("Images assignées manuellement (facultatif)")]
    [SerializeField] private Texture2D welcomeImage; // Image du mail de bienvenue
    [SerializeField] private Texture2D missionImage; // Image de la mission débloquée


    [Header("GameOver Overlay")]
    public GameObject gameOverOverlay;

    [Header("Solitude Bar")]
    public SolitudeBar solitudePanel; // Référence au script contenant la valeur de solitude (int solitudePanel.solitude)

    [Header("Audio Settings")]
    public AudioSource audioSource; // Référence à l'AudioSource pour jouer les sons
    public AudioClip[] creepySounds; // Tableau de sons bizarres à choisir aléatoirement

    private float lastSoundTime = 0f; // Temps écoulé depuis le dernier son joué
    private float soundInterval = 60f; // Intervalle initial entre les sons (sera ajusté dynamiquement)
    private int lastPlayedSoundIndex = -1; // Index du dernier son joué (-1 signifie aucun son joué encore)

    void Start()
    {
        // Initialiser les événements ici
        InitializeEvents();
    }

    void Update()
    {
        if (!isTimerActive)
        {
            elapsedTime += Time.deltaTime;

            // Vérifie si un certain temps s'est écoulé (60 secondes)
            if (elapsedTime >= 60f)
            {
                StartTimer();
            }

            return; // Quitte la méthode si le timer n'est pas actif
        }

        elapsedTime += Time.deltaTime;

        // Vérifier si un événement doit se déclencher
        for (int i = eventQueue.Count - 1; i >= 0; i--)
        {
            if (elapsedTime >= eventQueue[i].TriggerTime)
            {
                // Exécuter l'action de l'événement
                eventQueue[i].Action?.Invoke();
                Debug.Log($"Événement déclenché : {eventQueue[i].Description}");

                // Retirer l'événement de la liste
                eventQueue.RemoveAt(i);
            }
        }

        // Drainer la solitude
        solitudePanel.solitude = Mathf.Max(0, solitudePanel.solitude - Time.deltaTime * drainRate);

        // Vérifier la solitude et générer des sons bizarres si nécessaire
        CheckSolitudeAndPlaySound();

        if (solitudePanel.solitude <= 0.1)
        {
            gameOverOverlay.SetActive(true);
            Invoke("LoadMenu", 5.0f);
        }
    }

    void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Initialiser les événements (hardcodés)
    void InitializeEvents()
    {
        // Charger les images depuis les fichiers du jeu (Resources)
        Texture2D image1 = Resources.Load<Texture2D>("Images/image222");
        Texture2D image2 = Resources.Load<Texture2D>("Images/xan-griffin-UCFgM_AojFg-unsplash");
        Texture2D image3 = Resources.Load<Texture2D>("Images/jennifer-kalenberg-K-4Yugk0b1Y-unsplash");
        Texture2D image4 = Resources.Load<Texture2D>("Images/jessica-rockowitz-mNhsrdUsqis-unsplash");
        Texture2D image5 = Resources.Load<Texture2D>("Images/MissionImage");

        string email1 = @"
        Salut frérot,

        Comment ça va là-haut ? On espère que tu tiens le coup, même si ça ne doit pas être facile de rester seul tout ce temps. Ici, tout le monde pense à toi. Papa a installé un télescope pour qu’on puisse regarder les étoiles en pensant que tu es quelque part là-bas.

        Maman a cuisiné ton plat préféré hier, et c’était un peu triste de ne pas te voir te resservir trois fois comme d’habitude… Mais on s’est dit que la prochaine fois qu’on le fera, ce sera pour fêter ton retour.

        Prends soin de toi, on t’aime très fort. Écris-nous dès que tu peux.
        Bisous,
        Ton petit frère adoré
        ";
        eventQueue.Add(new GameEvent(60f, "Mail 1", () =>
        {
            AddMail("Tu nous manques, vraiment beaucoup", email1, image1);
        }));

        string email2 = @"
        Salut mon pote,

        Devine quoi ? Antonin a fait quelque chose de complètement fou ! Il s'est vautré sur le canapé en étant bourré pendant le réveillon. On a bien rigolé, mais ce n’était pas pareil sans toi pour te moquer avec nous.

        On espère que tu trouves des choses qui te font rire là où tu es. Si ce n’est pas le cas, imagine Antonin dans sa bêtise, ça devrait t’aider un peu. On t’attend avec impatience.

        Force à toi,
        Zackary
        ";
        eventQueue.Add(new GameEvent(120f, "Mail 2", () =>
        {
            AddMail("Une nouvelle qui te fera sourire", email2, image2);
        }));

        string email3 = @"
        Bonjour mon fils,

        Juste un petit mot pour te donner quelques nouvelles d’ici. On a fait un peu de jardinage, et figure-toi que les tomates que tu avais plantées l’an dernier sont incroyablement belles cette année. On les a appelées “les tomates de l’espace”, en ton honneur.

        Sinon, la maison est calme… Trop calme, en fait. Même ton grand frère commence à dire que tu lui manques (même s’il ne l’admettra jamais en face).

        Continue à être fort là-bas, on est fiers de toi. Reviens-nous vite, OK ?

        Amour et tomates,
        Maman
        ";
        eventQueue.Add(new GameEvent(180f, "Mail 3", () =>
        {
            AddMail("Petite update de la maison", email3, image3);
        }));

        string email4 = @"
        Bonjour mon amoureux,

        Hier soir, le ciel était incroyablement clair. On a tous passé un moment dehors à regarder les étoiles et à se demander laquelle tu pouvais bien observer en ce moment. Peut-être que toi aussi, tu regardais dans notre direction ?

        On sait que tu fais quelque chose d’incroyable, même si ça doit être difficile parfois. On est toujours là pour toi, même à des millions de kilomètres. Et n’oublie jamais : on t’aime jusqu’aux étoiles, et encore plus loin.

        Garde courage,
        Ta chérie
        ";
        eventQueue.Add(new GameEvent(240f, "Mail 4", () =>
        {
            AddMail("Ciel étoilé et pensées vagabondes", email4, image4);
        }));

        string email5 = @"
        Salut p'tit frère,

        On voulait juste te raconter une petite anecdote : Twitty a essayé de sauter dans une boîte et a complètement raté son coup.

        Des choses simples, tu vois, mais on sait que tu aimerais être là pour les voir. On espère que tu prends soin de toi et que tu gardes le moral.

        Tu nous manques à chaque instant, mais on est fiers de toi. Fais-nous signe dès que tu peux.

        A bientôt,
        Josh
        ";
        eventQueue.Add(new GameEvent(300f, "Mail 5", () =>
        {
            AddMail("Les petits riens de la vie", email5, image5);
        }));
    }

    // Fonction pour ajouter un mail
    void AddMail(string title, string content, Texture2D image = null)
    {
        mailList.Add(new Mail(title, content, image));
        Debug.Log($"Nouveau mail reçu : {title}");
    }

    // Fonction pour vérifier la solitude et jouer des sons
    void CheckSolitudeAndPlaySound()
    {
        if (solitudePanel == null || audioSource == null || creepySounds == null || creepySounds.Length == 0)
        {
            Debug.LogWarning("SolitudePanel, AudioSource ou CreepySounds n'est pas configuré.");
            return;
        }

        float solitude = solitudePanel.solitude;

        // Ne rien faire si la solitude est supérieure ou égale à 60
        if (solitude >= 60) return;

        // Ajuster l'intervalle entre les sons en fonction de la solitude
        if (solitude >= 20)
        {
            // Entre 60 et 20, intervalle entre 60s et 10s
            soundInterval = Mathf.Lerp(100f, 20f, (60f - solitude) / 40f);
        }
        else
        {
            // En dessous de 20, intervalle fixe à 10s
            soundInterval = 20f;
        }

        // Si le temps écoulé depuis le dernier son est supérieur à l'intervalle, jouer un son
        if (Time.time - lastSoundTime >= soundInterval)
        {
            PlayRandomCreepySound(); // Joue un son aléatoire
            lastSoundTime = Time.time; // Met à jour le dernier temps où un son a été joué
            Debug.Log($"Son bizarre joué (Solitude: {solitude}, Intervalle: {soundInterval}s)");
        }
    }

    // Fonction pour jouer un son aléatoire
    void PlayRandomCreepySound()
    {
        if (creepySounds.Length > 0)
        {
            int randomIndex;

            // Choisir un son aléatoire qui n'est pas le même que le dernier joué
            do
            {
                randomIndex = Random.Range(0, creepySounds.Length);
            } while (randomIndex == lastPlayedSoundIndex && creepySounds.Length > 1);

            // Mettre à jour le dernier son joué
            lastPlayedSoundIndex = randomIndex;

            // Modifier le pitch aléatoirement entre 0.8 et 1.0
            audioSource.pitch = Random.Range(0.8f, 1.0f);

            // Jouer le son
            audioSource.PlayOneShot(creepySounds[randomIndex]);

            // Afficher le nom du son dans le debug
            Debug.Log($"Son joué : {creepySounds[randomIndex].name}");
        }
    }

    // Fonction pour ouvrir un mail (modifie le booléen "IsOpened")
    public void OpenMail(int mailIndex)
    {
        if (mailIndex >= 0 && mailIndex < mailList.Count)
        {
            Mail mail = mailList[mailIndex];
            if (!mail.IsOpened)
            {
                mail.IsOpened = true;
                Debug.Log($"Mail ouvert : {mail.Title}");
            }
            else
            {
                Debug.Log($"Ce mail est déjà ouvert : {mail.Title}");
            }
        }
        else
        {
            Debug.LogError("Index de mail invalide.");
        }
    }

    // Fonction pour activer le timer
    public void StartTimer()
    {
        if (!isTimerActive)
        {
            isTimerActive = true;
            solitudePanel.ToggleCanvasVisibility(true);
            solitudePanel.isCanvasVisible = true;
            Debug.Log("Le timer a été activé !");
        }
        else
        {
            Debug.Log("Le timer est déjà actif !");
        }
    }

    // Bouton dans l'inspecteur pour activer le timer
    [ContextMenu("Activer le Timer")]
    private void StartTimerFromInspector()
    {
        StartTimer();
    }
}