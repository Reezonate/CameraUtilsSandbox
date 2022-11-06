Shader "Ree/CompositionShader_3Cams"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("MaskTex", 2D) = "white" {}
        
        _Cam0Tex ("Cam0Tex", 2D) = "white" {}
        _Cam1Tex ("Cam1Tex", 2D) = "white" {}
        _Cam2Tex ("Cam2Tex", 2D) = "white" {}
        
        _Cam0Rect ("Cam0Rect", Vector) = (0, 0, 1, 1)
        _Cam1Rect ("Cam1Rect", Vector) = (0, 0, 1, 1)
        _Cam2Rect ("Cam2Rect", Vector) = (0, 0, 1, 1)
    }
    
    SubShader
    {
        Tags {
            "PreviewType"="Plane"
        }
        
        ZTest Off
        ZWrite Off
        Blend One Zero

        Pass
        {
            CGPROGRAM
            #pragma vertex vertex_shader
            #pragma fragment fragment_shader_3_cams

            #include "UnityCG.cginc"
            #include "CameraComposer.cginc"
            ENDCG
        }
    }
}
