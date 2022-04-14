using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.DialogueSystem.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private Dialogue _selectedDialogue;

        private GUIStyle _nodeStyle;
        
        [MenuItem("Window/Dialogue Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<DialogueEditor>();
            window.titleContent = new GUIContent("Dialogue Editor");
            window.Show();
        }

        private void OnGUI()
        {
            if (_selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue selected");
                return;
            }

            EditorGUILayout.LabelField($"Dialogue name: {_selectedDialogue.name}");
            
            foreach (var dialogueNode in _selectedDialogue.Nodes)
            {
                OnGUINode(dialogueNode);
            }
        }

        private void OnGUINode(DialogueNode dialogueNode)
        {
            GUILayout.BeginArea(dialogueNode.Position, _nodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("ID:", EditorStyles.whiteLabel);
            var newID = EditorGUILayout.TextField(dialogueNode.ID, EditorStyles.textField);

            EditorGUILayout.LabelField("Text:", EditorStyles.whiteLabel);
            var newText = EditorGUILayout.TextField(dialogueNode.Text, EditorStyles.textField);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_selectedDialogue, "Modify dialogue node");
                dialogueNode.Text = newText;
                dialogueNode.ID = newID;
            }
            
            GUILayout.EndArea();
        }

        private void OnSelectionChange()
        {
            var dialog = Selection.activeObject as Dialogue;
            // allow switching only between Dialog ScriptableObjects
            if (dialog != null)
            {
                _selectedDialogue = dialog;
                Repaint();
            }
        }

        private void OnEnable()
        {
            _nodeStyle = new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load("node0") as Texture2D
                },
                padding = new RectOffset(15, 15, 15, 15),
                border = new RectOffset(10, 10, 10, 10),
            };
        }

        [OnOpenAsset]
        public static bool OnOpenDialogueAsset(int instanceID, int line)
        {
            var dialogueAsset = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogueAsset == null) return false;

            ShowWindow();
            return true;
        }
    }
}