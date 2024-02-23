using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XMLResourceTree
{
    public class ToolAppBuildPipeline
    {
        //[InitializeOnLoadMethod]
        //static void Init()
        //{
        //    // Register the build handler
        //    BuildPlayerWindow.RegisterBuildPlayerHandler(BuildPlayerHandler);
        //}

        //static void BuildPlayerHandler(BuildPlayerOptions options)
        //{
        //    // Add your custom build logic here
        //    // For example, you can check the scenes and build based on conditions
        //    if (options.scenes.Length > 0 && options.scenes[0].ToLower().Contains("originalscene"))
        //    {
        //        // Build original application
        //        BuildPipeline.BuildPlayer(options.scenes, "OriginalApp.exe", options.target, options.options);
        //    }
        //    else if (options.scenes.Length > 0 && options.scenes[0].ToLower().Contains("anotherscene"))
        //    {
        //        // Build another application
        //        BuildPipeline.BuildPlayer(options.scenes, "AnotherApp.exe", options.target, options.options);
        //    }
        //    // Call the default build method to proceed with Unity's regular build process
        //    BuildPlayerWindow.DefaultBuildMethods.BuildPlayer(options);
        //}
    }
}