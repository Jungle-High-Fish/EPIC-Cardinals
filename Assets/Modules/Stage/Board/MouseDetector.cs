using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Util;

namespace Cardinals.Board {

	public class MouseDetector: MonoBehaviour {
		public bool IsMouseHover => _isMouseHover;
		public int Idx => _idx;

		private MeshFilter _meshFilter;
		private Mesh _mesh;
		private MeshCollider _meshCollider;

		private Vector3[] _vertices;
		private int[] _triangles;

		private int _idx;
		private bool _isMouseHover = false;
		private Action<int> _onMouseHover;

		public void Init(int idx, Vector3[] vertices, int[] triangles, Action<int> onMouseHover, float convexOffset = 0f) {
			_meshFilter = gameObject.AddComponent<MeshFilter>();
			_mesh = _meshFilter.mesh;
			
			_vertices = vertices;
			_triangles = triangles;

			_idx = idx;
			_onMouseHover = onMouseHover;

			InitMesh();

			transform.localPosition = new Vector3(0, convexOffset, 0);
		}

		private void InitMesh() {
			_mesh.Clear();

			_mesh.vertices = _vertices;
			_mesh.triangles = _triangles;
			_mesh.RecalculateNormals();

			_meshCollider = gameObject.AddComponent<MeshCollider>();
			_meshCollider.sharedMesh = _mesh;

			// if (_triangles.Length > 3) {
			// 	_meshCollider.convex = true;
			// 	_meshCollider.isTrigger = true;
			// }
		}

		private void OnMouseEnter() {
			//Debug.Log($"Mouse Enter{_idx}");
			_isMouseHover = true;
			//_onMouseHover?.Invoke(_idx);
		}

		private void OnMouseExit() {
			//Debug.Log("Mouse Exit");
			_isMouseHover = false;
			//_onMouseHover?.Invoke(-1);
		}
	}

}
