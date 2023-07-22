using Nox7atra.Mazes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private GameContext _gameContext;
    [SerializeField] private EcsStartup _ecsStartup;


    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _gameContext.AddListener(_gameUI);
        _gameContext.AddListener(_ecsStartup);


        _gameUI.SetStartButton(() => _gameContext.StartGame());
    }

}
