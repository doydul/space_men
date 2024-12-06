using UnityEditor.Animations;
using UnityEngine;

public class WeaponSprite : MonoBehaviour {
    
    public Transform muzzle;
    public AnimatorController animatorController;
    
    public Vector3 muzzlePosition => muzzle.position;
}