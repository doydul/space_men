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

            // for (int i = 0; i < 21; i++) {
            //     Campaign.NextLevel(PlayerSave.current);
            //     Debug.Log("----------------------------------------");
            //     Debug.Log($"Level {i}, difficulty: {PlayerSave.current.difficulty} ( groupishness: {PlayerSave.current.groupishness}, armouredness: {PlayerSave.current.armouredness}, quickness: {PlayerSave.current.quickness}, bigness: {PlayerSave.current.bigness} )");
            //     var set = EnemyProfileSet.Generate(PlayerSave.current);
            //     Debug.Log("Primaries:");
            //     foreach (var prof in set.primaries) {
            //         Debug.Log(prof.name);
            //     }
            //     Debug.Log("Secondaries:");
            //     foreach (var prof in set.secondaries) {
            //         if (prof != null) Debug.Log(prof.name);
            //     }
            // }

            UnityEngine.SceneManagement.SceneManager.LoadScene("CampaignUI");
        }
    }
}