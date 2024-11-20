using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Cameras
{
    public class TargetCameraLook : MonoBehaviour
    {
        #region fields & properties
        [SerializeField] private Transform followTarget;
        [SerializeField] private Transform lookAt;
        [Inject] private CinemachineCamerasController camerasController;
        #endregion fields & properties

        #region methods
        public void Look()
        {
            camerasController.FollowTarget = followTarget;
            camerasController.LookAt = lookAt;
            camerasController.ChangeCamera(CinemachineCamerasController.CameraType.InstantTargetLook);
        }
        #endregion methods
    }
}