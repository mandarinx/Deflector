using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor {

	static public TransformInspector instance;

	SerializedProperty mPos;
	SerializedProperty mRot;
	SerializedProperty mScale;

	[MenuItem("CONTEXT/Transform/Copy position")]
	private static void CopyPosition() {
		EditorGUIUtility.systemCopyBuffer = instance.mPos.vector3Value.x.ToString("R") + "f, "+
											instance.mPos.vector3Value.y.ToString("R") + "f, "+
											instance.mPos.vector3Value.z.ToString("R") + "f";
	}

	void OnEnable() {
		instance = this;

		mPos = serializedObject.FindProperty("m_LocalPosition");
		mRot = serializedObject.FindProperty("m_LocalRotation");
		mScale = serializedObject.FindProperty("m_LocalScale");
	}

	void OnDestroy() { instance = null; }

	public override void OnInspectorGUI() {
		EditorGUIUtility.labelWidth = 15.0f;

		serializedObject.Update();

		bool widgets = false;
		DrawPosition();
		DrawRotation(widgets);
		DrawScale(widgets);

		serializedObject.ApplyModifiedProperties();
	}

	void DrawPosition() {
		GUILayout.BeginHorizontal();
		{
			bool reset = GUILayout.Button("P", GUILayout.Width(20f),
			                              	   GUILayout.Height(14f));

			EditorGUILayout.PropertyField(mPos.FindPropertyRelative("x"));
			EditorGUILayout.PropertyField(mPos.FindPropertyRelative("y"));
			EditorGUILayout.PropertyField(mPos.FindPropertyRelative("z"));

			if (reset) mPos.vector3Value = Vector3.zero;
		}
		GUILayout.EndHorizontal();
	}

	void DrawScale (bool isWidget) {
		GUILayout.BeginHorizontal();
		{
			bool reset = GUILayout.Button("S", GUILayout.Width(20f),
			                              	   GUILayout.Height(14f));

			EditorGUILayout.PropertyField(mScale.FindPropertyRelative("x"));
			EditorGUILayout.PropertyField(mScale.FindPropertyRelative("y"));

			if (isWidget) GUI.color = new Color(0.7f, 0.7f, 0.7f);
			EditorGUILayout.PropertyField(mScale.FindPropertyRelative("z"));
			if (isWidget) GUI.color = Color.white;

			if (reset) mScale.vector3Value = Vector3.one;
		}
		GUILayout.EndHorizontal();
	}

	enum Axes : int {
		None = 0,
		X = 1,
		Y = 2,
		Z = 4,
		All = 7,
	}

	Axes CheckDifference (Transform t, Vector3 original) {
		Vector3 next = t.localEulerAngles;

		Axes axes = Axes.None;

		if (Differs(next.x, original.x)) axes |= Axes.X;
		if (Differs(next.y, original.y)) axes |= Axes.Y;
		if (Differs(next.z, original.z)) axes |= Axes.Z;

		return axes;
	}

	Axes CheckDifference (SerializedProperty property) {
		Axes axes = Axes.None;

		if (property.hasMultipleDifferentValues) {
			Vector3 original = property.quaternionValue.eulerAngles;

			foreach (Object obj in serializedObject.targetObjects) {
				axes |= CheckDifference(obj as Transform, original);
				if (axes == Axes.All) break;
			}
		}
		return axes;
	}

	static bool FloatField (string name, ref float value, bool hidden, bool greyedOut, GUILayoutOption opt) {
		float newValue = value;
		GUI.changed = false;

		if (!hidden) {
			if (greyedOut) {
				GUI.color = new Color(0.7f, 0.7f, 0.7f);
				newValue = EditorGUILayout.FloatField(name, newValue, opt);
				GUI.color = Color.white;
			} else {
				newValue = EditorGUILayout.FloatField(name, newValue, opt);
			}
		} else if (greyedOut) {
			GUI.color = new Color(0.7f, 0.7f, 0.7f);
			float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
			GUI.color = Color.white;
		} else {
			float.TryParse(EditorGUILayout.TextField(name, "--", opt), out newValue);
		}

		if (GUI.changed && Differs(newValue, value)) {
			value = newValue;
			return true;
		}
		return false;
	}

	static bool Differs (float a, float b) { return Mathf.Abs(a - b) > 0.0001f; }

	void DrawRotation (bool isWidget) {
		GUILayout.BeginHorizontal();
		{
			bool reset = GUILayout.Button("R", GUILayout.Width(20f),
			                              	   GUILayout.Height(14f));

			Vector3 visible = (serializedObject.targetObject as Transform).localEulerAngles;
			Axes changed = CheckDifference(mRot);
			Axes altered = Axes.None;

			GUILayoutOption opt = GUILayout.MinWidth(30f);

			if (FloatField("X", ref visible.x, (changed & Axes.X) != 0, isWidget, opt)) altered |= Axes.X;
			if (FloatField("Y", ref visible.y, (changed & Axes.Y) != 0, isWidget, opt)) altered |= Axes.Y;
			if (FloatField("Z", ref visible.z, (changed & Axes.Z) != 0, false, opt)) altered |= Axes.Z;

			if (reset) {
				mRot.quaternionValue = Quaternion.identity;
			} else if (altered != Axes.None) {
				foreach (Object obj in serializedObject.targetObjects) {
					Transform t = obj as Transform;
					Vector3 v = t.localEulerAngles;

					if ((altered & Axes.X) != 0) v.x = visible.x;
					if ((altered & Axes.Y) != 0) v.y = visible.y;
					if ((altered & Axes.Z) != 0) v.z = visible.z;

					t.localEulerAngles = v;
				}
			}
		}
		GUILayout.EndHorizontal();
	}
}
