using UnityEngine;
public class Tester : MonoBehaviour {

    public Soldier sol;
    public Tile tile;
    public AnimationInteractor inter;

    void Start() {
        new WorldAnimator(inter).PlayStandardShootAnimation(sol, tile, ShootingAnimationType.Missed);
    }
}
