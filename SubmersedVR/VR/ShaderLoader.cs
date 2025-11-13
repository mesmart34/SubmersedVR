using System.Collections.Generic;
using UnityEngine;

namespace SubmersedVR.VR;

public class ShaderLoader
{
    private static bool initialized;
    private static readonly Dictionary<string, Shader> Shaders = new();

    public static bool Initialize(string assetBundlePath)
    {
        return loadAllShadersFromAssetBundle(assetBundlePath);
    }

    public static Shader GetShader(string name)
    {
        Debug.Log("GetShader( " + name + " )");
        if (!initialized)
        {
            Debug.Log("GetShader called before Initializing.");
            return null;
        }
        if (!Shaders.ContainsKey(name))
        {
            Debug.Log("shaders dictionary does not contain shader: " + name);
            return null;
        }
        return Shaders[name];
    }

    private static bool loadAllShadersFromAssetBundle(string assetBundlePath)
    {
        var loadedAssetBundle = AssetBundle.LoadFromFile(assetBundlePath);
        Debug.Log("loadAllShadersFromAssetBundle called.");
        foreach (var a in loadedAssetBundle.LoadAllAssets())
        {
            Debug.Log($"{a.name}");
            Shaders[a.name] = a as Shader;
        }
        initialized = true;
        return true;
    }

}