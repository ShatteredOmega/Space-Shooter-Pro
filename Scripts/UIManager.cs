using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    private int _score = 0;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;
    [SerializeField]
    private GameManager _gameManager;
    void Start()
    {
        _scoreText.text = "Score: " + _score;
    }

    // Update is called once per frame

    public void AddScore(int pointsGained)
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _score += pointsGained;
        _scoreText.text = "Score: " + _score;
        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    public void SetLifeUI(int currentLives)
    {
        if(currentLives < 0)
        {
            currentLives = 0;
        }
        _livesImage.sprite = _liveSprites[currentLives];
    }
    public void GameOver()
    {
        GameOverSequence();
    }
    void GameOverSequence()
    {
        _gameManager.SetGameOver();
        _restartText.SetActive(true);
        StartCoroutine("GameOverFlickerRoutine");
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }    
}
