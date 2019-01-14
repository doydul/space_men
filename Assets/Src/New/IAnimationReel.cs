public enum ShootingAnimationType {
    Hit,
    Missed,
    Deflected
}

public interface IAnimationReel {

    DelayedAction PlayStandardShootAnimation(Soldier shooter, Tile target, ShootingAnimationType type);
    DelayedAction PlayOrdnanceShootAnimation(Soldier shooter, Explosion explosion);

    DelayedAction PlayAlienAttackAnimation(Alien attacker);
}
