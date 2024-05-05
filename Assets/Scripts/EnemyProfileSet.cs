using System;
using System.Linq;
using System.Collections.Generic;

[Serializable]
public class EnemyProfileSet {
    public int groupishness;
    public int armouredness;
    public int quickness;
    public int bigness;
    public int difficulty = 1;
    public List<EnemyProfile> primaries = new();
    public List<EnemyProfile> secondaries = new();

    public List<EnemyProfile> profiles = new();

    public static EnemyProfileSet Generate(int difficulty) {
        var set = new EnemyProfileSet {
            groupishness = 0,
            armouredness = 0,
            quickness = 0,
            bigness = 0
        };
        var potentialPrimaries = EnemyProfile.GetAll().Where(prof => prof.Fits(set)).ToList();
        potentialPrimaries.Sort((prof1, prof2) => prof1.BestScore(set) > prof2.BestScore(set) ? 1 : -1 );
        set.primaries = potentialPrimaries.Take(2).ToList();
        var potentialSecondaries = EnemyProfile.GetAll().Where(prof => prof.unlocked && !set.primaries.Contains(prof));
        set.secondaries = potentialSecondaries.Sample(2);
        return set;
    }
}