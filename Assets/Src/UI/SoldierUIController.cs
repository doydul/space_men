using UnityEngine;
using System;

[RequireComponent(typeof(Soldier))]
public class SoldierUIController : MonoBehaviour {
    
    public SpriteRenderer moveIndicator;
    public GameObject ammoIndicator;
    public SpriteRenderer healthIndicator;
    
    private Soldier soldier;
    private bool movementIndicators;
    private float healthSpriteSize;
    
    void Awake() {
        soldier = GetComponent<Soldier>();
        healthSpriteSize = healthIndicator.size.x;
                
        moveIndicator.enabled = false;
        ammoIndicator.SetActive(false);
        healthIndicator.enabled = false;
    }
    
    void Update() {
        SetMovementIndicators();
        SetAmmoIndicator();
        SetHealthIndicatorSize();
    }
    
    private void SetMovementIndicators() {
        if (movementIndicators) {
            if (soldier.tilesMoved > 0) {
                moveIndicator.enabled = true;
                moveIndicator.color = Color.yellow;
            }
            if (soldier.tilesMoved > soldier.baseMovement) {
                moveIndicator.color = Color.red;
            }
        } else {
            moveIndicator.enabled = false;
        }
    }
    
    private void SetAmmoIndicator() {
        if (!movementIndicators) {
            ammoIndicator.SetActive(true);
            var ammoTransform = ammoIndicator.transform;
            for (int i = 0; i < ammoTransform.childCount; i++) {
                ammoTransform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < soldier.shotsRemaining; i++) {
                ammoTransform.GetChild(i).gameObject.SetActive(true);
            }
        } else {
            ammoIndicator.SetActive(false);
        }
    }
    
    private void SetHealthIndicatorSize() {
        var currentSize = healthIndicator.size;
        currentSize.x = soldier.health * healthSpriteSize / soldier.maxHealth;
        healthIndicator.size = currentSize;
    }
    
    public void ShowMovementIndicators() {
        movementIndicators = true;
    }
    
    public void ShowAmmoIndicators() {
        movementIndicators = false;
    }
    
    public void Select() {
        healthIndicator.enabled = true;
    }
    
    public void Deselect() {
        healthIndicator.enabled = false;
    }
}