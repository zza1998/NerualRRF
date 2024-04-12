using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


//--------------------Film--------------------
public class PBRTFilm
{
    public enum FilmType : int
    {
        rgb,
        gbuffer,
        spectral
    }

    public FilmType type;
    public int xresolution;
    public int yresolution;
    public string fileName;

    public PBRTFilm(FilmType _type, int _xresolution, int _yresolution, string _fileName)
    {
        type = _type;
        xresolution = _xresolution;
        yresolution = _yresolution;
        fileName = _fileName;
    }
}


//--------------------Camera--------------------
public abstract class PBRTCamera
{
    public enum CameraType : int
    {
        orthographic,
        perspective,
        realistic,
        spherical,
        cube,
        sample
    }

    public CameraType type;

    public PBRTCamera(CameraType _type)
    {
        type = _type;
    }
}

public class PBRTOrthographicCamera : PBRTCamera
{
    public float frameAspectRatio;

    public PBRTOrthographicCamera(float _frameAspectRatio) : base(CameraType.orthographic)
    {
        frameAspectRatio = _frameAspectRatio;
    }
}

public class PBRTPerspectiveCamera : PBRTCamera
{
    public float frameAspectRatio;
    public float fov;

    public PBRTPerspectiveCamera(float _frameAspectRatio, float _fov) : base(CameraType.perspective)
    {
        frameAspectRatio = _frameAspectRatio;
        fov = _fov;
    }
}

public class PBRTCubeCamera : PBRTCamera
{
    public PBRTCubeCamera() : base(CameraType.cube)
    {
    }
}

public class PBRTSampleCamera : PBRTCamera
{
    public PBRTSampleCamera() : base(CameraType.sample)
    {
    }
}

//--------------------Sampler--------------------
public class PBRTSampler
{
    public enum SamplerType : int
    {
        halton,
        independent,
        paddedsobol,
        sobol,
        stratified,
        zsobol
    }

    public SamplerType type;
    public int pixelsamples;

    public PBRTSampler(SamplerType _type, int _pixelsamples)
    {
        type = _type;
        pixelsamples = _pixelsamples;
    }
}


//--------------------Integrator--------------------
public class PBRTIntegrator
{
    public enum IntegratorType : int
    {
        ambientocclusion,
        bdpt,
        lightpath,
        mlt,
        path,
        randomwalk,
        simplepath,
        simplevolpath,
        sppm,
        volpath
    }

    public IntegratorType type;
    public int maxDepth;

    public PBRTIntegrator(IntegratorType _type, int _maxDepth)
    {
        type = _type;
        maxDepth = _maxDepth;
    }
}


//--------------------Light--------------------
public abstract class PBRTLight
{
    public enum LightType : int
    {
        distant,  //UnityEngine.LightType.Directional
        infinite, //UnityEngine.LightType.Environment
        point,    //UnityEngine.LightType.Point
        spot      //UnityEngine.LightType.Spot
    }

    public LightType type;
    public float power;    //illuminance
    public float scale;

    public PBRTLight(LightType _type, float _power, float _scale)
    {
        type = _type;
        power = _power;
        scale = _scale;
    }

    public virtual void OutputProperty(PBRT.LightSource lightSource) { }
}

//Unity Directional Light
public class PBRTDistantLight : PBRTLight
{
    Color L;
    Vector3 from;
    Vector3 to;

    public PBRTDistantLight(float _power, float _scale, Color _L, Vector3 _from, Vector3 _to) : base(LightType.distant, _power, _scale)
    {
        L = _L;
        from = _from;
        to = _to;
    }
}

//Unity Environment Light
public class PBRTInfiniteLight : PBRTLight
{
    string fileName;
    Vector3[] portal;
    Color L;

    float hours;
    float mins;

    public PBRTInfiniteLight(float _power, float _scale, string _fileName, Vector3[] _portal, Color _L, float _hours, float _mins) : base(LightType.infinite, _power, _scale)
    {
        fileName = _fileName;
        portal = _portal;
        L = _L;
        hours = _hours;
        mins = _mins;
    }

    public void SetTime(float _hours, float _mins)
    {
        hours = _hours;
        mins = _mins;
    }

    public override void OutputProperty(PBRT.LightSource lightSource) 
    {
        if(!string.IsNullOrEmpty(fileName))
            lightSource.SetParameter("filename", fileName);
        else
        {
            lightSource.SetParameters("date_time_location_timezone", new List<float> { 2023.0f, 12.0f, 22.0f, hours, mins, 114.062996f, 22.542883f, 8.0f });
            lightSource.SetParameter("power", base.power);
            lightSource.SetParameter("sunsize", 0.08f);
            lightSource.SetParameter("sunsizeconvergence", 5.0f);
            lightSource.SetParameter("atmospherethickness", 1.0f);
            lightSource.SetParameter("skyexposure", 1.3f);
            lightSource.SetParameter("imagesize", 512);
        }
    }
}

//Unity Point Light
public class PBRTPointLight : PBRTLight
{
    Color L;
    Vector3 from;

    public PBRTPointLight(float _power, float _scale, Color _L, Vector3 _from) : base(LightType.point, _power, _scale)
    {
        L = _L;
        from = _from;
    }

    public override void OutputProperty(PBRT.LightSource lightSource)
    {
        lightSource.SetParameter("scale", base.scale);
        lightSource.SetParameter("I", new PBRT.RGBSpectrum(L.r, L.g, L.b));
        lightSource.SetParameter("from", new PBRT.Point3(from.x, from.y, from.z));
    }
}

//Unity Spot Light
public class PBRTSpotLight : PBRTLight
{
    Color L;
    Vector3 from;
    Vector3 to;
    float coneAngle;
    float coneDeltaAngle;

    public PBRTSpotLight(float _power, float _scale, Color _L, Vector3 _from, Vector3 _to, float _coneAngle, float _coneDeltaAngle) : base(LightType.spot, _power, _scale)
    {
        L = _L;
        from = _from;
        to = _to;
        coneAngle = _coneAngle;
        coneDeltaAngle = _coneDeltaAngle;
    }
}


//--------------------Area Light--------------------
//Unity Area Light
public class PBRTAreaLight
{
    string fileName;
    Color L;
    bool twosided;

    public PBRTAreaLight(string fileName, Color L, bool twosided)
    {

    }
}


//--------------------Texture--------------------
public abstract class PBRTTexture
{
    public enum TextureType
    {
        imageMap
    }

    public TextureType textureType;
    public string name;
    public string fileName;
    public int index;

    public PBRTTexture(TextureType _type, string _name, string _fileName, int _index)
    {
        textureType = _type;
        name = _name;
        fileName = _fileName;
        index = _index;
    }

    public virtual void AddPBRTProperty(string name, Vector4 value) { }
    public virtual void AddPBRTProperty(string name, float value) { }

    public virtual void OutputProperty(PBRT.Texture tex, string assetPath) 
    {
        tex.SetParameter("filename", string.Format("{0}/{1}", assetPath, fileName));
    }
}

public class PBRTImageMapTexture : PBRTTexture
{
    public PBRTImageMapTexture(string _name, string _fileName, int _index) : base(TextureType.imageMap, _name, _fileName, _index)
    {
    }

    public override void OutputProperty(PBRT.Texture tex, string assetPath) 
    {
        base.OutputProperty(tex, assetPath);
        //tex.SetParameter("filter", "trilinear");
    }
}


//--------------------Material--------------------
public abstract class PBRTMaterial
{
    public enum MaterialType : int
    {
        coateddiffuse,
        coatedconductor,
        conductor,
        dielectric,
        diffuse,
        diffusetransmission,
        hair,
        interfaceMat,
        measured,
        mix,
        subsurface,
        thindielectric,
        disney,
        uber
    }

    public MaterialType type;
    public string name;

    public Dictionary<string, PBRTTexture> textureMap;

    public PBRTMaterial(MaterialType _type, string _name)
    {
        type = _type;
        name = _name;

        textureMap = new Dictionary<string, PBRTTexture>();
    }

    public virtual void AddPBRTTexture(string propertyName, PBRTTexture texture){}

    public virtual void AddPBRTProperty(string propertyName, Vector4 value){}
    public virtual void AddPBRTProperty(string propertyName, float value) { }

    public virtual void OutputProperty(PBRT.MakeNamedMaterial makedMat, List<PBRT.Texture> texList) { }
}

public class coateddiffuseMaterial : PBRTMaterial
{
    public float uroughness;
    public float vroughness;

    public Vector4 reflectance;

    public coateddiffuseMaterial(string _name) : base(MaterialType.coateddiffuse, _name)
    {
    }

    public override void AddPBRTTexture(string propertyName, PBRTTexture texture)
    {
        if (propertyName.Equals("_MainTex"))
            textureMap["reflectance"] = texture;
    }

    public override void AddPBRTProperty(string propertyName, Vector4 value)
    {
        if (propertyName.Equals("_BaseColorLinear"))
            reflectance = value;
    }

    public override void AddPBRTProperty(string propertyName, float value)
    {
        if(propertyName.Equals("_roughnessU"))
            uroughness = value;

        if(propertyName.Equals("_roughnessV"))
            vroughness = value;
    }

    public override void OutputProperty(PBRT.MakeNamedMaterial makedMat, List<PBRT.Texture> texList)
    {
        if (textureMap.ContainsKey("reflectance"))
            makedMat.SetParameter("reflectance", texList[textureMap["reflectance"].index]);
        else
            makedMat.SetParameter("reflectance", new PBRT.RGBSpectrum(reflectance.x, reflectance.y, reflectance.z));

        makedMat.SetParameter("uroughness", uroughness);
        makedMat.SetParameter("vroughness", vroughness);
        makedMat.SetParameter("remaproughness", false);
    }
}

public class coatedconductorMaterial : PBRTMaterial
{
    public Color albedo;
    public Texture2D albedoTex;

    public coatedconductorMaterial(string _name, Color _albedo, Texture2D _albedoTex) : base(MaterialType.coatedconductor, _name)
    {
        albedo = _albedo;
        albedoTex = _albedoTex;
    }
}

public class conductorMaterial : PBRTMaterial
{
    public Vector3 eta;
    public Vector3 k;

    public float uroughness;
    public float vroughness;

    public conductorMaterial(string _name) : base(MaterialType.conductor, _name)
    {
    }

    public override void AddPBRTProperty(string propertyName, Vector4 value)
    {
        if (propertyName.Equals("_eta"))
            eta = value;

        if (propertyName.Equals("_k"))
            k = value;
    }

    public override void AddPBRTProperty(string propertyName, float value)
    {
        if (propertyName.Equals("_roughnessU"))
            uroughness = value;

        if (propertyName.Equals("_roughnessV"))
            vroughness = value;
    }

    public override void OutputProperty(PBRT.MakeNamedMaterial makedMat, List<PBRT.Texture> texList)
    {
        makedMat.SetParameter("k", new PBRT.RGBSpectrum(k.x, k.y, k.z));
        makedMat.SetParameter("eta", new PBRT.RGBSpectrum(eta.x, eta.y, eta.z));

        if(uroughness == 0 && vroughness == 0)
        {
            makedMat.SetParameter("roughness", 0.0f);
        }
        else
        {
            makedMat.SetParameter("uroughness", uroughness);
            makedMat.SetParameter("vroughness", vroughness);
            makedMat.SetParameter("remaproughness", false);
        }
    }
}

public class dielectricMaterial : PBRTMaterial
{
    public Vector3 eta;

    public dielectricMaterial(string _name) : base(MaterialType.dielectric, _name)
    {
    }

    public override void AddPBRTProperty(string propertyName, Vector4 value)
    {
        if (propertyName.Equals("_eta"))
            eta = value;
    }

    public override void OutputProperty(PBRT.MakeNamedMaterial makedMat, List<PBRT.Texture> texList)
    {
        makedMat.SetParameter("eta", eta.x);
    }
}

public class diffuseMaterial : PBRTMaterial
{
    public Vector4 reflectance;

    public diffuseMaterial(string _name) : base(MaterialType.diffuse, _name)
    {
    }

    public override void AddPBRTTexture(string propertyName, PBRTTexture texture)
    {
        if(propertyName.Equals("_MainTex"))
            textureMap["reflectance"] = texture;
    }

    public override void AddPBRTProperty(string propertyName, Vector4 value)
    {
        if (propertyName.Equals("_BaseColorLinear"))
            reflectance = value;
    }

    public override void OutputProperty(PBRT.MakeNamedMaterial makedMat, List<PBRT.Texture> texList)
    {
        if(textureMap.ContainsKey("reflectance"))
            makedMat.SetParameter("reflectance", texList[textureMap["reflectance"].index]);
        else
            makedMat.SetParameter("reflectance", new PBRT.RGBSpectrum(reflectance.x, reflectance.y, reflectance.z));
    }
}

public class diffusetransmissionMaterial : PBRTMaterial
{
    public diffusetransmissionMaterial(string _name) : base(MaterialType.diffusetransmission, _name)
    {
    }
}

public class hairMaterial : PBRTMaterial
{
    public hairMaterial(string _name) : base(MaterialType.hair, _name)
    {
    }
}

public class interfaceMaterial : PBRTMaterial
{
    public interfaceMaterial(string _name) : base(MaterialType.interfaceMat, _name)
    {
    }
}

public class measuredMaterial : PBRTMaterial
{
    public measuredMaterial(string _name) : base(MaterialType.measured, _name)
    {
    }
}

public class mixMaterial : PBRTMaterial
{
    public mixMaterial(string _name) : base(MaterialType.mix, _name)
    {
    }
}

public class subsurfaceMaterial : PBRTMaterial
{
    public subsurfaceMaterial(string _name) : base(MaterialType.subsurface, _name)
    {
    }
}

public class thindielectricMaterial : PBRTMaterial
{
    public thindielectricMaterial(string _name) : base(MaterialType.thindielectric, _name)
    {
    }
}

public class disneyMaterial : PBRTMaterial
{
    public Vector4 color;
    public float anisotropic;
    public float clearcoat;
    public float clearcoatgloss;
    public float eta;
    public float metallic;
    public float roughness;
    public Vector4 scatterdistance;
    public float sheen;
    public float sheentint;
    public float spectrans;
    public float speculartint;
    public string normalMap;

    public float collectFlag;

    public disneyMaterial(string _name) : base(MaterialType.disney, _name)
    {
    }

    public override void AddPBRTTexture(string propertyName, PBRTTexture texture)
    {
        if (propertyName.Equals("mainTex"))
            textureMap["color"] = texture;
        else if (propertyName.Equals("normalMap"))
            normalMap = texture.fileName;
    }

    public override void AddPBRTProperty(string propertyName, float value)
    {
        if (propertyName.Equals("anisotropic"))
            anisotropic = value;
        else if (propertyName.Equals("clearcoat"))
            clearcoat = value;
        else if (propertyName.Equals("clearcoatGloss"))
            clearcoatgloss = value;
        else if (propertyName.Equals("eta"))
            eta = value;
        else if (propertyName.Equals("metallic"))
            metallic = value;
        else if (propertyName.Equals("roughness"))
            roughness = value;
        else if (propertyName.Equals("sheen"))
            sheen = value;
        else if (propertyName.Equals("sheenTint"))
            sheentint = value;
        else if (propertyName.Equals("specTrans"))
            spectrans = value;
        else if (propertyName.Equals("specularTint"))
            speculartint = value;
        else if (propertyName.Equals("collectFlag"))
            collectFlag = value;
    }

    public override void AddPBRTProperty(string propertyName, Vector4 value)
    {
        if (propertyName.Equals("basecolor"))
            color = value;
        else if (propertyName.Equals("scatterDistance"))
            scatterdistance = value;
    }

    public override void OutputProperty(PBRT.MakeNamedMaterial makedMat, List<PBRT.Texture> texList)
    {
        if (!string.IsNullOrEmpty(normalMap))
            makedMat.SetParameter("normalmap", "textures/" + normalMap);

        if (textureMap.ContainsKey("color"))
            makedMat.SetParameter("color", texList[textureMap["color"].index]);
        else
            makedMat.SetParameter("color", new PBRT.RGBSpectrum(color.x, color.y, color.z));

        makedMat.SetParameter("anisotropic", anisotropic);
        makedMat.SetParameter("clearcoat", clearcoat);
        makedMat.SetParameter("clearcoatgloss", clearcoatgloss);
        makedMat.SetParameter("eta", eta);
        makedMat.SetParameter("metallic", metallic);
        makedMat.SetParameter("roughness", roughness);
        makedMat.SetParameter("scatterdistance", new PBRT.RGBSpectrum(scatterdistance.x, scatterdistance.y, scatterdistance.z));
        makedMat.SetParameter("sheen", sheen);
        makedMat.SetParameter("sheentint", sheentint);
        makedMat.SetParameter("spectrans", spectrans);
        makedMat.SetParameter("speculartint", speculartint);

        makedMat.SetParameter("collect", collectFlag);
    }
}

public class uberMaterial : PBRTMaterial
{
    public uberMaterial(string _name) : base(MaterialType.uber, _name)
    {
    }
}

//--------------------Shape--------------------
public abstract class PBRTShape
{
    public enum ShapeType
    {
        bilinearmesh,
        curve,
        cylinder,
        disk,
        sphere,
        trianglemesh,
        ply
    }

    public ShapeType type;
    public string name;
    public Matrix4x4 transform;

    public PBRTShape(ShapeType _type, string _name, Matrix4x4 _transform)
    {
        type = _type;
        name = _name;
        transform = _transform;
    }
}

public class PBRTCylinderShape : PBRTShape
{
    public float radius;

    public PBRTCylinderShape(string _name, float _radius, Matrix4x4 _transform) : base(ShapeType.cylinder, _name, _transform)
    {
        radius = _radius;
    }
}

public class PBRTSphereShape : PBRTShape
{
    public float radius;

    public PBRTSphereShape(string _name, float _radius, Matrix4x4 _transform) : base(ShapeType.sphere, _name, _transform)
    {
        radius = _radius;
    }
}

public class PBRTPLYMesh : PBRTShape
{
    public string fileName;

    public PBRTPLYMesh(string _name, string _fileName, Matrix4x4 _transform) : base(ShapeType.ply, _name, _transform)
    {
        fileName = _fileName;
    }
}


//--------------------Header--------------------
public class PBRTHeader
{
    public Matrix4x4 globalTransform;
    public PBRTCamera pbrtCamera;
    public PBRTSampler pbrtSampler;
    public PBRTFilm pbrtFilm;
    public PBRTIntegrator pbrtIntegrator;
}

//--------------------Engine asset--------------------
public abstract class EngineAsset
{
    public int instanceID;

    public string assetName;
    public string assetPath;
    public string fullPath;

    public EngineAsset(int _instanceID)
    {
        instanceID = _instanceID;

        assetPath = AssetDatabase.GetAssetPath(instanceID);
        if (!string.IsNullOrEmpty(assetPath))
        {
            assetName = assetPath.Substring(assetPath.LastIndexOf('/') + 1);
            fullPath = Application.dataPath.Replace("Assets", "") + assetPath;
        }
    }
}

public class MeshAsset : EngineAsset
{
    public Mesh mesh;

    public MeshAsset(Mesh _mesh) : base(_mesh.GetInstanceID())
    {
        mesh = _mesh;

        if(string.IsNullOrEmpty(assetName) || assetName.Equals("unity default resources"))
        {
            assetName = mesh.name;
        }
        else
        {
            assetName = assetName.Remove(assetName.LastIndexOf("."));
        }
    }
}

public class MaterialAsset : EngineAsset
{
    public Material material;
    public int pbrtMatIndex;

    public MaterialAsset(Material _material, int _pbrtMatIndex) : base(_material.GetInstanceID())
    {
        material = _material;
        pbrtMatIndex = _pbrtMatIndex;
    }
}

public class TextureAsset : EngineAsset
{
    public Texture texture;

    public TextureAsset(Texture _texture) : base(_texture.GetInstanceID())
    {
        texture = _texture;
    }
}

public class PBRTSceneData
{
    //PBRT data
    public PBRTHeader pbrtHeader;

    public List<PBRTLight> pbrtLights;
    public List<PBRTAreaLight> pbrtAreaLights;

    public List<PBRTTexture> pbrtTextures;
    public List<PBRTMaterial> pbrtMaterials;
    public List<PBRTShape> pbrtShapes;

    public List<MeshAsset> meshAssetList;         //Mesh instanceID list
    public List<TextureAsset> textureAssetList;   //Texture instanceID list
    public List<MaterialAsset> materialAssetList; //Material instanceID list

    public Dictionary<int, List<int>> materialShapeMap;

    public Bounds bounds;

    public PBRTSceneData()
    {
        pbrtHeader = new PBRTHeader();
        pbrtLights = new List<PBRTLight>();
        pbrtAreaLights = new List<PBRTAreaLight>();
        pbrtTextures = new List<PBRTTexture>();
        pbrtMaterials = new List<PBRTMaterial>();
        pbrtShapes = new List<PBRTShape>();

        meshAssetList = new List<MeshAsset>();
        textureAssetList = new List<TextureAsset>();
        materialAssetList = new List<MaterialAsset>();

        materialShapeMap = new Dictionary<int, List<int>>();

        bounds = new Bounds();
    }
}

//--------------------Export tool window--------------------
public class PBRTSceneExport : EditorWindow
{
    [System.Serializable]
    public class PBRTOptions
    {
        public enum ExportFunc
        {
            PBRTSceneExport,
            LatinHypercubeSampling
        }

        public enum FilmFormat
        {
            EXR,
            JPG,
            PNG
        }

        public string pbrtfileName;
        public string filmName;
        public FilmFormat filmFormat = FilmFormat.EXR;
        public int xresolution = 1024;
        public int yresolution = 1024;
        public PBRTSampler.SamplerType sampleType = PBRTSampler.SamplerType.zsobol;
        public int pixelsamples = 16;
        public PBRTIntegrator.IntegratorType integratorType = PBRTIntegrator.IntegratorType.volpath;
        public int maxdepth = 5;
        public Vector3Int cameraDivide;
        public Vector3Int lightDivide;
        public Vector2 lightTimeRange;
        public float lightPower;
        public bool infiniteLight;
        public bool customBounds;
        public Vector3 boundMin;
        public Vector3 boundMax;
        public string pbrtFilePath;
        public ExportFunc exportFunc = ExportFunc.PBRTSceneExport;
        public bool exportPBRTFileAlone = false;

        public GameObject renderTarget;
    }

    public static PBRTOptions options;

    public const string PATH_BSDF = "bsdfs";
    public const string PATH_GEOMETRY = "geometry";
    public const string PATH_TEXTURE = "textures";
    public const string PATH_MATERIAL = "materials";
    public const string PATH_RRF = "rrf";

    private static string pbrtDirectory;

    Vector2 scrollPos;

    [MenuItem("PBRT/PBRT Scene Export")]
    static void Init()
    {
        PBRTSceneExport wnd = (PBRTSceneExport)GetWindow(typeof(PBRTSceneExport), false, "PBRT Exporter", true);
        pbrtDirectory = Application.dataPath.Replace("UnityProject/unity-raytracing/Assets", "DataSet/");
        options = new PBRTOptions();

        wnd.Show();
    }

    void OnGUI()
    {
        if (options == null)
            options = new PBRTOptions();

        options.exportFunc = (PBRTOptions.ExportFunc)EditorGUILayout.EnumPopup("Function Select:", options.exportFunc);
        switch (options.exportFunc)
        {
            case PBRTOptions.ExportFunc.LatinHypercubeSampling:
                LatinHypercubeSampling_GUI();
                break;

            case PBRTOptions.ExportFunc.PBRTSceneExport:
            default:
                PBRTSceneExport_GUI();
                break;
        }
    }

    //--------------------GUI--------------------
    public void LatinHypercubeSampling_GUI()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginVertical("Box");
        options.cameraDivide = EditorGUILayout.Vector3IntField("Camera divide:", options.cameraDivide);
        options.lightDivide = EditorGUILayout.Vector3IntField("Light divide:", options.lightDivide);

        options.boundMin = EditorGUILayout.Vector3Field("Bounding box min:", options.boundMin);
        options.boundMax = EditorGUILayout.Vector3Field("Bounding box max:", options.boundMax);
        if (options.boundMin.x >= options.boundMax.x || options.boundMin.y >= options.boundMax.y || options.boundMin.z >= options.boundMax.z)
        {
            EditorGUILayout.HelpBox("The size of the bounding box is incorrect!", MessageType.Error);
        }

        options.yresolution = EditorGUILayout.IntSlider("yresolution", options.yresolution, 100, 100000);
        options.sampleType = (PBRTSampler.SamplerType)EditorGUILayout.EnumPopup("Sampler Type", options.sampleType);
        options.pixelsamples = EditorGUILayout.IntSlider("pixelsamples", options.pixelsamples, 1, 100000);
        options.integratorType = (PBRTIntegrator.IntegratorType)EditorGUILayout.EnumPopup("Integrator Type", options.integratorType);
        options.maxdepth = EditorGUILayout.IntSlider("maxdepth", options.maxdepth, 1, 500);

        EditorGUILayout.Space(5);
        options.pbrtFilePath = EditorGUILayout.TextField("PBRT file path:", options.pbrtFilePath);
        if(string.IsNullOrEmpty(options.pbrtFilePath))
        {
            options.pbrtFilePath = pbrtDirectory;
        }
        if (!string.IsNullOrEmpty(options.pbrtFilePath))
        {
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Split PBRT Scene File"))
            {
                bool success = SplitPBRTSceneFile(options.pbrtFilePath);

                EditorUtility.DisplayDialog("Build PBRT Scene", success ? "Build Successful!" : "Build Failed!", "OK");
            }
        }
        EditorGUILayout.EndVertical();
    }

    public void PBRTSceneExport_GUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        string sceneName = SceneManager.GetActiveScene().name;
        if(string.IsNullOrEmpty(options.pbrtFilePath))
            options.pbrtFilePath = pbrtDirectory + sceneName;

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Export path:");
        EditorGUILayout.BeginVertical("Box");
        options.pbrtFilePath = EditorGUILayout.TextField(options.pbrtFilePath);
        options.exportPBRTFileAlone = EditorGUILayout.Toggle("Export PBRT File Alone", options.exportPBRTFileAlone);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("PBRT filename prefix:");
        EditorGUILayout.BeginVertical("Box");
        options.pbrtfileName = EditorGUILayout.TextField(options.pbrtfileName);
        if (string.IsNullOrEmpty(options.pbrtfileName))
        {
            options.pbrtfileName = sceneName;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Camera:");
        EditorGUILayout.BeginVertical("Box");
        options.cameraDivide = EditorGUILayout.Vector3IntField("Camera divide:", options.cameraDivide);
        options.customBounds = EditorGUILayout.Toggle("Custom Camera Bouds", options.customBounds);
        if(options.customBounds)
        {
            options.boundMin = EditorGUILayout.Vector3Field("Bounding box min:", options.boundMin);
            options.boundMax = EditorGUILayout.Vector3Field("Bounding box max:", options.boundMax);
            if (options.boundMin.x >= options.boundMax.x || options.boundMin.y >= options.boundMax.y || options.boundMin.z >= options.boundMax.z)
            {
                EditorGUILayout.HelpBox("The size of the bounding box is incorrect!", MessageType.Error);
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Light:");
        EditorGUILayout.BeginVertical("Box");
        options.infiniteLight = EditorGUILayout.Toggle("Use infinite Light", options.infiniteLight);
        if(options.infiniteLight)
        {
            options.lightDivide = EditorGUILayout.Vector3IntField("Light divide:", options.lightDivide);
            options.lightTimeRange = EditorGUILayout.Vector2Field("Light time range:", options.lightTimeRange);
            options.lightPower = EditorGUILayout.Slider("Light power", options.lightPower, 0.1f, 5.0f);
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Render:");
        EditorGUILayout.BeginVertical("Box");
        options.filmName = EditorGUILayout.TextField("Film name:", options.filmName);
        if (string.IsNullOrEmpty(options.filmName))
        {
            options.filmName = sceneName;
        }
        options.filmFormat = (PBRTOptions.FilmFormat)EditorGUILayout.EnumPopup("Film Format", options.filmFormat);
        options.xresolution = EditorGUILayout.IntSlider("xresolution", options.xresolution, 16, 100000);
        options.yresolution = EditorGUILayout.IntSlider("yresolution", options.yresolution, 16, 100000);
        options.sampleType = (PBRTSampler.SamplerType)EditorGUILayout.EnumPopup("Sampler Type", options.sampleType);
        options.pixelsamples = EditorGUILayout.IntSlider("pixelsamples", options.pixelsamples, 1, 100000);
        options.integratorType = (PBRTIntegrator.IntegratorType)EditorGUILayout.EnumPopup("Integrator Type", options.integratorType);
        options.maxdepth = EditorGUILayout.IntSlider("maxdepth", options.maxdepth, 1, 500);
        options.renderTarget = (GameObject)EditorGUILayout.ObjectField("render Target", options.renderTarget, typeof(GameObject), true);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Build PBRT Scene"))
        {
            bool success = ExportPBRTScene(options.pbrtFilePath, options.pbrtfileName);

            EditorUtility.DisplayDialog("Build PBRT Scene", success ? "Build Successful!" : "Build Failed!", "OK");
        }
        EditorGUILayout.EndScrollView();
    }

    //--------------------Export pbrt scene from unity--------------------
    public bool ExportPBRTScene(string exportDir, string sceneName)
    {
        string pbrtFile = string.Format("{0}/{1}.pbrt", exportDir, sceneName);
        string materialFile = string.Format("{0}/{1}.pbrt", exportDir, PATH_MATERIAL);
        string geometryFile = string.Format("{0}/{1}.pbrt", exportDir, PATH_GEOMETRY);
        string geometryPath = string.Format("{0}/{1}/", exportDir, PATH_GEOMETRY);
        string texturePath = string.Format("{0}/{1}/", exportDir, PATH_TEXTURE);
        string bsdfPath = string.Format("{0}/{1}/", exportDir, PATH_BSDF);
        string rrfPath = string.Format("{0}/{1}/", exportDir, PATH_RRF);

        PBRTSceneData pbrtSceneData;

        if (!CollectSceneData(out pbrtSceneData))
        {
            Debug.LogError("PBRT scene data collect failed!");
            return false;
        }

        CreateExportDirectories(exportDir, geometryPath, texturePath, bsdfPath, rrfPath);

        if(!options.exportPBRTFileAlone)
        {
            ExtractGeometryAsset(pbrtSceneData, geometryPath);
            ExtractTextureAsset(pbrtSceneData, texturePath);
        }

        BuildMaterialFile(pbrtSceneData, materialFile);
        BuildGeometryFile(pbrtSceneData, geometryFile);

        if(options.exportPBRTFileAlone)
        {
            BuildPBRTFile(pbrtSceneData, pbrtFile);
        }
        else
        {
            BuildDividedPBRTFiles(pbrtSceneData, options.cameraDivide, options.lightDivide, pbrtFile);
        }

        return true;
    }

    public bool CollectSceneData(out PBRTSceneData pbrtSceneData)
    {
        pbrtSceneData = new PBRTSceneData();

        MeshRenderer[] meshRenderers = GameObject.FindObjectsOfType<MeshRenderer>();
        UnityEngine.Camera[] cameras = GameObject.FindObjectsOfType<UnityEngine.Camera>();
        UnityEngine.Light[] lights = GameObject.FindObjectsOfType<UnityEngine.Light>();

        Camera mainCam = Camera.main;
        if (mainCam == null && cameras.Length > 0)
        {
            mainCam = cameras[0];
        }

        if (mainCam == null)
        {
            Debug.LogError("main camera is null!");
            return false;
        }
        else
        {
            if (!IsCameraTransformValid(mainCam.transform))
            {
                EditorUtility.DisplayDialog("Warning!", "Camera Transform invalid!", "OK");
            }
            pbrtSceneData.pbrtHeader.globalTransform = UnityTransformToInversePLYMatrix4x4(mainCam.transform.localToWorldMatrix);

            if (mainCam.orthographic)
            {
                pbrtSceneData.pbrtHeader.pbrtCamera = new PBRTOrthographicCamera(mainCam.aspect);
            }
            else
            {
                pbrtSceneData.pbrtHeader.pbrtCamera = new PBRTPerspectiveCamera(mainCam.aspect, mainCam.fieldOfView);
            }
        }

        string filmName = options.filmName + "." + options.filmFormat.ToString().ToLower();
        pbrtSceneData.pbrtHeader.pbrtFilm = new PBRTFilm(PBRTFilm.FilmType.gbuffer, options.xresolution, options.yresolution, filmName);
        pbrtSceneData.pbrtHeader.pbrtSampler = new PBRTSampler(options.sampleType, options.pixelsamples);
        pbrtSceneData.pbrtHeader.pbrtIntegrator = new PBRTIntegrator(options.integratorType, options.maxdepth);

        // collect light source
        if(!options.infiniteLight)
        {
            foreach (Light light in lights)
            {
                if (light == null)
                    continue;

                if (light.type == LightType.Point)
                {
                    pbrtSceneData.pbrtLights.Add(new PBRTPointLight(1.0f, light.range, light.color, light.transform.position));
                }
            }
        }
        else
        {
            pbrtSceneData.pbrtLights.Add(new PBRTInfiniteLight(options.lightPower, 0.0f, "", null, Color.black, 12.0f, 0.0f));
        }

        foreach (MeshRenderer meshRender in meshRenderers)
        {
            if (meshRender == null)
                continue;

            MeshFilter meshFilter = meshRender.GetComponent<MeshFilter>();
            Material mat = meshRender.sharedMaterial;
            if (meshFilter == null || meshFilter.sharedMesh == null || mat == null)
            {
                continue;
            }

            pbrtSceneData.bounds.Encapsulate(meshRender.bounds);

            //extract material
            MaterialAsset matAsset = null;
            if (!GetMaterialAsset(pbrtSceneData, mat, ref matAsset))
            {                
                PBRTMaterial pbrtMat = CreatePbrtMaterial(mat);
                if (pbrtMat == null)
                    continue;

                matAsset = new MaterialAsset(mat, pbrtSceneData.pbrtMaterials.Count);
                pbrtSceneData.materialAssetList.Add(matAsset);

                for (int i = 0; i < ShaderUtil.GetPropertyCount(mat.shader); i++)
                {
                    string propertyName = ShaderUtil.GetPropertyName(mat.shader, i);
                    ShaderUtil.ShaderPropertyType propType = ShaderUtil.GetPropertyType(mat.shader, i);
                    switch(propType)
                    {
                        case ShaderUtil.ShaderPropertyType.TexEnv: // Texture
                            {
                                Texture texture = mat.GetTexture(propertyName);
                                if (texture == null)
                                    continue;

                                //extract texture asset
                                TextureAsset textureAsset = null;
                                if (!GetTextureAsset(pbrtSceneData, texture, ref textureAsset))
                                {
                                    textureAsset = new TextureAsset(texture);
                                    pbrtSceneData.textureAssetList.Add(textureAsset);
                                }

                                //find pbrt texture
                                PBRTTexture pbrtTexture = null;
                                if (!GetPbrtTexture(pbrtSceneData, textureAsset, ref pbrtTexture))
                                {
                                    int idx = pbrtSceneData.pbrtTextures.Count;
                                    string texName = string.Format("Texture{0}", idx + 1);
                                    pbrtTexture = new PBRTImageMapTexture(texName, textureAsset.assetName, idx);
                                    pbrtSceneData.pbrtTextures.Add(pbrtTexture);
                                }

                                pbrtMat.AddPBRTTexture(propertyName, pbrtTexture);
                            }
                            break;
                        case ShaderUtil.ShaderPropertyType.Vector:
                            {
                                pbrtMat.AddPBRTProperty(propertyName, mat.GetVector(propertyName));
                            }
                            break;
                        case ShaderUtil.ShaderPropertyType.Range:
                        case ShaderUtil.ShaderPropertyType.Float:
                            {
                                pbrtMat.AddPBRTProperty(propertyName, mat.GetFloat(propertyName));
                            }
                            break;
                        default:
                            break;
                    }
                }

                pbrtSceneData.materialShapeMap.Add(pbrtSceneData.pbrtMaterials.Count, new List<int>());
                pbrtSceneData.pbrtMaterials.Add(pbrtMat);
            }

            // extract shape
            Mesh mesh = meshFilter.sharedMesh;
            MeshAsset meshAsset = null;
            if (!GetMeshAsset(pbrtSceneData, mesh, ref meshAsset))
            {
                meshAsset = new MeshAsset(mesh);
                pbrtSceneData.meshAssetList.Add(meshAsset);
            }

            Matrix4x4 plyMatrix = UnityTransformToPLYMatrix4x4(meshRender.transform.localToWorldMatrix);
            PBRTPLYMesh plyMeshShape = new PBRTPLYMesh(mesh.name, meshAsset.assetName, plyMatrix);

            pbrtSceneData.materialShapeMap[matAsset.pbrtMatIndex].Add(pbrtSceneData.pbrtShapes.Count);
            pbrtSceneData.pbrtShapes.Add(plyMeshShape);
        }

        return true;
    }

    public void BuildDividedPBRTFiles(PBRTSceneData pbrtSceneData, Vector3Int cameraDivide, Vector3Int lightDivide, string pbrtFile)
    {
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            UnityEngine.Camera[] cameras = GameObject.FindObjectsOfType<UnityEngine.Camera>();
            if (cameras.Length > 0)
                mainCam = cameras[0];
        }

        if (mainCam == null)
        {
            Debug.LogError("main camera is null!");
            return;
        }
        //mainCam.transform.eulerAngles = Vector3.zero;
        mainCam.transform.localScale = Vector3.one;

        //pbrtSceneData.pbrtHeader.pbrtCamera = new PBRTCubeCamera();
        //pbrtSceneData.pbrtHeader.pbrtCamera = new PBRTSampleCamera();

        Bounds bounds = pbrtSceneData.bounds;
        if (options.customBounds)
        {
            bounds.SetMinMax(options.boundMin, options.boundMax);
        }

        GameObject camSampleRoot = GameObject.Find("camSamplePoints");
        if (camSampleRoot != null)
            DestroyImmediate(camSampleRoot);

        camSampleRoot = new GameObject("camSamplePoints");

        //foreach cameras
        int camIdx = 0;
        for (int z = 0; z < cameraDivide.z; z++)
        {
            float interval_z = bounds.size.z / cameraDivide.z;
            float pos_z = bounds.min.z + z * interval_z + interval_z * 0.5f;

            for (int y = 0; y < cameraDivide.y; y++)
            {
                float interval_y = bounds.size.y / cameraDivide.y;
                float pos_y = bounds.min.y + y * interval_y + interval_y * 0.5f;

                for (int x = 0; x < cameraDivide.x; x++)
                {
                    float interval_x = bounds.size.x / cameraDivide.x;
                    float pos_x = bounds.min.x + x * interval_x + interval_x * 0.5f;

                    mainCam.transform.position = new Vector3(pos_x, pos_y, pos_z);
                    mainCam.transform.eulerAngles = Vector3.zero;
                    if (options.renderTarget)
                        CameraFocusRenderTarget(options.renderTarget);

                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.name = string.Format("Cam_{0}[{1}_{2}_{3}]", camIdx, x, y, z);
                    go.transform.SetParent(camSampleRoot.transform, false);
                    go.transform.position = mainCam.transform.position;
                    go.transform.eulerAngles = mainCam.transform.eulerAngles;
                    go.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                    if (!IsCameraTransformValid(mainCam.transform))
                    {
                        EditorUtility.DisplayDialog("Warning!", "Camera Transform invalid!", "OK");
                    }
                    pbrtSceneData.pbrtHeader.globalTransform = UnityTransformToInversePLYMatrix4x4(mainCam.transform.localToWorldMatrix);

                    //foreach lights
                    int lightSum = options.infiniteLight ? (lightDivide.x * lightDivide.y * lightDivide.z) : pbrtSceneData.pbrtLights.Count;
                    for (int lightIdx = 0; lightIdx < lightSum; lightIdx++)
                    {
                        if (options.infiniteLight)
                        {
                            PBRTInfiniteLight infiniteLight = (PBRTInfiniteLight)pbrtSceneData.pbrtLights[0];
                            if (infiniteLight != null)
                            {
                                float timeRange = options.lightTimeRange.y - options.lightTimeRange.x;
                                timeRange = Mathf.Clamp(timeRange, 1.0f, 24.0f);
                                float interval = (timeRange * 60.0f) / (lightDivide.x * lightDivide.y * lightDivide.z);
                                float time = lightIdx * interval + interval * 0.5f + Mathf.Clamp(options.lightTimeRange.x, 0.0f, 23.0f);
                                infiniteLight.SetTime((int)(time / 60), (int)(time % 60));
                            }
                        }

                        string[] paths = pbrtFile.Split('/');
                        string dirName = paths[paths.Length-2];
                        string fileName = pbrtFile.Replace(".pbrt", string.Format("_light{0}_cam{1}.pbrt", lightIdx, camIdx));
                        string filmName = string.Format("{0}/{1}/{2}_light{3}_cam{4}.{5}", dirName, PATH_RRF, options.filmName, lightIdx, camIdx, options.filmFormat.ToString().ToLower());

                        pbrtSceneData.pbrtHeader.pbrtFilm.fileName = filmName;

                        BuildPBRTFile(pbrtSceneData, fileName, options.infiniteLight ? 0 : lightIdx);
                    }
                    camIdx++;
                }
            }
        }
    }

    public void CreateExportDirectories(string exportPath, string geometryPath, string texturePath, string bsdfPath, string rrfPath)
    {
        if (Directory.Exists(exportPath))
        {
            DirectoryInfo di = new DirectoryInfo(exportPath);

            if(!options.exportPBRTFileAlone)
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }

        Directory.CreateDirectory(exportPath);
        Directory.CreateDirectory(geometryPath);
        Directory.CreateDirectory(texturePath);
        Directory.CreateDirectory(bsdfPath);
        Directory.CreateDirectory(rrfPath);
    }

    public PBRTMaterial CreatePbrtMaterial(Material mat)
    {
        PBRTMaterial pbrtMaterial = null;
        if (mat == null)
            return pbrtMaterial;

        if (!mat.shader.name.Equals("Disney") && !mat.shader.name.Equals("NerualGI"))
            return pbrtMaterial;

        pbrtMaterial = new disneyMaterial(mat.name); 

        return pbrtMaterial;
    }

    public bool GetMeshAsset(PBRTSceneData pbrtSceneData, Mesh mesh, ref MeshAsset meshAsset)
    {
        if (pbrtSceneData.meshAssetList.Count > 0)
        {
            meshAsset = pbrtSceneData.meshAssetList.Find(x => x.instanceID == mesh.GetInstanceID());
            if (meshAsset != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool GetMaterialAsset(PBRTSceneData pbrtSceneData, Material mat, ref MaterialAsset matAsset)
    {
        if (pbrtSceneData.materialAssetList.Count > 0)
        {
            matAsset = pbrtSceneData.materialAssetList.Find(x => x.instanceID == mat.GetInstanceID());
            if (matAsset != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool GetTextureAsset(PBRTSceneData pbrtSceneData, Texture texture, ref TextureAsset textureAsset)
    {
        if (pbrtSceneData.textureAssetList.Count > 0)
        {
            textureAsset = pbrtSceneData.textureAssetList.Find(x => x.instanceID == texture.GetInstanceID());
            if (textureAsset != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool GetPbrtTexture(PBRTSceneData pbrtSceneData, TextureAsset textureAsset, ref PBRTTexture pbrtTexture)
    {
        if (pbrtSceneData.pbrtTextures.Count > 0)
        {
            pbrtTexture = pbrtSceneData.pbrtTextures.Find(x => x.fileName == textureAsset.assetName);
            if (pbrtTexture != null)
            {
                return true;
            }
        }
        return false;
    }

    public void ExtractGeometryAsset(PBRTSceneData pbrtSceneData, string extractPath)
    {
        foreach (MeshAsset meshAsset in pbrtSceneData.meshAssetList)
        {
            string extractFullPath = extractPath + meshAsset.assetName;
            ConvertPLYFile(meshAsset.mesh, extractFullPath);
        }
    }

    public void ExtractTextureAsset(PBRTSceneData pbrtSceneData, string extractPath)
    {
        foreach (TextureAsset textureAsset in pbrtSceneData.textureAssetList)
        {
            string extractFullPath = extractPath + textureAsset.assetName;

            if(!File.Exists(textureAsset.fullPath))
            {
                Debug.LogWarning("Texture asset isn't exist ! asset path: " + textureAsset.fullPath);
                continue;
            }

            File.Copy(textureAsset.fullPath, extractFullPath, true);
        }
    }

    public void BuildPBRTFile(PBRTSceneData pbrtSceneData, string filePath, int lightIdx = -1)
    {
        PBRT.PBRTScene pbrtScene = new PBRT.PBRTScene();

        Matrix4x4 gMatrix = pbrtSceneData.pbrtHeader.globalTransform;
        pbrtScene.Transform(gMatrix.m00, gMatrix.m01, gMatrix.m02, gMatrix.m03,
                            gMatrix.m10, gMatrix.m11, gMatrix.m12, gMatrix.m13,
                            gMatrix.m20, gMatrix.m21, gMatrix.m22, gMatrix.m23,
                            gMatrix.m30, gMatrix.m31, gMatrix.m32, gMatrix.m33);

        //output Film
        var pbrtFilm = pbrtScene.Film((PBRT.Film.Type)pbrtSceneData.pbrtHeader.pbrtFilm.type);
        pbrtFilm.SetParameter("xresolution", pbrtSceneData.pbrtHeader.pbrtFilm.xresolution);
        pbrtFilm.SetParameter("yresolution", pbrtSceneData.pbrtHeader.pbrtFilm.yresolution);
        pbrtFilm.SetParameter("filename", pbrtSceneData.pbrtHeader.pbrtFilm.fileName);

        //output Camera
        var pbrtCam = pbrtScene.Camera((PBRT.Camera.Type)pbrtSceneData.pbrtHeader.pbrtCamera.type);
        if (pbrtSceneData.pbrtHeader.pbrtCamera.type == PBRTCamera.CameraType.orthographic)
        {
            PBRTOrthographicCamera cam = (PBRTOrthographicCamera)pbrtSceneData.pbrtHeader.pbrtCamera;
            //pbrtCam.SetParameter("frameaspectratio", cam.frameAspectRatio);
        }
        else if (pbrtSceneData.pbrtHeader.pbrtCamera.type == PBRTCamera.CameraType.perspective)
        {
            UnityEngine.Matrix4x4 p = Camera.main.projectionMatrix;
            UnityEngine.Matrix4x4 v = Camera.main.worldToCameraMatrix;
            UnityEngine.Matrix4x4 m_WorldToClip = p * v;

            string matrixStr = "# Visual Matrix";
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    matrixStr += " " + m_WorldToClip[i,j].ToString();
                }
            }

            pbrtScene.Custom(matrixStr);

            PBRTPerspectiveCamera cam = (PBRTPerspectiveCamera)pbrtSceneData.pbrtHeader.pbrtCamera;
            //pbrtCam.SetParameter("frameaspectratio", cam.frameAspectRatio);
            pbrtCam.SetParameter("fov", cam.fov);
        }
        else if (pbrtSceneData.pbrtHeader.pbrtCamera.type == PBRTCamera.CameraType.cube)
        {
            PBRTCubeCamera cam = (PBRTCubeCamera)pbrtSceneData.pbrtHeader.pbrtCamera;
        }
        else if (pbrtSceneData.pbrtHeader.pbrtCamera.type == PBRTCamera.CameraType.sample)
        {
            PBRTSampleCamera cam = (PBRTSampleCamera)pbrtSceneData.pbrtHeader.pbrtCamera;
        }

        //output Sampler
        var pbrtSampler = pbrtScene.Sampler((PBRT.Sampler.Type)pbrtSceneData.pbrtHeader.pbrtSampler.type);
        pbrtSampler.SetParameter("pixelsamples", pbrtSceneData.pbrtHeader.pbrtSampler.pixelsamples);

        //output Integrator
        var pbrtIntegrator = pbrtScene.Integrator((PBRT.Integrator.Type)pbrtSceneData.pbrtHeader.pbrtIntegrator.type);
        pbrtIntegrator.SetParameter("maxdepth", pbrtSceneData.pbrtHeader.pbrtIntegrator.maxDepth);

        var worldBegin = pbrtScene.WorldBegin();

        //output Light
        pbrtScene.AttributeBegin();
      
        for(int i = 0; i < pbrtSceneData.pbrtLights.Count; i++)
        {
            PBRTLight pbrtLight = pbrtSceneData.pbrtLights[i];
            if(lightIdx != -1 && lightIdx != i)
                continue;

            if (pbrtLight.type == PBRTLight.LightType.infinite)
            {
                pbrtScene.Transform(-1.0f, 0.0f, 0.0f, 0.0f,
                     0.0f, 0.0f, -1.0f, 0.0f,
                     0.0f, 1.0f, 0.0f, 0.0f,
                     0.0f, 0.0f, 0.0f, 1.0f);

                var lightSource = pbrtScene.LightSource(PBRT.LightSource.Type.Infinite);
                pbrtLight.OutputProperty(lightSource);
            }
            else if(pbrtLight.type == PBRTLight.LightType.point)
            {
                var lightSource = pbrtScene.LightSource(PBRT.LightSource.Type.point);
                pbrtLight.OutputProperty(lightSource);
            }
        }
        pbrtScene.AttributeEnd();

        pbrtScene.IncludeFile("materials.pbrt");
        pbrtScene.IncludeFile("geometry.pbrt");

        pbrtScene.Save(filePath);
    }

    public void BuildMaterialFile(PBRTSceneData pbrtSceneData, string filePath)
    {
        PBRT.PBRTScene pbrtScene = new PBRT.PBRTScene();

        List<PBRT.Texture> texList = new List<PBRT.Texture>();
        foreach (PBRTTexture pbrtTex in pbrtSceneData.pbrtTextures)
        {
            var tex = pbrtScene.Texture(PBRT.Texture.DataType.Spectrum, PBRT.Texture.ClassType.ImageMap, PBRT.Texture.Mapping.UV, pbrtTex.name);
            pbrtTex.OutputProperty(tex, PATH_TEXTURE);

            texList.Add(tex);
        }

        foreach (PBRTMaterial pbrtMat in pbrtSceneData.pbrtMaterials)
        {
            var mat = pbrtScene.MakeNamedMaterial((PBRT.Material.Type)pbrtMat.type, pbrtMat.name);
            pbrtMat.OutputProperty(mat, texList);
        }

        pbrtScene.Save(filePath);
    }

    public void BuildGeometryFile(PBRTSceneData pbrtSceneData, string filePath)
    {
        PBRT.PBRTScene pbrtScene = new PBRT.PBRTScene();

        foreach(KeyValuePair<int, List<int>> kvp in pbrtSceneData.materialShapeMap)
        {
            int matIdx = kvp.Key;
            PBRTMaterial pbrtMat = pbrtSceneData.pbrtMaterials[matIdx];

            pbrtScene.AttributeBegin();
            pbrtScene.NamedMaterial(pbrtMat.name);
            
            foreach(int idx in kvp.Value)
            {
                PBRTShape pbrtShape = pbrtSceneData.pbrtShapes[idx];
                if(pbrtShape.type == PBRTShape.ShapeType.ply)
                {
                    pbrtScene.Transform(pbrtShape.transform.m00, pbrtShape.transform.m01, pbrtShape.transform.m02, pbrtShape.transform.m03, 
                                        pbrtShape.transform.m10, pbrtShape.transform.m11, pbrtShape.transform.m12, pbrtShape.transform.m13,
                                        pbrtShape.transform.m20, pbrtShape.transform.m21, pbrtShape.transform.m22, pbrtShape.transform.m23,
                                        pbrtShape.transform.m30, pbrtShape.transform.m31, pbrtShape.transform.m32, pbrtShape.transform.m33);
                    PBRTPLYMesh plyMesh = (PBRTPLYMesh)pbrtShape;
                    var shape = pbrtScene.Shape(PBRT.Shape.Type.PLYMesh);
                    shape.SetParameter("filename", string.Format("{0}/{1}.ply", PATH_GEOMETRY, plyMesh.fileName));
                }
            }

            pbrtScene.AttributeEnd();
        }

        pbrtScene.Save(filePath);
    }

    //--------------------PBRT scene file split--------------------
    public bool SplitPBRTSceneFile(string pbrtFilePath)
    {
        pbrtFilePath = pbrtFilePath.Replace("\\", "/");

        float[] scaleInfo;
        List<Vector3> camSampleList = SampleCameras();
        List<string> fileContentList = ParseFile(pbrtFilePath, out scaleInfo);

        for(int i = 0; i < camSampleList.Count; i++)
        {
            string oriName = pbrtFilePath.Substring(pbrtFilePath.LastIndexOf('/') + 1).Replace(".pbrt", "");
            string newName = string.Format("{0}_cam_{1}", oriName, i);

            string outputPath = pbrtFilePath.Substring(0, pbrtFilePath.LastIndexOf('/'));
            string newFilePath = string.Format("{0}/{1}.pbrt", outputPath, newName);

            PBRT.PBRTScene pbrtScene = new PBRT.PBRTScene();

            if (scaleInfo != null)
                pbrtScene.Scale(scaleInfo[0], scaleInfo[1], scaleInfo[2]);

            Vector3 camPos = camSampleList[i];

            pbrtScene.LookAt(camPos.x, camPos.y, camPos.z, camPos.x, camPos.y, camPos.z - 1.0f, 0, 1, 0);

            pbrtScene.Camera(PBRT.Camera.Type.Cube);

            var sampler = pbrtScene.Sampler((PBRT.Sampler.Type)options.sampleType);
            sampler.SetParameter("pixelsamples", options.pixelsamples);

            var integrator = pbrtScene.Integrator((PBRT.Integrator.Type)options.integratorType);
            integrator.SetParameter("maxdepth", options.maxdepth);

            var film = pbrtScene.Film(PBRT.Film.Type.GBuffer);
            film.SetParameter("filename", newName + ".exr");
            film.SetParameter("xresolution", options.yresolution * 6);
            film.SetParameter("yresolution", options.yresolution);

            pbrtScene.Save(newFilePath);

            using (StreamWriter fileWriter = new StreamWriter(newFilePath, true))
            {
                fileWriter.WriteLine("##################################################\n");
                for (int line = 0; line < fileContentList.Count; line++)
                {
                    string content = fileContentList[line];
                    fileWriter.WriteLine(content);
                }

                fileWriter.Flush();
                fileWriter.Close();
            }
        }

        return true;
    }

    public List<string> ParseFile(string pbrtFilePath, out float[] scale)
    {
        scale = null;
        bool findWorldBegin = false;
        List<string> fileContentList = new List<string>();

        using (StreamReader fileReader = new StreamReader(pbrtFilePath))
        {
            while (!fileReader.EndOfStream)
            {
                string content = fileReader.ReadLine();
                if (content.StartsWith("Scale "))
                {
                    string[] scaleInfo = content.Replace("Scale ", "").Split(' ');
                    if(scaleInfo.Length == 3)
                    {
                        scale = new float[3] { 0, 0, 0 };
                        for (int i = 0; i < 3; i++)
                        {
                            float.TryParse(scaleInfo[i], out scale[i]);
                        }
                    }
                }

                if (content.StartsWith("WorldBegin"))
                    findWorldBegin = true;

                if(findWorldBegin)
                    fileContentList.Add(content);
            }

            fileReader.Close();
        }
        return fileContentList;
    }

    public List<Vector3> SampleCameras()
    {
        List<Vector3> camSamples = new List<Vector3>();

        float padding_x = (options.boundMax.x - options.boundMin.x) / options.cameraDivide.x;
        float padding_y = (options.boundMax.y - options.boundMin.y) / options.cameraDivide.y;
        float padding_z = (options.boundMax.z - options.boundMin.z) / options.cameraDivide.z;

        for (int x = 0; x < options.cameraDivide.x; x++)
        {
            for (int y = 0; y < options.cameraDivide.y; y++)
            {
                for (int z = 0; z < options.cameraDivide.z; z++)
                {
                    float pos_x = options.boundMin.x + (x * padding_x) + (padding_x * 0.5f);
                    float pos_y = options.boundMin.y + (y * padding_y) + (padding_y * 0.5f);
                    float pos_z = options.boundMin.z + (z * padding_z) + (padding_z * 0.5f);

                    camSamples.Add(new Vector3(pos_x, pos_y, pos_z));
                }
            }
        }

        return camSamples;
    }

    //--------------------Utility--------------------
    public Vector3 UnityVector3ToPLYVector3(Vector3 unityVector) // for vector
    {
        return /* A* */ unityVector;
    }

    public Matrix4x4 UnityTransformToPLYMatrix4x4(Matrix4x4 m) // for object
    {
        Matrix4x4 mTranpose = m.transpose;

        return /* A * */ mTranpose /* * At */;
    }

    public Matrix4x4 UnityTransformToInversePLYMatrix4x4(Matrix4x4 m) // for camera
    {
        Matrix4x4 mInverse = m.inverse;
        Matrix4x4 mTranpose = mInverse.transpose;

        return /* A * */ mTranpose /* * At */;
    }

    public bool IsCameraTransformValid(Transform m)
    {
        return Mathf.Abs(m.localScale.x - 1.0f) < float.Epsilon && Mathf.Abs(m.localScale.y - 1.0f) < float.Epsilon && Mathf.Abs(m.localScale.z - 1.0f) < float.Epsilon;
    }

    public void ConvertPLYFile(Mesh mesh, string filename)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;
        Vector2[] uvs = mesh.uv;

        using (StreamWriter writer = new StreamWriter(filename + ".data"))
        {
            string str_p = "";
            string str_n = "";
            string str_i = "";
            string str_u = "";

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 p = UnityVector3ToPLYVector3(vertices[i]);
                str_p += string.Format("{0} {1} {2} ", p.x, p.y, p.z);
            }
            str_p = str_p.TrimEnd(' ');

            for (int i = 0; i < normals.Length; i++)
            {
                Vector3 n = UnityVector3ToPLYVector3(normals[i]);
                str_n += string.Format("{0} {1} {2} ", n.x, n.y, n.z);
            }
            str_n = str_n.TrimEnd(' ');

            for (int i = 0; i < triangles.Length; i++)
            {
                str_i += string.Format("{0} ", triangles[i]);
            }
            str_i = str_i.TrimEnd(' ');

            for (int i = 0; i < uvs.Length; i++)
            {
                Vector2 uv = uvs[i];
                str_u += string.Format("{0} {1} ", uv.x, uv.y);
            }
            str_u = str_u.TrimEnd(' ');

            writer.WriteLine("\n# Mesh data\n");

            writer.WriteLine("vertices " + vertices.Length);
            writer.WriteLine(str_p);

            writer.WriteLine("normals " + normals.Length);
            writer.WriteLine(str_n);

            writer.WriteLine("indices " + triangles.Length);
            writer.WriteLine(str_i);

            writer.WriteLine("uvs " + uvs.Length);
            writer.WriteLine(str_u);

            writer.Flush();
            writer.Close();
        }

        string arguments = string.Format("write --outfile={0}.ply {1}.data", filename, filename);

        try
        {
            System.Diagnostics.Process foo = new System.Diagnostics.Process();
            foo.StartInfo.FileName = Application.dataPath.Replace("Assets", "Tools/plytool.exe");
            foo.StartInfo.Arguments = arguments;
            foo.Start();
            foo.WaitForExit();

            if (File.Exists(filename + ".data"))
                File.Delete(filename + ".data");
        }
        catch(System.Exception e)
        {
            Debug.LogError("plytool execute failed! Error: " + e.Message);
        }
    }

    public static void CameraFocusRenderTarget(GameObject renderTarget)
    {
        if (renderTarget == null || UnityEngine.Camera.main == null)
            return;

        Collider collider = renderTarget.GetComponentInChildren<Collider>();
        if (collider == null)
        {
            Debug.LogError("There are no collider components on the render target!");
            return;
        }

        Bounds bounds = collider.bounds;

        Vector3 boundCenter = bounds.center;
        Vector3 boundA = bounds.min;
        Vector3 boundB = bounds.min + new Vector3(bounds.size.x, 0, 0);
        Vector3 boundC = bounds.max + new Vector3(0, -bounds.size.y, 0);
        Vector3 boundD = bounds.min + new Vector3(0, 0, bounds.size.z);

        Vector3 boundE = boundA + new Vector3(0, bounds.size.y, 0);
        Vector3 boundF = boundB + new Vector3(0, bounds.size.y, 0);
        Vector3 boundG = boundC + new Vector3(0, bounds.size.y, 0);
        Vector3 boundH = boundD + new Vector3(0, bounds.size.y, 0);

        UnityEngine.Camera mainCam = UnityEngine.Camera.main;
        mainCam.orthographicSize = 1.0f;
        mainCam.transform.LookAt(boundCenter);

        if (!mainCam.orthographic)
        {
            Debug.LogWarning("Focusing render target requires an orthogonal camera!");
            return;
        }

        Vector3 vsA = mainCam.WorldToViewportPoint(boundA);
        Vector3 vsB = mainCam.WorldToViewportPoint(boundB);
        Vector3 vsC = mainCam.WorldToViewportPoint(boundC);
        Vector3 vsD = mainCam.WorldToViewportPoint(boundD);
        Vector3 vsE = mainCam.WorldToViewportPoint(boundE);
        Vector3 vsF = mainCam.WorldToViewportPoint(boundF);
        Vector3 vsG = mainCam.WorldToViewportPoint(boundG);
        Vector3 vsH = mainCam.WorldToViewportPoint(boundH);

        Vector2 minMaxX;
        minMaxX.x = Mathf.Min(vsA.x, vsB.x, vsC.x, vsD.x, vsE.x, vsF.x, vsG.x, vsH.x);
        minMaxX.y = Mathf.Max(vsA.x, vsB.x, vsC.x, vsD.x, vsE.x, vsF.x, vsG.x, vsH.x);

        Vector2 minMaxY;
        minMaxY.x = Mathf.Min(vsA.y, vsB.y, vsC.y, vsD.y, vsE.y, vsF.y, vsG.y, vsH.y);
        minMaxY.y = Mathf.Max(vsA.y, vsB.y, vsC.y, vsD.y, vsE.y, vsF.y, vsG.y, vsH.y);

        if (minMaxX.x < 0 || minMaxY.x < 0)
        {
            float margin = Mathf.Min(minMaxX.x, minMaxY.x);
            mainCam.orthographicSize += (Mathf.Abs(margin) * 2.0f + 0.02f);
        }
        else
        {
            if (minMaxX.y > 1 || minMaxY.y > 1)
            {
                float margin = Mathf.Max(minMaxX.y - 1.0f, minMaxY.y - 1.0f);
                mainCam.orthographicSize += (Mathf.Abs(margin) * 2.0f + 0.02f);
            }
            else
            {
                float margin = Mathf.Min(minMaxX.x, minMaxY.x, 1.0f - minMaxX.y, 1.0f - minMaxY.y);
                mainCam.orthographicSize -= (Mathf.Abs(margin) * 2.0f - 0.02f);
            }
        }
    }

    [MenuItem("PBRT/Camera Focus Target")]
    public static void CameraFocusTarget()
    {
        GameObject target = Selection.activeGameObject;
        if(target != null)
            CameraFocusRenderTarget(target);
    }
}