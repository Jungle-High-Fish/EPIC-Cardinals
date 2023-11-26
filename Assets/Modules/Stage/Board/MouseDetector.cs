using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Cardinals.Board {

	public class MouseDetector: MonoBehaviour {
		public event Action<bool> OnMouseHover;
		public bool IsMouseHover => _isMouseHover;

		private ComponentGetter<MeshFilter> _meshFilter
			= new ComponentGetter<MeshFilter>(TypeOfGetter.This);
		private MeshCollider _meshCollider;
		private Mesh _mesh;
		private BoardBuilder _builder;

		private Vector3[] _vertices;
		private int[] _triangles;

		private bool _isMouseHover = false;

		public void Init(BoardBuilder builder) {
			_mesh = _meshFilter.Get(gameObject).mesh;
			_builder = builder;

			SetMeshData();
			CreateMesh();
		}

		private void OnMouseEnter() {
			_isMouseHover = true;
			OnMouseHover?.Invoke(true);
		}

		private void OnMouseExit() {
			_isMouseHover = false;
			OnMouseHover?.Invoke(false);
		}

		private void SetMeshData() {
			float yOffset = 0.01f;
			float offset = Constants.GameSetting.Board.TileHeight / 2;

			transform.position = new Vector3(0, yOffset, 0);

			Vector3 topLeftPos = new Vector3(
				_builder.TileInstantiateLeftTopPos.x + offset,
				-yOffset,
				_builder.TileInstantiateLeftTopPos.z - offset
			);

			Vector3 topRightPos = new Vector3(
				-_builder.TileInstantiateLeftTopPos.x - offset,
				0,
				_builder.TileInstantiateLeftTopPos.z - offset
			);

			Vector3 bottomLeftPos = new Vector3(
				_builder.TileInstantiateLeftTopPos.x + offset,
				0,
				- _builder.TileInstantiateLeftTopPos.z + offset
			);

			Vector3 bottomRightPos = new Vector3(
				- _builder.TileInstantiateLeftTopPos.x - offset,
				-yOffset,
				- _builder.TileInstantiateLeftTopPos.z + offset
			);

			_vertices = new Vector3[] {
				topLeftPos,
				topRightPos,
				bottomLeftPos,
				bottomRightPos
			};

			_triangles = new int[] {
				0, 1, 2,
				1, 3, 2
			};
		}

		private void CreateMesh() {
			_mesh.Clear();
			_mesh.vertices = _vertices;
			_mesh.triangles = _triangles;

			_mesh.RecalculateNormals();
			_meshCollider = gameObject.AddComponent<MeshCollider>();
			
			_meshCollider.sharedMesh = null;
			_meshCollider.sharedMesh = _mesh;
			_meshCollider.convex = true;
			_meshCollider.isTrigger = true;
		}
	}

}
