using UnityEngine;

public static class GameEvents
{
    public delegate void EnableSquareSelection();
    public delegate void DisableSquareSelection();
    public delegate void SelectSquare(Vector3 position);
    public delegate void CheckSquare(string letter, Vector3 squarePosition, int squareIndex);
    public delegate void ClearSelection();

    public static event EnableSquareSelection OnEnableSquareSelection;
    public static event DisableSquareSelection OnDisableSquareSelection;
    public static event SelectSquare OnSelectSquare;
    public static event CheckSquare OnCheckSquare;
    public static event ClearSelection OnClearSelection;

    public static void EnableSquareSelectionMethod()
    {
        OnEnableSquareSelection?.Invoke();
    }

    public static void DisableSquareSelectionMethod()
    {
        OnDisableSquareSelection?.Invoke();
    }

    public static void SelectSquareMethod(Vector3 position)
    {
        OnSelectSquare?.Invoke(position);
    }

    public static void CheckSquareMethod(string letter, Vector3 squarePosition, int squareIndex)
    {
        OnCheckSquare?.Invoke(letter, squarePosition, squareIndex);
    }

    public static void ClearSelectionMethod()
    {
        OnClearSelection?.Invoke();
    }
}