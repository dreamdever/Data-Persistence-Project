using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string playerName;
    public HighScore highScore;
    public TMP_InputField playerNameInput;
    public TextMeshProUGUI bestScoreText;

    private const string saveFileName = "highScore.json";

    void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        LoadHighScore();
        DontDestroyOnLoad(gameObject);
    }

    public void SaveHighScore(float score) {
        if (highScore == null || score > highScore.score) {
            HighScore saveData = new HighScore(){
                score = score,
                name = playerName
            };

            string json = JsonUtility.ToJson(saveData);

            File.WriteAllText(Application.persistentDataPath + $"/{saveFileName}", json);

            highScore = saveData;
        }
    }

    private void LoadHighScore()
    {
        string path = Application.persistentDataPath + $"/{saveFileName}";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScore data = JsonUtility.FromJson<HighScore>(json);

            highScore = data;

            bestScoreText.text = $"Best score : {DataManager.instance.highScore.name} : {DataManager.instance.highScore.score}";
        }
    }

    public void StartGame() {
        playerName = playerNameInput.text;
        SceneManager.LoadScene(1);
    }

    [Serializable]
    public class HighScore {
        public float score;
        public string name;
    }
}
