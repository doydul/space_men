using UnityEngine;

public class WeaponSprite : MonoBehaviour {
    
    public Transform muzzle;
    public RuntimeAnimatorController animatorController;
    
    public Vector3 muzzlePosition => muzzle.position;
}