using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Border : MonoBehaviour {
    
    public MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    LineRenderer line;
    
    void Awake() {
        meshRenderer = meshFilter.GetComponent<MeshRenderer>();
        line = GetComponent<LineRenderer>();
    }
    
    public void SetPoints(IEnumerable<Vector3> points) {
        var ary = points.ToArray();
        line.positionCount = ary.Length;
        line.SetPositions(ary);
        Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
        poly.outside = points.ToList();
        meshFilter.mesh = Poly2Mesh.CreateMesh(poly);
        meshFilter.transform.position = new Vector3(0, 0, meshFilter.transform.position.z);
    }
    
    public void SetColor(Color color) {
        line.startColor = color;
        line.endColor = color;
        var meshColor = color;
        meshColor.a = 0.3f;
        meshRenderer.material.SetColor("_BaseColor", meshColor);
    }
    
    public void Remove() {
        Destroy(gameObject);
    }
}