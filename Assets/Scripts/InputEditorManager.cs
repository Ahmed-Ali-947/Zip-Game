using UnityEngine;

public class EditorInputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LevelEditorManager levelEditorManager;

    private GameInput inputActions;
    private bool wasMousePressed;

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
        HandleEditorInput();
    }

    private void HandleEditorInput()
    {
        HandleSave();
        HandleClear();
        HandleCluePlacement();
    }

    private void HandleSave()
    {
        if (inputActions.Editor.SaveLevel.WasPressedThisFrame())
        {
            levelEditorManager.SaveLevel();
        }
    }

    private void HandleClear()
    {
        if (inputActions.Editor.ClearLevel.WasPressedThisFrame())
        {
            levelEditorManager.ClearLevel();
        }
    }

    private void HandleCluePlacement()
    {
        bool clueHeld = inputActions.Editor.PlaceClue.IsPressed();

        bool mousePressed = inputActions.Player.Press.IsPressed();

        if (!clueHeld)
        {
            wasMousePressed = mousePressed;
            return;
        }

        Vector2 screenPos =
            inputActions.Player.PointerPosition.ReadValue<Vector2>();

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0f;

        Vector2Int coords = gridManager.WorldToGrid(worldPos);
        Cell hoveredCell = gridManager.GetCell(coords);

        if (mousePressed && !wasMousePressed)
        {
            if (hoveredCell != null)
            {
                levelEditorManager.AddClue(hoveredCell);
            }
        }

        wasMousePressed = mousePressed;
    }
}