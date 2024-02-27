using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class EnemyProfileSet {
    public List<EnemyProfile> profiles = new();

    public static EnemyProfileSet Generate(int difficulty) {
        var result = new EnemyProfileSet{ profiles = EnemyProfile.GetAll().Where(prof => prof.difficultyLevel == 1).Sample(5) };
        result.profiles.Add(EnemyProfile.none);
        return result;
    }
}