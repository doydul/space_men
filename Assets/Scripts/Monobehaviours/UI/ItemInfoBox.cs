using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoBox : MonoBehaviour {
    
    public TMP_Text statsElement;
    public TMP_Text descElement;
    public Image image;
    public Sprite armourSprite;
    public Sprite weaponSprite;
    
    Weapon weapon;
    Armour armour;
    
    public void SetItem(Weapon weapon) {
        image.sprite = weaponSprite;
        this.weapon = weapon;
        string type = "basic";
        if (weapon.isHeavy && weapon.damageType == DamageType.Energy) {
            type = "heavy energy";
        } else if (weapon.isHeavy) {
            type = "heavy";
        } else if (weapon.damageType == DamageType.Energy) {
            type = "energy";
        }
        string lvl = "0";
        if (weapon.techLevel > 0) lvl = new string('I', weapon.techLevel);
        statsElement.text = $"<allcaps><line-height=0.85em><align=center>{weapon.name} <size=80%> lvl {lvl}\n" +
                            $"<align=left>dpt {weapon.dpt}<line-height=0>\n" +
                            $"<align=right>type {type}";
        descElement.text = $"<allcaps>{weapon.description}";
    }
    
    public void SetItem(Armour armour) {
        image.sprite = armourSprite;
        this.armour = armour;
        string type = "LIGHT";
        if (armour.isMedium) type = "medium";
        else if (armour.isHeavy) type = "heavy";
        string lvl = "0";
        if (armour.techLevel > 0) lvl = new string('I', armour.techLevel);
        statsElement.text = $"<allcaps><line-height=0.85em><align=center>{armour.name} <size=80%> tlvl {lvl}\n" +
                            $"<align=left>hitpoints {armour.maxHealth}<line-height=0>\n" +
                            $"<align=center>movement {armour.movement + armour.sprint}\n" +
                            $"<align=right>type {type}";
        descElement.text = $"<allcaps>{armour.description}";
    }
}