using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class GameManagerPro : NetworkBehaviour
{
    public static GameManagerPro instance;

    public GameObject textPrefab;
    public Transform contemt;
    public Kien InputActions;

    public GameObject leaderUI;

    private void Awake()
    {
        instance = this;
        InputActions.Player.Leader.started += ToggleLeader;
    }

    public List<PlayerStas> allPlayer = new List<PlayerStas>();

    public void RegisterPlayer(PlayerStas player)
    {
        if (!allPlayer.Contains(player))
        {
            allPlayer.Add(player);
        }
    }

    public List<PlayerStas> GetLeaderBoard()
    {
        List<PlayerStas> result = new List<PlayerStas>(allPlayer);
        result.Sort((x, y) => y.Score.CompareTo(x.Score));
        return result;
    }

    public void ToggleLeader(InputAction.CallbackContext context)
    {
        leaderUI.SetActive(!leaderUI.activeSelf);  
    }
}
