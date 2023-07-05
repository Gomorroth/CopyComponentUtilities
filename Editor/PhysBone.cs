using BestHTTP.JSON;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Dynamics.PhysBone.Components;

namespace gomoru.su.CopyComponentUtilities
{
    public static class PhysBone
    {
        [MenuItem("CONTEXT/VRCPhysBone/Copy PhysBone to Clipboard")]
        public static void Copy(MenuCommand menuCommand)
        {
            var target = menuCommand.context as VRCPhysBone;
            var json = JsonUtility.ToJson(target);
            GUIUtility.systemCopyBuffer = json;
        }

        [MenuItem("CONTEXT/Component/Paste PhysBone As New from Clipboard")]
        public static void PasteAsNew(MenuCommand menuCommand) => Paste(GUIUtility.systemCopyBuffer, (menuCommand.context as Component).gameObject.AddComponent<VRCPhysBone>());

        [MenuItem("CONTEXT/VRCPhysBone/Paste PhysBone Values from Clipboard")]
        public static void PasteValues(MenuCommand menuCommand) => Paste(GUIUtility.systemCopyBuffer, menuCommand.context as VRCPhysBone);

        private static void Paste(string json, VRCPhysBone target) => JsonUtility.FromJsonOverwrite(json, target);

        [MenuItem("CONTEXT/Component/Paste PhysBone As New from Clipboard", validate = true)]
        public static bool CanPasteAsNew() => IsPhysBoneInClipboard();

        [MenuItem("CONTEXT/VRCPhysBone/Paste PhysBone Values from Clipboard", validate = true)]
        public static bool CanPasteValues() => IsPhysBoneInClipboard();

        private static bool IsPhysBoneInClipboard()
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