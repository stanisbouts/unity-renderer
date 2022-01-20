using System.Collections.Generic;
using DCL.Helpers;
using UnityEngine;

namespace AvatarSystem
{
    public class BonesRetargeter : IBonesRetargeter
    {
        internal static readonly Dictionary<SkinnedMeshRenderer, Dictionary<string, Transform>> boneMapsCache = new Dictionary<SkinnedMeshRenderer, Dictionary<string, Transform>>();

        public void Retarget(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, SkinnedMeshRenderer target)
        {
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
            {
                Retarget(skinnedMeshRenderer, target);
            }
        }

        public void Retarget(SkinnedMeshRenderer skinnedMeshRenderer, SkinnedMeshRenderer target)
        {
            var bonesMap = GetBonesMap(target);

            Transform[] bones = skinnedMeshRenderer.bones;
            Transform[] newBones = new Transform[target.bones.Length];

            // Tengo que respetar indices de los huesos del wearable!
            Debug.Log($"Bones: {bones.Length} {skinnedMeshRenderer.transform.GetHierarchyPath()}");
            for ( int j = 0; j < newBones.Length; j++ )
            {
                if (bones[j] == null)
                {
                    newBones[j] = target.bones[j];
                    continue;
                }
                if (!bonesMap.TryGetValue(bones[j].name, out Transform bone))
                {
                    newBones[j] = target.bones[j];
                    continue;
                }

                newBones[j] = bone;
            }

            skinnedMeshRenderer.bones = newBones;
            skinnedMeshRenderer.rootBone = target.rootBone;
        }

        private Dictionary<string, Transform> GetBonesMap(SkinnedMeshRenderer skinnedMeshRenderer)
        {
            Debug.Log("Hey");
            if (boneMapsCache.TryGetValue(skinnedMeshRenderer, out Dictionary<string, Transform> bonesMap))
                return bonesMap;

            Debug.Log("Hey");
            bonesMap = new Dictionary<string, Transform>();
            Transform[] bones = skinnedMeshRenderer.bones;
            for ( int jj = 0; jj < bones.Length; jj++ )
            {
                bonesMap[bones[jj].name] = bones[jj];
            }
            boneMapsCache.Add(skinnedMeshRenderer, bonesMap);
            return bonesMap;
        }
    }
}