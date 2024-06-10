using System;
using ScriptableObjects;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

    [CustomEditor(typeof(BoardData), false)]
    [CanEditMultipleObjects]
    [System.Serializable]
    public class BoardDataDrawer : UnityEditor.Editor
    {
        private BoardData GameDataInstance => target as BoardData;
        private ReorderableList _dataList;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawColumnsRowsInputFields();
            EditorGUILayout.Space();

            if (GameDataInstance.board != null && GameDataInstance.columns > 0 && GameDataInstance.rows > 0)
                DrawBoardTable();

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(GameDataInstance);
            }
        }

        private void OnEnable()
        {

        }

        private void DrawColumnsRowsInputFields()
        {
            var columnsTemp = GameDataInstance.columns;
            var rowsTemp = GameDataInstance.columns;

            GameDataInstance.columns = EditorGUILayout.IntField("Columns", GameDataInstance.columns);
            GameDataInstance.rows = EditorGUILayout.IntField("Rows", GameDataInstance.rows);

            if ((GameDataInstance.columns != columnsTemp || GameDataInstance.rows != rowsTemp) && GameDataInstance.columns > 0 && GameDataInstance.rows > 0)
                GameDataInstance.CreateNewBoard();
        }

        private void DrawBoardTable()
        {
            var tableStyle = new GUIStyle("box") {padding = new RectOffset(10, 10, 10, 10), margin = {left = 32}};
            var headerColumnStyle = new GUIStyle {fixedWidth = 35};
            var columnStyle = new GUIStyle {fixedWidth = 50};
            var rowStyle = new GUIStyle {fixedHeight = 25, fixedWidth = 40, alignment = TextAnchor.MiddleCenter};

            var textFieldStyle = new GUIStyle
            {
                normal = {background = Texture2D.grayTexture, textColor = Color.white},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            EditorGUILayout.BeginHorizontal(tableStyle);

            for (int x = 0; x < GameDataInstance.columns; x++)
            {
                EditorGUILayout.BeginVertical(x == -1 ? headerColumnStyle : columnStyle);

                for (int y = 0; y < GameDataInstance.rows; y++)
                {
                    if (x >= 0 && y >= 0)
                    {
                        EditorGUILayout.BeginHorizontal(rowStyle);
                        var character =
                            (string) EditorGUILayout.TextArea(GameDataInstance.board[x].row[y], textFieldStyle);

                        if (GameDataInstance.board[x].row[y].Length > 1)
                        {
                            character = GameDataInstance.board[x].row[y].Substring(0, 1);
                        }

                        GameDataInstance.board[x].row[y] = character;
                        EditorGUILayout.EndHorizontal();
                    }
                }
                
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndVertical();
        }
    }