#ifdef SM4

// Macros for targetting shader model 4.0 (DX11)
#define TECHNIQUE(name, vsname, psname ) \
	technique name { pass { VertexShader = compile vs_4_0 vsname (); PixelShader = compile ps_4_0 psname(); } }

#define DECLARE_TEXTURE(Name, index) \
    Texture2D<float4> Name : register(t##index); \
    sampler Name##Sampler : register(s##index)
#define DECLARE_TEXTURE3(Name, index) \
    Texture3D<float4> Name : register(t##index); \
    sampler Name##Sampler : register(s##index)

#define SAMPLE_TEXTURE(Name, texCoord)  Name.Sample(Name##Sampler, texCoord)
#define SAMPLE_TEXTURE3(Name, texCoord)  Name.Sample(Name##Sampler, texCoord)

#else

// Macros for targetting shader model 2.0 (DX9)
#define TECHNIQUE(name, vsname, psname ) \
	technique name { pass { VertexShader = compile vs_2_0 vsname (); PixelShader = compile ps_2_0 psname(); } }

#define DECLARE_TEXTURE(Name, index) \
    sampler2D Name : register(s##index);
#define DECLARE_TEXTURE3(Name, index) \
    sampler3D Name : register(s##index);

#define SAMPLE_TEXTURE(Name, texCoord)  tex2D(Name, texCoord)
#define SAMPLE_TEXTURE(Name, texCoord)  tex3D(Name, texCoord)

#endif

#if OPENGL
    #define SV_POSITION POSITION
#endif

float4 Palette[255];

DECLARE_TEXTURE3(Voxels, 0);

float4 SimpleVS(float4 position : POSITION0) : POSITION0
{
    return position;
}

float4 DirectPS(float4 position : POSITION0) : COLOR0
{
    return float4(1, 0, 0, 0);
}

TECHNIQUE(Direct, SimpleVS, DirectPS);