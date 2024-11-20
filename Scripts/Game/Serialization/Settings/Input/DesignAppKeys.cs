using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Serialization.Settings.Input
{
    [System.Serializable]
    public class DesignAppKeys : KeysCollection
    {
        #region fields & properties
        public KeyCodeInfo MoveUpKey => moveUpKey;
        [SerializeField] private KeyCodeInfo moveUpKey = new(KeyCode.W, KeyCodeDescription.DesignMoveUp);
        public KeyCodeInfo MoveDownKey => moveDownKey;
        [SerializeField] private KeyCodeInfo moveDownKey = new(KeyCode.S, KeyCodeDescription.DesignMoveDown);
        public KeyCodeInfo MoveRightKey => moveRightKey;
        [SerializeField] private KeyCodeInfo moveRightKey = new(KeyCode.D, KeyCodeDescription.DesignMoveRight);
        public KeyCodeInfo MoveLeftKey => moveLeftKey;
        [SerializeField] private KeyCodeInfo moveLeftKey = new(KeyCode.A, KeyCodeDescription.DesignMoveLeft);
        public KeyCodeInfo RotateKey => rotateKey;
        [SerializeField] private KeyCodeInfo rotateKey = new(KeyCode.R, KeyCodeDescription.DesignRotate);
        public KeyCodeInfo DeselectKey => deselectKey;
        [SerializeField] private KeyCodeInfo deselectKey = new(KeyCode.Q, KeyCodeDescription.DesignDeselect);
        public KeyCodeInfo RemoveKey => removeKey;
        [SerializeField] private KeyCodeInfo removeKey = new(KeyCode.G, KeyCodeDescription.DesignRemove);
        public KeyCodeInfo DuplicateKey => duplicateKey;
        [SerializeField] private KeyCodeInfo duplicateKey = new(KeyCode.C, KeyCodeDescription.DesignDuplicate);
        public KeyCodeInfo FocusKey => focusKey;
        [SerializeField] private KeyCodeInfo focusKey = new(KeyCode.F, KeyCodeDescription.DesignFocus);
        public KeyCodeInfo UndoKey => undoKey;
        [SerializeField] private KeyCodeInfo undoKey = new(KeyCode.Z, KeyCodeDescription.DesignUndo);
        public KeyCodeInfo ActionKey => actionKey;
        [SerializeField] private KeyCodeInfo actionKey = new(KeyCode.LeftShift, KeyCodeDescription.DesignAction);
        public KeyCodeInfo SelectSquareKey => selectSquareKey;
        [SerializeField] private KeyCodeInfo selectSquareKey = new(KeyCode.Mouse1, KeyCodeDescription.DesignSelectSquare);
        #endregion fields & properties

        #region methods
        public override void ResetKeys()
        {
            moveUpKey.Key = KeyCode.W;
            moveDownKey.Key = KeyCode.S;
            moveRightKey.Key = KeyCode.D;
            moveLeftKey.Key = KeyCode.A;
            rotateKey.Key = KeyCode.R;
            deselectKey.Key = KeyCode.Q;
            removeKey.Key = KeyCode.G;
            duplicateKey.Key = KeyCode.C;
            focusKey.Key = KeyCode.F;
            undoKey.Key = KeyCode.Z;
            actionKey.Key = KeyCode.LeftShift;
            selectSquareKey.Key = KeyCode.Mouse1;
        }
        public override List<KeyCodeInfo> GetKeys()
        {
            List<KeyCodeInfo> list = new()
            {
                MoveUpKey,
                MoveDownKey,
                MoveRightKey,
                MoveLeftKey,
                RotateKey,
                DeselectKey,
                RemoveKey,
                DuplicateKey,
                FocusKey,
                UndoKey,
                ActionKey,
                SelectSquareKey,
            };

            return list;
        }
        #endregion methods
    }
}