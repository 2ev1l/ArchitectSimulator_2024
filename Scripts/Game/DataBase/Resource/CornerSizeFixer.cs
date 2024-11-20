using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorCustom.Attributes;
using DebugStuff;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

namespace Game.DataBase
{
    [RequireComponent(typeof(BlueprintResource))]
    public class CornerSizeFixer : MonoBehaviour
    {
#if UNITY_EDITOR
        #region fields & properties
        [SerializeField] private BlueprintResource bpResource;
        [SerializeField][Min(10)] private int connectWallSizeCm = 10;
        private Transform WallsParent => transform.GetChild(0).GetChild(0);
        #endregion fields & properties

        #region methods
        private RectTransform GetChild(int index) => WallsParent.GetChild(index) as RectTransform;
        private void SetAnchors(RectTransform rect, float A, float B, float C, float D)
        {
            rect.anchorMin = new(A, B);
            rect.anchorMax = new(C, D);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
        [Button(nameof(FixCorner))]
        private void FixCorner()
        {
            if (!CanFixSize(out float partSize, out ConstructionSubtype subtype)) return;
            if (subtype == ConstructionSubtype.CornerIn) FixCornerIn(partSize);
            if (subtype == ConstructionSubtype.CornerOut) FixCornerOut(partSize);
        }
        private bool CanFixSize(out float partSize, out ConstructionSubtype subtype)
        {
            Vector3Int sizeCm = bpResource.resourceReference.Data.Prefab.SizeCentimeters;
            partSize = 0;
            subtype = 0;
            if (!WallsParent.TryGetComponent(out BlueprintGraphicUnitState unit))
            {
                Debug.LogError("Wrong walls parent object");
                return false;
            }
            subtype = unit.ApplyableSubtype;
            if (subtype != ConstructionSubtype.CornerIn && subtype != ConstructionSubtype.CornerOut)
            {
                Debug.LogError("Can't modfiy object with this subtype");
                return false;
            }
            if (sizeCm.x != sizeCm.z)
            {
                Debug.LogError("Object width != length");
                return false;
            }
            float parts = (sizeCm.x / (float)connectWallSizeCm) * 2;
            partSize = 1f / parts;
            return true;
        }
        private void FixCornerIn(float partSize)
        {
            float A = 0;
            float B = 1;
            float C = 1 - partSize;
            float D = 1 - partSize * 2;
            RectTransform ACBB = GetChild(0);
            RectTransform CABC = GetChild(1);
            RectTransform ADDC = GetChild(2);
            RectTransform DACC = GetChild(3);
            RectTransform AADD = GetChild(4); //outline
            RectTransform ADDD = GetChild(7);
            RectTransform DADD = GetChild(8);
            Undo.RegisterCompleteObjectUndo(gameObject, "Fix corner In");
            SetAnchors(ACBB, A, C, B, B);
            SetAnchors(CABC, C, A, B, C);
            SetAnchors(ADDC, A, D, D, C);
            SetAnchors(DACC, D, A, C, C);
            SetAnchors(AADD, A, A, D * 1.01f, D * 1.01f);
            SetAnchors(ADDD, A, D, D, D);
            SetAnchors(DADD, D, A, D, D);
            EditorUtility.SetDirty(gameObject);
        }
        private void FixCornerOut(float partSize)
        {
            float A = 0;
            float B = 1;
            float C = partSize;
            float D = partSize * 2;
            RectTransform AABC = GetChild(0);
            RectTransform ACCB = GetChild(1);
            RectTransform DCBD = GetChild(2);
            RectTransform CCDB = GetChild(3);
            RectTransform DDBB = GetChild(4); //outline
            RectTransform DDBD = GetChild(7);
            RectTransform DDDB = GetChild(8);
            Undo.RegisterCompleteObjectUndo(gameObject, "Fix corner Out");
            SetAnchors(AABC, A, A, B, C);
            SetAnchors(ACCB, A, C, C, B);
            SetAnchors(DCBD, D, C, B, D);
            SetAnchors(CCDB, C, C, D, B);
            SetAnchors(DDBB, D / 1.01f, D / 1.01f, B, B);
            SetAnchors(DDBD, D, D, B, D);
            SetAnchors(DDDB, D, D, D, B);
            EditorUtility.SetDirty(gameObject);
        }
        private void OnValidate()
        {
            if (bpResource == null)
                bpResource = GetComponent<BlueprintResource>();
        }
        #endregion methods
#endif //UNITY_EDITOR
    }
}