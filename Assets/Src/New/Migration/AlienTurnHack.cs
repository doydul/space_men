using UnityEngine;

public static class AlienTurnHack {

    public static void SpawnAliens() {
        AlienDeployer.instance.Iterate();
    }

    public static void MoveAliens() {
        AlienMovementPhaseDirector.instance.MoveAliens();
    }
}
