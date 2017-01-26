#ifdef SM4

// Macros for targetting shader model 4.0 (DX11)
#define TECHNIQUE(name, vsname, psname ) \
	technique name { pass { VertexShader = compile vs_4_0 vsname (); PixelShader = compile ps_4_0 psname(); } }
#define DECLARE_TEXTURE(Name, index) \
    Texture2D<float4> Name : register(t##index); \
    sampler Name##Sampler : register(s##index)
#define SAMPLE_TEXTURE(Name, texCoord)  Name.Sample(Name##Sampler, texCoord)

#else

// Macros for targetting shader model 2.0 (DX9)
#define TECHNIQUE(name, vsname, psname ) \
	technique name { pass { VertexShader = compile vs_2_0 vsname (); PixelShader = compile ps_2_0 psname(); } }
#define DECLARE_TEXTURE(Name, index) \
    sampler2D Name : register(s##index);
#define SAMPLE_TEXTURE(Name, texCoord)  tex2D(Name, texCoord)

#endif

#if OPENGL
    #define SV_POSITION POSITION
#endif

struct BatchInput
{
    float4 Position : POSITION0;
    float3 Normal   : NORMAL0;
    float4 Color    : COLOR0;
};

struct CubeData
{
    float4 Position : POSITION0;
    float3 Normal   : NORMAL0;
};

struct BlockData
{
    // first three components are position, fourth is palette index
    uint4 OffsetIndex : POSITION1;
};

struct QuadData
{
    uint4 OffsetIndex : POSITION;
    uint4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Color    : COLOR0;
};

struct WithShadowOutput
{
    float4 Position : POSITION0;
    float4 Color    : COLOR0;
    float3 Diffuse  : COLOR1;
    float4 WorldPos : TEXCOORD0;
};

struct ShadowMapOutput
{
    float4 Position : POSITION0;
    float Depth     : TEXCOORD0;
};

float3 Normals[6];

float4 Palette[255];

// Camera settings.
float3 ChunkPosition;
float4x4 View;
float4x4 Projection;

matrix DirectionalLightMatrix;
float3 LightDirection = normalize(float3(-1, -1, -1));
float3 DiffuseLight = 1.25;
float3 AmbientLight = 0.25;

float DepthBias = 0.001f;
DECLARE_TEXTURE(ShadowMap, 0);

VertexShaderOutput DebugVS(CubeData unitCube, BlockData blockData)
{
    VertexShaderOutput output;

    float4 worldPosition = float4(ChunkPosition + unitCube.Position.xyz + blockData.OffsetIndex.xyz, 1);
    float4 viewPosition = mul(worldPosition, View);

    output.Position = mul(viewPosition, Projection);
    output.Color = Palette[int(blockData.OffsetIndex.z) - 1];

    return output;
}

VertexShaderOutput BatchVS(BatchInput input)
{
    VertexShaderOutput output;

    // Apply the world and camera matrices to compute the output position.
    float4 worldPosition = float4(ChunkPosition + input.Position.xyz, 1);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // Compute lighting, using a simple Lambert model.
    float3 worldNormal = input.Normal;
    float diffuseAmount = max(-dot(worldNormal, LightDirection), 0);
    float3 lightingResult = saturate(diffuseAmount * DiffuseLight + AmbientLight);
    
    output.Color = float4(lightingResult, 1) * input.Color;

    return output;
}

VertexShaderOutput InstancingVS(CubeData unitCube, BlockData block)
{
    VertexShaderOutput output;

    // Apply the world and camera matrices to compute the output position.
    float4 worldPosition = float4(ChunkPosition + unitCube.Position.xyz + block.OffsetIndex.xyz, 1);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // Compute lighting, using a simple Lambert model.
    float diffuseAmount = max(-dot(unitCube.Normal, LightDirection), 0);
    float3 lightingResult = saturate(diffuseAmount * DiffuseLight + AmbientLight);
    
    output.Color = float4(lightingResult, 1) * Palette[int(block.OffsetIndex.w) - 1];

    return output;
}

VertexShaderOutput MeshVS(QuadData quad)
{
    VertexShaderOutput output;

    float3 normal = Normals[quad.Normal.x];
    /*int dir = (int)((quad.Normal.x + 1) / 2) - 1;
    // offset in direction of normal should be 0.5*sign(normal), others should be -0.5
    float offset[3] = { -0.5, -0.5, -0.5 };
    offset[dir] = 0.5 * dot(sign(normal), float3(1,1,1));*/
    // quads are rendered at the center of the blocks, so we need to offset them along their normals by half the size of a voxel
    float3 pos = quad.OffsetIndex.xyz;
    float4 worldPosition = float4(ChunkPosition + pos, 1);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // Compute lighting, using a simple Lambert model.
    float diffuseAmount = max(-dot(normal, LightDirection), 0);
    float3 lightingResult = saturate(diffuseAmount * DiffuseLight + AmbientLight);

    output.Color = float4(lightingResult, 1) * Palette[int(quad.OffsetIndex.w) - 1];

    return output;
}

WithShadowOutput InstancingWithShadowVS(CubeData unitCube, BlockData blockData)
{
    WithShadowOutput output;

    float4 worldPosition = float4(ChunkPosition + unitCube.Position.xyz + blockData.OffsetIndex.xyz, 1);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    // Color lookup
    output.Color = Palette[int(blockData.OffsetIndex.w) - 1];

    // Compute lighting, using a simple Lambert model.
    float diffuseAmount = max(-dot(unitCube.Normal, LightDirection), 0);
    output.Diffuse = diffuseAmount * DiffuseLight;
    
    // for finding the shadowmap coordinates
    output.WorldPos = worldPosition;

    return output;
}

float4 InstancingWithShadowPS(WithShadowOutput input) : COLOR0
{
    // Find the position of this pixel in light space
    float4 lightingPosition = mul(input.WorldPos, DirectionalLightMatrix);

    float2 ShadowTexCoord = 0.5 * lightingPosition.xy / lightingPosition.w + 0.5;
    ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;

    // Get the current depth stored in the shadow map
    float shadowDepth = SAMPLE_TEXTURE(ShadowMap, ShadowTexCoord).r;
    
    // Calculate the current pixel depth
    // The bias is used to prevent floating point errors that occur when
    // the pixel of the occluder is being drawn
    float currentDepth = (lightingPosition.z / lightingPosition.w) - DepthBias;

    float3 light = AmbientLight;

    if (currentDepth < shadowDepth)
        light += input.Diffuse;

    return float4(saturate(light), 1)*input.Color;
}

ShadowMapOutput InstancingShadowMapVS(CubeData unitCube, BlockData blockData)
{
    ShadowMapOutput output;
    float4 worldPosition = float4(ChunkPosition + unitCube.Position.xyz + blockData.OffsetIndex.xyz, 1);
    output.Position = mul(worldPosition, DirectionalLightMatrix);
    output.Depth = output.Position.z / output.Position.w;
    return output;
}

float4 ShadowMapPS(ShadowMapOutput input) : COLOR0
{
    return float4(input.Depth, 0, 0, 0);
}

float4 CommonPS(VertexShaderOutput input) : COLOR0
{
    return input.Color;
}

float4 DepthPS(WithShadowOutput input) : COLOR0
{
    // Find the position of this pixel in light space
    float4 lightingPosition = mul(input.WorldPos, DirectionalLightMatrix);

    float2 ShadowTexCoord = 0.5 * lightingPosition.xy / lightingPosition.w + 0.5;
    ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;

    // Get the current depth stored in the shadow map
    float shadowDepth = SAMPLE_TEXTURE(ShadowMap, ShadowTexCoord).r;
    
    return float4(shadowDepth, 0, 0, 0);
}

TECHNIQUE(Debug, DebugVS, CommonPS);
TECHNIQUE(Batch, BatchVS, CommonPS);
TECHNIQUE(Instancing, InstancingVS, CommonPS);
TECHNIQUE(Mesh, MeshVS, CommonPS);
TECHNIQUE(InstancingDepth, InstancingWithShadowVS, DepthPS);
TECHNIQUE(InstancingWithShadow, InstancingWithShadowVS, InstancingWithShadowPS);
TECHNIQUE(InstancingShadowMap, InstancingShadowMapVS, ShadowMapPS);