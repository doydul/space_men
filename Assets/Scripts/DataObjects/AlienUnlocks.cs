using System.Collections.Generic;

[System.Serializable]
public class AlienUnlocks {

    public List<string> unlockedProfileNames = new();

    public bool Unlocked(string profileName) => unlockedProfileNames.Contains(profileName);
    public void Unlock(string profileName) {
        if (!Unlocked(profileName)) unlockedProfileNames.Add(profileName);
    }
}