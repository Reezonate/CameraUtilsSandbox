Shader "Ree/CompositionShader_1Cam"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("MaskTex", 2D) = "white" {}
        _Cam0Tex ("Cam0Tex", 2D) = "white" {}
        _Cam0Rect ("Cam0Rect", Vector) = (0, 0, 1, 1)
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
            #pragma fragment fragment_shader_1_cam

            #include "UnityCG.cginc"
            #include "CameraComposer.cginc"
            ENDCG
        }
    }
}
