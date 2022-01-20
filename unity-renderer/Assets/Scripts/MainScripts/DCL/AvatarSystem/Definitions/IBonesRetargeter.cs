using System.Collections.Generic;
using UnityEngine;

namespace AvatarSystem
{
    public interface IBonesRetargeter
    {
        void Retarget(IEnumerable<SkinnedMeshRenderer> skinnedMeshRenderers, SkinnedMeshRenderer target);
        void Retarget(SkinnedMeshRenderer skinnedMeshRenderer, SkinnedMeshRenderer target);
    }
}