using UnityEngine;
using TMPro;

public class StatsPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _statsText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void DisplayStats(int waveNumber, int enemiesKilled){
        _statsText.text = $"Wave: {waveNumber}\nEnemies Killed: {enemiesKilled}";
    }

}
