using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour{

    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _quitGameButton;
    [SerializeField] private Button _tutorialButton;

    void Start(){
        _startGameButton.onClick.AddListener(StartGame);
        _quitGameButton.onClick.AddListener(QuitGame);
        _tutorialButton.onClick.AddListener(Tutorial);
    }

    public void StartGame(){
        SceneManager.LoadScene("MainGameScene");
    }

    public void Tutorial(){
        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitGame(){
        Application.Quit();
    }
}