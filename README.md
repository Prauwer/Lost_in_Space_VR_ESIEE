# TODO MODIFIER LE README POUR CORRESPONDRE AVEC LE NOUVEAU RAPPORT


# Rapport Lost in Space | Antonin Mansour - Zackary Saada - Paul Mallard | IG4

> *Ressentir le vécu d’un astronaute va vous en apprendre beaucoup sur la solitude*

![Untitled](https://github.com/Prauwer/LittleOuterWilds/assets/75014657/8f05b791-5e3f-4e1a-a053-1133b3a8552f)

# Introduction

Dans ce rapport, nous allons vous présenter en détails la nouvelle version de notre jeu, inspiré de Outer Wilds. Cette fois, nous nous sommes éloigné du jeu pour construire notre propre lore, basé principalement sur notre rapport a la solitude.

En effet, le joueur incarne un astronaute qui va être amené a se balader de planètes en planètes à l’aide de son vaisseau afin d’explorer le système solaire. Il va se rendre compte assez rapidement que le plus gros challenge va être la gestion de la solitude plutôt que l’exploration du système. On détaillera dans ce rapport les effets réalisés pour appuyer cela.

Un prérequis très important défini par notre professeur était de mettre l’accent sur le ressenti du joueur. Nous allons voir dans ce rapport avec quelles méthodes nous avons réussi à installer un sentiment particulier.

Les sentiments en particulier à instaurer au joueur sont : le malheur, la tristesse, la solitude et la dépression. Nous avons prévu des mécaniques de jeu pour représenter accentuer ou atténuer ces sentiments, comme l’implémentation d’une barre de solitude, des mails de proches ou photographies à trouver sur le système solaire. Ceci permet aussi de représenter la vie du joueur par différents éléments de gameplay.

# Le Lore du jeu

=============================================================================

Dans un futur proche, la Terre est au bord du chaos. Les ressources s’épuisent, et l’humanité regarde les étoiles pour trouver un espoir. Vous êtes envoyé en exploration dans l’inconnu spatial pour cartographier des systèmes solaires inexplorés, chercher de nouvelles planètes, d’éventuelles colonies. Cette mission est une véritable confrontation avec l’immensité de l’espace et la fragilité des liens humains.

À bord de votre vaisseau, les communications avec la Terre sont votre seul lien avec la vie que vous avez laissée derrière. Des messages de vos proches vous parviennent, porteurs de nouvelles tantôt réconfortantes, tantôt déchirantes. Ces messages, rares mais émouvants, vous font ressentir l’écart grandissant entre vous et le reste de l’humanité. L’espace, immense et silencieux, devient le reflet de votre solitude, tandis que les découvertes que vous faites sur des planètes inconnues remettent en question votre mission et sa véritable signification.

Votre quête n’est pas seulement celle d’un explorateur, mais d’un individu qui cherche à comprendre ce que signifie rester humain, même perdu dans l’infini. C’est le combat d’un individu contre la solitude. Un combat dont vous allez sûrement en tirer des enseignements.

=============================================================================

Nous souhaitons faire comprendre au joueur l’intérêt véritable du jeu, qui réside en la compréhension et le ressenti de la solitude. Le but est de survivre à la solitude, voire même de la vaincre.

“*Ressentir le vécu d’un astronaute va vous en apprendre beaucoup sur la solitude”*

## Démo globale

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/6cjWKbErk7E/0.jpg)](https://www.youtube.com/watch?v=6cjWKbErk7E)

# Fonctionnalités

### La barre de solitude

Afin d’afficher le niveau de solitude du joueur, une barre de solitude va apparaître après quelques temps. 

Cette barre de solitude est drainée en permanence durant toute la durée du jeu. A l’inverse, le joueur peut revenir dans un meilleur état de santé s’il rencontre des éléments réconfortant (photos, lettres, mails reçus).



https://github.com/user-attachments/assets/ce2c6138-0c4e-423a-b37e-226cbe67acae

Cette barre de Solitude utilise le Composant “Slider” dont on peut modifier la valeur et régler entre 0 et 100.

Un script lui est attaché afin de gérer son apparition/disparition (isCanvasVisible) dans le jeu ainsi que la valeur du slider (Solitude) qui doit être modifiable depuis un script si nécessaire.
De plus, la valeur (Solitude) permet d’afficher dans un texte le % de Solitude/SantéMentale, ce qui est plus pratique pour le joueur.

![image](https://github.com/user-attachments/assets/824ff530-8967-4d85-9530-f59227e51d76)

Le corps du code :

```csharp
void Start()
{
    // Initialize Canvas visibility and UI
    ToggleCanvasVisibility(isCanvasVisible);
    UpdateBar();
}

void Update()
{
    UpdateBar();
}
```

Les fonctions principales

```csharp
void UpdateBar()
{
    if (!solitudeCanvas.gameObject.activeSelf)
        return; // Skip updates if Canvas is hidden

    // Update the slider and text
    slider.value = solitude; // Set the slider value to the solitude (float)
    solitudeText.text = "Solitude: " + Mathf.RoundToInt(solitude) + "%"; // Display rounded value for clarity
}
```

```csharp
public void ToggleCanvasVisibility(bool isVisible)
{
    solitudeCanvas.gameObject.SetActive(isVisible);

    if (isVisible)
    {
        UpdateBar();
    }
}
```

Nous verrons que cette barre de solitude peut changer en fonction des actions du joueur. 

C’est ce niveau de solitude qui va permettre de définir des effets sonores et de post processing avec plus ou moins d’intensité par la suite.

### Réception de messages

Boite de messagerie Mail Direct”

Nous avons introduit un concept de messagerie pour le joueur de sorte qu’il reçoive des messages depuis la Terre. Le joueur peut alors recevoir ponctuellement des mails de News ou de la famille, à un intervalle T de temps donné. 

```csharp
eventQueue.Add(new GameEvent(10f, "Mail 2", () =>
{
    mailList.Add(new Mail("Une nouvelle qui te fera sourire", email2, image2));
}));
```

Afin de mettre en place une messagerie, on met en place un visuel “Outside” et “Inside”:

- Le GameObject “Outside” reste toujours visible, il affiche le logo Mail et les notifications qui pop-up avec un compteur de mails reçus.
- Le GameObject “Inside” quand à lui est invisible, à moins d’appuyer sur M. Il s’agit de la boite de réception affichée sous la forme d’un ScrollView qui affiche plusieurs prefabs de mail-boutons.


![image 1](https://github.com/user-attachments/assets/bed0a1e2-058b-4f30-89f4-2709e4a88f91)

![image 2](https://github.com/user-attachments/assets/c825e283-9905-4bf7-8ea4-f4b3b5ddbd49)

Dans le script EventManager, on commence par initialiser une liste de mails et rendre les interfaces invisibles :

```csharp
void Start()
{
    // Initialiser les événements ici
    InitializeEvents();

    // S'assurer que la scroll view et le mail panel sont invisibles au départ
    scrollView.SetActive(false);
    mailDetailPanel.SetActive(false);
}
```

Ensuite il faut vérifier si l’utilisateur souhaite ouvrir l’interface graphique avec “M”:

```csharp
void Update()
{
    // Touche "M" pour ouvrir/fermer la scroll view
    if (Input.GetKeyDown(KeyCode.M))
    {
        openInside(!isInsideVisible);
    }
```

… openInside permet d’ouvrir ou fermer les UI quand on appuie sur M et de “désancrer” la souris lorsque la boite de réception apparaît. De cette manière là, on peut utiliser la souris afin de sélectionner le mail dans la messagerie.

```csharp
private void openInside(bool isVisible)
{
    isInsideVisible = isVisible;

    // Afficher ou cacher la scroll view
    scrollView.SetActive(isInsideVisible);

    // Si la vue "Inside" est ouverte, on déverrouille la souris
    if (isInsideVisible)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    else
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Si on ferme "Inside", on cache aussi le panneau de détail du mail
    if (!isInsideVisible)
    {
        mailDetailPanel.SetActive(false);
        isMailDetailVisible = false;
    }
} 
```

Les mails sont ajoutés à l’UI > Inside > ScrollView avec la fonction suivante :

```csharp
void AddMail(string title, string content, Texture2D image = null)
{
    mailScrollView.Add(new Mail(title, content, image));
    Debug.Log($"Nouveau mail reçu : {title}");

    // Mettre à jour l'affichage des mails
    UpdateMailUI();
    mailCounterText.text = mailScrollView.Count.ToString();
    mailIndex++;
}
```

Cette fonction permet aussi de mettre à jour le compteur des notifications reçues, dans la partie Outside, que l’on voit ci-contre.

![image 3](https://github.com/user-attachments/assets/8a50692a-f527-45d4-9576-c6bde31fb720)

Et ci-dessous on peut voir un extrait de UpdateMailUI() afin d’afficher la sélection.

```csharp
GameObject mailButton = Instantiate(mailPrefab, mailListContent);

// Mettre à jour le texte du bouton (TextMeshProUGUI)
TextMeshProUGUI buttonText = mailButton.GetComponentInChildren<TextMeshProUGUI>();
if (buttonText != null)
{
    buttonText.text = mail.Title;
}
```

A savoir que pour un bon affichage des boutons dans la ScrollView, on a ajouté les composants “Content Size Filter” et “Vertical Layout Group” au Content de la ScrollView.

Voir le résultat ci-dessous :


https://github.com/user-attachments/assets/ebbf444a-d661-4d89-bd41-a59dac0ae825



L’affichage des Mails se fait grâce à MailDetailPanel, il faut l’afficher quand on clique sur un des boutons/prefabs visible dans la ScrollView. Une fois fait, cela rend visible une fenêtre de mail dont les variables texte “Titre” et “Contenu” sont modifiées en fonction du préfab sélectionné/cliqué.

```csharp
// Fonction pour ouvrir un mail (modifie le booléen "IsOpened")
public void OpenMail()
{
    if (mailIndex > 0 && mailIndex <= mailList.Count)
    {
        Mail mail = mailList[mailIndex-1];
        if (!mail.IsOpened)
        {
            mail.IsOpened = true;
            Debug.Log($"Mail ouvert : {mail.Title}");

            mailDetailPanel.SetActive(true);
            mailDetailTitle.text = mail.Title;
            mailDetailContent.text = mail.Content;
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
```

![image 4](https://github.com/user-attachments/assets/5d74959d-ad35-49d8-8ca9-34d01bc1aa52)



https://github.com/user-attachments/assets/c8ba4d0b-a8ef-4894-844a-f4cb91e5e61b



Lire ses mails peut baisser la solitude ou même parfois l’augmenter ! Ce qui peut aider dans le gameplay.

<aside>
💡

On pourrait imaginer la possibilité du joueur d’envoyer des mails, les proches répondent, généré par de l’IA
Les messages accélère la progression du joueur (si il est joyeux, il est encore plus joyeux, et inversement)

</aside>

### Photos et Lettres à récupérer

Lors de son exploration, en plus des petites planètes à récupérer, le joueur va être amené à récupérer des photos de familles, et des lettres d’inconnus. Ces éléments vont lui rappeler des bon souvenirs, et ainsi lui rajouter du % de solitude (il va se sentir moins seul). Voici la vidéo démo (à noter qu’il y a 6 photos présentes sur les différentes planètes, et 4 lettres) :


https://github.com/user-attachments/assets/4980595c-bab8-4ada-a508-ec6282793b7f



https://github.com/user-attachments/assets/76e95c8a-7a60-4384-90b6-267800386e91



Comme on le remarque, quand on trigger les objets, le script active une boîte de dialogue qui affiche un message. De plus la texture de l’objet (la photo ou la lettre) est appliqué à un objet `RawImage` , ce qui l’affiche à l’écran.

Ci-dessous le script correspondant (à noter que les objets ayant le tag `“PickUp”` ne nous intéressent pas, il s’agit des mini-planètes implémentées l’année dernière.

```csharp
void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText(); 
        }
        else if (other.gameObject.CompareTag("familyPhoto"))
        {
            frontScreenImage.gameObject.SetActive(true);
            dialogueBox.gameObject.SetActive(true);
            other.gameObject.SetActive(false);
            Renderer objectRenderer = other.gameObject.GetComponent<Renderer>();
            if (objectRenderer != null && objectRenderer.material.mainTexture != null)
            {
                frontScreenImage.texture = objectRenderer.material.mainTexture;
            }
            solitudePanel.solitude = Math.Min(solitudePanel.solitude + 35, 100);
        }
    }
```

### Event Loop

Nous avons écrit un script `EventManager` gère tous les évènements qui arrivent en jeu. Il dispose d’une interface complexe qui nous permet de configurer différents aspects du jeu.

Voici un aperçu de son interface.

![Sans_titre](https://github.com/user-attachments/assets/94d2b62f-b238-459f-86a7-bbe4406f42b6)


Son but est multiple, voici une liste de ses fonctionnalités :

**Déclenchement initial et drain de solitude**

Le manager déclenche au bout d’un évènement en jeu. On avait prévu à la base un trigger en particulier, mais on s’est décidé sur un timer de 60 secondes après le début de la partie. Au bout de 60 secondes, la barre de solitude qui était invisible apparaît et commence à se drainer. Le drain de solitude par seconde est configurable dans le manager. Il est également possible de forcer le déclenchement du trigger à l’aide d’un menu contextuel dans l’inspecteur.

![image 5](https://github.com/user-attachments/assets/6b04478e-2ef1-4411-bb0c-25be27a80b8c)

![image 6](https://github.com/user-attachments/assets/a45fc1ab-baed-4829-a7a4-33ee399ea451)

**Evènements et mails**

Le manager est initialisé avec une série d’évènement à déclencher au bout d’un certain nombre de temps passé en jeu. Ces évènements vont créer un objet `Mail` dans une variable `mailList` . C’est de cette manière que les mails sont régulièrement envoyés au personnage principal.
Un mail se définit par les attributs suivants :

```csharp
public Mail(string title, string content, Texture2D image = null)
{
		Title = title;
		Content = content;
		Image = image;
		IsOpened = false;
}
```

Le booléen IsOpened permet de vérifier si un mail a été lu ou non, et les autres attributs sont du contenu.

On peut voir les évènements à suivre et les mails reçus dans le manager dans l’inspecteur. Sur la capture d’écran ci-jointe, on peut voir que 3 évènements sont à suivre, et le suivant arrive 3 minutes après le début de la partie. Plus bas, nous avons reçu deux mails qui contiennent un titre, un corps et une image à afficher lorsque le joueur décidera de les ouvrir.

![image 7](https://github.com/user-attachments/assets/fc6cb406-c522-47ed-8a64-521866a12470)

**Gestion de la défaite**

La gestion de la défaite est aussi effectuée par le manager. Lorsque celui-ci constate que la solitude du joueur a atteint 0%, il affiche un message de défaite et nous renvoie 5 secondes plus tard au menu principal. Le code utilisé est celui-ci :

```csharp
if (solitudePanel.solitude <= 0)
{
		gameOverOverlay.SetActive(true);
		Invoke("LoadMenu", 5.0f);
}
```

![image 8](https://github.com/user-attachments/assets/c4fcf7c9-2743-41ab-8305-735804c32cfc)

**Hallucinations auditives**

Le manager permet aussi de définit une liste de “sons d’ambiance bizarres” à jouer lorsque la solitude du joueur devient critique. Nous avons fait en sorte qu’il soit possible d’ajouter n’importe quel son dans une liste dans le manager.
Une fois en jeu, le manager jouera ces sons à intervalle de plus en plus réguliers à mesure que le joueur perd en solitude. Ces sons sont joués de manière aléatoire, avec un pitch un peu différent de sorte à ce qu’il soit difficile de les différencier. Voici le code qui joue ces sons.

```csharp
if (Time.time - lastSoundTime >= soundInterval)
{
	  randomIndex = Random.Range(0, creepySounds.Length);
		audioSource.pitch = Random.Range(0.8f, 1.0f);
		audioSource.PlayOneShot(creepySounds[randomIndex]);
		lastSoundTime = Time.time;
}
```

**Code**

Le code complet et détaillé du manager est le suivant :

![image 9](https://github.com/user-attachments/assets/e3dfb1fe-4d94-4eaf-8f59-bb30499c2d54)

```csharp
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
            Image = image;
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
    private int lastPlayedSoundIndex = -1; // Index du dernier son joué

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

            // Vérifie si 60s se sont écoulées
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
		    // CETTE FONCTION INITIALISE LES MAILS. ELLE EST COUPEE CAR TROP LONGUE SUR LE RAPPORT
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
            lastSoundTime = Time.time;
            Debug.Log($"Son bizarre joué (Solitude: {solitude}, Intervalle: {soundInterval}s)");
        }
    }

    // Fonction pour jouer un son aléatoire
    void PlayRandomCreepySound()
    {
        if (creepySounds.Length > 0)
        {
            int randomIndex;

            // Choisir un son aléatoire
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

    // Fonction pour ouvrir un mail
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
```

### Liquides (Lac et Cascade)

Un des prérequis technique était de réaliser de l’eau en mouvement à l’aide d’un shader. On a donc trouvé ce [tutoriel](https://www.youtube.com/watch?v=_H8gBKGKbnU) qu’on a suivis pour réaliser cet effet. Concrètement, on a créer d’abord un shader, avec des textures voronoi (voir les textures dans le screenshot du VFX Graph), ce qui a permis de faire un effet d’eau animé convainquant pour un style “cartoon”. Ensuite, on a créé un VFX graph pour créer un cyclindre et appliquer cet effet, en y insérant deux couche de ce shader pour ajouter du volume. On a également appliquer ce shader a un graphe pour simuler un effet d’eau.

Voici le rendu final →



https://github.com/user-attachments/assets/a359b195-b33d-4b15-aa0d-2fa0fa0bf688



Il n’y a pas eu de code, mais voici les captures d’écrans du shader créé ainsi que du VFX Graph :

![image 10](https://github.com/user-attachments/assets/a0283e04-09f5-4be4-9283-7e54e0dcbd3f)

Shader

![image 11](https://github.com/user-attachments/assets/fee4f4db-9918-4cdd-9396-58e77e3698bb)

VFX Graph

Pour le VFX Graph (screenshot ci-contre), nous avons créer qu’une seule particule, de forme cylindrique, qui est immobile. Cette particule, nous créons deux mesh, un cylindre intérieur, avec des paramètres précis du shader, pour créer la texture d’eau. Il y a aussi un cylindre extérieur, qui comporte des “trous” dans la texture, qui sert à ajouter des formes blanches au dessus du cylindre pour rendre l’effet un peu plus “organique”.

Petit détail, nous avons pas suivis l’entièreté du tutoriel, étant donné qu’il demandais de créer des formes dans blender, donc des compétences que nous n’avons pas.

Pour le shader (screenshot ci-dessus), nous avons créer utilisé une couleur de base pour l’eau (modifiable dans le VFX Graph), multiplié par une texture voronoï, afin de créer un effet d’eau. On joue avec l’alpha pour faire la deuxième couche qui ne comporte que la partie blanche, qui sert a créer du relief. On applique également une puissance (mathématique) sur la texture afin d’accentuer ou non la partie blanche. Enfin, on applique un effet de tilling et offset. le tilling sert a étirer la texture pour qu’elle soit plus allongé et ainsi mieux représenter de l’eau. L’offset, géré par une variable de temps. sert à animer l’eau.

### Drapeaux

Des drapeaux sont dispersés dans l’univers, un sur chaque planète. Ils reprennent la méthode qui est enseignée dans le TD 7. Dans le lore, ils représentent la faction du joueur.



https://github.com/user-attachments/assets/5cc27f26-18bb-4bd2-96ab-4a7261c2cf08



### Post-Processing

Nous voulons d’abord créer un sentiment d’émerveillement face à l’espace qui va ensuite laisser place à un sentiment de désespoir au fur et à mesure que la santé mentale du joueur descend. Pour ce faire, nous allons créer deux volumes de post-processing qui seront actifs en même temps, mais dont le poids sera géré par notre valeur de solitude.

Voyons d’abord le **volume “heureux”** qu’on rencontre en premier dans le jeu.

**Bloom**

- **Threshold** : 0.5
- **Intensity** : 1.5
- **Tint**: Blanc (#FFFFFF)

**Motion Blur**

- **Quality** : High
- **Intensity** : 1
- **Clamp** : 0.05

**Vignette**

- **Color**: Black (#000000)
- **Intensity** : 0.1
- **Smoothness** : 1
- **Rounded**: True

**Depth of Field**

- **Mode** : Bokeh
- **Focus Distance** : 12

**Color Adjustments**

- **Post Exposure** : 1
- **Saturation :** 20

Ce volume accentue les couleurs et les lumières, permettant ainsi de mieux apprécier les différents objets et feux de camps dispersés autour de la planète. On peut aussi apprécier l’espace, car les étoiles les “plus proches” de la skybox se mettent à briller plus fort que les autres. (elles ne sont pas vraiment plus proches, mais cet effet est simulé grâce au bloom !)

La vignette très légère permet de concentrer le regard du joueur au milieu de l’écran et atténue un peu les effets de lumière qui peuvent être trop présents sur les bords de l’écran.

Il y a aussi un effet de Depth of Field et de Motion Blur qui sont universels aux deux shaders, ceux-ci permettent de simuler l’effet de mouvement de caméra qu’on peut constater dans la vie réelle et de faire en sorte que les planètes ne soient pas trop claires lorsqu’on les regarde de loin.




https://github.com/user-attachments/assets/fc56933b-e103-4e18-8325-4e6fe47f41ff



Voyons désormais le **volume “triste”** qui prend la place de celui-ci lorsque le joueur a une barre de solitude au minimum.

**Motion Blur**

- **Quality** : High
- **Intensity** : 1
- **Clamp** : 0.05

**Vignette**

- **Color** : Bleu foncé (#1E1E46)
- **Intensity** : 0.8
- **Smoothness** : 0.4

**Depth of Field**

- **Mode** : Bokeh
- **Focus Distance** : 3

**Film Grain**

- **Type** : Thin 1
- **Intensity** : 1
- **Response** : 0.5

**Lens Distortion**

- **Intensity** : -0.5
- **Scale** : 0.9

**Chromatic Aberration**

- **Intensity** : 1

**Color Adjustments**

- **Post Exposure** : -1
- **Saturation :** -80

Ce shader peut immédiatement paraître impressionnant, ou même injouable d’un point de vue du gameplay, mais il faut se rappeler que cette vue est visible au PIRE cas de solitude, et qu’atteindre ce niveau de solitude signifie un game over immédiat. Il faudra plutôt apprécier le mélange entre celui-ci et le précédent.

Mention au motion blur qui reste identique pour ne pas gêner lorsque les volumes changent de poids.

L’absence de bloom rend toutes les lumières fades et quasiment inexistantes. Le soleil devient une sorte de balle de ping-pong fade plutôt que la boule de chaleur que nous avions au début du jeu.

La vignette devient puissante et presque omniprésente. Nous avons choisi une couleur bleu foncé pour simuler le fait de se faire “engloutir” par le vide spatial autour de nous.

La distance de focus du depth of field devient beaucoup plus petit, ce qui fait que les éléments à peine éloignés sont immédiatement moins visibles.

On ajoute un grain pour brouiller la vision du joueur et créer des artefacts qui sonnent comme des hallucinations au joueur. Celles-ci correspondent avec le sentiment de folie qu’on veut recréer.

On a également ajouté un lens distortion, ceci a pour effet de faire paraître les objets beaucoup plus éloignés. On augmente ainsi l’inconfort du joueur au fur et à mesure que la partie progresse. Cet effet va de pair avec l’aberration chromatique pour donner un champ de vision sur les bords de l’écran éloigné et flou.

Enfin, le plus important sont les ajustements de couleur. On inverse complètement l’exposition des couleurs et on réduit la saturation à -80. Ca a pour effet de retirer quasiment toute couleur à la scène, on est presque sur du noir et blanc lorsque le joueur est sur le point de mourir de solitude. Avec cet ajustement, on obtient enfin ce sentiment de dépression qui veut être partagé par le jeu.



https://github.com/user-attachments/assets/01dbc40a-84a7-4b72-9da4-6cf2b8e89989



Enfin, voici le script qui gère le mélange entre ces deux shaders en fonction de la solitude

```csharp
public class PostProcessBlender : MonoBehaviour
{
    public Volume happyVolume;              // Volume heureux
    public Volume sadVolume;                // Volume triste
    public SolitudeBar solitudePanel;       // Référence au script contenant la solitude

    void Start()
    {
        happyVolume.gameObject.SetActive(true);
        sadVolume.gameObject.SetActive(true);
    }

    void Update()
    {
        int solitudeLevel = solitudePanel.solitude; 
        float blendFactor = Mathf.Clamp01(solitudeLevel / 100f);

        happyVolume.weight = blendFactor;   // Volume heureux augmente avec le facteur
        sadVolume.weight = 1 - blendFactor; // Volume triste diminue
    }
}
```

### Système d’éclairage avancé

Nous voulions avoir un effet d’ombre projetés (voir ci-contre). Cet effet peut être créé avec une directional light. Cet effet de lumière simule des rayons parallèles, qui traversent toute la scène de part en part, en suivant une direction. C’est donc bien adapté pour simuler la lumière du soleil dans le ciel. Le problème que nous avons rencontré, c’est que dans notre cas, le soleil n’est pas dans le ciel, mais au milieu de notre scène. les rayons ne sont donc pas parallèles, mais partent du centre de la scène, pour aller dans toutes les direction. Un autre type de lumière fonctionnerais mal en raison de la grande distance de nos planètes. L’idée que nous avons eu est alors d’utiliser une directional light, qui va dans la direction du joueur, laissant l’impression que les rayons vont dans toutes les directions.

![image 12](https://github.com/user-attachments/assets/f29cd29a-3042-426b-9826-68bef3d5730c)

Pour cela, nous avons créer un programme qui calcule l’angle du joueur entre l’axe X, l’origine et le joueur, à l’aide de la trigonométrie. 

Voici le code :

```csharp
public class LightRotator : MonoBehaviour
{
    public Transform Player;

    void Update()
    {
        SetRotation();
    }

    void SetRotation()
    {
        // Accès aux coordonnées du Player via Player.position
        Vector3 playerPosition = Player.position;

        // Calcul de l'angle en radians (autour de l'axe Y)
        double angleX_rad = Math.Atan2(playerPosition.x, playerPosition.z);

        // Conversion en degrés
        float angleX_deg = (float)(angleX_rad * (180.0 / Math.PI));

        // Appliquer la rotation de l'objet courant (fixer l'orientation)
        transform.rotation = Quaternion.Euler(0, angleX_deg, 0);
    }
}
```

### Shader du Soleil

Un premier Soleil avait été créé par nos soins, nous avions suivi un tutoriel qui utilisait des particules. Afin de l’améliorer, et d’utiliser un shader plutôt qu’un système de particules, nous avons décidé de le re-créer entièrement.

Pour cela, nous avons suivi [ce tutoriel](https://youtu.be/ykwvCCqdcCs). On utilise un shader avec des textures voronoi, que l’on va attribuer à un material. Combiné avec des effets de post-processing, comme le bloom, qui ont été détaillés précédemment, on obtient un résultat plutôt convaincant :

![image 13](https://github.com/user-attachments/assets/ff7b0a1d-7d3a-4a65-9e71-218ca6a459bf)

![image 14](https://github.com/user-attachments/assets/563739ab-e6e3-4207-ab14-06c7909ad56e)

Voici le shader créé :

![image 15](https://github.com/user-attachments/assets/6377e9d6-6614-432e-a6e8-3c4fd0d1b8c6)

### Système de particules

Nous avons ajouté un système de particules pour remplacer les effets de feu du réacteur du vaisseau. Ils sont semblable techniquement au système de neige vu dans les TD.


https://github.com/user-attachments/assets/2f31b143-b6e1-4a4e-aa68-752d095f37ba

![image 16](https://github.com/user-attachments/assets/d5412f35-0e4b-4519-8edd-69e4b597f595)

Le résultat est une série d’expérimentations pour essayer d’avoir le rendus le plus réaliste possible, en s’appuyant sur le TD de la neige sur le système de particules.

Nous avons utilisé une texture de feu comme base, et joué sur la durée de vie, l’angle de propagation, la vitesse, le nombre de particules et le changement de taille principalement.

Nous avons également joué sur la couleur, qui commence jaune claire (comme sur la photo), puis deviens de plus en plus rouge, et fini par devenir transparent.

Pour essayer de simuler une flamme, nous avons réglé le `start over lifetime` sur une courbe qui décroit rapidement.

De plus, nous avons activé le module Noise avec des paramètre adéquat afin de simuler des perturbation pour augmenter le réalisme.

(à noter que  le résultat dans d’autres vidéos de ce rapport peut être différent car on utilisait précédemment un système de particule trouvé sur l’asset store)

### Effet de FOV et shader de vitesse

Pour simuler la sensation de vitesse dans un vaisseau spatial, nous allons combiner deux effets visuels : une augmentation du champ de vision lorsqu’on accélère, et un shader de distorsion de la vision lorsqu’on accélère.

**Augmentation du FOV**

Ce script est attaché à la caméra du vaisseau pour calculer dynamiquement un nouveau FOV en fonction de la vélocité du ridigbody. On a aussi un paramètre pour gérer la vitesse de transition pour que le changement de FOV ne soit pas trop brusque lors d’un arrêt ou accélération soudaine du vaisseau.

```csharp
public class DynamicFOV : MonoBehaviour
{
    public Camera mainCamera; // Référence à la caméra du vaisseau
    public Rigidbody spaceshipRigidbody; // Référence au Rigidbody du vaisseau
    public float baseFOV = 60f; // FOV de base (valeur au repos)
    public float maxFOV = 90f; // FOV maximum (valeur à vitesse max)
    public float speedForMaxFOV = 50f; // Vitesse à laquelle le FOV atteint sa valeur max
    public float fovSmoothness = 5f; // Vitesse de transition entre les FOV

    private float targetFOV;

    void Update()
    {
        if (mainCamera != null && spaceshipRigidbody != null)
        {
            // Récupérer la vitesse du vaisseau
            float speed = spaceshipRigidbody.velocity.magnitude;

            // Calculer le FOV cible en fonction de la vitesse
            targetFOV = Mathf.Lerp(baseFOV, maxFOV, speed / speedForMaxFOV);

            // Lisser la transition entre le FOV actuel et le FOV cible
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * fovSmoothness);
        }
    }
}

```

**Shader de vitesse**

Ce shader a malheureusement dû être abandonné lors du changement d’URP pour le shader du Soleil car les technologies utilisées ne permettent pas de recréer l’effet sur lequel nous avions travaillé. Cependant, nous avons conservé une version antérieure que nous pouvons présenter pour montrer comment ça fonctionnait.

Il s’agit d’ajouter un script sur la caméra qui donne accentue la puissance d’un shader de vitesse. Voici le script :

```csharp
public class SpeedTunnelEffect : MonoBehaviour
{
    public Material tunnelMaterial; // Matériel avec le shader
    public Rigidbody spaceshipRigidbody; // Référence au Rigidbody du vaisseau
    public float blurMultiplier = 0.001f; // Facteur de multiplication pour le flou
    public float maxBlur = 0.05f; // Valeur maximale du flou

    private float currentBlurStrength;

    void Update()
    {
        if (spaceshipRigidbody != null && tunnelMaterial != null)
        {
            // Calculer la vitesse du vaisseau
            float speed = spaceshipRigidbody.velocity.magnitude;

            // Calculer l'intensité du flou proportionnellement à la vitesse
            currentBlurStrength = Mathf.Clamp(speed * blurMultiplier, 0, maxBlur);

            // Appliquer l'intensité du flou au shader
            tunnelMaterial.SetFloat("_BlurStrength", currentBlurStrength);
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (tunnelMaterial != null)
        {
            Graphics.Blit(src, dest, tunnelMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
```

On lie ce script à un matériau, qui lui même est défini par un shader customisé.

![image 17](https://github.com/user-attachments/assets/fcd2b3b9-eea4-4a34-8c9d-60cf83b88b18)

![image 18](https://github.com/user-attachments/assets/425d2533-8544-4ae9-8136-44e9039814d3)

Le résultat combiné des deux techniques est le suivant. On peut constater que le shader et le FOV augmentent petit à petit avec le démarrage du vaisseau, puis s’adoucissent jusqu’à s’annuler lorsque le vaisseau ralentit.


https://github.com/user-attachments/assets/6559b44e-29f2-4b88-ae2b-77fad0b7ba59

### Sons (hallucinations auditives et distorsion de la musique)

Le joueur pourra entendre différentes anomalies auditives au fur et à mesure que sa santé mentale diminue. Pour ce faire, nous modifions le pitch, l’effet 3D et le reverb de la musique lorsque la santé mentale se situe entre 0 et 60%. Le code qui modifie ces valeurs est le suivant :

```csharp
// Création d'un multiplicateur à l'aide de la solitude
float blendFactor = Mathf.Clamp01((60f - solitudeLevel) / 60f);

// Ajustement des paramètres
musicSource.pitch = Mathf.Lerp(1f, 0.5f, blendFactor); // Diminue le pitch
musicSource.spatialBlend = Mathf.Lerp(0f, 1f, blendFactor); // Ajoute un effet 3D progressif
musicSource.reverbZoneMix = Mathf.Lerp(0f, 1f, blendFactor); // Augmente le mix de reverb
```


https://github.com/user-attachments/assets/b1fac548-e1a5-4350-a882-581710491135

Le joueur pourra aussi expérimenter l’écoute de “bruits effrayants”, tel une hallucination auditive. Ces hallucinations peuvent apparaître à partir de 60% de santé mentale et sont de plus en plus fréquentes au fur et à mesure que cette valeur descend. Voilà un exemple à écouter (**attention aux oreilles !**).

La manière dont ces sons sont générés et choisis ont été décrits dans la partie **Event Loop** du rapport.


https://github.com/user-attachments/assets/b84ec5c5-c7b4-4410-af8a-b504aef9b300

### Passage à la première personne

Dans le jeu de l’année dernière, nous avions gardé le système de déplacement du joueur présent dans l’asset de gravité, faute d’avoir réussi à faire le nôtre. Cette fois, nous avons décidé de le changer, pour entre autre passer à une vue en première personne. Pour cela, nous avons repris le script de l’asset, pour le modifier. Ce script gère tout ce qui est relatif au déplacement du joueur, y compris la gravité.

La première chose à été de supprimer toute la partie de contrôle du déplacement. nous gardons que la partie relative à la gravité. Ensuite, nous avons re-créer un système de déplacement de zéro, pour en avoir un bien adapté. Par exemple, un des problèmes était que le joueur ne pouvais déplacer sa souris sur l’axe vertical. C’est désormais chose faite. de plus, déplacer la souris sur l’axe vertical fait pivoter le joueur, et appuyer sur les touches Q et D fait déplacer le joueur latéralement (précédemment, ces touches faisait pivoter le joueur). Voici les scripts :

Un premier script pour que la caméra suive les mouvement de la souris 

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollowPlayer : MonoBehaviour
{
    // Vitesse de rotation
    public float rotationSpeed = 5.0f;

    // Référence à l'objet joueur
    private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

        parentTransform.Rotate(Vector3.up, mouseX, Space.Self);
    }
}
```

Un second script qui gère le reste :

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;

    private int count;
    public TextMeshProUGUI countText;

    public GameObject winTextObject;

    [Header("Mouvement")]
    public float speed = 6f;
    public float jumpForce = 5f;
    private bool isGrounded;
    public Vector3 groundNormal;

    private MyGravityCharacterController characterController;

    [Header("Souris")]
    public float mouseSensitivity = 250f; // Sensibilité de la souris
    public Transform playerCamera; // Référence à la caméra
    private float xRotation = 0f; // Rotation verticale accumulée
    private Vector3 velocity; // Vitesse actuelle (gravité incluse)
    private Vector3 direction;
    public float moveSpeed = 5f; // Vitesse de déplacement

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<MyGravityCharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Bloque le curseur au centre de l'écran
        Cursor.visible = false; // Masque le curseur
        
        winTextObject.gameObject.SetActive(false);

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // La gravité est gérée ailleurs
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        count = 0;
        SetCountText();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText(); 
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        countText.color = Color.red;

        if (count >= 5)
        {
            winTextObject.SetActive(true);
        }
    }

    private void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotation verticale de la caméra (limite l'angle)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Applique la rotation à la caméra et au joueur
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void CheckGroundStatus()
    {
        // Augmentez la distance de détection pour plus de robustesse
        float groundCheckDistance = 1.5f;

        RaycastHit hitInfo;
        bool raycastHit = Physics.Raycast(
            transform.position,
            -transform.up,
            out hitInfo,
            groundCheckDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore
        );

        if (raycastHit)
        {
            // Vérifier l'angle entre la normale et l'up du personnage
            float angleToUp = Vector3.Angle(hitInfo.normal, transform.up);
            if (angleToUp < 45f)
            {
                characterController.m_IsGrounded = true;
                characterController.m_GroundNormal = hitInfo.normal;
            }
            else
            {
                // Utiliser une normale de repli si l'angle est trop important
                characterController.m_IsGrounded = true;
                characterController.m_GroundNormal = transform.up;
            }
        }
        else
        {
            characterController.m_IsGrounded = false;

            // Logique existante pour définir characterController.m_GroundNormal en l'air
        }
    }

    /// <summary>
    /// Gère les déplacements du joueur avec ZQSD (ou WASD).
    /// </summary>
    private void HandleMovement()
    {
        // Vérifier le statut du sol à chaque frame
        CheckGroundStatus();

        // Récupération des entrées utilisateur
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Créer le vecteur de mouvement basé sur l'orientation du joueur
        Vector3 moveDirection = transform.right * x + transform.forward * z;

        // Projection du mouvement sur la surface (si au sol)
        if (characterController.m_IsGrounded && characterController.m_GroundNormal != Vector3.zero)
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, characterController.m_GroundNormal);
        }

        // Normaliser et multiplier par la vitesse
        Vector3 targetVelocity = moveDirection.normalized * speed;

        // Conserver la composante verticale de la vélocité existante
        targetVelocity.y = rb.velocity.y;

        // Appliquer la vélocité cible
        rb.velocity = targetVelocity;

        // Saut
        if (Input.GetButtonDown("Jump") && characterController.m_IsGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        // Logs de débogage
        Debug.Log("Move Direction: " + moveDirection);
        Debug.Log("Ground Normal: " + characterController.m_GroundNormal);
        Debug.Log("Is Grounded: " + characterController.m_IsGrounded);
    }
}
```

Ici, les fonction importantes sont surtout `HandleMovement()`, où tout est géré

Voici maintenant la version du code modifié de l’asset de gravité

```csharp
using UnityEngine;

/// <summary>
/// The bulk of character-related movement and animation.
/// Heavily modified from the standard assets' ThirdPersonCharacter.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class MyGravityCharacterController : MonoBehaviour
{
    [SerializeField] float m_IncreasedGravity = 1.3f; // By what factor gravity increases when not holding jump in mid-air
    [SerializeField] float m_RunCycleLegOffset = 0.2f; // Specific to the character in sample assets, will need to be modified to work with others
	[SerializeField] float m_RunSpeedMultiplier = 1.25f;
	[SerializeField] float m_GroundCheckDistance = 1.2f; // Higher value leads to more slope acceptance
    [SerializeField] float m_FallCheckDistance = 4f; // For standOnNormals planets, how far the ground is allowed to be under the player before ignoring standOnNormals

    [HideInInspector] public bool m_IsGrounded;
    [HideInInspector] public bool m_Crouching;
    [HideInInspector] public Vector3 m_GroundNormal;

    Rigidbody m_Rigidbody;
    public MyPlayerPhysics m_Phys;
    float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_TurnAmount;
	float m_ForwardAmount;
	float m_CapsuleHeight;
	Vector3 m_CapsuleCenter;
	CapsuleCollider m_Capsule;
    GameObject m_prevGravity;

	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
        m_Phys = GetComponent<MyPlayerPhysics>();
        m_Capsule = GetComponent<CapsuleCollider>();
		m_CapsuleHeight = m_Capsule.height;
		m_CapsuleCenter = m_Capsule.center;

		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		m_OrigGroundCheckDistance = m_GroundCheckDistance;
	}

	void ScaleCapsuleForCrouching(bool crouch)
	{
		if (m_IsGrounded && crouch)
		{
			if (m_Crouching) return;
			m_Capsule.height = m_Capsule.height / 2f;
			m_Capsule.center = m_Capsule.center / 2f;
			m_Crouching = true;
		}
		else
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + transform.up * m_Capsule.radius * k_Half, transform.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
				return;
			}
			m_Capsule.height = m_CapsuleHeight;
			m_Capsule.center = m_CapsuleCenter;
			m_Crouching = false;
		}
	}

	void PreventStandingInLowHeadroom()
	{
		// prevent standing up in crouch-only zones
		if (!m_Crouching)
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + transform.up * m_Capsule.radius * k_Half, transform.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
			}
		}
	}

    /// <summary>
    /// Gives an additional pull of gravity and lowers lateral velocity.
    /// </summary>
    private void ExtraGravity(float factor)
    {
        Vector3 extraGravityForce;
        if (m_Phys.effector)
            extraGravityForce = Time.fixedDeltaTime * m_Phys.effector.gravity;
        else if (m_Phys.attractor)
            extraGravityForce = Time.fixedDeltaTime * m_Phys.attractor.gravity * Vector3.Normalize(m_Phys.attractor.gameObject.transform.position - transform.position);
        else
            return;

        m_Rigidbody.velocity *= (1 - factor) + (factor * Mathf.Abs(Vector3.Dot(m_Rigidbody.velocity.normalized, extraGravityForce.normalized)));
        m_Rigidbody.AddForce(factor * extraGravityForce);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Draws the CheckGroundStatus SphereCast in the inspector (gizmos need to be enabled to see it)
        // It displays the endpoint of the SphereCast, not the full cast!
        float radius = 0.3f; // Set this to the collider capsule radius if you change the capsule
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.up * radius) - (transform.up * m_GroundCheckDistance), radius - 0.05f);
    }
#endif

    public void CheckGroundStatus()
	{
        float verticalSpeed = Vector3.Dot(m_Rigidbody.velocity, transform.up); // Only check ground if not moving upwards

        Debug.Log($"Vertical Speed: {verticalSpeed}");
        Debug.Log($"Rigid Body Position: {m_Rigidbody.position}");
        Debug.Log($"Transform Position: {transform.position}");
        Debug.Log($"Ground Check Distance: {m_GroundCheckDistance}");

        RaycastHit hitInfo;
        // Afficher des informations détaillées sur le raycast
        bool raycastHit = Physics.SphereCast(
            transform.position + (transform.up * m_Capsule.radius),
            m_Capsule.radius - 0.05f,
            -transform.up,
            out hitInfo,
            m_GroundCheckDistance
        );

        Debug.Log($"Raycast Hit: {raycastHit}");
        if (raycastHit)
        {
            Debug.Log($"Hit Point: {hitInfo.point}");
            Debug.Log($"Hit Normal: {hitInfo.normal}");
            Debug.Log($"Hit Distance: {hitInfo.distance}");
        }

        // Casts a sphere along a ray for a ground collision check.
        // The sphere is 0.05f smaller than the collider capsule and starts at the bottom point of the capsule, giving it a 0.1f offset for safety.
        if ((verticalSpeed <= 0.85f) && Physics.SphereCast(transform.position + (transform.up * m_Capsule.radius), m_Capsule.radius - 0.05f, -transform.up, out hitInfo, m_GroundCheckDistance))
        {
            m_IsGrounded = true;
            m_GroundNormal = hitInfo.normal;
        }
		else
		{
            // Airborne
			m_IsGrounded = false;
            
            if (m_Phys.effector) // Effector: set ground normal to direction of gravity
                m_GroundNormal = Vector3.Normalize(m_Phys.effector.gravity);
            else if (m_Phys.attractor) // Attractor:
            {
                if (m_prevGravity != m_Phys.attractor.gameObject) // After switching planets, set ground normal to direction of gravity
                    GetGroundNormalFromGravity();
                else if (m_Phys.attractor.standOnNormals && // On StandOnNormals planets...
                    ((!Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, m_FallCheckDistance)) || // If ground is not under player
                            (Vector3.Angle(hitInfo.normal, transform.up) > 45))) // or at too steep of a slope
                    GetGroundNormalFromGravity(); // Make sure that the player does not fall off the planet
            }
        }

        // Update the previous source of gravity
        if (m_Phys.effector)
            m_prevGravity = m_Phys.effector.gameObject;
        else if (m_Phys.attractor)
            m_prevGravity = m_Phys.attractor.gameObject;
	}

    private void GetGroundNormalFromGravity()
    {
        m_GroundNormal = Vector3.Normalize(transform.position - m_Phys.attractor.gameObject.transform.position);
    }
}

```

Globalement, les principales modifications ont été de supprimer les fonction suivante :

- `Move()`  → Gestion du déplacement
- `UpdateAnimator()`  → Gestion de l’animation du joueur
- `HandleAirborneMovement()` → Gestion du mouvement en l’air
- `HandleGroundedMovement()` → Gestion du mouvement au sol

### Menu


https://github.com/user-attachments/assets/b4e63218-dd97-4e07-8967-3d6e089ebcb5

Lorsqu’on lance le jeu, un menu simple apparaît, qui permet de lancer le jeu (on load la main scene), ou de quitter le jeu.

Pour réaliser ce menu, nous avons suivis une partie de [ce tutoriel](https://www.youtube.com/watch?v=DX7HyN7oJjE&t=425s).

Il s’agit d’un canvas, avec des boutons qui lancent des fonctions que voici :

```csharp
public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
```

# Conclusion

Dans cette nouvelle version de notre jeu, nous avons mit l’accent sur de nouvelles fonctionnalités immersives et réalistes, comme des effets de post-processing, des shaders, etc… Nous avons voulus aborder le sujet de la solitude, a travers ce personnage qui explore l’espace, et qui va vite se rendre compte que son principal ennemi est la solitude. On peu facilement associer ce personnage a des personnes seules au quotidien, notamment certaines personnes âgées. Notre jeu n’est pas la pour faire ressentir les véritables effets de la solitude, mais plutôt d’aborder le sujet, et d’illustrer le fait qu’elle peu être extrêmement pesante, voire fatale dans le pire des cas.

# Sources

**Assets supplémentaires** utilisés en plus du projet précédent (max 3) **:**

- Boite mail :

**Images/Photos :**

- https://unsplash.com/fr

**Bruitages :**

- https://universal-soundbank.com/

**Tutoriels :**

- https://www.youtube.com/watch?v=DX7HyN7oJjE&t=425s
- https://www.youtube.com/watch?v=ykwvCCqdcCs
- https://www.youtube.com/watch?v=_H8gBKGKbnU
- ChatGPT
