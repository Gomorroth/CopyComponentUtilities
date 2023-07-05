using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace gomoru.su.CopyComponentUtilities
{
    public static class PhysBone
    {
        [MenuItem("CONTEXT/VRCPhysBone/Copy Component to Clipboard")]
        public static void Copy(MenuCommand menuCommand)
        {
            var target = menuCommand.context as VRCPhysBone;
            var json = JsonUtility.ToJson(target);
            GUIUtility.systemCopyBuffer = json;
        }

        [MenuItem("CONTEXT/VRCPhysBone/Paste Component from Clipboard")]
        public static void Paste(MenuCommand menuCommand)
        {
            var target = menuCommand.context as VRCPhysBone;
            var json = GUIUtility.systemCopyBuffer;
            JsonUtility.FromJsonOverwrite(json, target);
        }

        [MenuItem("CONTEXT/VRCPhysBone/Paste Component from Clipboard", validate = true)]
        public static bool CanPaste()
        {
            var obj = new GameObject() { hideFlags = HideFlags.HideInHierarchy };
            try
            {
                var pb = obj.AddComponent<VRCPhysBone>();
                JsonUtility.FromJsonOverwrite(GUIUtility.systemCopyBuffer, pb);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                GameObject.DestroyImmediate(obj);
            }
        }
    }
}