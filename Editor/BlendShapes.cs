using System;
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
            
            var json = JsonUtility.ToJson(new BlendShapesContainer() { BlendShapes = target.GetBlendShapes() });
            GUIUtility.systemCopyBuffer = json;
        }

        private static BlendShape[] _buffer;


        [MenuItem("CONTEXT/SkinnedMeshRenderer/Paste BlendShapes from Clipboard")]
        public static void Paste(MenuCommand menuCommand)
        {
            var buffer = _buffer;
            var target = menuCommand.context as SkinnedMeshRenderer;
            var shapes = target.GetBlendShapes().Select((x, i) => (BlendSpape: x, Index: i)).ToDictionary(x => x.BlendSpape.Name, x => x.Index);
            foreach(var x in buffer)
            {
                if (shapes.TryGetValue(x.Name, out var index))
                {
                    target.SetBlendShapeWeight(index, x.Weight);
                }
            }
        }

        [MenuItem("CONTEXT/SkinnedMeshRenderer/Paste BlendShapes from Clipboard", true)]
        public static bool CanPaste()
        {
            try
            {
                var container = JsonUtility.FromJson<BlendShapesContainer>(GUIUtility.systemCopyBuffer);
                _buffer = container.BlendShapes;
                return true;
            }
            catch
            {
                return false;
            }
        }


        [Serializable]
        private struct BlendShapesContainer
        {
            public BlendShape[] BlendShapes;
        }

        [Serializable]
        private struct BlendShape
        {
            [SerializeField] public string Name;
            [SerializeField] public float Weight;

            public BlendShape(string key, float value)
            {
                Name = key;
                Weight = value;
            }
        }

        private static BlendShape[] GetBlendShapes(this SkinnedMeshRenderer renderer)
        {
            var mesh = renderer.sharedMesh;
            if (mesh == null)
                return null;

            int count = mesh.blendShapeCount;
            var array = new BlendShape[count];
            for(int i = 0; i < array.Length; i++)
            {
                var name = mesh.GetBlendShapeName(i);
                var value = renderer.GetBlendShapeWeight(i);
                array[i] = new BlendShape(name, value);
            }
            return array;
        }
    }
}