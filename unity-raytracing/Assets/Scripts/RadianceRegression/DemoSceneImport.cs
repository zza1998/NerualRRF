using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public static class DemoSceneImport
{
    [MenuItem("Demo/Import demo scene")]
    public static void LoadRaytracingScene()
    {
        string path = EditorUtility.OpenFilePanel("open raytracing scene", "", "json");
        int pathIndex = path.LastIndexOf("\\");
        if (pathIndex < 0)
            pathIndex = path.LastIndexOf("/");
        string filePath = path.Substring(0, pathIndex + 1);
        int startSubIndex = path.LastIndexOf("Assets");
        path = path.Substring(startSubIndex);
        string json = File.ReadAllText(path);
        filePath = filePath.Substring(startSubIndex);
        JsonScene.Scene scene = JsonUtility.FromJson<JsonScene.Scene>(json);
        if (scene != null)
        {
            int startIndex = path.LastIndexOf("/");
            if (startIndex < 0)
                startIndex = path.LastIndexOf("\\");
            int endIndex = path.LastIndexOf(".") - 1;
            int subLength = endIndex - startIndex;
            string sceneName = path.Substring(startIndex + 1, subLength);
            Scene unityScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single); //SceneManager.CreateScene(sceneName);
            unityScene.name = sceneName;
            Dictionary<string, Texture> textures = new Dictionary<string, Texture>();
            List<Material> materials = new List<Material>();
            for (int i = 0; i < scene.materials.Length; ++i)
            {
                JsonScene.Material jsonMaterial = scene.materials[i];
                Material unityMaterial = new Material(Shader.Find(jsonMaterial.shaderName));
                unityMaterial.name = jsonMaterial.name;
                if (jsonMaterial.shaderName == "NerualGI")
                {
                    Vector3 baseColor = jsonMaterial.baseColor;
                    unityMaterial.SetVector("basecolor", new Vector4(baseColor.x, baseColor.y, baseColor.z, 1.0f));
                    unityMaterial.SetFloat("eta", jsonMaterial.eta);
                    unityMaterial.SetFloat("roughness", jsonMaterial.roughness);

                    if (jsonMaterial.albedoTexture.Length > 0)
                    {
                        Texture2D albedo = GetTexture(jsonMaterial.albedoTexture) as Texture2D;
                        if (albedo != null)
                            unityMaterial.SetTexture("mainTex", albedo);
                    }

                    if (jsonMaterial.bumpMap.Length > 0)
                    {
                        Texture2D bumpMap = GetTexture(jsonMaterial.bumpMap) as Texture2D;
                        if (bumpMap != null)
                            unityMaterial.SetTexture("normalMap", bumpMap);
                    }
                }

                materials.Add(unityMaterial);
            }

            Material GetMaterialByName(string name)
            {
                return materials.Find(a => a.name == name);
            }

            Material GetDefaultMaterial()
            {
                Material unityMaterial = new Material(Shader.Find("NerualGI"));
                unityMaterial.name = "defaultLambert";
                return unityMaterial;
            }

            Texture GetTexture(string textureFile)
            {
                Texture texture = null;
                if (!textures.TryGetValue(textureFile, out texture))
                {
                    string texturePath = filePath + textureFile;
                    texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture)) as Texture;
                    if (texture != null)
                        textures.Add(textureFile, texture);
                }
                return texture;
            }

            for (int i = 0; i < scene.entities.Length; ++i)
            {
                JsonScene.Entity entity = scene.entities[i];
                GameObject gameObject = null;//new GameObject(entity.name);

                Mesh mesh = null;

                if (entity.mesh.Length > 0)
                {
                    string meshPath = filePath + entity.mesh;
                    gameObject = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath(meshPath, typeof(GameObject)) as GameObject);
                }
                else if (entity.meshType.ToLower() == "cube")
                {
                    gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                }
                else if (entity.meshType.ToLower() == "sphere")
                {
                    gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                }
                else if (entity.meshType.ToLower() == "plane")
                {
                    gameObject = new GameObject();//GameObject.CreatePrimitive(PrimitiveType.Plane);
                    mesh = liangairan.raytracer.MeshUtil.GetMesh(liangairan.raytracer.MeshUtil.MeshType.Plane);
                }
                else if (entity.meshType.ToLower() == "quad")
                {
                    gameObject = new GameObject();//GameObject.CreatePrimitive(PrimitiveType.Quad);
                    mesh = liangairan.raytracer.MeshUtil.GetMesh(liangairan.raytracer.MeshUtil.MeshType.Quad);
                }
                else if (entity.meshType.ToLower() == "disk")
                {
                    gameObject = new GameObject();
                    mesh = liangairan.raytracer.MeshUtil.GetMesh(liangairan.raytracer.MeshUtil.MeshType.Disk);
                }
                else
                {
                    gameObject = new GameObject();
                    Debug.Log("unknown mesh type! " + entity.meshType.ToLower());
                }
                gameObject.name = entity.name;

                gameObject.transform.position = entity.position;
                gameObject.transform.localScale = entity.scale;
                gameObject.transform.eulerAngles = entity.rotation;

                MeshRenderer meshRenderer = null;
                if (mesh != null)
                {
                    meshRenderer = gameObject.GetComponent<MeshRenderer>();
                    if (meshRenderer == null)
                    {
                        meshRenderer = gameObject.AddComponent<MeshRenderer>();
                        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
                        if (mesh != null)
                            meshFilter.mesh = mesh;
                    }
                }
                else
                {
                    meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
                }

                if (meshRenderer != null)
                {
                    if (entity.emission != Vector3.zero || entity.power > 0)
                    {
                        meshRenderer.sharedMaterial = new Material(Shader.Find("RayTracing/AreaLight"));
                        if (entity.emission != Vector3.zero)
                            meshRenderer.sharedMaterial.SetFloat("_Intensity", entity.emission.x);
                        else
                        {
                            if (mesh != null)
                            {
                                //caculate the emission
                                float area = 0;
                                List<Vector3> meshVertices = new List<Vector3>();

                                mesh.GetVertices(meshVertices);
                                for (int sm = 0; sm < mesh.subMeshCount; ++sm)
                                {
                                    List<int> meshTriangles = new List<int>();
                                    mesh.GetTriangles(meshTriangles, sm);
                                    for (int t = 0; t < meshTriangles.Count; t += 3)
                                    {
                                        Vector3 p0 = gameObject.transform.TransformPoint(meshVertices[meshTriangles[t]]);
                                        Vector3 p1 = gameObject.transform.TransformPoint(meshVertices[meshTriangles[t + 1]]);
                                        Vector3 p2 = gameObject.transform.TransformPoint(meshVertices[meshTriangles[t + 2]]);
                                        float triangleArea = Vector3.Cross(p1 - p0, p2 - p0).magnitude * 0.5f;
                                        area += triangleArea;
                                    }
                                }

                                float radiance = entity.power / area;
                                entity.emission = new Vector3(radiance, radiance, radiance);
                                meshRenderer.sharedMaterial.SetFloat("_Intensity", entity.emission.x);
                            }
                        }
                    }
                    else
                    {
                        if (entity.material.Length > 0)
                            meshRenderer.sharedMaterial = GetMaterialByName(entity.material);
                        else
                        {
                            if (meshRenderer != null)
                                meshRenderer.sharedMaterial = GetDefaultMaterial();
                        }
                    }
                }

                if (entity.emission.magnitude > 0 || entity.power > 0)
                {
                    //arealight
                    Light light = gameObject.AddComponent<Light>();
                    light.type = LightType.Area;
                    gameObject.name += "_light";
                }
            }

            if (scene.envLight.envmap != null && scene.envLight.envmap.Length > 0)
            {
                Texture envtex = GetTexture(scene.envLight.envmap);
                if (envtex != null)
                {
                    if (scene.envLight.material.Length > 0)
                    {
                        Material skyBoxMaterial = AssetDatabase.LoadAssetAtPath(scene.envLight.material, typeof(Material)) as Material;
                        if (skyBoxMaterial != null)
                            RenderSettings.skybox = skyBoxMaterial;
                    }
                }
            }

            //create camera
            GameObject mainCamera = new GameObject("MainCamera");
            Camera camera = mainCamera.AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.transform.position = scene.jsonCamera.position;
            if (scene.jsonCamera.useLookAt)
            {
                camera.transform.LookAt(scene.jsonCamera.lookAt, scene.jsonCamera.up);
            }
            else
                camera.transform.eulerAngles = scene.jsonCamera.rotation;
            camera.fieldOfView = scene.jsonCamera.fov;
            camera.nearClipPlane = scene.jsonCamera.near;
            camera.farClipPlane = scene.jsonCamera.far;

            //Directional Light
            GameObject mainLight = new GameObject("Directional Light");
            mainLight.transform.localEulerAngles = new Vector3(50, -30, 0);
            mainLight.AddComponent<Light>().type = LightType.Directional;
        }
    }
}
