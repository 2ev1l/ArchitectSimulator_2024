using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Core;

namespace Game.UI.Overlay
{
    public class Collider2DPhysicsFix : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private Camera mainCamera;
        #endregion fields & properties

        #region methods
        private void OnEnable()
        {
            FixCamera();
        }
        private void FixCamera()
        {
            float approximately = 5f;
            Transform cameraTransform = mainCamera.transform;
            Vector3 eulerAngles = cameraTransform.eulerAngles;
            if (eulerAngles.x.Approximately(90, approximately) || eulerAngles.x.Approximately(270, approximately))
            {
                eulerAngles.x += approximately + 3;
            }
            if (eulerAngles.y.Approximately(90, approximately) || eulerAngles.y.Approximately(270, approximately))
            {
                eulerAngles.y += approximately + 3;
            }
            eulerAngles.x = (int)eulerAngles.x;
            eulerAngles.y = (int)eulerAngles.y;
            eulerAngles.z = 0;
            cameraTransform.eulerAngles = eulerAngles;
        }
        #endregion methods
    }
}