using System.Linq;
using System;
using UnityEngine;

using Data;

public class AlienStore : IAlienStore {
    
    public AlienStats GetAlienStats(AlienType alienType) {
        var data = GetData(alienType.ToString());
        return new AlienStats {
            health = data.maxHealth,
            armour = data.armour,
            accModifier = data.accModifier,
            damage = data.damage,
            armourPen = data.armourPen,
            movement = data.movement
        };
    }
    
    AlienData GetData(string alienName) {
        return Resources.Load<AlienData>("Aliens/" + alienName);
    }
}
