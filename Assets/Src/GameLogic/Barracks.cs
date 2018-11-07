public class Barracks {

    private Squad squad;

    public bool canHire { get { return squad._credits >= HireCost(); } }

    public static int HireCost() {
        return 600;
    }

    public Barracks(Squad squad) {
        this.squad = squad;
    }

    public void HireSoldier() {
        squad._credits -= HireCost();
        squad._reserveSoldiers.Add(GenerateDefaultSoldier());
    }

    public static SoldierData GenerateDefaultSoldier() {
        var result = new SoldierData();
        result.armour = "Basic";
        result.weapon = "Assault Rifle";
        return result;
    }
}
