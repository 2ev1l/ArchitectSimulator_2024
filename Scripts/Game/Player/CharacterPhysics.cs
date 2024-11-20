using UnityEngine;
using EditorCustom.Attributes;
using DebugStuff;

namespace Game.Player
{
    public class CharacterPhysics : MonoBehaviour
    {
        #region fields & properties
        public float GravityConstant => Physics.gravity.y * GravityScale;

        [SerializeField] private CharacterController characterController;
        public float GravityScale
        {
            get => gravityScale;
            set => gravityScale = value;
        }
        [SerializeField] private float gravityScale = 1f;

        [Title("Read Only")]
        [SerializeField][ReadOnly] private Vector3 nextVelocity;
        private static readonly Vector3 deltaMoveFix = new(0.001f, 0f, 0.001f);
        public float CurrentSpeed => GetCurrentSpeed();
        public float FlyingSpeed => GetFlyingSpeed();
        #endregion fields & properties

        #region methods
        private void Awake()
        {
            characterController.detectCollisions = true;
        }
        private void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;
            if (!characterController.enabled) return;
            characterController.Move(deltaMoveFix * deltaTime);
            characterController.Move(-deltaMoveFix * deltaTime);

            nextVelocity.y += GravityConstant * deltaTime;
            characterController.Move(nextVelocity * deltaTime);

            if (characterController.isGrounded)
                nextVelocity.y = 0;
        }
        public void AddVerticalForce(float value)
        {
            nextVelocity.y += value;
        }
        private float GetCurrentSpeed()
        {
            Vector2 flatVelocity = new(characterController.velocity.x, characterController.velocity.z);
            return flatVelocity.magnitude;
        }
        private float GetFlyingSpeed()
        {
            return characterController.velocity.y;
        }

        #endregion methods
#if UNITY_EDITOR
        [Button(nameof(CreatePosition))]
        private void CreatePosition()
        {
            GameObject pos = new GameObject("Player Position");
            pos.transform.position = transform.position;
        }
#endif //UNITY_EDITOR
    }
}