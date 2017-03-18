using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ObjectPoolGroup))]
public class ObjectPoolGroupInspector : Editor
{
    private ObjectPoolGroup poolGroup;

    public override void OnInspectorGUI()
    {
        poolGroup = (ObjectPoolGroup)target;
        int newSize = EditorGUILayout.IntField("Num of Pools", poolGroup.pools.Length);

        if (newSize != poolGroup.pools.Length)
        {
            Array.Resize(ref poolGroup.pools, newSize);
        }

        for (int i = 0; i < poolGroup.pools.Length; i++)
        {
            poolGroup.pools[i] = (ObjectPooler)EditorGUILayout.ObjectField(poolGroup.pools[i], typeof(ObjectPooler));
        }

        poolGroup.size = EditorGUILayout.IntField("Size of Pools", poolGroup.size);

        newSize = EditorGUILayout.IntField("Spreads", poolGroup.spreads.Length);

        if (newSize != poolGroup.spreads.Length)
        {
            Array.Resize(ref poolGroup.spreads, newSize);
        }

        for (int i = 0; i < poolGroup.spreads.Length; i++)
        {
            EditorGUILayout.LabelField(i.ToString(), EditorStyles.boldLabel);

            Array.Resize(ref poolGroup.spreads[i].distribution, poolGroup.pools.Length);

            for (int p = 0; p < poolGroup.pools.Length; p++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("", GUILayout.Width(20));
                EditorGUILayout.LabelField(poolGroup.pools[p].gameObject.name);
                poolGroup.spreads[i].distribution[p] = EditorGUILayout.IntSlider(poolGroup.spreads[i].distribution[p], 0, 100);
                GUILayout.EndHorizontal();
            }
        }

        poolGroup.currentSpread = EditorGUILayout.IntField("Current Spread", poolGroup.currentSpread);
    }
}