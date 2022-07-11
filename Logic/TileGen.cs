using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TileGen//TODO rework more methods
{
	 private static Mesh _mesh = new Mesh();
	 private static GameObject GO;
	 private static Material MatTile, MatSeam;
	 private static int _xSize, _zSize;
	 private static int[] _triangle, tris;	
	 private static Vector2[] _uv, _uvs;
	 private static Vector3[] _vertices, _vert;
	 private static Vector4[] _tangents;
	 private static Vector4 _tangent = new Vector4(1f, 0, 0, 1f);
	 static TileGen() { }
	 public static void SetMat(Material tile, Material seam) { MatTile = tile; MatSeam = seam; }
	 private static void DbuGInfo()
	 {
			Debug.Log("vertices: " + _vertices.Length);
			Debug.Log("triangle: " + _triangle.Length);
	 }
	 public static Vector3[] Test(GameObject go, int sizeX, int sizeZ)
	 {
			_xSize = sizeX;
			_zSize = sizeZ;
			MeshComponent();
			GO = go;
			Generate();

			return _vertices;
	 }

	 private static void MeshComponent()
	 {
			_vertices = new Vector3[_xSize * _zSize * 9];
			_vert = new Vector3[9];
			_triangle = new int[_xSize * _zSize * 24];
			tris = new int[24];
			_uv = new Vector2[_vertices.Length];
			_uvs = new Vector2[_vertices.Length];

			_tangents = new Vector4[_vertices.Length];
			//_tangent = new Vector4(1f, 0, 0, -1f);
	 }
	 private static void GenVert()
	 {

	 }

	 private static float tileX = 2f, tileZ = 1f, seamW = 0.1f, offset = 0.0f;
	 private static void VertTiles(float gor, float vert,int num)//TODO rework more methods
	 {
			int y = 0;
			gor += num * offset;
			_vert[0] = new Vector3(gor + 0, y, vert + 0);
			_vert[1] = new Vector3(gor + seamW, y, vert + 0);
			_vert[2] = new Vector3(gor + seamW + tileX, y, vert + 0);
			_vert[3] = new Vector3(gor + 0, y, vert + tileZ);
			_vert[4] = new Vector3(gor + seamW, y, vert + tileZ);
			_vert[5] = new Vector3(gor + seamW + tileX, y, vert + tileZ);
			_vert[6] = new Vector3(gor + 0, y, tileZ + vert + seamW);
			_vert[7] = new Vector3(gor + seamW, y, vert + tileZ + seamW);
			_vert[8] = new Vector3(gor + seamW + tileX, y, vert + tileZ + seamW);

			_uvs[0] = new Vector2(_vert[0].x, _vert[0].z);
			_uvs[1] = new Vector2(0, 0);
			_uvs[2] = new Vector2(1, 0);
			_uvs[3] = new Vector2(_vert[3].x, _vert[3].z);
			_uvs[4] = new Vector2(0, 1);
			_uvs[5] = new Vector2(1, 1);
			_uvs[6] = new Vector2(_vert[6].x, _vert[6].z);
			_uvs[7] = new Vector2(_vert[7].x, _vert[7].z);
			_uvs[8] = new Vector2(_vert[8].x, _vert[8].z);
			//_triangle[0]
			int tile = 2;
			for (int z = 0, ti = 0, vi = 0 ; z < 2 ; z++, vi++)
			{
				 for (int x = 0 ; x < tile ; x++, ti += 6, vi++)
				 {
						tris[ti] = vi;
						tris[ti + 1] = tris[ti + 4] = vi + tile + 1;
						tris[ti + 2] = tris[ti + 3] = vi + 1;
						tris[ti + 5] = vi + tile + 2;


				 }
			}
	 }
	 private static void Generate()
	 {
			DbuGInfo();
			_mesh.name = "Tiles";
			_mesh.subMeshCount = 2;
			List<int> seam = new List<int>();
			List<int> tile = new List<int>();
			//_vertices = _vert;

			//MeshFilter meshfilter = GO.AddComponent<MeshFilter>();
			//GetComponent<MeshFilter>().mesh = _mesh;
			//GO.AddComponent<MeshRenderer>();
			int koef;
			for (int z = 0, i = 0, scr = 0 ; z < _zSize ; z++)//FIXME rework funk
			{
				 for (int x = 0 ; x < _xSize ; x++, i++)
				 {
						koef = (z * _xSize * 9) + x * 9;
						VertTiles(x * (tileX + seamW/*+0.3f*/), z * (tileZ + seamW/*+0.3f*/),z);
						for (int a = 0, ind = 0 ; a < 9 ; a++, scr++)//FIXME rework funk
						{
							 ind = koef + a;
							 _vertices[ind] = _vert[a];
							 //FIXME rework funk for ref
							 _uv[ind] = _uvs[a];
							 //_uv[ind] = new Vector2(_vertices[ind].x, _vertices[ind].z);
							 //_uv[ind] = new Vector2(_vertices[ind].x / (tileX + seamW), _vertices[ind].z / (tileZ + seamW));
							 //_uv[ind] = new Vector2(_vertices[ind].x/tileX, (_vertices[ind].z)/tileZ);
							 //_uv[ind] = new Vector2((float)x, (float)z);
							 _tangents[ind] = _tangent;
							 Debug.Log("vert init");
							 if (ind != scr)
							 {
									Debug.LogError("ERROR NUM VERT");
									Debug.Log(scr + " (" + ind + ") " + _vertices[z * 9 + a] + " / " + z + " " + x + " - " + (z + x) * 9);
							 }
							 //scr++;
						}
						for (int b = 0, ind = 0, scr2 = 0 ; b < 24 ; b++, scr2++)//FIXME rework funk
						{
							 ind = (z * _xSize * 24) + x * 24 + b;//TODO math
							 _triangle[ind] = tris[b] + koef;
							 
							 _mesh.SetTriangles(seam.ToArray(), 0);
							 //_mesh.SetTriangles(tile.ToArray(), 1);
							 Debug.Log("tris init");
							 //Debug.Log(ind + " (" + scr2 + ") " + _triangle[ind] + " / " + z + "-" + x);
						}
						Debug.Log("------------------");

						//_vertices[i] = new Vector3(x * tileX, 0, z * tileZ);

				 }
			}
			//for (int i = 0 ; i < _uv.Length ; i++) {
			//	 _uv[i] = new Vector2(_vertices[i].x/3.5f, _vertices[i].z);
			//}
			/*for (int z = 0, ti = 0, vi = 0 ; z < _zSize ; z++, vi++) {
				 for (int x = 0 ; x < _xSize ; x++, ti += 6, vi++) {
						_triangle[ti] = vi;
						_triangle[ti + 1] = _triangle[ti + 4] = vi + _xSize + 1;
						_triangle[ti + 2] = _triangle[ti + 3] = vi + 1;
						//_triangle[3] = 1;
						//_triangle[4] = _xSize + 1;
						_triangle[ti + 5] = vi + _xSize + 2;
				 }
			}*/
			/*int vi = 0;
			_triangle[0] = 0;
			_triangle[1] = 3;
			_triangle[2] = 4;
			_triangle[3] = 0;
			_triangle[4] = 4;
			_triangle[5] = 1;
			_triangle[6] = 
			_triangle[7] = 
			_triangle[8] = */

			_mesh.vertices = _vertices;//FIXME rework funk
			_mesh.triangles = _triangle;
			_mesh.RecalculateNormals();
			_mesh.uv = _uv;
			_mesh.tangents = _tangents;

			//GameObject GO = new GameObject();
			GO.transform.position = Vector3.zero;
			//GO.name = "ProceduralTile";
			//go.tag = "Generated";

			MeshFilter mf = GO.AddComponent<MeshFilter>();
			mf.mesh = _mesh;

			MeshRenderer mr = GO.AddComponent<MeshRenderer>();
			mr.materials = new Material[2] { MatTile, MatSeam };
	 }
}
