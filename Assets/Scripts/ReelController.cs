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
    public float symbolHeight = 85f; // The vertical spacing between symbols
    public int totalSymbols = 6;      // How many symbols to stack (covers window + buffer)

    private List<RectTransform> activeSymbols = new List<RectTransform>();       // using RectTransform because entire slot machine movement relies on the anchoredPosition property so thats why using RectTransform 
    private bool isSpinning = false;

    void Start()
    {
        InitializeReel();
    }

    void InitializeReel()        //6 times symbolPrefab is Instantiated on different y position but Rect Mask component will only show until we have stretched the Image Gameobject
    {
        for (int i = 0; i < totalSymbols; i++)       //loops the code inside it exactly 6 times
        {
            // Spawn the prefab as a child of this Reel Window
            RectTransform newSymbol = Instantiate(symbolPrefab, transform);      // the Instantiated images will have transform set as parent 

            // Stack them vertically (0, 300, 600, etc.)
            // anchoredPosition is a property of RectTransform Class when we set its values here they will adjust in Inspector for position x and y 
            // when i=0 then answer (0,-85)  . When i=1 then answer (0,0) . When i=2 then answer (0,85) .When i=3 then answer (0,170) .When i=4 then answer (0,255). When i=5 then answer (0,340)
            newSymbol.anchoredPosition = new Vector2(0, (i - 1) * symbolHeight);             
            // Assign a random symbol from your data
            AssignRandomSymbol(newSymbol);

            activeSymbols.Add(newSymbol);  // 6 symbols have been added to the list activeSymbols
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

    private void SnapSymbols()    //When StopSpinning method stops, means when reels are stopped so the icon stop as our alignment thats why we made this method
    {
        foreach (RectTransform symbol in activeSymbols)
        {
            // Round the Y position to the nearest multiple of your symbolHeight
            // Round off (symbol.anchoredPosition.y / symbolHeight) then multiply by symbolHeight
            float snappedY = Mathf.Round(symbol.anchoredPosition.y / symbolHeight) * symbolHeight; 
            symbol.anchoredPosition = new Vector2(symbol.anchoredPosition.x, snappedY);
        }
    }

    void SpinReel()
    {
        foreach (RectTransform symbol in activeSymbols)
        {
            // Move symbol down
            //continously moving downwards because Vector2.down is basically [0,-1]. Vector2.down is making the calculation '-' hence resulting in downward movement
            symbol.anchoredPosition += Vector2.down * scrollSpeed * Time.deltaTime;              

            // Allow the symbol to drop completely below the extended view area
            if (symbol.anchoredPosition.y <= -(symbolHeight * 2f))         // if  y <= -170
            {
                // Teleport it to the very top of the stack
                symbol.anchoredPosition = new Vector2(0, symbol.anchoredPosition.y + (totalSymbols * symbolHeight));

                // Swap the picture so the pattern looks random
                AssignRandomSymbol(symbol);
            }
        }
    }

    void AssignRandomSymbol(RectTransform symbol)                   //RectTransform tells Unity to only accept a UI object
    {
        Image symbolImage = symbol.GetComponent<Image>();
        SlotSymbol randomData = symbolData[Random.Range(0, symbolData.Length)];       // suppose random range gives 1 then symbolData[1]; // so that data is stored in randomData
        symbolImage.sprite = randomData.symbolSprite;      // by randomData.symbolSprite we got image 
    }

    public SlotSymbol GetCenterSymbol()        // this method is bascially to get closest symbol to y=0  // SlotSymbol will return a SlotSymbol data.
    {
        RectTransform closestSymbol = null;       
        float minDistance = float.MaxValue;   // float.MaxValue is 3.4028235E+38F      

        // Find which symbol is closest to the middle of the reel window (Y = 0)
        foreach (RectTransform symbol in activeSymbols)    // Our List activeSymbols has RectTransform type assigned at the top so here too we will write same type 
        {
            float distance = Mathf.Abs(symbol.anchoredPosition.y);   // suppose y=85 
            if (distance < minDistance)                              // 85 < 3.4028235E+38F . condition is True 
            {
                minDistance = distance;                              // now minDistance = 85
                closestSymbol = symbol;                              // closestSymbol = current iteration symbol
            }                                                        // This loop will run for our all iterations and find out which symbol is closest to y=0
        }

        if (closestSymbol != null)
        {
            // Read the sprite from the image and match it back to our ScriptableObject data
            Image img = closestSymbol.GetComponent<Image>();
            foreach (SlotSymbol data in symbolData)       // type SlotSymbol because symbolData variable has same type SlotSymbol at the top
            {
                if (data.symbolSprite == img.sprite)
                {
                    return data;               // get full data like symbolName,symbolSprite,payoutMultiplier
                }
            }
        }
        return null;
    }
}