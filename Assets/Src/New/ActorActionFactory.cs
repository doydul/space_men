public class ActorActionFactory : GameActions.IActionFactory {

    public ActorActionFactory(IWorld world, IAnimationReel animationReel) {
        this.world = world;
        this.animationReel = animationReel;
    }

    IWorld world;
    IAnimationReel animationReel;

    public GameActions.ISoldierShootAction SoldierShootAction() {
        var result = new ShootAction(world, new Exploder(world));
        result.SetAnimationReel(animationReel);
        return result;
    }
}
