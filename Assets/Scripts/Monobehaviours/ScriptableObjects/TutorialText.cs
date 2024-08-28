using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "Tutorials", menuName = "Tutorial Text", order = 1)]
public class TutorialText : ScriptableObject {
    
    [System.Serializable]
    public class KeyVal
    {
        public string key;
        [TextArea] public string value;
    }

    public List<KeyVal> tutorials;
    
    public string Get(string key) {
        return tutorials.First(kv => kv.key == key).value;
    }
}
