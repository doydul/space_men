using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Campaign", order = 1)]
public class Campaign : ScriptableObject {
    
    public const string DEFAULT = "Default";

    public string[] missionNames;
    
    public static Campaign FromString(string campaignName) {
        return Resources.Load<Campaign>("Campaigns/" + campaignName);
    }
}