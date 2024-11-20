using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorCustom.Attributes;
using DebugStuff;
using Game.Serialization.World;
using Universal.Core;
using static Game.DataBase.PolygonBlueprintGraphic;
using Game.DataBase;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

namespace Game.Environment
{
    public class BlueprintMaker : MonoBehaviour
    {
#if UNITY_EDITOR
        #region fields & properties
        private const float GROUND_OFFSET = BuildingCreator.GROUND_OFFSET;
        private const float WORKFLOW_TO_WORLD_SCALE = BuildingCreator.WORKFLOW_TO_WORLD_SCALE;
        private const float AREA_LIMIT = 20.5f;
        [SerializeField] private List<Transform> areaObjects = new();
        [SerializeField] private Vector2 templateToCreate;
        [SerializeField] private List<BlueprintZoneInfo> output;

        [Title("Remove")]
        [SerializeField] private Vector2 removeTemplate;
        [SerializeField][Range(-1, 1)] private float quarterX;
        [SerializeField][Range(-1, 1)] private float quarterY;
        #endregion fields & properties

        #region methods
        [Button(nameof(GetChilds))]
        private void GetChilds()
        {
            Undo.RecordObject(this, "Get child transform");
            int childCount = transform.childCount;
            areaObjects.Clear();
            for (int i = 0; i < childCount; ++i)
            {
                areaObjects.Add(transform.GetChild(i));
            }
        }
        [Button(nameof(FixChildsToGrid))]
        private void FixChildsToGrid()
        {
            if (areaObjects.Count == 0) return;
            Undo.RecordObject(this, "Fix childs to grid");
            Vector3 oldPos = areaObjects[areaObjects.Count - 1].localPosition;
            oldPos = FixPosition(oldPos);
            foreach (Transform child in areaObjects)
            {
                Vector3 pos = child.localPosition;
                pos = FixPosition(pos);
                if (!pos.x.Approximately(oldPos.x) && !pos.x.Approximately(oldPos.z))
                {
                    float difX = Mathf.Abs(pos.x - oldPos.x);
                    float difZ = Mathf.Abs(pos.z - oldPos.z);
                    if (difX > difZ)
                    {
                        pos.z = oldPos.z;
                    }
                    else
                    {
                        pos.x = oldPos.x;
                    }
                }
                child.localPosition = pos;
                oldPos = pos;
            }
        }
        private Vector3 FixPosition(Vector3 pos)
        {
            return new(FixFloat(pos.x), FixFloat(pos.y), FixFloat(pos.z));
        }
        private float FixFloat(float num)
        {
            return CustomMath.ConvertToFloat(num.ToString("F1"));
        }
        [Button(nameof(CreateZone))]
        private void CreateZone()
        {
            output.Clear();
            AddZone(PlacementType.Good);
        }
        private void AddZone(PlacementType placement)
        {
            List<Vector2> texturePoints = new();
            foreach (Transform child in areaObjects)
            {
                Vector3 pos = child.localPosition;
                texturePoints.Add(new(Mathf.RoundToInt(pos.x * WORKFLOW_TO_WORLD_SCALE), Mathf.RoundToInt(pos.z * WORKFLOW_TO_WORLD_SCALE)));
            }
            BlueprintZoneInfo zone = new(Vector3.zero, texturePoints, DataBase.BuildingFloor.F1_Flooring, placement);
            output.Add(zone);
        }
        private void FixTemplate()
        {
            templateToCreate = new(FixFloat(templateToCreate.x), FixFloat(templateToCreate.y));
            removeTemplate = new(FixFloat(removeTemplate.x), FixFloat(removeTemplate.y));
        }
        [Button(nameof(CreateTemplate))]
        private void CreateTemplate()
        {
            Undo.RecordObject(this, "Create blueprint zones by template");
            FixTemplate();
            if (areaObjects.Count != 4)
            {
                Debug.LogError("You must set area objects count to 4");
                return;
            }
            float templateX = templateToCreate.x / 2;
            float templateY = templateToCreate.y / 2;
            GetZoneFix(templateX, templateY, out float zoneFixX, out float zoneFixY);
            float x01 = -templateX - zoneFixX;
            float x23 = templateX - zoneFixX;
            float y12 = templateY - zoneFixY;
            float y03 = -templateY - zoneFixY;
            SetSquareCoords(x01, x23, y12, y03);
            CreateZone();
        }
        [Button(nameof(CreateInversedTemplate))]
        private void CreateInversedTemplate()
        {
            Undo.RecordObject(this, "Create blueprint zone by inversed template");
            FixTemplate();
            if (areaObjects.Count != 4)
            {
                Debug.LogError("You must set area objects count to 4");
                return;
            }
            float templateX = templateToCreate.x / 2;
            float templateY = templateToCreate.y / 2;
            GetZoneFix(templateX, templateY, out float zoneFixX, out float zoneFixY);
            float x01 = templateX - zoneFixX;
            float x23 = -templateX - zoneFixX;
            float y12 = -templateY - zoneFixY;
            float y03 = templateY - zoneFixY;

            float x01a = -AREA_LIMIT;
            float x23a = AREA_LIMIT;
            float y12a = AREA_LIMIT;
            float y03a = -AREA_LIMIT;
            
            output.Clear();
            SetSquareCoords(x01a, x23, y12a, y03a);
            AddZone(PlacementType.Bad);

            SetSquareCoords(x01, x23a, y12a, y03a);
            AddZone(PlacementType.Bad);

            SetSquareCoords(x01a, x23a, y12, y03a);
            AddZone(PlacementType.Bad);

            SetSquareCoords(x01a, x23a, y12a, y03);
            AddZone(PlacementType.Bad);
        }
        [Button(nameof(Remove))]
        private void Remove()
        {
            Undo.RecordObject(this, "Create blueprint zone by remove");
            FixTemplate();
            output.Clear();
            float mainTemplateX = templateToCreate.x / 2;
            float mainTemplateY = templateToCreate.y / 2;
            float removeTemplateX = removeTemplate.x;
            float removeTemplateY = removeTemplate.y;

            Vector2 quarterScale = new(quarterX, quarterY);
            Vector2 scale = new();
            scale.x = (quarterScale.x) switch
            {
                float i when i < 0 => -1,
                float i when i >= 0 => 1,
                _ => 0
            };
            scale.y = (quarterScale.y) switch
            {
                float i when i < 0 => -1,
                float i when i >= 0 => 1,
                _ => 0
            };

            GetZoneFix(mainTemplateX, mainTemplateY, out float mainZoneFixX, out float mainZoneFixY);
            mainZoneFixX *= scale.x;
            mainZoneFixY *= scale.y;
            mainTemplateX -= mainZoneFixX;
            mainTemplateY -= mainZoneFixY;
            float x01 = -removeTemplateX;
            float x23 = removeTemplateX;
            float y12 = removeTemplateY;
            float y03 = -removeTemplateY;

            x01 += quarterScale.x * mainTemplateX - scale.x * (scale.x * x01 + removeTemplateX) / 2;
            x23 += quarterScale.x * mainTemplateX - scale.x * (scale.x * x23 + removeTemplateX) / 2;
            y12 += quarterScale.y * mainTemplateY - scale.y * (scale.y * y12 + removeTemplateY) / 2;
            y03 += quarterScale.y * mainTemplateY - scale.y * (scale.y * y03 + removeTemplateY) / 2;
            SetSquareCoords(x01, x23, y12, y03);
            AddZone(PlacementType.Bad);
        }
        private void SetSquareCoords(float x01, float x23, float y12, float y03)
        {
            areaObjects[0].localPosition = new(x01, 0, y03);
            areaObjects[1].localPosition = new(x01, 0, y12);
            areaObjects[2].localPosition = new(x23, 0, y12);
            areaObjects[3].localPosition = new(x23, 0, y03);
        }
        private static void GetZoneFix(float templateX, float templateY, out float zoneFixX, out float zoneFixY)
        {
            GetZoneFixOne(templateX, out zoneFixX);
            GetZoneFixOne(templateY, out zoneFixY);
        }
        private static void GetZoneFixOne(float template, out float zoneFix)
        {
            zoneFix = 10 / WORKFLOW_TO_WORLD_SCALE;
            if (Mathf.RoundToInt((template - zoneFix) * WORKFLOW_TO_WORLD_SCALE) % 20 != 0)
                zoneFix = 0;
        }

        [Title("Debug")]
        [SerializeField] private bool doDebug = true;
        [SerializeField][DrawIf(nameof(doDebug), true)] private bool debugAlways = false;
        private float qx = 0;
        private float qy = 0;

        private void OnValidate()
        {
            bool remove = false;
            if (!qx.Approximately(quarterX))
            {
                qx = quarterX;
                remove = true;
            }
            if (!qy.Approximately(quarterY))
            {
                qy = quarterY;
                remove = true;
            }
            if (remove)
            {
                Remove();
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (!doDebug) return;
            if (debugAlways) return;
            DebugDraw();
        }
        private void OnDrawGizmos()
        {
            if (!doDebug) return;
            if (!debugAlways) return;
            DebugDraw();
        }

        private void DebugDraw()
        {
            if (areaObjects.Count == 0) return;
            try
            {
                Vector3 start = areaObjects[areaObjects.Count - 1].position;
                foreach (Transform child in areaObjects)
                {
                    Gizmos.DrawLine(start, child.position);
                    start = child.position;
                }
            }
            catch { }

        }

        #endregion methods
#endif //UNITY_EDITOR
    }
}