using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace gomoru.su.CopyComponentUtilities
{
    public static class BlendShapes
    {
        [MenuItem("CONTEXT/SkinnedMeshRenderer/Copy BlendShapes to Clipboard")]
        public static void Copy(MenuCommand menuCommand)
        {
            var target = menuCommand.context as SkinnedMeshRenderer;
            GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(target.GetBlendShapes().ToDictionary(x => x.Key, x => x.Value));
        }

        private static Dictionary<string, float> _buffer;


        [MenuItem("CONTEXT/SkinnedMeshRenderer/Paste BlendShapes from Clipboard")]
        public static void Paste(MenuCommand menuCommand)
        {
            var buffer = _buffer;
            var target = menuCommand.context as SkinnedMeshRenderer;
            var shapes = target.GetBlendShapes().Select((x, i) => (BlendSpape: x, Index: i)).ToDictionary(x => x.BlendSpape.Key, x => x.Index);
            foreach(var x in buffer)
            {
                if (shapes.TryGetValue(x.Key, out var index))
                {
                    target.SetBlendShapeWeight(index, x.Value);
                }
            }
        }

        [MenuItem("CONTEXT/SkinnedMeshRenderer/Paste BlendShapes from Clipboard", true)]
        public static bool CanPaste()
        {
            try
            {
                _buffer = JsonConvert.DeserializeObject<Dictionary<string, float>>(GUIUtility.systemCopyBuffer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static KeyValuePair<string, float>[] GetBlendShapes(this SkinnedMeshRenderer renderer)
        {
            var mesh = renderer.sharedMesh;
            if (mesh == null)
                return null;

            int count = mesh.blendShapeCount;
            var array = new KeyValuePair<string, float>[count];
            for(int i = 0; i < array.Length; i++)
            {
                var name = mesh.GetBlendShapeName(i);
                var value = renderer.GetBlendShapeWeight(i);
                array[i] = new KeyValuePair<string, float>(name, value);
            }
            return array;
        }
    }
}