﻿using System.Text.RegularExpressions;
using ScriptableObjects;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EditorScripts
{
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
            ConvertToUpperButton();

            if (GameDataInstance.board != null && GameDataInstance.columns > 0 && GameDataInstance.rows > 0)
                DrawBoardTable();

            GUILayout.BeginHorizontal();

            ClearBoardButton();
            FillUpWithRandomLettersButton();
            
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            _dataList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(GameDataInstance);
            }
        }

        private void OnEnable()
        {
            InitializeReordableList(ref _dataList, "searchingWords", "Searching Words");
        }

        private void DrawColumnsRowsInputFields()
        {
            var columnsTemp = GameDataInstance.columns;
            var rowsTemp = GameDataInstance.rows;

            GameDataInstance.columns = EditorGUILayout.IntField("Columns", GameDataInstance.columns);
            GameDataInstance.rows = EditorGUILayout.IntField("Rows", GameDataInstance.rows);

            if ((GameDataInstance.columns != columnsTemp || GameDataInstance.rows != rowsTemp) && GameDataInstance.columns > 0 && GameDataInstance.rows > 0)
                GameDataInstance.CreateNewBoard();
        }

        private void DrawBoardTable()
        {
            var tableStyle = new GUIStyle("box") { padding = new RectOffset(10, 10, 10, 10), margin = { left = 32 } };
            var headerColumnStyle = new GUIStyle { fixedWidth = 35 };
            var columnStyle = new GUIStyle { fixedWidth = 50 };
            var rowStyle = new GUIStyle { fixedHeight = 25, fixedWidth = 40, alignment = TextAnchor.MiddleCenter };

            var textFieldStyle = new GUIStyle
            {
                normal = { background = Texture2D.grayTexture, textColor = Color.white },
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
                        if (x < GameDataInstance.board.Length && y < GameDataInstance.board[x].row.Length) // Добавлены проверки
                        {
                            var character = (string)EditorGUILayout.TextArea(GameDataInstance.board[x].row[y], textFieldStyle);

                            if (character.Length > 1)
                            {
                                character = character.Substring(0, 1);
                            }

                            GameDataInstance.board[x].row[y] = character;
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Out of range");
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void InitializeReordableList(ref ReorderableList list, string propertyName, string listLabel)
        {
            list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), 
                true, true, true,
                true);

            list.drawHeaderCallback = (Rect rect) => 
                { EditorGUI.LabelField(rect, listLabel); };

            var reorderableList = list;
            
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => 
            {
                var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("word"), GUIContent.none); 
            };
        }

        private void ConvertToUpperButton()
        {
            if (GUILayout.Button("To Upper"))
            {
                for (int i = 0; i < GameDataInstance.columns; i++)
                {
                    for (int j = 0; j < GameDataInstance.rows; j++)
                    {
                        var errorCounter = Regex.Match(GameDataInstance.board[i].row[j],@"[a-z]").Length;

                        if (errorCounter > 0)
                        {
                            GameDataInstance.board[i].row[j] = GameDataInstance.board[i].row[j].ToUpper();
                        }
                    }
                }

                foreach (var searchWord in GameDataInstance.searchingWords)
                {
                    var errorCounter = Regex.Match(searchWord.word,@"[a-z]").Length;

                    if (errorCounter > 0) 
                        searchWord.word = searchWord.word.ToUpper();
                }
            }
        }

        private void ClearBoardButton()
        {
            if (GUILayout.Button("Clear Board"))
            {
                for (int i = 0; i < GameDataInstance.columns; i++)
                {
                    for (int j = 0; j < GameDataInstance.rows; j++)
                    {
                        GameDataInstance.board[i].row[j] = " ";
                    }
                }
            }
        }

        private void FillUpWithRandomLettersButton()
        {
            if (GUILayout.Button("Fill Up With Random"))
            {
                for (int i = 0; i < GameDataInstance.columns; i++)
                {
                    for (int j = 0; j < GameDataInstance.rows; j++)
                    {
                        int errorCount = Regex.Matches(GameDataInstance.board[i].row[j],@"[a-zA-Z]").Count;
                        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        int index = UnityEngine.Random.Range(0, letters.Length);

                        if (errorCount == 0)
                        {
                            GameDataInstance.board[i].row[j] = letters[index].ToString();
                        }
                    }
                }
            }
        }
    }
}