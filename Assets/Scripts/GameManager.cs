using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 1. The State Machine enum
    public enum GameState { Idle, Spinning, Evaluating }
    
    [Header("State")]
    public GameState currentState = GameState.Idle;

    [Header("References")]
    public ReelController[] reels;

    void Update()
    {
        // Only allow spinning if the game is currently idle
        if (Input.GetKeyDown(KeyCode.Space) && currentState == GameState.Idle)
        {
            StartCoroutine(RoutineSpin());
        }
    }

    // A Coroutine lets us manage time (like staggering the reel spins later)
    private System.Collections.IEnumerator RoutineSpin()
    {
        currentState = GameState.Spinning;

        // Tell all 3 reels to start moving
        foreach (ReelController reel in reels)
        {
            reel.StartSpinning();
        }

        // Let them spin at full speed for 2 seconds
        yield return new WaitForSeconds(2f); 

        // Stop them one by one
        foreach (ReelController reel in reels)
        {
            reel.StopSpinning();
            
            // Wait half a second before stopping the next reel
            yield return new WaitForSeconds(0.5f); 
        }

        // Move to the next state so we can calculate payouts later
        currentState = GameState.Evaluating;
    }
}