using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    Playing, Paused, Ended, WaitingToStart
};

public class Controller : Singleton<Controller>
{
    public int score = 0;

    public EntityQueue EntityQueue;
    public Transform targetTransform;
    public float moveDownspeed = 300;
    public GameState gameState = GameState.WaitingToStart;
    public static event Action GameEnded;
    void Start()
    {
        InputController.PointerDown += OnPointerDown;

        score = 0;
        InGamePanel.instance.UpdateScore(0, 0);
        InGamePanel.instance.UpdateTimer(GameData.remainingTime);
    }

    private void OnDestroy()
    {
        InputController.PointerDown -= OnPointerDown;

    }
    public void StartTimer()
    {
        StartCoroutine(Timer());
    }
    public IEnumerator Timer()
    {
        while (GameData.remainingTime > 0)
        {
            GameData.remainingTime -= Time.deltaTime;

            InGamePanel.instance.UpdateTimer(GameData.remainingTime);
            yield return null;
        }

        ChangeGameState(GameState.Ended);
        GameEnded?.Invoke();
    }
    public void ChangeGameState(GameState state)
    {
        gameState = state;
    }
    private void OnPointerDown(Vector3 pos)
    {
        if(!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (gameState == GameState.WaitingToStart) {
            StartTimer();
            ChangeGameState(GameState.Playing);
        }

        if (Controller.instance.gameState != GameState.Playing) return;


        var entity = EntityQueue.GetNextEntity();

        if (entity == null) return;

        entity.MoveDown(targetTransform.position, moveDownspeed);

    }


    public void AddScore(int amount, Entity relatedEntity = null)
    {
        var previousScore = score;
        score += amount;
        InGamePanel.instance.UpdateScore(score, previousScore);

        if (relatedEntity != null) { 
            relatedEntity.ScoreAdded += amount;

            if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    var pos = relatedEntity.scoreBubbleSpawnTransform.position;
                    var color = relatedEntity.color;
                    DOVirtual.DelayedCall(i * 0.1f + 0.3f, () =>
                    {

                        ScoreBubbleSpawner.instance.SpawnScoreBubble(relatedEntity.scoreBubbleSpawnTransform.position, color);

                    });
                }

            }
            else if (amount < 0){
                for (int i = 0; i < -amount; i++)
                {
                    var pos = relatedEntity.scoreBubbleSpawnTransform.position;
                    DOVirtual.DelayedCall(i * 0.005f + 0.005f, () =>
                    {
                        ScoreBubbleSpawner.instance.SpawnNegativeScoreBubble(pos, Color.red);

                    });
                }
            }
        }
    }
}

public static class GameData
{
    public static float buildingRange = 0.3f;
    public static float treeRange = 0.2f;
    public static float remainingTime = 120f; 

}
