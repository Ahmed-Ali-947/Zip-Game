using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PathManager pathManager;
    [SerializeField] private Camera mainCamera;

    private GameInput inputActions;

    private bool wasPressed;
    private bool isDragging;
    private Cell currentHoveredCell;

    private void Awake()
    {
        inputActions = new GameInput();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        bool isPressed = inputActions.Player.Press.IsPressed();

        Vector2 screenPos =
            inputActions.Player.PointerPosition.ReadValue<Vector2>();

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        Vector2Int coords = gridManager.WorldToGrid(worldPos);
        Cell hoveredCell = gridManager.GetCell(coords);

        // Press started
        if (isPressed && !wasPressed)
        {
            if (hoveredCell != null)
            {
                isDragging = pathManager.BeginPath(hoveredCell);
            }
        }

        // While dragging
        if (isPressed && isDragging)
        {
            if (hoveredCell != null && hoveredCell != currentHoveredCell)
            {
                currentHoveredCell = hoveredCell;
                pathManager.TryMove(hoveredCell);
            }
        }

        // Released
        if (!isPressed && wasPressed)
        {
            pathManager.EndPath();
            isDragging = false;
            currentHoveredCell = null;
        }

        wasPressed = isPressed;
    }
}