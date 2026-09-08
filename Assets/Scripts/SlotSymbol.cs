using UnityEngine;

[CreateAssetMenu(fileName = "New Symbol", menuName = "SlotGame/Symbol")]
public class SlotSymbol : ScriptableObject
{
    public string symbolName;
    public Sprite symbolSprite;
    public int payoutMultiplier;
}