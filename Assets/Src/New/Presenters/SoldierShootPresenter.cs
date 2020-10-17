using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data;

public class SoldierShootPresenter : Presenter, IPresenter<SoldierShootOutput> {
  
    public static SoldierShootPresenter instance { get; private set; }

    public Map map;
    public MapController mapInput;
    public AllControllers controllers;
    public StandardAnimations animations;
    
    void Awake() {
        instance = this;
    }
    
    public void Present(SoldierShootOutput input) {
        var soldier = map.GetActorByIndex(input.soldierIndex) as Soldier;
        var soldierUI = soldier.GetComponent<SoldierUIController>();
        soldierUI.SetAmmoCount(input.shotsLeft);
        soldier.ammo = input.ammoLeft;
        controllers.DisableAll();
        if (input.blastCoverage == null) {
            animations.NormalShootAnimation(input.soldierIndex, input.damageInstances, () => {
                Cleanup(input);
            });
        } else {
            animations.ExplosiveShootAnimation(input.soldierIndex, input.blastCoverage, input.damageInstances, () => {
                Cleanup(input);
            });
        }
    }

    void Cleanup(SoldierShootOutput input) {
        controllers.EnableAll();
        mapInput.DisplayActions(input.soldierIndex);
    }
}

