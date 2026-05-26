using UnityEditor;
using UnityEngine;

namespace KimJeongDev.EditorTools
{
    /// <summary>
    /// Fenêtre d'outils personnalisée pour centraliser les actions de développement.
    /// </summary>
    public class CustomToolsWindow : EditorWindow
    {
        // 1. Définition du point d'entrée dans la barre de menu d'Unity
        [MenuItem("KJD Protocol/My Tools")]
        public static void ShowWindow()
        {
            // 2. Instanciation ou récupération de la fenêtre si elle est déjà ouverte
            EditorWindow window = GetWindow<CustomToolsWindow>("My Tools");

            // 3. Affichage forcé de la fenêtre
            window.Show();
        }

        // 4. Boucle de rendu de l'interface utilisateur (UI) de l'éditeur
        private void OnGUI()
        {
            // C'est ici que la structure de l'outil sera dessinée.
            // Pour l'instant, plaçons un simple texte et un bouton de validation.

            GUILayout.Label("Initialisation du panneau de contrôle...", EditorStyles.boldLabel);

            GUILayout.Space(10); // Ajoute un espace de 10 pixels pour aérer

            if (GUILayout.Button("Test d'intégrité (Clique-moi)"))
            {
                Debug.Log("Les outils personnalisés sont opérationnels.");
            }
        }
    }
}