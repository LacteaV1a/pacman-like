using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : GameSystem
{
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _uiContent;

    public override void OnFinishGame()
    {
        SetActive(true);
    }

    public override void OnStartGame()
    {
        SetActive(false);
    }

    public void SetActive(bool value)
    {
        _uiContent.SetActive(value);
    }

    public void SetStartButton(Action action)
    {
        _startButton.onClick.AddListener(()=>action?.Invoke());
    }

}
