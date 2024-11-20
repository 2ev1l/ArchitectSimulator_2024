using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EditorCustom.Attributes;
using Universal.Core;
using Game.Animation;
using Universal.Time;
using Zenject;

namespace Game.Player
{
    public class Moving : MonoBehaviour
    {
        #region fields & properties
        public UnityAction OnMoved;
        /// <summary>
        /// <see cref="{T0}"/> isMoved
        /// </summary>
        public UnityAction<bool> AfterPossibleMove;

        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera moveCamera;
        public float DefaultMoveSpeed => moveSpeed;
        [Title("Settings")]
        [SerializeField][Range(0, 10)] private float moveSpeed = 1f;
        public float AccelerateMultiplier => accelerateMultiplier;
        [SerializeField][Range(1, 3)] private float accelerateMultiplier = 1.5f;
        public float SlowMultiplier => slowMultiplier;
        [SerializeField][Range(0.1f, 1f)] private float slowMultiplier = 0.5f;

        [SerializeField] private AnimationCurve moveHoldDependenceCurve = ValueTimeChanger.DefaultCurve;

        public bool ResetInputAtEachFrame => resetInputAtEachFrame;
        [SerializeField] private bool resetInputAtEachFrame = true;
        public bool IsAlwaysMovingWithCamera => isAlwaysMovingWithCamera;
        [SerializeField] private bool isAlwaysMovingWithCamera = false;

        /// <summary>
        /// Accelerate at the end of frame. Resets every frame if <see cref="resetInputAtEachFrame"/>
        /// </summary>
        public bool DoAccelerate
        {
            get => doAccelerate;
            set
            {
                doAccelerate = value;
            }
        }
        private bool doAccelerate = false;
        /// <summary>
        /// Slow at the end of frame. Doesn't reset every frame
        /// </summary>
        public bool DoSlow
        {
            get => doSlow;
            set
            {
                doSlow = value;
            }
        }
        private bool doSlow = false;

        public bool LastFrameMoved => lastFrameMoved;
        private bool lastFrameMoved = false;
        public bool LastFrameAccelerate => lastFrameAccelerate;
        private bool lastFrameAccelerate = false;

        [Title("Read Only")]
        [SerializeField][ReadOnly] private Vector3 surfaceNormal = Vector3.up;
        public float FinalSpeed => moveSpeed * (DoAccelerate ? accelerateMultiplier : 1) * (DoSlow ? slowMultiplier : 1) * moveHoldDependenceCurve.Evaluate(moveHoldTime);
        public Vector2 LastInput => lastInput;
        [SerializeField][ReadOnly] private Vector2 lastInput = Vector2.zero;
        [SerializeField][ReadOnly] private float moveHoldTime = 1f;
        #endregion fields & properties

        #region methods
        private Vector3 GetNormalizedDirection(Vector3 moveVector)
        {
            moveVector = CustomMath.Project(moveVector, surfaceNormal);
            return moveVector.normalized;
        }
        public void SetDefaultMoveSpeed(float value)
        {
            moveSpeed = value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveVector">x = forward; y = right</param>
        public void SetInputMove(Vector2 moveVector)
        {
            lastInput = moveVector;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveVector">x = forward; y = right</param>
        public void AddInputMove(Vector2 moveVector)
        {
            lastInput += moveVector;
        }
        private void LateUpdate()
        {
            bool isMoved = TryMove();
            AfterPossibleMove?.Invoke(isMoved);

            lastFrameMoved = isMoved;
            lastFrameAccelerate = DoAccelerate;

            if (resetInputAtEachFrame)
                ResetLastInput();

            if (isMoved) moveHoldTime += Time.deltaTime;
            else moveHoldTime = 0;
            moveHoldTime = Mathf.Clamp01(moveHoldTime);
        }
        private bool TryMove()
        {
            if (lastInput.Equals(Vector2.zero)) return false;
            DoMove(isAlwaysMovingWithCamera ? moveCamera.transform : characterController.transform);
            return true;
        }

        public void CalculateMoveVector(Vector2 lastInput, Transform relativeDirection, out Vector2 clampedInput, out Vector3 moveVector)
        {
            clampedInput = Vector2.ClampMagnitude(lastInput, 1);
            moveVector = GetMoveVector(lastInput, relativeDirection);
            moveVector = GetNormalizedDirection(moveVector);
        }
        private void DoMove(Transform relativeDirection)
        {
            CalculateMoveVector(lastInput, relativeDirection, out lastInput, out Vector3 moveVector);
            Vector3 offset = FinalSpeed * Time.deltaTime * moveVector;
            characterController.Move(offset);
            OnMoved?.Invoke();
        }
        private void ResetLastInput()
        {
            lastInput = Vector2.zero;
            DoAccelerate = false;
        }


        /// <summary>
        /// Teleports safely to open location. <br></br>
        /// Camera fixes automatically
        /// </summary>
        /// <param name="position"></param>
        public void TeleportTo(Vector3 position)
        {
            characterController.Move(position - characterController.transform.position);
            moveCamera.transform.position = position + Vector3.up * 0.66f;
        }
        /// <summary>
        /// Teleports safely ignoring layers. Triggers may be activated during move. <br></br>
        /// Camera fixes automatically
        /// </summary>
        public void TeleportToIgnoreLayer(Vector3 position, LayerMask ignoreLayers)
        {
            LayerMask oldLayers = characterController.excludeLayers;
            int oldPriority = characterController.layerOverridePriority;

            characterController.excludeLayers = ignoreLayers;
            characterController.layerOverridePriority = 100;

            TeleportTo(position);

            characterController.layerOverridePriority = oldPriority;
            characterController.excludeLayers = oldLayers;
        }
        /// <summary>
        /// May cause bugs inside the triggers
        /// </summary>
        /// <param name="position"></param>
        public void TeleportToUnsafe(Vector3 position)
        {
            bool lastCharacterControllerState = characterController.enabled;
            characterController.enabled = false;
            if (characterController.transform.parent != null) characterController.transform.SetParent(null);
            characterController.transform.position = position;
            characterController.enabled = lastCharacterControllerState;
        }
        private Vector3 GetMoveVector(Vector2 inputVector, Transform relativeDirection)
        {
            Vector3 result = Vector3.zero;

            result += relativeDirection.forward * inputVector.x;
            result += relativeDirection.right * inputVector.y;

            result = Vector3.ClampMagnitude(result, 1);
            return result;
        }
        #endregion methods
    }
}