public class GameActions {

    static IActionFactory factory;

    public static void SetFactory(IActionFactory factory) {
        GameActions.factory = factory;
    }

    public static DelayedAction Shoot(Soldier soldier, Alien alien) {
        return factory.SoldierShootAction().Perform(soldier, alien);
    }

    //

    public interface IActionFactory {
        ISoldierShootAction SoldierShootAction();
    }

    public interface ISoldierShootAction {
        DelayedAction Perform(Soldier soldier, Alien alien);
    }
}
