public class GameAction {

    static ShootAction shootAction;

    public static void SetShootAction(ShootAction shootAction) {
        GameAction.shootAction = shootAction;
    }

    public static void Shoot(Soldier soldier, Alien alien) {
        shootAction.Perform(soldier, alien);
    }
}
