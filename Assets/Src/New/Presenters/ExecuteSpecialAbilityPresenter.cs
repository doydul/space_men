using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data;
using Workers;

public class ExecuteSpecialAbilityPresenter : Presenter, IPresenter<ExecuteSpecialAbilityOutput> {
  
    public static ExecuteSpecialAbilityPresenter instance { get; private set; }

    public Map map;
    public MapController mapInput;
    public AllControllers controllers;
    public StandardAnimations animations;
    public Scripting scripting;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(ExecuteSpecialAbilityOutput input) {
        StartCoroutine(DoPresent(input));
    }

    IEnumerator DoPresent(ExecuteSpecialAbilityOutput input) {
        var output = input.output;
        Debug.Log("Executing special ability: " + output.GetType());
        controllers.DisableAll();
        if (output is FireAtGround.Output) {
            yield return FireAtGroundPresenter.instance.Present((FireAtGround.Output)output);
        } else if (output is CollectAmmo.Output) {
            yield return CollectAmmoPresenter.instance.Present((CollectAmmo.Output)output);
        } else if (output is Grenade.Output) {
            yield return GrenadePresenter.instance.Present((Grenade.Output)output);
        } else if (output is StunShot.Output) {
            yield return StunShotPresenter.instance.Present((StunShot.Output)output);
        }
        Cleanup(input.soldierId);
    }

    void Cleanup(long soldierIndex) {
        controllers.EnableAll();
        mapInput.DisplayActions(soldierIndex);
    }
}

