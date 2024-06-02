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

    public static EnemyProfileSet Generate(float difficulty) {
        var set = new EnemyProfileSet {
            groupishness = 0,
            armouredness = 0,
            quickness = 0,
            bigness = 0
        };
        int baseLevel = (int)difficulty;
        float nextLevelRatio = difficulty - baseLevel;

        if (nextLevelRatio > 0.83f) {
            Pattern6(baseLevel, set);
        } else if (nextLevelRatio > 0.66f) {
            Pattern5(baseLevel, set);
        } else if (nextLevelRatio > 0.5f) {
            Pattern4(baseLevel, set);
        } else if (nextLevelRatio > 0.33f) {
            Pattern3(baseLevel, set);
        } else if (nextLevelRatio > 0.16f) {
            Pattern2(baseLevel, set);
        } else {
            Pattern1(baseLevel, set);
        }
        return set;
    }

    static IEnumerable<EnemyProfile> PrimariesFor(int difficultyLevel, EnemyProfileSet set) {
        var primaries = EnemyProfile.GetAll().Where(prof => prof.difficultyLevel == difficultyLevel).Where(prof => prof.Fits(set)).ToList();
        primaries.Sort((prof1, prof2) => prof1.BestScore(set) < prof2.BestScore(set) ? 1 : -1 );
        return primaries;
    }

    static IEnumerable<EnemyProfile> SecondariesFor(int difficultyLevel, EnemyProfileSet set) {
        var secondaries = EnemyProfile.GetAll().Where(prof => prof.difficultyLevel == difficultyLevel).Where(prof => PlayerSave.current.alienUnlocks.Unlocked(prof.name) && !set.primaries.Contains(prof));
        return secondaries;
    }

    // contains only enemies of the current difficulty level
    static void Pattern1(int baseLevel, EnemyProfileSet set) {
        set.primaries = PrimariesFor(baseLevel, set).Take(2).ToList();
        set.secondaries = SecondariesFor(baseLevel, set).Sample(2);
    }

    // 1 of the secondaries is upgraded to the next difficulty level
    static void Pattern2(int baseLevel, EnemyProfileSet set) {
        set.primaries = PrimariesFor(baseLevel, set).Take(2).ToList();
        set.secondaries = SecondariesFor(baseLevel, set).Sample(1);
        set.secondaries.Add(SecondariesFor(baseLevel + 1, set).Sample());
    }

    // 1 of the primaries is upgraded to the next difficulty level
    static void Pattern3(int baseLevel, EnemyProfileSet set) {
        set.primaries = PrimariesFor(baseLevel, set).Take(1).ToList();
        set.primaries.Add(PrimariesFor(baseLevel + 1, set).First());
        set.secondaries = SecondariesFor(baseLevel, set).Sample(2);
    }

    // 1 primary and 1 secondary are upgraded to the next difficulty level
    static void Pattern4(int baseLevel, EnemyProfileSet set) {
        set.primaries = PrimariesFor(baseLevel, set).Take(1).ToList();
        set.primaries.Add(PrimariesFor(baseLevel + 1, set).First());
        set.secondaries = SecondariesFor(baseLevel, set).Sample(1);
        set.secondaries.Add(SecondariesFor(baseLevel + 1, set).Sample());
    }

    // both primaries are upgraded to the next difficulty level
    static void Pattern5(int baseLevel, EnemyProfileSet set) {
        set.primaries = PrimariesFor(baseLevel + 1, set).Take(2).ToList();
        set.secondaries = SecondariesFor(baseLevel, set).Sample(2);
    }

    // both primaries and 1 secondary are upgraded to the next difficulty level
    static void Pattern6(int baseLevel, EnemyProfileSet set) {
        set.primaries = PrimariesFor(baseLevel + 1, set).Take(2).ToList();
        set.secondaries = SecondariesFor(baseLevel, set).Sample(1);
        set.secondaries.Add(SecondariesFor(baseLevel + 1, set).Sample());
    }
}