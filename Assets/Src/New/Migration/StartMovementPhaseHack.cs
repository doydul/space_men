public static class StartMovementPhaseHack {
    
    public static void StartMovementPhase() {
        foreach (var soldier in Map.instance.GetActors<Soldier>()) {
            soldier.StartMovementPhase();
        }
        RadarBlipController.instance.ShowRadarBlips();
    }
}
