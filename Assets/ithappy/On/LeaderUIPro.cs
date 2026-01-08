using TMPro;
using UnityEngine;

public class LeaderUIPro : MonoBehaviour
{
    private void OnEnable()
    {
        var leaderBoard = GameManagerPro.instance.GetLeaderBoard();
        foreach(Transform t in GameManagerPro.instance.contemt)
        {
            Destroy(t.gameObject);
        }

        foreach(var p in leaderBoard)
        {
            var obj = Instantiate(GameManagerPro.instance.textPrefab, GameManagerPro.instance.contemt);
            obj.GetComponent<TextMeshProUGUI>().text = $"Player {p.Object.InputAuthority.PlayerId} : {p.Score}";
        }
    }
}
