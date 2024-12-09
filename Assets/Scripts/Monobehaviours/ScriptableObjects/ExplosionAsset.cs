using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionAsset", menuName = "ExplosionAsset", order = 10)]
public class ExplosionAsset : ScriptableObject {
    
    public ParticleBurst explosionVFX;
    public Fire fireVFX;
    public Decal[] blastDecals;
    
    public bool hasDecals => blastDecals.Length > 0;
    
    public Decal GetDecal() => blastDecals.Sample();
}