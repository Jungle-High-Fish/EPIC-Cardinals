using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Cardinals.Board {

	public class EnemyPlaceHandler: MonoBehaviour {
		public event Action<int> OnMouseHover;
		public bool IsMouseHover => _isMouseHover;

		private Vector3 _leftTopPos;
		private Vector3 _rightTopPos;
		private Vector3 _leftBottomPos;
		private Vector3 _rightBottomPos;

		private Vector3 _groundLeftTopPos;
		private Vector3 _groundRightTopPos;
		private Vector3 _groundLeftBottomPos;
		private Vector3 _groundRightBottomPos;

		private float _convexOffset = 0f;
		private float _tileVerticePosOffset = Constants.GameSetting.Board.TileHeight / 2;

		private BoardBuilder _builder;

		private bool _isMouseHover = false;
		private int _hoveredIdx = int.MaxValue;
		private List<MouseDetector> _mouseDetectors = new List<MouseDetector>();

		public void Init(BoardBuilder builder) {
			_builder = builder;

			SetMeshData();
		}

		public void CreateMouseDetectors(int count) {
			foreach (var mouseDetector in _mouseDetectors) {
				Destroy(mouseDetector.gameObject);
			}
			_mouseDetectors.Clear();

			CreateGroundDetector();

			if (count == 0) return;
			if (count == 1) {
				GameObject mouseDetectorObj = new GameObject("MouseDetector");
				mouseDetectorObj.transform.SetParent(transform);
				mouseDetectorObj.layer = LayerMask.NameToLayer("MouseDetector");

				MouseDetector mouseDetector = mouseDetectorObj.AddComponent<MouseDetector>();
				mouseDetector.Init(0, new[] {
					_leftTopPos + new Vector3(0, -_convexOffset, 0),
					_rightTopPos,
					_leftBottomPos,
					_rightBottomPos + new Vector3(0, -_convexOffset, 0)
				}, new[] {
					0, 1, 2,
					1, 3, 2
				}, OnMouseHoverCallback);

				_mouseDetectors.Add(mouseDetector);
			} else if (count == 2) {
				MouseDetector[] mouseDetectors = new MouseDetector[2];

				for (int i = 0; i < 2; i++) {
					GameObject mouseDetectorObj = new GameObject("$MouseDetector_{i}");
					mouseDetectorObj.transform.SetParent(transform);
					mouseDetectorObj.layer = LayerMask.NameToLayer("MouseDetector");

					mouseDetectors[i] = mouseDetectorObj.AddComponent<MouseDetector>();
				}
				
				Vector3[] _vertices0 = new [] {
					_leftTopPos,
					_rightBottomPos,
					_leftBottomPos
				};

				int[] _triangles0 = new [] {
					0, 1, 2
				};

				Vector3[] _vertices1 = new [] {
					_leftTopPos,
					_rightTopPos,
					_rightBottomPos
				};

				int[] _triangles1 = new [] {
					0, 1, 2
				};

				mouseDetectors[0].Init(0, _vertices0, _triangles0, OnMouseHoverCallback);
				mouseDetectors[1].Init(1, _vertices1, _triangles1, OnMouseHoverCallback);

				_mouseDetectors.AddRange(mouseDetectors);
			}
		}

		private void Update() {
			if (_mouseDetectors.Count == 0) return;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layerMask = 1 << LayerMask.NameToLayer("MouseDetector");
			if (Physics.Raycast(ray, out hit, 200f, layerMask)) {
				_isMouseHover = true;
				int newIdx = hit.collider.GetComponent<MouseDetector>().Idx;
				if (_hoveredIdx != newIdx) {
					_hoveredIdx = newIdx;
					OnMouseHover?.Invoke(_hoveredIdx);
					//Debug.Log($"Mouse Hover {_hoveredIdx}");
				}
			} else {
				_isMouseHover = false;
			}
		}

		private void CreateGroundDetector() {
			GameObject mouseDetectorObj = new GameObject("GroundMouseDetector");
			mouseDetectorObj.transform.SetParent(transform);
			mouseDetectorObj.layer = LayerMask.NameToLayer("MouseDetector");

			MouseDetector mouseDetector = mouseDetectorObj.AddComponent<MouseDetector>();
			mouseDetector.Init(-1, new[] {
				_groundLeftTopPos + new Vector3(0, -_convexOffset, 0),
				_groundRightTopPos,
				_groundLeftBottomPos,
				_groundRightBottomPos + new Vector3(0, -_convexOffset, 0),
				_leftTopPos + new Vector3(0, -_convexOffset / 2, 0),
				_rightTopPos,
				_leftBottomPos,
				_rightBottomPos + new Vector3(0, -_convexOffset / 2, 0)
			}, new[] {
				0, 4, 2,
				4, 6, 2,
				6, 7, 2,
				2, 7, 3,
				7, 1, 3,
				5, 1, 7,
				0, 1, 4,
				4, 1, 5
			}, OnMouseHoverCallback);

			_mouseDetectors.Add(mouseDetector);
		}

		private void OnMouseHoverCallback(int idx) {
			_isMouseHover = idx >= 0;
			OnMouseHover?.Invoke(idx);
		}

		private void SetMeshData() {
			transform.position = new Vector3(0, _convexOffset, 0);

			_leftTopPos = new Vector3(
				_builder.TileInstantiateLeftTopPos.x + _tileVerticePosOffset,
				0,
				_builder.TileInstantiateLeftTopPos.z - _tileVerticePosOffset
			);

			_rightTopPos = new Vector3(
				-_builder.TileInstantiateLeftTopPos.x - _tileVerticePosOffset,
				0,
				_builder.TileInstantiateLeftTopPos.z - _tileVerticePosOffset
			);

			_leftBottomPos = new Vector3(
				_builder.TileInstantiateLeftTopPos.x + _tileVerticePosOffset,
				0,
				- _builder.TileInstantiateLeftTopPos.z + _tileVerticePosOffset
			);

			_rightBottomPos = new Vector3(
				- _builder.TileInstantiateLeftTopPos.x - _tileVerticePosOffset,
				0,
				- _builder.TileInstantiateLeftTopPos.z + _tileVerticePosOffset
			);

			_groundLeftTopPos = new Vector3(
				-100,
				0,
				100
			);

			_groundRightTopPos = new Vector3(
				100,
				0,
				100
			);

			_groundLeftBottomPos = new Vector3(
				-100,
				0,
				-100
			);

			_groundRightBottomPos = new Vector3(
				100,
				0,
				-100
			);
		}
	}

}
