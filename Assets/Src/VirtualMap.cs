using UnityEngine;
using System.Collections.Generic;

public class VirtualMap {
    
    List<VirtualSpawner> virtualSpawners;
    
    public List<VirtualAlien> virtualAliens { get; private set; }
    
    public VirtualMap() {
        virtualSpawners = new List<VirtualSpawner>();
        virtualAliens = new List<VirtualAlien>();
    }
    
    public void AddVirtualSpawner(VirtualSpawner spawner) {
        virtualSpawners.Add(spawner);
    }
    
    public void Populate(List<Vector2> targets) {
        foreach (var virtualSpawner in new List<VirtualSpawner>(virtualSpawners)) {
            virtualAliens.AddRange(virtualSpawner.GetVirtualAliens(targets));
            if (virtualSpawner.finished) virtualSpawners.Remove(virtualSpawner);
        }
    }
    
    public void Depopulate() {
        virtualAliens.Clear();
    }
}