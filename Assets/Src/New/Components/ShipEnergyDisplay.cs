using UnityEngine;
using System.Linq;

public class ShipEnergyDisplay : MonoBehaviour {

    public EnergyDisplayPip[] pips;

    int pipCount;
    int nextPipIndex;

    public bool atCapacity => nextPipIndex == pipCount;

    public void Init(int pipCount) {
        this.pipCount = pipCount;
        for (int i = 0; i < pips.Length - pipCount; i++) {
            pips[pips.Length - 1 - i].Disable();
        }
        Drain();
    }

    public void FillNextPip() {
        if (atCapacity) return;
        pips[nextPipIndex].Fill();
        nextPipIndex++;
    }

    public void SetLevel(int level) {
        Drain();
        for (int i = 0; i < level; i++) {
            FillNextPip();
        }
    }

    public void Drain() {
        foreach (var pip in pips) {
            pip.Drain();
        }
        nextPipIndex = 0;
    }
}