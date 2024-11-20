using EditorCustom.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DebugStuff
{
    public class MaterialReplacer : MonoBehaviour
    {
#if UNITY_EDITOR

        #region fields & properties
        [SerializeField] private List<Material> newMaterials = new();
        [SerializeField][DrawIf(nameof(replaceByName), false, DisablingType.ReadOnly)] private List<Material> oldMaterials = new();
        [SerializeField] private bool replaceChilds = true;
        [SerializeField][DrawIf(nameof(replaceAll), false, DisablingType.ReadOnly)] private bool replaceByName = true;
        [SerializeField][DrawIf(nameof(replaceByName), false, DisablingType.ReadOnly)] private bool replaceAll = false;
        #endregion fields & properties

        #region methods
        [Button(nameof(ReplaceMaterials))]
        private void ReplaceMaterials()
        {
            if (TryGetComponent(out Renderer mainRender))
            {
                Replace(mainRender);
            }
            if (replaceChilds)
            {
                var renders = transform.GetComponentsInChildren<Renderer>();
                foreach (var el in renders)
                {
                    Replace(el);
                }
            }
        }
        private void Replace(Renderer render)
        {
            Material[] materials = render.sharedMaterials;
            for (int c = 0; c < materials.Length; ++c)
            {
                Material material = materials[c];
                for (int i = 0; i < newMaterials.Count; ++i)
                {
                    Material newMaterial = newMaterials[i];
                    if (replaceByName)
                    {
                        if (material.name.Equals(newMaterial.name))
                        {
                            materials[c] = newMaterial;
                            break;
                        }
                        continue;
                    }
                    if (replaceAll)
                    {
                        materials[c] = newMaterial;
                        continue;
                    }
                    if (oldMaterials.Count <= i) break;
                    if (oldMaterials[i] == material)
                    {
                        materials[c] = newMaterial;
                        break;
                    }
                }
            }
            render.sharedMaterials = materials;
        }
        #endregion methods

#endif //UNITY_EDITOR
    }
}