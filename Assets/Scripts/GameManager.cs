using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    PlayerController player;

    public bool gameOver = false;
    public bool pauseGame = false;
    public bool beatLevel = false;

    //Levels
    public string playAgainLevelToLoad;
    public string nextLevelToLoad;

    //UI elements
    public Slider slider;
    public Text ammoCount;
    public Text waveIndicator;
    public GameObject pauseMenuUI;
    public GameObject endMenuUI;

    void Start()
    {
        // Get a reference to the GameManager component for use by other scripts
        if (gm == null)
            gm = this.gameObject.GetComponent<GameManager>();

        player = GameObject.Find("Player").GetComponent<PlayerController>();

        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (pauseGame)
                ResumeGame();
            else
                PauseGame();

        if (!player.isAlive)
            EndGame();
    }

    public void EndGame()
    {
        endMenuUI.SetActive(true);
        gameOver = true;
    }

    public void BeatLevel()
    {
        waveIndicator.text = "Level Complete!";
        gameOver = true;
    }

    //Game UI
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void SetAmmoCount(int count)
    {
        if (count > 0)
            ammoCount.text = count.ToString();
        else
            ammoCount.text = "Reload!";
    }

    public void SetWaveCount(int wave)
    {
        waveIndicator.text = ("Wave " + wave);
    }

    //Menu Elements
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        pauseGame = true;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pauseGame = false;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(playAgainLevelToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
