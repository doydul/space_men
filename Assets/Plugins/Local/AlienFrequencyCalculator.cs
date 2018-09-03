using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class AlienFrequencyCalculator {

    private float spawnPool;
    private float totalThreat;
    private float totalFrequency;

    private List<AlienFrequencyInput> profiles;

    public AlienFrequencyCalculator(List<AlienFrequencyInput> input) {
        profiles = input;
        totalThreat = TotalThreat();
        totalFrequency = TotalFrequency();
    }

    public List<AlienFrequencyOutput> Iterate() {
        spawnPool += totalThreat;

        var mean = totalFrequency * spawnPool / totalThreat;
        var gaus = new Gaussian(mean, totalThreat); //

        var spawnCount = gaus.value;
        var result = profiles.Select(profile => new AlienFrequencyOutput() {alienType = profile.alienType}).ToList();
        for (int i = 1; i < spawnCount; i++) {
            var profile = WeightedSelect();
            result.First(output => output.alienType == profile.alienType).spawnCount += 1;
            spawnPool -= profile.threat;
        }
        return result;
    }

    private float TotalThreat() {
        float result = 0;
        foreach (var profile in profiles) {
            result += profile.threat * profile.frequency;
        }
        return result;
    }

    private float TotalFrequency() {
        float result = 0;
        foreach (var profile in profiles) {
            result += profile.frequency;
        }
        return result;
    }

    private AlienFrequencyInput WeightedSelect() {
        var remainingFrequency = totalFrequency * Random.value;
        foreach (var profile in profiles) {
            remainingFrequency -= profile.frequency;
            if (remainingFrequency <= 0) {
                return profile;
            }
        }
        return profiles[profiles.Count - 1];
    }
}
