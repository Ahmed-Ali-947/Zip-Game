using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2Int Coordinates { get; private set; }

    [SerializeField] private SpriteRenderer pathRenderer;
    [SerializeField] private SpriteRenderer numberRenderer;
    private int number;
    private bool hasNumber;
    private bool visited;

    public bool Visited => visited;
    public int Number => number;
    public bool HasNumber => hasNumber;

    public SpriteRenderer PathRenderer => pathRenderer;
    public SpriteRenderer NumberRenderer => numberRenderer;

    public void Initialize(Vector2Int coords)
    {
        Coordinates = coords;

        number = 0;
        hasNumber = false;

        numberRenderer.sprite = null;
        pathRenderer.sprite = null;
    }

    public void SetVisited(bool value)
    {
        visited = value;
    }

    public void SetNumber(int value)
    {
        number = value;
        hasNumber = true;
    }

    public void SetNumber(int value, Sprite sprite)
    {
        number = value;
        hasNumber = true;

        numberRenderer.sprite = sprite;
    }

    public void ClearPath()
    {
        pathRenderer.sprite = null;
    }
}