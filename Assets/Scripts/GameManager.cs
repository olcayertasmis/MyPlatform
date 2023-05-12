using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,
    Play,
    Pause,
    GameOver,
    Loading
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Player player;
    public UIManager _UIManager;

    public GameState currentState;

    private int lastLevelIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        player = FindObjectOfType<Player>();
        _UIManager = FindObjectOfType<UIManager>();
        Subscribe();

        UpdateGameState(GameState.Menu);
    }

    private void Subscribe()
    {
        player.OnDead += Player_OnDead;
        SceneManager.sceneLoaded += SceneManager_OnSceneLoaded;
    }

    private void UpdateGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Menu:
                player.isStart = false;
                _UIManager.ActivateStartPanel();
                _UIManager.DeactivateRestartPanel();
                break;
            case GameState.Play:
                player = FindObjectOfType<Player>();
                Subscribe();
                player.isStart = true;
                _UIManager.DeactivateStartPanel();
                _UIManager.DeactivateRestartPanel();
                break;
            case GameState.Pause:
                break;
            case GameState.GameOver:
                player.isStart = false;
                _UIManager.DeactivateStartPanel();
                _UIManager.ActivateRestartPanel();
                break;
            case GameState.Loading:
                player.isStart = false;
                _UIManager.DeactivateStartPanel();
                _UIManager.DeactivateRestartPanel();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        currentState = state;
    }


    #region Events

    private void Player_OnDead()
    {
        _UIManager.ActivateRestartPanel();
        UpdateGameState(GameState.GameOver);
    }

    private void SceneManager_OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _UIManager = FindObjectOfType<UIManager>();
    }

    #endregion

    public void RestartGame()
    {
        StartCoroutine(IERestartGame());
    }

    private IEnumerator IERestartGame()
    {
        UpdateGameState(GameState.Loading);

        var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

        AsyncOperation levelLoader = null;
        levelLoader = SceneManager.LoadSceneAsync(activeSceneIndex);

        while (!levelLoader.isDone) yield return null;
        UpdateGameState(GameState.Play);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        // _UIManager.DeactivateStartPanel();
        // UpdateGameState(GameState.Play);
        StartCoroutine(IEStartGame());
    }

    private IEnumerator IEStartGame()
    {
        UpdateGameState(GameState.Loading);

        AsyncOperation levelLoader = null;
        levelLoader = SceneManager.LoadSceneAsync("Level1");

        while (!levelLoader.isDone) yield return null;
        UpdateGameState(GameState.Play);
    }

    public void NextLevel()
    {
        StartCoroutine(IENextLevel());
    }

    private IEnumerator IENextLevel()
    {
        var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex;

        if (activeSceneIndex == SceneManager.sceneCount) nextLevelIndex = 0;
        else nextLevelIndex = activeSceneIndex + 1;

        if (lastLevelIndex < nextLevelIndex)
        {
            lastLevelIndex = nextLevelIndex;
            PlayerPrefs.SetInt("LastLevel", lastLevelIndex);
        }

        UpdateGameState(GameState.Loading);

        AsyncOperation levelLoader = null;
        levelLoader = SceneManager.LoadSceneAsync(nextLevelIndex);

        while (!levelLoader.isDone) yield return null;
        UpdateGameState(GameState.Play);
    }


    public void ContinueGame()
    {
        StartCoroutine(IEContinueGame());
    }

    private IEnumerator IEContinueGame()
    {
        UpdateGameState(GameState.Loading);

        AsyncOperation levelLoader = null;
        levelLoader = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt("LastLevel"));

        while (!levelLoader.isDone) yield return null;
        UpdateGameState(GameState.Play);
    }
}