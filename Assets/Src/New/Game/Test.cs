using UnityEngine;
using System.Collections.Generic;
using Workers;
using Data;
using System.Linq;

public class Test : MonoBehaviour {

    void Start() {
        var actor = new SoldierActor();
        actor.AddData(new GrenadeAmmo { ammoUsed = 1 });
        actor.AddData(new Description { desc = "Thing 1" });

        Debug.Log(actor.GetData<GrenadeAmmo>().ammoUsed);
        Debug.Log(actor.GetData<Description>().desc);

        actor.SetData(new GrenadeAmmo { ammoUsed = 0 });
        Debug.Log(actor.GetData<GrenadeAmmo>().ammoUsed);

        actor.AddData(new Description { desc = "Thing 2" });
        actor.AddData(new Description { desc = "Thing 3" });
        foreach (var obj in actor.GetAllData<Description>()) {
            Debug.Log(obj.desc);
        }
        actor.SetData(new Description { desc = "Thing 5" });
        foreach (var obj in actor.GetAllData<Description>()) {
            Debug.Log(obj.desc);
        }
    }

    public class GrenadeAmmo {
        public int ammoUsed;
    }

    public struct Description {
        public string desc;
    }
}
