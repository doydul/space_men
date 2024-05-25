using System.Collections.Generic;

[System.Serializable]
public class AlienUnlocks {

    public List<string> unlockedProfileNames = new();

    public void Unlock(string profileName) => unlockedProfileNames.Add(profileName);
    public bool Unlocked(string profileName) => unlockedProfileNames.Contains(profileName);
}