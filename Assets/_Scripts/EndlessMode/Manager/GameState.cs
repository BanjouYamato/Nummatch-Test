using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }
    State _state;
    public State state => _state;
    public static event Action OnClear;
    public static event Action OnGOV;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void SelectState(State state)
    {
        _state = state;
        switch(state)
        {
            case State.impossibleInput:
                break;
            case State.play:
                break;
            case State.lose:
                HandleGOV(); break;
            case State.stageClear:
                HandleClearStage(); break;
        }
    }
    void HandleGOV()
    {
        OnGOV.Invoke();
    }
    void HandleClearStage()
    {
        OnClear.Invoke();
    }
}
public enum State
{
    stageClear,
    lose,
    play,
    impossibleInput
}
