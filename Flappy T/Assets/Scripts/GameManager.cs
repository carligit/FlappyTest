using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System;

public class GameManager : MonoBehaviour 
{
    public enum ScrollObjectLayer { background, midground, foreground, powerUps };

    public static GameManager instance;
    public bool GameOver;
    public bool GameStarted;
    public int defaultScrollSpeed;
    public ScrollLayerSpeed[] scrollSpeeds;
    public GameObject player;
    public float bottomSpawnHeight;
    public float topSpawnHeight;

    public StringEvent OnScoreUpdated;
    public StringEvent OnGameOver;

    private int currentScore;
    private int HighestScore;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        if (PlayerPrefs.HasKey("HighScore"))
        {
            HighestScore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }

    }

    void OnLevelWasLoaded(int level)
    {
        GameManager.instance.GameOver = false;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void StartGame()
    {
        GameStarted = true;
    }

    public void SetGameOver()
    {
        GameManager.instance.GameOver = true;

        if (currentScore > HighestScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            HighestScore = currentScore;
        }

        Array.ForEach(FindObjectsOfType<Bullet>(), (b) => {b.Return();});

        OnGameOver.Invoke(HighestScore.ToString());
    }

    public void AddToScore()
    {
        if (!GameOver)
        {
            currentScore++;
            OnScoreUpdated.Invoke(currentScore.ToString());
        }
    }

    public float GetScrollSpeed(ScrollObjectLayer layer)
    {
        var speedLayer = scrollSpeeds.FirstOrDefault(s => s.scrollLayer == layer);

        if (speedLayer == null)
        {
            return defaultScrollSpeed;
        }

        return speedLayer.speed;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [System.Serializable]
    public class ScrollLayerSpeed
    {
        public ScrollObjectLayer scrollLayer;
        public float speed;
    }
}

[System.Serializable]
public class StringEvent : UnityEvent<string>
{

}

[System.Serializable]
public class IntEvent : UnityEvent<int>
{

}
