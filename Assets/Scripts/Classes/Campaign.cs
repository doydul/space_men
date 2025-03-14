using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Campaign {

    public static void NextLevel(PlayerSave save) {
        save.levelNumber++;
        save.IncreaseDifficulty(1f / 5f);
        MungeMapGenerationValues(save);
        Mission.Generate(save);
        save.Save(0);
    }
    
    public static void MungeMapGenerationValues(PlayerSave save) {
        if (save.levelNumber % 3 == 0 || save.enemyGenerationVelocities.Sum() <= 0) {
            // randomize enemy generation values
            var randList = new List<List<float>> {
                new List<float> { 0, 0, 10, 25 },
                new List<float> { 0, 0, 0, 30 },
                new List<float> { 0, 0, 20, 20 },
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
        Debug.Log("XXX New map generation values:");
        Debug.Log($"XXX difficulty: {save.difficulty}");
        Debug.Log($"XXX groupishness: {save.groupishness}");
        Debug.Log($"XXX armouredness: {save.armouredness}");
        Debug.Log($"XXX quickness: {save.quickness}");
        Debug.Log($"XXX bigness: {save.bigness}");
    }
}