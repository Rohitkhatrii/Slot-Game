using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public LeverController arcadeLever;

    public TextMeshProUGUI freeSpinsText;

    public int freeSpins = 0;

    public GameObject bettingPanel;

    public enum GameState { Idle, Spinning, Evaluating }

    [Header("State")]
    public GameState currentState = GameState.Idle;

    [Header("Economy")]
    public int playerGold = 100;
    private int currentBet = 10;
    public TextMeshProUGUI goldText;

    [Header("References")]
    public ReelController[] reels;

    void Start()
    {
        UpdateGoldUI();
        UpdateFreeSpinsUI();
    }

    void Update()
    {
        // Pressing spacebar triggers the spin safely through our economy check
        if (Input.GetKeyDown(KeyCode.Space) && currentState == GameState.Idle)
        {
            TrySpin();
        }
    }

    // Called by your Bet buttons
    public void SetBet(int amount)
    {
        if (currentState != GameState.Idle) return;
        currentBet = amount;
        Debug.Log($"Bet set to: {currentBet}G");
    }

    // Checks gold, takes the bet, and starts the spin routine
    public void TrySpin()
    {
        if (currentState != GameState.Idle) return;

        if (freeSpins > 0)
        {
            // Free Spin: Deduct a free spin, but DO NOT deduct gold
            freeSpins--;
            UpdateFreeSpinsUI();
            Debug.Log("Free Spin used! Remaining: " + freeSpins);
            StartCoroutine(RoutineSpin());
        }
        else if (playerGold >= currentBet)
        {
            // Normal Spin: Deduct gold based on current bet
            playerGold -= currentBet;
            UpdateGoldUI();
            StartCoroutine(RoutineSpin());
        }
        else
        {
            Debug.Log("Not enough gold to spin!");
        }
    }

    private System.Collections.IEnumerator RoutineSpin()
    {
        currentState = GameState.Spinning;

        if (arcadeLever != null) arcadeLever.PullDown();
        // Hide the panel as soon as the spin starts
        if (bettingPanel != null) bettingPanel.SetActive(false);

        foreach (ReelController reel in reels)
        {
            reel.StartSpinning();
        }

        yield return new WaitForSeconds(2f);

        foreach (ReelController reel in reels)
        {
            reel.StopSpinning();
            yield return new WaitForSeconds(0.5f);
        }

        currentState = GameState.Evaluating;
        EvaluatePayouts();

        if (arcadeLever != null) arcadeLever.ResetUp();
        // Show the panel again after payouts are done
        if (bettingPanel != null) bettingPanel.SetActive(true);

        currentState = GameState.Idle;
    }

    private void EvaluatePayouts()
    {
        SlotSymbol reel1Result = reels[0].GetCenterSymbol();
        SlotSymbol reel2Result = reels[1].GetCenterSymbol();
        SlotSymbol reel3Result = reels[2].GetCenterSymbol();

        if (reel1Result == null || reel2Result == null || reel3Result == null) return;

        // Check if all 3 symbols match
        // Check if all 3 symbols match
        if (reel1Result == reel2Result && reel2Result == reel3Result)
        {
            int winnings = reel1Result.payoutMultiplier * currentBet;
            playerGold += winnings;
            Debug.Log($"<color=green>JACKPOT! Won {winnings}G!</color>");

            // AWARD FREE SPINS if the matching symbol is a Bell OR a BAR
            if (reel1Result.name == "BellData" || reel1Result.name == "BarData")
            {
                freeSpins += 5;
                UpdateFreeSpinsUI();
                Debug.Log("<color=yellow>BONUS! 5 Free Spins Awarded!</color>");
            }
        }
        else
        {
            Debug.Log("<color=red>No match.</color>");
        }

        UpdateGoldUI();
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = playerGold.ToString();
        }
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void UpdateFreeSpinsUI()
{
    if (freeSpinsText != null)
    {
        if (freeSpins > 0)
        {
            freeSpinsText.text = "Free Spins: " + freeSpins;
            freeSpinsText.gameObject.SetActive(true); // Show text
        }
        else
        {
            freeSpinsText.gameObject.SetActive(false); // Hide text
        }
    }
}
}