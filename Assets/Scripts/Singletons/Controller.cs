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

    private string scoreKey = "Score";
    private string highScoreKey = "HighScore";

    public float buildingRange = 0.3f;
    public float treeRange = 0.2f;
    public float remainingTime = 120f;
    public int moveCount = 100;

    int highScore;

    public static event Action GameEnded;
    public static event Action EntityFell;



    void Start()
    {
        InputController.PointerDown += OnPointerDown;

        score = 0;
        InGamePanel.instance.UpdateScore(0, 0);
        InGamePanel.instance.UpdateTimer(moveCount);
    }

    private void OnDestroy()
    {
        InputController.PointerDown -= OnPointerDown;
        DOTween.KillAll();

    }
    public void StartTimer()
    {
        StartCoroutine(Timer());
    }
    public IEnumerator Timer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            //InGamePanel.instance.UpdateTimer(remainingTime);
            yield return null;
        }
        EndGame();
    }
    public int GetHighScore()
    {
        return  score > highScore ? score : highScore;
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
            //StartTimer();
            ChangeGameState(GameState.Playing);
        }

        if (Controller.instance.gameState != GameState.Playing) return;

        if (moveCount <= 0) return;

        var entity = EntityQueue.GetNextEntity();

        if (entity == null) return;

        entity.MoveDown(targetTransform.position, moveDownspeed);
        moveCount--;

        InGamePanel.instance.UpdateTimer(moveCount);
        EntityFell?.Invoke();
        if(moveCount == 0)
        {
            EndGame();
        }

    }

    public void EndGame()
    {
        SoundController.instance.PlayAudio(SoundController.instance.levelComplete, 0.4f);
        PlayerPrefs.SetInt(scoreKey, score);
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        highScore = score > highScore ? score : highScore;
        PlayerPrefs.SetInt(highScoreKey, highScore);
        
        ChangeGameState(GameState.Ended);
        GameEnded?.Invoke();
    }
    public void AddScore(int amount, Entity relatedEntity = null)
    {
        var previousScore = score;
        //score += amount;
        //InGamePanel.instance.UpdateScore(score, previousScore);

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

                        ScoreBubbleSpawner.instance.SpawnScoreBubble(relatedEntity.scoreBubbleSpawnTransform.position, color, () => {

                            score += 1;
                            InGamePanel.instance.UpdateScore(score, previousScore);
                        });

                    });
                }

            }
            else if (amount < 0){
                for (int i = 0; i < -amount; i++)
                {
                    var pos = relatedEntity.scoreBubbleSpawnTransform.position;
                    DOVirtual.DelayedCall(i * 0.05f + 0.05f, () =>
                    {
                        ScoreBubbleSpawner.instance.SpawnNegativeScoreBubble(pos, Color.red, () => {

                            score -= 1;

                            score = score < 0 ? 0 : score;  

                            InGamePanel.instance.UpdateScore(score, previousScore);
                        });

                    });
                }
            }
        }
    }
}

public static class GameData
{


}
