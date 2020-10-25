namespace Workers {

    public class MetaSoldierDecorator {

        MetaSoldier soldier;

        public MetaSoldierDecorator(MetaSoldier soldier) {
            this.soldier = soldier;
        }

        public int level => UnityEngine.Mathf.FloorToInt(1.2f * UnityEngine.Mathf.Pow(0.2f * soldier.exp, 0.54f));
        public int remainingAbilityPoints => level - soldier.spentAbilityPoints;

        public void SpendAbilityPoint() {
            soldier.spentAbilityPoints++;
        }
    }
}