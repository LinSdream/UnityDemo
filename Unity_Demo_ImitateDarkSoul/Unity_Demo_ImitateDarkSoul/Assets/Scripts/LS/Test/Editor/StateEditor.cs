//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEditorInternal;
//using TestDemo;

//namespace LS.LsEditor
//{
//    [CustomEditor(typeof(Test.AI.States.State))]
//    public class StateEditor : Editor
//    {

//        Test.AI.States.State _state;
//        ReorderableList _actionsList;
//        ReorderableList _transitionList;

//        private void OnEnable()
//        {
//            _actionsList = new ReorderableList(serializedObject, serializedObject.FindProperty("Actions"));
//            var prop = serializedObject.FindProperty("Actions");

//            _actionsList.drawElementBackgroundCallback = (rect, index, isActive, isFocused) =>
//            {
//                if (Event.current.type == EventType.Repaint)
//                {
//                    EditorStyles.miniButton.Draw(rect, false, isActive, isFocused, false);
//                }
//            };

//            _actionsList.drawElementCallback = (rect, index, isActive, isFocused) =>
//              {
//                  var element = prop.GetArrayElementAtIndex(index);
//                  rect.height -= 4;
//                  rect.y += 2;
//                  EditorGUI.PropertyField(rect, element);
//              };

//            _actionsList.onAddCallback += (list) =>
//            {
//                prop.arraySize++;

//                list.index = prop.arraySize - 1;

//                var element = prop.GetArrayElementAtIndex(list.index);
//                element.stringValue = "Action" + list.index;
//            };

//            _actionsList.onReorderCallback = (list) =>
//            {
//                //元素更新
//                Debug.Log("onReorderCallback");
//            };

//        }

//        public override void OnInspectorGUI()
//        {
//            serializedObject.Update();
//            _actionsList.DoLayoutList();
//            serializedObject.ApplyModifiedProperties();
//        }

//    }

//    [CustomPropertyDrawer(typeof(Test.AI.States.State), true)]
//    public class ActionsDrawer : PropertyDrawer
//    {
//        private ReorderableList _list;

//        private ReorderableList GetReorderableList(SerializedProperty property)
//        {

//            var listProperty = property.FindPropertyRelative("Actions");

//            if (listProperty == null)
//                Debug.LogError("!!!!!!!!");
//            _list = new ReorderableList(property.serializedObject, listProperty, true, true, true, true);

//            //if (_list == null)
//            //{
//            _list = new ReorderableList(property.serializedObject, listProperty, true, true, true, true);

//            _list.drawHeaderCallback += delegate (Rect rect)
//            {
//                EditorGUI.LabelField(rect, property.displayName);
//            };

//            _list.drawElementCallback = delegate (Rect rect, int index, bool isActive, bool isFocused)
//            {
//                EditorGUI.PropertyField(rect, listProperty.GetArrayElementAtIndex(index), true);
//            };
//            //}

//            return _list;
//        }


//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            //var list = GetReorderableList(property);

//            //var listProperty = property.FindPropertyRelative("Actions");
//            //var height = 0f;
//            //for (var i = 0; i < listProperty.arraySize; i++)
//            //{
//            //    height = Mathf.Max(height, EditorGUI.GetPropertyHeight(listProperty.GetArrayElementAtIndex(i)));
//            //}

//            //list.elementHeight = height;
//            //list.DoList(position);
//        }
//    }
//}