using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InvertedMaskImage : Image {
    
    Material _materialForRendering;
    public override Material materialForRendering {
        get {
            if (_materialForRendering != null) return _materialForRendering;
            Material result = Instantiate(base.materialForRendering);
            result.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            _materialForRendering = result;
            return _materialForRendering;
        }
    }
}