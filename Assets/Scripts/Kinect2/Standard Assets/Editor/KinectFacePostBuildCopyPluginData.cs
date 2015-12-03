using UnityEditor;
using UnityEditor.Callbacks;
using System;

namespace Kinect
{
    public static class KinectFacePostBuildCopyPluginData
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string path)
        {
            KinectCopyPluginDataHelper.CopyPluginData(target, path, "NuiDatabase");
        }
    }
}