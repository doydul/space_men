public enum ShootingAnimationType {
    Hit,
    Missed
}

public interface IAnimationReel {

    DelayedAction PlayStandardShootAnimation(Soldier shooter, Tile target, ShootingAnimationType type);
    DelayedAction PlayOrdnanceShootAnimation(Soldier shooter, Exploder.ExploderOutput exploderOutput);
}
