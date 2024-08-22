using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Campaign {

    public static void NextLevel(PlayerSave save) {
        save.levelNumber++;
        save.difficulty += 1f / 8f;
        MungeMapGenerationValues(save);
        Debug.Log($"difficulty: {save.difficulty}");
        Mission.Generate();
        PlayerSave.current.Save(0);
    }
    
    public static void MungeMapGenerationValues(PlayerSave save) {
        if (save.levelNumber % 4 == 0 || save.enemyGenerationValues.Sum() <= 0) {
            // randomize enemy generation values
            var randList = new List<List<float>> {
                new List<float> { 0, 0, 10, 20 },
                new List<float> { 0, 0, 0, 25 },
                new List<float> { 0, 0, 15, 15 },
            }.Sample().Sample(4);
            save.enemyGenerationVelocities = randList;
        }
        for (int i = 0; i < save.enemyGenerationValues.Count; i++) {
            save.enemyGenerationValues[i] += save.enemyGenerationVelocities[i];
        }
        float sum = save.enemyGenerationValues.Sum();
        if (sum > 100) {
            float reduct = 100f / sum;
            for (int i = 0; i < save.enemyGenerationValues.Count; i++) {
                save.enemyGenerationValues[i] *= reduct;
            }
        }
        Debug.Log("New map generation values:");
        Debug.Log($"groupishness: {save.groupishness}");
        Debug.Log($"armouredness: {save.armouredness}");
        Debug.Log($"quickness: {save.quickness}");
        Debug.Log($"bigness: {save.bigness}");
    }
}