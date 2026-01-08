using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;

public class PlayerStas : NetworkBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    [Networked, OnChangedRender(nameof(OnhealthChanged))] public int currentHealth { get; set; }
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    [Header("Score")]
    [Networked, OnChangedRender(nameof(OnChangeScore))] public int Score { get; set; }

    public override void Spawned()
    {
        currentHealth = maxHealth;
        Score = 0;
        UpDateUI();
        UpDateScore();

        GameManagerPro.instance.RegisterPlayer(this);
    }

    public void OnhealthChanged()
    {
        UpDateUI();
    }

    public void OnChangeScore()
    {
        UpDateScore();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RpcAddScoreRequest(int value)
    {
        RpcAddScore(value);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcAddScore(int value)
    {
        Score += value;
    }

    public void UpDateScore()
    {
        if (!Object.HasInputAuthority) return;
        UIManager.instance.scoretext.text = "Score: " + Score;
    }

    public void UpDateUI()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasInputAuthority) return;
        if (other.CompareTag("Coin"))
        {
            RpcAddScoreRequest(10);
        }
    }
}
