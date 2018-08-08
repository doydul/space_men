using System;

public class ShootingPhase {
    
    public static event Action PhaseEnd;
    
    private const int TOTAL_ITERATIONS = 4;
    
    private int iterations;
    
    public bool phaseOver { get { return iterations >= TOTAL_ITERATIONS; } }
    
    public void Reset() {
        iterations = 0;
    }
    
    public void NextIteration() {
        iterations += 1;
    }
}