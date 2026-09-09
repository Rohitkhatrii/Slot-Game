using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelController : MonoBehaviour
{
    [Header("References")]
    public RectTransform symbolPrefab;
    public SlotSymbol[] symbolData;

    [Header("Settings")]
    public float scrollSpeed = 2000f;
    public float symbolHeight = 300f; // The vertical spacing between symbols
    public int totalSymbols = 6;      // How many symbols to stack (covers window + buffer)

    private List<RectTransform> activeSymbols = new List<RectTransform>();
    private bool isSpinning = false;

    void Start()
    {
        InitializeReel();
    }

    void InitializeReel()
    {
        for (int i = 0; i < totalSymbols; i++)
        {
            // Spawn the prefab as a child of this Reel Window
            RectTransform newSymbol = Instantiate(symbolPrefab, transform);

            // Stack them vertically (0, 300, 600, etc.)
            newSymbol.anchoredPosition = new Vector2(0, (i - 1) * symbolHeight);
            // Assign a random symbol from your data
            AssignRandomSymbol(newSymbol);

            activeSymbols.Add(newSymbol);
        }
    }

    void Update()
    {
        if (isSpinning)
        {
            SpinReel();
        }
    }

    public void StartSpinning()
    {
        isSpinning = true;
    }

    public void StopSpinning()
    {
        isSpinning = false;
        SnapSymbols();
    }

    private void SnapSymbols()
    {
        foreach (RectTransform symbol in activeSymbols)
        {
            // Round the Y position to the nearest multiple of your symbolHeight
            float snappedY = Mathf.Round(symbol.anchoredPosition.y / symbolHeight) * symbolHeight;
            symbol.anchoredPosition = new Vector2(symbol.anchoredPosition.x, snappedY);
        }
    }

    void SpinReel()
    {
        foreach (RectTransform symbol in activeSymbols)
        {
            // Move symbol down
            symbol.anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;

            // Allow the symbol to drop completely below the extended view area
            if (symbol.anchoredPosition.y <= -(symbolHeight * 1.5f))
            {
                // Teleport it to the very top of the stack
                symbol.anchoredPosition = new Vector2(0, symbol.anchoredPosition.y + (totalSymbols * symbolHeight));

                // Swap the picture so the pattern looks random
                AssignRandomSymbol(symbol);
            }
        }
    }

    void AssignRandomSymbol(RectTransform symbol)
    {
        Image symbolImage = symbol.GetComponent<Image>();
        SlotSymbol randomData = symbolData[Random.Range(0, symbolData.Length)];
        symbolImage.sprite = randomData.symbolSprite;
    }

    public SlotSymbol GetCenterSymbol()
    {
        RectTransform closestSymbol = null;
        float minDistance = float.MaxValue;

        // Find which symbol is closest to the middle of the reel window (Y = 0)
        foreach (RectTransform symbol in activeSymbols)
        {
            float distance = Mathf.Abs(symbol.anchoredPosition.y);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSymbol = symbol;
            }
        }

        if (closestSymbol != null)
        {
            // Read the sprite from the image and match it back to our ScriptableObject data
            Image img = closestSymbol.GetComponent<Image>();
            foreach (SlotSymbol data in symbolData)
            {
                if (data.symbolSprite == img.sprite)
                {
                    return data;
                }
            }
        }
        return null;
    }
}