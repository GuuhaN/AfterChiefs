using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI.Scripts.Main_Menu_Buttons
{
    public class MainMenuUI : MonoBehaviour
    {
        void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            Button createGame = root.Q<Button>("Create_Game");
            Button quitGame = root.Q<Button>("Quit_Game");
            Button debug = root.Q<Button>("Debug");

            createGame.clicked += CreateGameButton;
            quitGame.clicked += QuitGameButton;
            debug.clicked += DebugButton;
        }

        void CreateGameButton()
        {
            
        }
        
        void QuitGameButton()
        {
            Application.Quit();
        }
        
        void DebugButton()
        {
            SceneManager.LoadScene("Scenes/Arena");
        }
    }
}
