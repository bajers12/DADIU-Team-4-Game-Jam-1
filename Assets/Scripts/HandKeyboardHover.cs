// HandKeyboardHover.cs
using UnityEngine;
using UnityEngine.InputSystem; // new Input System

public class HandKeyboardHover : MonoBehaviour
{
    [SerializeField] private HandView handView;
    [SerializeField] private CardViewHoverSystem hoverSystem;

    [SerializeField] private float hoverY = -2f;           // where the big hover card sits
    [SerializeField] private bool startAtRightMost = true; // start selection on the right-most card
    [SerializeField] private bool wrapAround = true;       // cycle at ends with arrows

    private int selected = -1;
    private int lastCount = -1;

    private InputAction left;
    private InputAction right;

    private void OnEnable()
    {
        left  = new InputAction("Left",  binding: "<Keyboard>/leftArrow");
        right = new InputAction("Right", binding: "<Keyboard>/rightArrow");

        // optional extras
        left.AddBinding("<Keyboard>/a");
        right.AddBinding("<Keyboard>/d");
        left.AddBinding("<Gamepad>/dpad/left");
        right.AddBinding("<Gamepad>/dpad/right");

        left.Enable();
        right.Enable();
    }

    private void OnDisable()
    {
        left?.Disable(); right?.Disable();
        left?.Dispose(); right?.Dispose();
    }

    private void Start()
    {
        RefreshSelection(force:true);
    }

    private void Update()
    {
        if (handView.CardsCount != lastCount)
            RefreshSelection(force:false);

        int dir = 0;
        if (left.WasPressedThisFrame())  dir = -1;
        if (right.WasPressedThisFrame()) dir = +1;

        if (dir != 0) Step(dir);
    }

    private void RefreshSelection(bool force)
    {
        int count = handView.CardsCount;
        lastCount = count;

        if (count <= 0) { ClearSelection(); return; }

        int target = selected;
        if (force || target < 0 || target >= count)
            target = startAtRightMost ? count - 1 : 0;

        SetSelection(target);
    }

    private void Step(int dir)
    {
        int count = handView.CardsCount;
        if (count == 0) return;

        int next = selected + dir;
        next = wrapAround ? (next % count + count) % count : Mathf.Clamp(next, 0, count - 1);
        SetSelection(next);
    }

    private void SetSelection(int index)
    {
        if (selected >= 0 && selected < handView.CardsCount)
            handView.GetCard(selected).SetHovered(false);

        selected = index;
        var card = handView.GetCard(selected);
        card.SetHovered(true);

        // Show the big hover card at the same X, fixed Y
        Vector3 pos = card.transform.position;
        pos.y = hoverY;
        hoverSystem.Show(card.Card, pos);
    }

    private void ClearSelection()
    {
        if (selected >= 0 && selected < handView.CardsCount)
            handView.GetCard(selected).SetHovered(false);

        selected = -1;
        hoverSystem.Hide();
    }
}
