using UnityEngine;

public class CampaignUITester : MonoBehaviour {
    
    void Start() {
        if (PlayerSave.current == null) {
            var save = new PlayerSave();
            PlayerSave.current = save;
            save.credits = 1234;

            var squad = MetaSquad.GenerateDefault();
            save.squad = squad;

            squad.AddMetaSoldier(new MetaSoldier() {
                name = "John Doe",
                armour = new InventoryItem() {
                    name = "Heavy",
                    type = InventoryItem.Type.Armour
                },
                weapon = new InventoryItem() {
                    name = "Flamer",
                    type = InventoryItem.Type.Weapon
                }
            });

            save.inventory.AddItem(new InventoryItem {
                name = "Assault Rifle",
                type = InventoryItem.Type.Weapon
            });
            save.inventory.AddItem(new InventoryItem {
                name = "Assault Rifle",
                type = InventoryItem.Type.Weapon
            });
            save.inventory.AddItem(new InventoryItem {
                name = "Medium",
                type = InventoryItem.Type.Armour
            });

            UnityEngine.SceneManagement.SceneManager.LoadScene("CampaignUI");
        }
    }
}