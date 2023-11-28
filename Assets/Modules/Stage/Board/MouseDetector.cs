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
		private MeshRenderer _meshRenderer;

		private Vector3[] _vertices;
		private int[] _triangles;
		private Vector2[] _uvs;

		private int _idx;
		private bool _isMouseHover = false;

		private bool _hasInit = false;

		public void Init(int idx, Vector3[] vertices, int[] triangles, Vector2[] uvs, float convexOffset = 0f) {
			_hasInit = true;

			_meshFilter = gameObject.AddComponent<MeshFilter>();
			_mesh = _meshFilter.mesh;
			
			_vertices = vertices;
			_triangles = triangles;
			_uvs = uvs;

			_idx = idx;

			InitMesh();

			transform.localPosition = new Vector3(0, convexOffset, 0);
		}

		public void RendererEnable(bool enable) {
			if (!_hasInit) return;
			_meshRenderer.enabled = enable;
		}

		private void InitMesh() {
			_mesh.Clear();

			_mesh.vertices = _vertices;
			_mesh.triangles = _triangles;
			_mesh.uv = _uvs;
			_mesh.RecalculateNormals();

			_meshCollider = gameObject.AddComponent<MeshCollider>();
			_meshCollider.sharedMesh = _mesh;

			_meshRenderer = gameObject.AddComponent<MeshRenderer>();
			_meshRenderer.material 
				= new Material(Shader.Find("Universal Render Pipeline/Lit"));

			if (_vertices.Length > 3) {
				_meshRenderer.material.SetTexture (
					"_BaseMap", 
					ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_BoardInput_Square).texture
				);
			} else {
				_meshRenderer.material.SetTexture (
					"_BaseMap", 
					ResourceLoader.LoadSprite(Constants.FilePath.Resources.Sprites_BoardInput_Triangle).texture
				);
			}

			_meshRenderer.material.SetMaterialTransparent();

			_meshRenderer.enabled = false;

			// if (_triangles.Length > 3) {
			// 	_meshCollider.convex = true;
			// 	_meshCollider.isTrigger = true;
			// }
		}
	}

}
