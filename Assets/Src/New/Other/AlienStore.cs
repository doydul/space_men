using System.Linq;
using System;
using UnityEngine;

using Data;

public class AlienStore : IAlienStore {
    
    public AlienStats GetAlienStats(string alienType) {
        var data = GetData(alienType);
        return new AlienStats {
            health = data.maxHealth,
            armour = data.armour,
            accModifier = data.accModifier,
            damage = data.damage,
            armourPen = data.armourPen,
            movement = data.movement,
            radarBlipChance = data.chanceOfCreatingRadarBlip,
            maxHealth = data.maxHealth,
            expReward = data.expReward,
            name = alienType
        };
    }
    
    AlienData GetData(string alienName) {
        return Resources.Load<AlienData>("Aliens/" + alienName);
    }
}
