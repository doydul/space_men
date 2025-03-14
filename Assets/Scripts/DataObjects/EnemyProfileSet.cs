using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyProfileSet : ISerializationCallbackReceiver {
    public int groupishness;
    public int armouredness;
    public int quickness;
    public int bigness;
    public int difficulty = 1;
    public List<EnemyProfile> primaries = new();
    public List<EnemyProfile> secondaries = new();
    public List<string> primaryNames;
    public List<string> secondaryNames;
    
    public void OnBeforeSerialize() {
        primaryNames = new();
        secondaryNames = new();
        foreach (var profile in primaries) primaryNames.Add(profile.name);
        foreach (var profile in secondaries) secondaryNames.Add(profile.name);
    }
    
    public void OnAfterDeserialize() {
        primaries = new();
        secondaries = new();
        foreach (var name in primaryNames) primaries.Add(EnemyProfile.GetAll().First(prof => prof.name == name));
        foreach (var name in secondaryNames) secondaries.Add(EnemyProfile.GetAll().First(prof => prof.name == name));
    }

    public static EnemyProfileSet Generate(PlayerSave save) {
        float difficulty = save.difficulty;
        var set = new EnemyProfileSet {
            groupishness = save.groupishness,
            armouredness = save.armouredness,
            quickness = save.quickness,
            bigness = save.bigness
        };
        int baseLevel = (int)difficulty;
        float nextLevelRatio = difficulty - baseLevel;

        if (nextLevelRatio >= 7f / 8f) {
            Pattern8(baseLevel, set, save);
        } else if (nextLevelRatio >= 6f / 8f) {
            Pattern7(baseLevel, set, save);
        } else if (nextLevelRatio >= 5f / 8f) {
            Pattern6(baseLevel, set, save);
        } else if (nextLevelRatio >= 4f / 8f) {
            Pattern5(baseLevel, set, save);
        } else if (nextLevelRatio >= 3f / 8f) {
            Pattern4(baseLevel, set, save);
        } else if (nextLevelRatio >= 2f / 8f) {
            Pattern3(baseLevel, set, save);
        } else if (nextLevelRatio >= 1f / 8f) {
            Pattern2(baseLevel, set, save);
        } else {
            Pattern1(baseLevel, set, save);
        }
        return set;
    }

    static IEnumerable<EnemyProfile> PrimariesFor(int difficultyLevel, EnemyProfileSet set) {
        var primaries = EnemyProfile.GetAll().Where(prof => prof.difficultyLevel == difficultyLevel).Where(prof => prof.Fits(set)).ToList();
        primaries.Sort((prof1, prof2) => prof1.BestScore(set) + UnityEngine.Random.value < prof2.BestScore(set) + UnityEngine.Random.value ? 1 : -1 );
        return primaries;
    }

    static IEnumerable<EnemyProfile> SecondariesFor(int difficultyLevel, EnemyProfileSet set, PlayerSave playerSave) {
        var secondaries = EnemyProfile.GetAll().Where(prof => prof.difficultyLevel == difficultyLevel).Where(prof => (prof.Fits(set) || playerSave.alienUnlocks.Unlocked(prof.name)) && !set.primaries.Contains(prof));
        return secondaries;
    }

    // contains only enemies of the current difficulty level
    static void Pattern1(int baseLevel, EnemyProfileSet set, PlayerSave playerSave) {
        set.primaries = PrimariesFor(baseLevel, set).Take(2).ToList();
        set.secondaries = SecondariesFor(baseLevel, set, playerSave).Sample(2);
    }

    // 1 of the secondaries is upgraded to the next difficulty level
    static void Pattern2(int baseLevel, EnemyProfileSet set, PlayerSave playerSave) {
        set.primaries = PrimariesFor(baseLevel, set).Take(2).ToList();
        set.secondaries = SecondariesFor(baseLevel, set, playerSave).Sample(1);
        set.secondaries.AddRange(SecondariesFor(baseLevel + 1, set, playerSave).Sample(1));
    }

    // both of the secondaries are upgraded to the next difficulty level
    static void Pattern3(int baseLevel, EnemyProfileSet set, PlayerSave playerSave) {
        set.primaries = PrimariesFor(baseLevel, set).Take(2).ToList();
        set.secondaries = SecondariesFor(baseLevel + 1, set, playerSave).Sample(2);
    }

    // 1 of the primaries is upgraded to the next difficulty level
    static void Pattern4(int baseLevel, EnemyProfileSet set, PlayerSave playerSave) {
        set.primaries = PrimariesFor(baseLevel, set).Take(1).ToList();
        set.primaries.Add(PrimariesFor(baseLevel + 1, set).First());
        set.secondaries = SecondariesFor(baseLevel, set, playerSave).Sample(2);
    }

    // 1 primary and 1 secondary are upgraded to the next difficulty level
    static void Pattern5(int baseLevel, EnemyProfileSet set, PlayerSave playerSave) {
        set.primaries = PrimariesFor(baseLevel, set).Take(1).ToList();
        set.primaries.Add(PrimariesFor(baseLevel + 1, set).First());
        set.secondaries = SecondariesFor(baseLevel, set, playerSave).Sample(1);
        set.secondaries.AddRange(SecondariesFor(baseLevel + 1, set, playerSave).Sample(1));
    }

    // 1 primary and both secondaries are upgraded to the next difficulty level
    static void Pattern6(int baseLevel, EnemyProfileSet set, PlayerSave playerSave) {
        set.primaries = PrimariesFor(baseLevel, set).Take(1).ToList();
        set.primaries.Add(PrimariesFor(baseLevel + 1, set).First());
        set.secondaries = SecondariesFor(baseLevel + 1, set, playerSave).Sample(2);
    }

    // both primaries are upgraded to the next difficulty level
    static void Pattern7(int baseLevel, EnemyProfileSet set, PlayerSave playerSave) {
        set.primaries = PrimariesFor(baseLevel + 1, set).Take(2).ToList();
        set.secondaries = SecondariesFor(baseLevel, set, playerSave).Sample(2);
    }

    // both primaries and 1 secondary are upgraded to the next difficulty level
    static void Pattern8(int baseLevel, EnemyProfileSet set, PlayerSave playerSave) {
        set.primaries = PrimariesFor(baseLevel + 1, set).Take(2).ToList();
        set.secondaries = SecondariesFor(baseLevel, set, playerSave).Sample(1);
        set.secondaries.AddRange(SecondariesFor(baseLevel + 1, set, playerSave).Sample(1));
    }
}