// <----- PROPERTIES -----------------------------------------------------------------------------------

sampler2D _MainTex;
sampler2D _MaskTex;

sampler2D _Cam0Tex;
sampler2D _Cam1Tex;
sampler2D _Cam2Tex;
sampler2D _Cam3Tex;

float4 _Cam0Rect;
float4 _Cam1Rect;
float4 _Cam2Rect;
float4 _Cam3Rect;

// <----- UTILS ----------------------------------------------------------------------------------------

float4 overlay_camera(float4 source_color, float2 uv, sampler2D camera_texture, float4 camera_rect)
{
    const float2 camera_uv = float2(
        (uv.x - camera_rect.x) / camera_rect.z,
        (uv.y - camera_rect.y) / camera_rect.w
    );
    const bool out_of_bounds = camera_uv.x < 0 || camera_uv.x > 1 || camera_uv.y < 0 || camera_uv.y > 1;
    
    const float4 col = tex2D(camera_texture, camera_uv);
    const float4 mask = tex2D(_MaskTex, camera_uv).r * !out_of_bounds;
    return lerp(source_color, col, mask);
}

// <----- DATA STRUCTS ---------------------------------------------------------------------------------

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
};

// <----- VERTEX SHADER --------------------------------------------------------------------------------

v2f vertex_shader(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

// <----- FRAGMENT SHADERS -----------------------------------------------------------------------------

float4 fragment_shader_1_cam(v2f i) : SV_Target
{
    float4 col = tex2D(_MainTex, i.uv);
    col = overlay_camera(col, i.uv, _Cam0Tex, _Cam0Rect);
    return col;
}

float4 fragment_shader_2_cams(v2f i) : SV_Target
{
    float4 col = tex2D(_MainTex, i.uv);
    col = overlay_camera(col, i.uv, _Cam0Tex, _Cam0Rect);
    col = overlay_camera(col, i.uv, _Cam1Tex, _Cam1Rect);
    return col;
}

float4 fragment_shader_3_cams(v2f i) : SV_Target
{
    float4 col = tex2D(_MainTex, i.uv);
    col = overlay_camera(col, i.uv, _Cam0Tex, _Cam0Rect);
    col = overlay_camera(col, i.uv, _Cam1Tex, _Cam1Rect);
    col = overlay_camera(col, i.uv, _Cam2Tex, _Cam2Rect);
    return col;
}

float4 fragment_shader_4_cams(v2f i) : SV_Target
{
    float4 col = tex2D(_MainTex, i.uv);
    col = overlay_camera(col, i.uv, _Cam0Tex, _Cam0Rect);
    col = overlay_camera(col, i.uv, _Cam1Tex, _Cam1Rect);
    col = overlay_camera(col, i.uv, _Cam2Tex, _Cam2Rect);
    col = overlay_camera(col, i.uv, _Cam3Tex, _Cam3Rect);
    return col;
}
