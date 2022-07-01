using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GetScripts m_GetScripts;
    public List<GameObject> FinishWalls;
    public Vector3 BombMovePos;

    public Transform PowerBombParent;
    [Range(0, 20)]
    public int PowerBombCount;
    public int WallNumb;

    public bool HasTheGameStarted;
    public bool FinishGame;

    void Awake()
    {
        PowerBombCount = PowerBombParent.childCount;
        WallNumb = 0;
        HasTheGameStarted = false;
        FinishGame = false;
        m_GetScripts.Player.Initialize();
        m_GetScripts.InGameCanvasManager.Initialize();
    }

    public void GameOver()
    {
        m_GetScripts.Player.PlayerGameOver();
        m_GetScripts.Bomb.BombActivated();
        m_GetScripts.InGameCanvasManager.CanvasGameOver();
        HasTheGameStarted = false;
    }

    public void FinishLevel()
    {
        StartCoroutine(m_GetScripts.Player.PlayerFinishGame());
        HasTheGameStarted = false;
        WallNumb = (int)Mathf.Round(m_GetScripts.InGameCanvasManager.BombPowerRedBar.fillAmount * FinishWalls.Count) - 1;
        if (WallNumb <= 0.05f)
        {
            BombMovePos = FinishWalls[0].transform.position;
        }
        else
        {
            BombMovePos = FinishWalls[WallNumb].transform.position;
        }
    }
}
