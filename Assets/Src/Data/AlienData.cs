using UnityEngine;

[CreateAssetMenu(fileName = "New Alien", menuName = "Scriptable Objects/Alien", order = 1)]
public class AlienData : ScriptableObject {

    public int maxHealth;
    public int armour;
    public int accModifier;
    public int damage;
    public int armourPen;
    public int movement;
    public float threat;
    public int expReward;
    public int chanceOfCreatingRadarBlip;
}
