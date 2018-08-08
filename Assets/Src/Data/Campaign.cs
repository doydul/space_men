using System.Collections.Generic;
using UnityEngine;

public class Campaign : MonoBehaviour {
    
    public const string DEFAULT = "Default";
    
    public Mission[] missions {
        get {
            return GetComponents<Mission>();
        }
    }
    
    public static Campaign FromString(string campaignName) {
        return Resources.Load<Campaign>("Campaigns/" + campaignName);
    }
}