using UnityEngine;

namespace SubmersedVR;

public static class MiscTweaks
{
    public static void BetterTextureQuality()
    {
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.masterTextureLimit = 0;
    }
}