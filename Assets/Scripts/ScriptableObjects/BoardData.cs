using System;
using UnityEngine;

namespace ScriptableObjects
{
    [System.Serializable]
    [CreateAssetMenu]
    public class BoardData : ScriptableObject
    {
        [System.Serializable]
        public class SearchingWord
        {
            public string word;
        }
        
        [System.Serializable]
        public class BoardRow
        {
            public int size;
            public string[] row;
            
            public BoardRow() {}

            public BoardRow(int size)
            {
                CreateRow(size);
            }

            private void CreateRow(int rowSize)
            {
                size = rowSize;
                row = new string[size];
                ClearRow();
            }

            public void ClearRow()
            {
                for (int i = 0; i < size; i++)
                {
                    row[i] = " ";
                }
            }
        }

        public float timeInSeconds;
        public int columns;
        public int rows;

        public BoardRow[] board;

        public void ClearWithEmptyString()
        {
            for (int i = 0; i < columns; i++)
            {
                board[i].ClearRow();
            }
        }

        public void CreateNewBoard()
        {
            board = new BoardRow[columns];

            for (int i = 0; i < columns; i++)
            {
                board[i] = new BoardRow(rows);
            }
        }
    }
}
