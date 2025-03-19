using UnityEngine;

[CreateAssetMenu(fileName = "Alien", menuName = "Alien", order = 1)]
public class AlienData : ScriptableObject {

    [TextArea] public string description;
    public Sprite sprite;
    public int maxHealth;
    public int armour;
    public int accModifier;
    public int damage;
    public int movement;
    public int displacementPriority;
    public int minSpawnDistance;
    public bool spawnsDuringWaveDefence;
    public AlienAudioProfile audio;
    public AlienBehaviour behaviour;
    public Trait[] traits;

    public void Dump(Alien target) {
        target.type = name;
        target.description = description;
        target.maxHealth = maxHealth;
        target.health = maxHealth;
        target.armour = armour;
        target.accModifier = accModifier;
        target.damage = damage;
        target.movement = movement;
        target.displacementPriority = displacementPriority;
        target.sensoryRange = 7;
        target.audio = audio;
        behaviour.Attach(target);
        target.traits = traits ?? new Trait[0];
    }
    
    public static AlienData Get(string name) {
        return Resources.Load<AlienData>("Aliens/" + name);
    }
}
