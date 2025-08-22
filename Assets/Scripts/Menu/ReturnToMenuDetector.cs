using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenuDetector : MonoBehaviour{

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}