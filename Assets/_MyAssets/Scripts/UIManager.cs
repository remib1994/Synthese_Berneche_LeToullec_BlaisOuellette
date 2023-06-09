﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class UIManager : MonoBehaviour  {
    
    [SerializeField] private int _score =  default;
    [SerializeField] private TextMeshProUGUI _txtScore = default;
    [SerializeField] private TextMeshProUGUI _txtTempsPartie = default;
    [SerializeField] private TextMeshProUGUI _txtTemps = default;
    [SerializeField] private GameObject _pausePanel = default;

    private bool _pauseOn = false;
    public bool _isSoundOn = true;
    // Start is called before the first frame update

    private void Start()
    {
        _score = 0;
        _txtScore.text = "Score : " + PlayerPrefs.GetInt("Score").ToString();
        _txtTempsPartie.text = "Temps : " + PlayerPrefs.GetFloat("TempsDuJeu").ToString("f2");
        Time.timeScale = 1;
    }

    private void Update()
    {
        _txtTemps.text = "Temps : " + Time.timeSinceLevelLoad.ToString("f2");
        UpdateScore();

        if (_pauseOn && Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(0);
        }
        if (_pauseOn && Input.GetKeyDown(KeyCode.S))
        {
            ToggleSound();
        }            

        // Permet la gestion du panneau de pause (marche/arrêt)
        if ((Input.GetKeyDown(KeyCode.Escape) && !_pauseOn))
        {
            _pausePanel.SetActive(true);
            Time.timeScale = 0;
            _pauseOn = true;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && _pauseOn))
        {
            _pausePanel.SetActive(false);
            Time.timeScale = 1;
            _pauseOn = false;
        }

        //if (_txtRestart.gameObject.activeSelf && Input.GetKeyDown(KeyCode.R)) {
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //}
        //else if (_txtRestart.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
        //    SceneManager.LoadScene(0);
        //}
    }

    public float getTime()
    {
        return Time.timeSinceLevelLoad;
    }

    public int getScore()
    {
        return _score;
    }
    public void AjouterScore(int points) {
        _score += points;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _txtScore.text = "Score : " + _score.ToString();
    }

    private void GameOverSequence() {

        StartCoroutine(GameOverBlinkRoutine());
    }

    IEnumerator GameOverBlinkRoutine() {
        while (true) {

            yield return new WaitForSeconds(0.7f);

            yield return new WaitForSeconds(0.7f);
        }
    }

    public void ResumeGame() {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
        _pauseOn = false;
    }

    public void ToggleSound()
    {
        _isSoundOn = !_isSoundOn;
        AudioListener.volume =_isSoundOn ? 1 : 0;
    }
}
