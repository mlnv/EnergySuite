using UnityEditor;

namespace EnergySuite
{
    [InitializeOnLoad]
    public class ExecutionOrderManager : Editor
    {
        static ExecutionOrderManager()
        {
            // Get the name of the script we want to change it's execution order
            string scriptName = typeof(EnergySuiteBehaviour).Name;

            // Iterate through all scripts (Might be a better way to do this?)
            for (int i = 0; i < MonoImporter.GetAllRuntimeMonoScripts().Length; i++)
            {
                MonoScript monoScript = MonoImporter.GetAllRuntimeMonoScripts()[i];

                // If found our script
                if (monoScript.name == scriptName)
                {
                    // And it's not at the execution time we want already
                    // (Without this we will get stuck in an infinite loop)
                    if (MonoImporter.GetExecutionOrder(monoScript) != -100)
                    {
                        MonoImporter.SetExecutionOrder(monoScript, -100);
                    }
                    break;
                }
            }
        }
    }
}
