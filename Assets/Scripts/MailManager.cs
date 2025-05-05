using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MailManager : MonoBehaviour
{
    [Header("Inside Elements")]
    public Transform mailListContent; // Parent où les boutons de mails sont générés
    public GameObject mailDetailPanel; // Panneau pour afficher le détail du mail
    public TextMeshProUGUI mailDetailTitle; // Titre du mail affiché
    public TextMeshProUGUI mailDetailContent; // Contenu du mail affiché

    [Header("Outside Elements")]
    public TextMeshProUGUI mailCounterText; // Texte pour afficher le compteur des mails

    [Header("Mail Prefab")]
    public GameObject mailPrefab; // Le prefab pour les boutons de mail

    [Header("UI Elements")]
    public GameObject scrollView; // ScrollView qui contient les mails (initialement invisible)

    private List<Mail> mailList = new List<Mail>(); // Liste des mails affichés
    private List<Mail> predefinedMails = new List<Mail>(); // Liste de 10 mails prédéfinis
    private bool isInsideVisible = false; // Indique si la section "Inside" est visible
    private bool isMailDetailVisible = false; // Indique si le mail detail panel est visible

    void Start()
    {
        // Initialiser les mails prédéfinis
        InitializePredefinedMails();

        // Ajouter deux mails de départ
        AddMail(predefinedMails[0].Title, predefinedMails[0].Content);
        AddMail(predefinedMails[1].Title, predefinedMails[1].Content);

        // S'assurer que la scroll view et le mail panel sont invisibles au départ
        scrollView.SetActive(false);
        mailDetailPanel.SetActive(false);
    }

    void Update()
    {
        // Touche "M" pour ouvrir/fermer la scroll view
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleInside(!isInsideVisible);
        }

        // Touche "P" pour ajouter un mail aléatoire
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddRandomMail();
        }
    }

    public void AddMail(string title, string content)
    {
        // Ajouter le mail à la liste
        mailList.Add(new Mail(title, content));

        // Mettre à jour l'affichage des mails
        UpdateMailUI();
        UpdateMailCounter();
    }

    private void AddRandomMail()
    {
        // Ajouter un mail aléatoire à partir de la liste prédéfinie
        if (predefinedMails.Count > 0)
        {
            int randomIndex = Random.Range(0, predefinedMails.Count);
            Mail selectedMail = predefinedMails[randomIndex];

            // Ajouter le mail et le retirer de la liste prédéfinie
            AddMail(selectedMail.Title, selectedMail.Content);
            predefinedMails.RemoveAt(randomIndex);
        }
        else
        {
            Debug.Log("Tous les mails prédéfinis ont déjà été ajoutés !");
        }
    }

    private void InitializePredefinedMails()
    {
        predefinedMails.Add(new Mail("Bienvenue à bord", "Nous sommes ravis de vous avoir parmi nous !"));
        predefinedMails.Add(new Mail("Mise à jour du système", "Une mise à jour importante a été installée."));
        predefinedMails.Add(new Mail("Invitation à un événement", "Rejoignez-nous pour un événement spécial."));
        predefinedMails.Add(new Mail("Nouvelle mission", "Votre prochaine mission est disponible !"));
        predefinedMails.Add(new Mail("Rappel de sécurité", "Assurez-vous de suivre les consignes de sécurité."));
        predefinedMails.Add(new Mail("Message personnel", "Un proche vous a envoyé un message !"));
        predefinedMails.Add(new Mail("Alertes système", "Une activité inhabituelle a été détectée."));
        predefinedMails.Add(new Mail("Conseils", "Voici quelques astuces pour avancer rapidement."));
        predefinedMails.Add(new Mail("Félicitations", "Vous avez débloqué une récompense !"));
        predefinedMails.Add(new Mail("Résumé quotidien", "Voici un résumé de votre activité aujourd’hui."));
    }

    private void UpdateMailUI()
    {
        // Supprimer les anciens boutons
        foreach (Transform child in mailListContent)
        {
            Destroy(child.gameObject);
        }

        // Créer un bouton pour chaque mail
        for (int i = 0; i < mailList.Count; i++)
        {
            Mail mail = mailList[i];
            GameObject mailButton = Instantiate(mailPrefab, mailListContent);

            // Mettre à jour le texte du bouton (TextMeshProUGUI)
            TextMeshProUGUI buttonText = mailButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = mail.Title;
            }

            // Ajouter un listener pour ouvrir les détails du mail
            int index = i; // Capturer l'index
            Button button = mailButton.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners(); // Nettoyer les anciens listeners
                button.onClick.AddListener(() => OpenMail(index));
            }
        }
    }

    private void OpenMail(int index)
    {
        if (index >= 0 && index < mailList.Count)
        {
            Mail mail = mailList[index];

            // Afficher les détails du mail
            mailDetailPanel.SetActive(true);
            mailDetailTitle.text = mail.Title;
            mailDetailContent.text = mail.Content;

            // Marquer le mail comme lu
            mail.IsOpened = true;

            // Afficher le mail detail panel
            isMailDetailVisible = true;
        }
    }

    private void UpdateMailCounter()
    {
        // Afficher uniquement le nombre total de mails reçus
        mailCounterText.text = mailList.Count.ToString();
    }

    private void ToggleInside(bool isVisible)
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

    // Classe pour un mail
    [System.Serializable]
    public class Mail
    {
        public string Title; // Titre du mail
        public string Content; // Contenu du mail
        public bool IsOpened; // Statut de lecture

        public Mail(string title, string content)
        {
            Title = title;
            Content = content;
            IsOpened = false;
        }
    }
}
