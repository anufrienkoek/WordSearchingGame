using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class WordChecker : MonoBehaviour
{
    public GameData currentGameData;

    private string _word;
    private int _assignedPoints;
    private int _completedWords;
    private Ray _currentRay;

    private Vector3 _rayStartPosition;
    private readonly List<int> _correctSquareList = new List<int>();

    private readonly Dictionary<RayDirection, Ray> _rays = new Dictionary<RayDirection, Ray>();

    private enum RayDirection
    {
        Up, Down, Left, Right, DiagonalLeftUp, DiagonalLeftDown, DiagonalRightUp, DiagonalRightDown
    }

    private void Start()
    {
        _assignedPoints = 0;
        _completedWords = 0;
    }

    private void Update()
    {
        if (_assignedPoints > 0 && Application.isEditor)
        {
            foreach (var ray in _rays.Values)
            {
                Debug.DrawRay(ray.origin, ray.direction * 4);
            }
        }
    }

    private void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
        GameEvents.OnClearSelection += ClearSelection;
    }

    private void OnDisable()
    {
        GameEvents.OnCheckSquare -= SquareSelected;
        GameEvents.OnClearSelection -= ClearSelection;
    }

    private void SquareSelected(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (_assignedPoints == 0)
        {
            InitializeFirstSelection(letter, squarePosition, squareIndex);
        }
        else
        {
            ProcessSubsequentSelections(letter, squarePosition, squareIndex);
        }

        _assignedPoints++;
    }

    private void InitializeFirstSelection(string letter, Vector3 squarePosition, int squareIndex)
    {
        _rayStartPosition = squarePosition;
        _correctSquareList.Add(squareIndex);
        _word += letter;

        _rays[RayDirection.Up] = new Ray(squarePosition, Vector2.up);
        _rays[RayDirection.Down] = new Ray(squarePosition, Vector2.down);
        _rays[RayDirection.Left] = new Ray(squarePosition, Vector2.left);
        _rays[RayDirection.Right] = new Ray(squarePosition, Vector2.right);
        _rays[RayDirection.DiagonalLeftUp] = new Ray(squarePosition, new Vector2(-1f, 1f));
        _rays[RayDirection.DiagonalLeftDown] = new Ray(squarePosition, new Vector2(-1f, -1f));
        _rays[RayDirection.DiagonalRightUp] = new Ray(squarePosition, new Vector2(1f, 1f));
        _rays[RayDirection.DiagonalRightDown] = new Ray(squarePosition, new Vector2(1f, -1f));
    }

    private void ProcessSubsequentSelections(string letter, Vector3 squarePosition, int squareIndex)
    {
        _correctSquareList.Add(squareIndex);

        if (_assignedPoints == 1)
        {
            _currentRay = SelectRay(_rayStartPosition, squarePosition);
        }

        if (IsPointOnTheRay(_currentRay, squarePosition))
        {
            GameEvents.SelectSquareMethod(squarePosition);
            _word += letter;
            CheckWord();
        }
    }

    private void ClearSelection()
    {
        _assignedPoints = 0;
        _word = string.Empty;
        _correctSquareList.Clear();
    }

    private void CheckWord()
    {
        foreach (var searchingWord in currentGameData.selectedBoardData.searchingWords)
        {
            if (_word == searchingWord.word)
            {
                _word = string.Empty;
                return;
            }
        }
    }

    private bool IsPointOnTheRay(Ray currentRay, Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 100.0f);

        foreach (var hit in hits)
        {
            if (hit.transform.position == point)
                return true;
        }

        return false;
    }

    private Ray SelectRay(Vector2 firstPosition, Vector2 secondPosition)
    {
        var direction = (secondPosition - firstPosition).normalized;
        const float tolerance = 0.01f;

        foreach (var ray in _rays)
        {
            if (Vector2.Distance(direction, ray.Value.direction) < tolerance)
            {
                return ray.Value;
            }
        }

        return _rays[RayDirection.Down];
    }
}