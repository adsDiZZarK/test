using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RUN : MonoBehaviour {
	 [SerializeField] private int _xSize, _ySize;
	 [SerializeField] private Material MatTile, MatSeam;
	 [SerializeField] private bool debug = false;
	 private Vector3[] _vertices;
	 private GameObject go /*= new GameObject()*/;

	 private void Start() {
			go = new GameObject("Grid");
			TalGen();
	 }
	 private void TalGen() {
			TileGen.SetMat(MatTile, MatSeam);
			_vertices = TileGen.Test(go,_xSize,_ySize);
	 }

	 private void OnDrawGizmos() {
			if (!debug) { return; }
			if (_vertices == null) {
				 //Debug.LogError("no vertex" + _vertexes.Length); 
				 return;
			}
			Gizmos.color = Color.red;
			for (int i = 0 ; i < _vertices.Length ; i++) {
				 Gizmos.DrawSphere(_vertices[i], 0.05f);
				 //drawString(x + ", " + y + ", " + z, new Vector3(x, y, z), Color.black);
				 drawString(i.ToString(), _vertices[i], Color.black);
			}
	 }

	 private static void drawString(string text, Vector3 worldPos, Color? colour = null) {
			UnityEditor.Handles.BeginGUI();
			if (colour.HasValue)
				 GUI.color = colour.Value;
			var view = UnityEditor.SceneView.currentDrawingSceneView;
			Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

			if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0) {
				 UnityEditor.Handles.EndGUI();
				 return;
			}

			Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
			GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height - 35, size.x, size.y), text);
			UnityEditor.Handles.EndGUI();
	 }

	 private void Update() {

	 }
}
