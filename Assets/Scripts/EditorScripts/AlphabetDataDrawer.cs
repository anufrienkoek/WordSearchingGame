using System;
using JetBrains.Annotations;
using ScriptableObjects;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EditorScripts
{
    [CustomEditor(typeof(AlphabetData))]
    [CanEditMultipleObjects]
    [System.Serializable]
    public class AlphabetDataDrawer : Editor
    {
        private ReorderableList _alphabetPlainList;
        private ReorderableList _alphabetNormalList;
        private ReorderableList _alphabetHighLightedList;
        private ReorderableList _alphabetWrongList;

        private void OnEnable()
        {
            InitializeReordableList(ref _alphabetPlainList, "alphabetPlain", "Alphabet Plain");
            InitializeReordableList(ref _alphabetNormalList, "alphabetNormal", "Alphabet Normal");
            InitializeReordableList(ref _alphabetHighLightedList, "alphabetHighLighted", "Alphabet HighLighted");
            InitializeReordableList(ref _alphabetWrongList, "alphabetWrong", "Alphabet Wrong");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            _alphabetPlainList.DoLayoutList();
            _alphabetNormalList.DoLayoutList();
            _alphabetHighLightedList.DoLayoutList();
            _alphabetWrongList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void InitializeReordableList([NotNull] ref ReorderableList list, string propertyName, string listLabel)
        {
            list = new ReorderableList(serializedObject,serializedObject.FindProperty(propertyName),
                true,true,true,true);

            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect,listLabel);
            };

            var l = list;

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = l.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("letter"), GUIContent.none);

                EditorGUI.PropertyField(
                new Rect(rect.x + 70,rect.y,rect.width - 60 -30,EditorGUIUtility.singleLineHeight),element.FindPropertyRelative("image"),GUIContent.none );
            };
        }
    }
}