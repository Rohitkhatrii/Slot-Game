using UnityEngine;

public class LeverController : MonoBehaviour
{
    public GameObject leverUp;
    public GameObject leverDown;

    void Start()
    {
        ResetUp();
    }

    public void PullDown()
    {
        if (leverUp != null) leverUp.SetActive(false);
        if (leverDown != null) leverDown.SetActive(true);
    }

    public void ResetUp()
    {
        if (leverUp != null) leverUp.SetActive(true);
        if (leverDown != null) leverDown.SetActive(false);
    }
}