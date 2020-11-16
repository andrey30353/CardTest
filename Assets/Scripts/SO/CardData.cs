using UnityEngine;

public enum CardParameter
{
    Mana,
    Attack,
    Health
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string Name;
    public string Description;   

    public int Mana;
    public int Attack;
    public int Health;
}
