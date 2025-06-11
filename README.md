# Rapport Lost in Space VR | Antonin Mansour - Zackary Saada - Jovan Rahme | IG5

> *Ressentir le vécu d’un astronaute va vous en apprendre beaucoup sur la solitude (maintenant en immersif !)*
> 

![Untitled](https://github.com/Prauwer/LittleOuterWilds/assets/75014657/8f05b791-5e3f-4e1a-a053-1133b3a8552f)

# Introduction

Dans ce rapport, nous allons vous présenter la version VR de notre Jeu : Lost In Space. Pendant IG4, nous avions mis l’accent sur le lore; ici, nous allons nous concentrer sur la partie VR : comment adapter un jeu sans qu’il ait de motion sickness, les contraintes associées, et notamment la théorie des I2.

# Le Lore du jeu

Dans un futur proche, la Terre est au bord du chaos. Les ressources s’épuisent, et l’humanité regarde les étoiles pour trouver un espoir. Vous êtes envoyé en exploration dans l’inconnu spatial pour cartographier des systèmes solaires inexplorés, chercher de nouvelles planètes, d’éventuelles colonies. Cette mission est une véritable confrontation avec l’immensité de l’espace et la fragilité des liens humains.

À bord de votre vaisseau, les communications avec la Terre sont votre seul lien avec la vie que vous avez laissée derrière. Des messages de vos proches vous parviennent, porteurs de nouvelles tantôt réconfortantes, tantôt déchirantes. Ces messages, rares mais émouvants, vous font ressentir l’écart grandissant entre vous et le reste de l’humanité. L’espace, immense et silencieux, devient le reflet de votre solitude, tandis que les découvertes que vous faites sur des planètes inconnues remettent en question votre mission et sa véritable signification.

Votre quête n’est pas seulement celle d’un explorateur, mais d’un individu qui cherche à comprendre ce que signifie rester humain, même perdu dans l’infini. C’est le combat d’un individu contre la solitude. Un combat dont vous allez sûrement en tirer des enseignements.

Nous souhaitons faire comprendre au joueur l’intérêt véritable du jeu, qui réside en la compréhension et le ressenti de la solitude. Le but est de survivre à la solitude, voire même de la vaincre.

“*Ressentir le vécu d’un astronaute va vous en apprendre beaucoup sur la solitude”*

### Inspiration artistique et littéraire

Le jeu s’inspire de deux grands courants : **l’existentialisme** et **le romantisme noir**.

L’existentialisme montre que l’humain est libre, mais que cette liberté a un prix : il est seul face à ses choix, sans règles toutes faites. Cela crée une forme d’angoisse, car il faut donner un sens à sa vie soi-même. Dans le jeu, cela se traduit par des décisions à prendre sans directives claires, et une solitude qui oblige à réfléchir à ses actes.

Le romantisme noir, lui, inspire l’ambiance : des paysages vides, silencieux, parfois inquiétants. On y ressent à la fois de la beauté et du malaise. Le joueur explore des mondes perdus, comme un miroir de son propre isolement. Chaque planète devient un reflet de son état intérieur.

Ces deux inspirations renforcent le cœur du jeu : faire vivre au joueur une véritable expérience de la solitude. Une solitude profonde, mais aussi pleine de sens.

# Démo globale

**TODO :** démo en cours de production

# Fonctionnalités

### Implémenter le vaisseau de Valem

Nous avons décidé d’implémenter directement le vaisseau présenté par Valem dans ses tutoriels car le cadre de notre jeu se situe dans l’espace. En effet, nous allons pouvoir profiter de cette opportunité pour faire apparaître notre monde depuis le vaisseau et naviguer dedans sans subir de motion sickness ou autre effet retors. Créer un intérieur pour la vaisseau était déjà quelque chose que nous avions en tête, donc le choix de l’intégrer dans notre jeux était évident. De plus, avec la VR, pouvoir piloter le vaisseau en première personne était nécessaire, et cela impliquait de faire un intérieur. Nous avons donc créer notre propre version du vaisseau a partir des props proposé dans le tutoriel. Nous avons implémenté plusieurs mécaniques proposés dans le tutoriel (les cubes, le déplacement complet du joueur)

Nous avons également profité de cet espace et du tutoriel de Valem pour intégrer des socles pour les planètes. il nous a suffit de remplacer les cubes dans [cette vidéo](https://youtu.be/pMOHX1qD2bE?list=PLpEoiloH-4eM-fykn_3_QcJ-A_MIJF5B9) par des mini-planètes. Nous l’avons intégrer à la mécanique principale. il faut parcourir les planètes, à la recherche des mini-planètes et les placer dans leurs socles pour gagner. Détail important : quand le joueur place la mini-planète sur son socle, elle devient dépendant du vaisseau au niveau de la hiérarchie, afin qu’elle reste dans le vaisseau tout au jeu.

### Se débarrasser des planètes rondes

Le passage de notre jeu “3D classique” à la VR implique de devoir faire des sacrifices au niveau des mécaniques de gameplay. En effet, il est hors de notre portée de recréer une mécanique de gravité sur des petites planètes rondes avec un XR Controller. De plus, cela créerait beaucoup trop de motion sickness.

A votre suggestion, nous avons décidé de faire en sorte de nous téléporter vers ces planètes à la place à l’aide d’un bouton “atterrir”. Les nouvelles planètes seront désormais de nouvelles scènes créées à la main pour correspondre à notre histoire racontée. Ainsi, tout l’environnement du jeu devient plan et est compatible avec un setup VR.

Pour cela, nous nous sommes appuyés sur ce qu’on a appris en début d’année dans les TD, concernant la **formation des reliefs**, les **variations de textures et de couleurs**. Après le relief,  des éléments on été ajouté comme les roches, les arbres ou la végétation, en utilisant le placement de masse vu également dans les td en début d’année .

Avec plus de temps, nous aurions aimé aller plus loin : ajouter de l’eau, des effets météo, ou encore plus de détails dans la végétation et les éléments du décor pour renforcer l’immersion.

Un aperçu de nos 5 planètes misent à plat.

Earth : 

![image](https://github.com/user-attachments/assets/448773f6-c22a-45f0-ac58-f2cc8fc26751)

![image 1](https://github.com/user-attachments/assets/8a2f87f8-357e-450b-b6fa-bae0f25f78cc)

Frozen Planet: 

![image 2](https://github.com/user-attachments/assets/6e29394e-fc4a-49ba-9e1b-6e475b33da0d)

Desert Planet:

![image 3](https://github.com/user-attachments/assets/b78b5455-ce77-4b7d-b967-8ddce1027311)

Alien Planet: 

![image 4](https://github.com/user-attachments/assets/e366c5f0-08b9-4ea9-9289-4d3ee0d0e5ee)

Thundra Planet:

![image 5](https://github.com/user-attachments/assets/63277f6e-b3c1-4d05-afd0-fbd2113cb635)

### Implémenter un loader pour les éléments persistent

Une nouvelle problématique est venue avec cette histoire de changement de scène : nous voulons sauvegarder les gameObjects liés au joueur et au vaisseau entre les scènes. En effet, il serait bizarre de perdre sa progression lorsqu’on atterit sur une nouvelle planète. Nous avons décidé de passer un groupe d’objets qui persiste entre les scènes : le spaceship (qui contient les collectibles à récupérer) et le gameobject du XR Controller (player).

Nous avons implémenté un script Singleton sur ce groupe d’objets persistent afin d’éviter la duplication. Pourtant, lors d’aller-retours entre les scènes, le jeu plantait et la console disait qu’il existait deux XR Interactible Managers. Pourtant, tous les autres objets étaient bien uniques grâce au script Singleton !

Après quelques recherches, nous avons appris qu’Unity gérait ses XR Controllers d’une manière bien spécifique et avait son propre système de Singleton, que nous avons échoué à Override avec une classe fille.

Pour contourner ce problème, nous avons décidé de passer par une scène “Loader” qui contenait nos objets persistents au début du jeu, et sur lequel nous n’allons jamais revenir plus tard. Ainsi, pas d’aller retours dans une scène avec plusieurs XR Controllers.

Le vaisseau, le joueur et les objets de gestion du jeu sont placés dans un gameObject appelé “DonTDestroy”. Dans GameLoader, le script qui suit est présent.

![image 6](https://github.com/user-attachments/assets/f0f8c5b6-1506-4414-aaff-59d2adec7845)

![image 7](https://github.com/user-attachments/assets/099dc7c6-d5ed-49fa-ad1f-af196cbde1f2)

```csharp
public class GameLoader : MonoBehaviour
{
    public GameObject objectToPersist;
    public string sceneToLoad;

    void Start()
    {
        if (objectToPersist != null)
        {
            DontDestroyOnLoad(objectToPersist);
        }

        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }
}
```

Ce script prend simplement un objet en paramètre, le rend persistant et charge la scène en paramètre.

### S’asseoir aux commandes du vaisseau

Les déplacements en vaisseau autour du système solaire ont posé une problématique pour éviter le motion sickness en VR. De une, il faut rester sur un contexte à la première personne, c’est impensable d’avoir du TPS en VR. De deux, il faut verrouiller l’utilisateur sur un point unique pour qu’il n’ait pas de nausées lorsqu’on va se déplacer autour du monde.

La solution : on fait en sorte que le joueur puisse s’asseoir sur un siège. En plaçant un “EntryAnchor” et un “ExitAnchor” au siège, nous avons des points pour téléporter le joueur quand il veut s’asseoir ou se lever du siège. Ainsi, quand il est assis il ne peut pas bouger (plus de nausées !) et les contrôles manettes serviront plus tard à déplacer le vaisseau à la place.

![image 8](https://github.com/user-attachments/assets/e2a691db-5665-43fc-8286-8b9d630ce8ee)

La fonction importante qui gère ceci est la suivante :

```csharp
    void ToggleSeat()
    {
        isSeated = !isSeated;

        // (Dé)activation des locomotion providers et du CharacterController
        foreach (var p in providers)
            p.enabled = !isSeated;

        if (characterController != null)
            characterController.enabled = !isSeated;

        // Téléportation du rig
        if (xrOriginComp != null)
        {
            var target = isSeated ? seatAnchor : exitAnchor;
            if (target != null)
            {
                xrOriginComp.transform.SetPositionAndRotation(
                    target.position,
                    target.rotation
                );
            }
        }

        // Active/désactive le script de contrôle assis
        if (seatedControls != null)
            seatedControls.enabled = isSeated;
    }
```

### Déplacements du vaisseau autour du monde

Une bonne pratique pour éviter le motion sickness lorsqu’on cherche à déplacer un véhicule est de garder les éléments proches du joueur statiques et de bouger le reste du monde à la place. C’est ce que le tutoriel de Valem dont nous nous sommes servis explique dans la partie 7 : lorsqu’on veut aller à gauche, il faut déplacer l’entièreté du monde (stocké dans un objet “spaceOutside”) à l’**inverse** de l’entrée utilisateur, c’est à dire à droite. Ainsi, le modèle du joueur et ses environs ne bougent pas ; cela évite des bugs quand on déplace le XR Rig et retire encore une potentielle source de motion sickness.

Une fois le script seatedControls activé, les inputs sont désormais utilisés pour déplacer l’espace tout entier dans la scène, autour du vaisseau.

Voici le fonction Update() du script qui gère ça :

```csharp
void Update()
    {
        // Réinitialiser les inputs à chaque frame
        leftStickInput = Vector2.zero;
        rightStickInput = Vector2.zero;

        // Parcourir tous les XRController du nouveau Input System
        foreach (var device in InputSystem.devices.OfType<XRController>())
        {
            var stick = device.TryGetChildControl<Vector2Control>("primary2DAxis");
            if (stick == null)
                continue;

            // Séparer les inputs main gauche et main droite
            var value = stick.ReadValue();
            if (device.usages.Contains(CommonUsages.LeftHand))
                leftStickInput = value;
            else if (device.usages.Contains(CommonUsages.RightHand))
                rightStickInput = value;
        }

        // Déplacements
        if (spaceOutside != null)
        {
            float deltaZ = leftStickInput.x * moveSpeed * Time.deltaTime;
            float deltaX = leftStickInput.y * moveSpeed * Time.deltaTime;
						
						// Translation
            spaceOutside.transform.Translate(-deltaZ, 0f, -deltaX, Space.World);

						// Rotation
            if (shipTransform != null && spaceOutside != null)
            {
                float yawAngle = -rightStickInput.x * rotationSpeed * Time.deltaTime;
                spaceOutside.transform.RotateAround(
                    shipTransform.position,
                    Vector3.up,
                    yawAngle
                );

                // Il faut aussi tourner la skybox
                float currentSkyboxRot = RenderSettings.skybox.GetFloat("_Rotation");
                RenderSettings.skybox.SetFloat("_Rotation", currentSkyboxRot - yawAngle);
            }

            // On réinitialise l'accélération des drapeaux pour éviter qu'ils partent en cacahuète
            childCloths = spaceOutside.GetComponentsInChildren<Cloth>();
            foreach (var c in childCloths)
            {
                c.useGravity = false;
                c.externalAcceleration = Vector3.zero;
            }
            foreach (var c in childCloths)
                c.ClearTransformMotion();
        }
    }
```

### Entrée, sortie et atterrissage du vaisseau

Pour les mécaniques d’entrée et sorties du vaisseau, nous avons repris les tutoriels de Valem avec les boutons pour ouvrir et fermer les portes, et au lieu de lancer une animation, le joueur se téléporte hors du vaisseau (à ce moment là, l’intérieur du vaisseau se désactive et l’extérieur s’active).

![image 9](https://github.com/user-attachments/assets/70efd081-0a02-44f8-97d2-423314a2787b)

![image 10](https://github.com/user-attachments/assets/02a2542e-3589-4abc-9d84-f808bd511996)

![image 11](https://github.com/user-attachments/assets/7eabcab8-0970-4023-b30a-508c232998f3)

Pour l’atterrissage, comme expliqué précédemment, le Spaceship est persistant entre les scènes. Quand le vaisseau proche d’une planète, et qu’il appuie sur un bouton sur sa manette il se téléporte dans la scène représentant la planète correspondante (le vaisseau ne bougeant dans aucune scene, il n’y a pas besoin de gérer son emplacement, il suffit juste de bien placer les objets sur la scène).

### Retrouver nos objets au bon endroit après le changement de scène

Lorsque qu’on passe d’une scène de planète à la scène d’espace, nous le vaisseau doit se retrouver l’endroit où on l’a laissé, près de la planète que l’on viens de visiter. Techniquement, étant donnée que c’est le monde qui se déplace, il fallait sauvegarder la position et rotation du monde au moment de quitter l’espace et remettre le monde a ces coordonnées quand on reviens dans l’espace. Cette partie nous a value beaucoup de bugs à résoudre a cause des relations entre les scènes.

### Relier les anciennes mécaniques

Les anciennes mécaniques ont été re-intégrées dans cette version du jeu (le post-processing, l’effet de solitude, les photos, etc…) en les passant en tant qu’objets persistants et ça n’a étonnamment pas causé de conflit particulier avec les nouvelles contraintes du projet.

# Sources

- Tuto de Valem : https://www.youtube.com/playlist?list=PLpEoiloH-4eM-fykn_3_QcJ-A_MIJF5B9
