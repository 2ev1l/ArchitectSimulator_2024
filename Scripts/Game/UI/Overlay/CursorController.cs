using UnityEngine;
using UnityEngine.Events;
using Game.Audio;
using Universal.Core;
using AudioType = Game.Audio.AudioType;
using Game.UI.Cursor;
using Universal.Events;
using Universal.Behaviour;
using Zenject;

namespace Game.UI.Overlay
{
    [System.Serializable]
    public class CursorController : RequestExecutor, ITickable
    {
        #region fields & properties
        /// <summary>
        /// <see cref="{T0}"/> changedValue;
        /// </summary>
        public static UnityAction<float> OnMouseWheelDirectionChanged;

        [SerializeField] private Texture2D cursorDefault;
        [SerializeField] private Texture2D cursorPoint;
        [SerializeField] private Texture2D cursorClicked;
        [SerializeField] private Vector2 cursorDefaultOffset;
        [SerializeField] private AudioClip onClickSound;
        [SerializeField] private LayerMask worldRaycastHitMask;

        public static CursorState CursorState { get; private set; }

        /// <summary>
        /// Represents direction based on screen resolution. 
        /// Use <see cref="Vector3.ClampMagnitude(Vector3, float)"/> if you want to use const value
        /// </summary>
        public static Vector3 MouseDirection { get; private set; } = Vector3.zero;
        public static float MouseWheelDirection { get; private set; } = 0;
        /// <summary>
        /// Represents position based on screen resolution
        /// </summary>
        public static Vector3 LastMousePointOnScreen { get; private set; } = Vector3.zero;
        /// <summary>
        /// Scaled <see cref="LastMousePointOnScreen"/> to (0,0)~(1,1)
        /// </summary>
        public static Vector3 LastMousePointOnScreenScaled { get; private set; } = Vector3.zero;
        public static Vector3 LastWorldPoint3D { get; private set; } = Vector3.zero;
        public static Ray LastRay3D { get; private set; } = new();
        public static RaycastHit LastRayHit3D { get; private set; } = new();
        private static Vector3 currentMousePosition3D = Vector3.zero;
        private static readonly string mouseWheelAxis = "Mouse ScrollWheel";

        private static Camera MainCamera
        {
            get
            {
                if (mainCamera == null)
                {
                    mainCamera = Camera.main;
                }
                return mainCamera;
            }
        }
        private static Camera mainCamera = null;
        #endregion fields & properties

        #region methods
        public override void Initialize()
        {
            base.Initialize();
            SetDefaultCursor();

            ScreenFade.OnBlackScreenFading += OnBlackScreenFading;
        }
        public override void Dispose()
        {
            base.Dispose();
            ScreenFade.OnBlackScreenFading -= OnBlackScreenFading;
        }

        private void OnBlackScreenFading(bool fadeUp)
        {
            if (!fadeUp) return;
            SetDefaultCursor();
        }
        public void SetDefaultCursor()
        {
            UnityEngine.Cursor.SetCursor(cursorDefault, cursorDefaultOffset, CursorMode.Auto);
            CursorState = CursorState.Normal;
        }
        public void SetPointCursor()
        {
            UnityEngine.Cursor.SetCursor(cursorPoint, cursorDefaultOffset, CursorMode.Auto);
            CursorState = CursorState.Point;
        }
        public void SetClickedCursor()
        {
            UnityEngine.Cursor.SetCursor(cursorClicked, cursorDefaultOffset, CursorMode.Auto);
            CursorState = CursorState.Hold;
        }

        public void DoClickSound() => AudioManager.PlayClip(onClickSound, AudioType.Sound);

        private void Update()
        {
            Vector3 inputMousePosition = Input.mousePosition;
            MouseDirection = inputMousePosition - LastMousePointOnScreen;
            LastMousePointOnScreen = inputMousePosition;
            LastMousePointOnScreenScaled = MainCamera.ScreenToViewportPoint(LastMousePointOnScreen);

            MouseWheelDirection = Input.GetAxisRaw(mouseWheelAxis);
            if (MouseWheelDirection != 0)
                OnMouseWheelDirectionChanged?.Invoke(MouseWheelDirection);

            currentMousePosition3D.x = inputMousePosition.x;
            currentMousePosition3D.y = inputMousePosition.y;
            currentMousePosition3D.z = MainCamera.nearClipPlane;
            LastWorldPoint3D = MainCamera.ScreenToWorldPoint(currentMousePosition3D);

            LastRay3D = MainCamera.ScreenPointToRay(currentMousePosition3D);

            if (!Physics.Raycast(CursorController.LastRay3D, out RaycastHit hit, Mathf.Infinity, worldRaycastHitMask, QueryTriggerInteraction.Ignore)) return;
            LastRayHit3D = hit;
        }

        public override bool TryExecuteRequest(ExecutableRequest request)
        {
            if (request is not CursorUIRequest cursorRequest) return false;
            if (cursorRequest.DoClickSound)
            {
                DoClickSound();
                return true;
            }

            switch (cursorRequest.SetState)
            {
                case CursorState.Normal: SetDefaultCursor(); break;
                case CursorState.Point: SetPointCursor(); break;
                case CursorState.Hold: SetClickedCursor(); break;
                default: throw new System.NotImplementedException("Cursor state " + cursorRequest.SetState);
            }

            cursorRequest.Close();
            return true;
        }

        public void Tick() => Update();
        #endregion methods
    }
}