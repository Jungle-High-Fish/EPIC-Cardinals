using System;
using System.Collections;
using System.Collections.Generic;
using Cardinals.Enums;
using Cardinals.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace Cardinals.Board {

	public class BoardInputHandler: MonoBehaviour {
		public bool IsMouseHover => _isMouseHover;
		public bool IsMouseHoverUI => _isMouseHoverUI;
		public int HoveredIdx => _hoveredIdx;
		public MouseDetectorType HoveredMouseDetectorType => _hoveredMouseDetectorType;

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
		private bool _isMouseHoverUI = false;
		private int _hoveredIdx = int.MaxValue;
		private MouseDetectorType _hoveredMouseDetectorType = default;
		private List<MouseDetector> _mouseDetectors = new List<MouseDetector>();

		public void Init(BoardBuilder builder) {
			_builder = builder;

			SetMeshData();
		}

		public Vector3[] CreateMouseDetectors(int count) {
			foreach (var mouseDetector in _mouseDetectors) {
				Destroy(mouseDetector.gameObject);
			}
			_mouseDetectors.Clear();

			CreateGroundDetector();

			if (count == 0) return null;
			if (count == 1) {
				GameObject mouseDetectorObj = new GameObject("MouseDetector");
				mouseDetectorObj.transform.SetParent(transform);
				mouseDetectorObj.layer = LayerMask.NameToLayer("MouseDetector");

				MouseDetector mouseDetector = mouseDetectorObj.AddComponent<MouseDetector>();

				Vector3[] vertices = new[] {
					_leftTopPos + new Vector3(0, -_convexOffset, 0),
					_rightTopPos,
					_leftBottomPos,
					_rightBottomPos + new Vector3(0, -_convexOffset, 0)
				};

				int[] triangles = new[] {
					0, 1, 2,
					1, 3, 2
				};

				Vector2[] uvs = new[] {
					new Vector2(0, 1),
					new Vector2(1, 1),
					new Vector2(0, 0),
					new Vector2(1, 0)
				};

				mouseDetector.Init(0, vertices, triangles, uvs);

				_mouseDetectors.Add(mouseDetector);

				// calculate center of mass
				Vector3 center = Vector3.zero;
				foreach (var vertice in vertices) {
					center += vertice;
				}
				center /= vertices.Length;

				return new[] {
					center
				};
			} else if (count == 2) {
				MouseDetector[] mouseDetectors = new MouseDetector[2];

				for (int i = 0; i < 2; i++) {
					GameObject mouseDetectorObj = new GameObject("$MouseDetector_{i}");
					mouseDetectorObj.transform.SetParent(transform);
					mouseDetectorObj.layer = LayerMask.NameToLayer("MouseDetector");

					mouseDetectors[i] = mouseDetectorObj.AddComponent<MouseDetector>();
				}
				
				Vector3[] vertices0 = new [] {
					_leftTopPos,
					_rightBottomPos,
					_leftBottomPos
				};

				int[] triangles0 = new [] {
					0, 1, 2
				};

				Vector2[] uvs0 = new[] {
					new Vector2(0, 1),
					new Vector2(1, 0),
					new Vector2(0, 0)
				};

				Vector3 center0 = Vector3.zero;
				foreach (var vertice in vertices0) {
					center0 += vertice;
				}
				center0 /= vertices0.Length;

				Vector3[] vertices1 = new [] {
					_leftTopPos,
					_rightTopPos,
					_rightBottomPos
				};

				int[] triangles1 = new [] {
					0, 1, 2
				};

				Vector2[] uvs1 = new[] {
					new Vector2(0, 1),
					new Vector2(1, 1),
					new Vector2(1, 0)
				};

				Vector3 center1 = Vector3.zero;
				foreach (var vertice in vertices1) {
					center1 += vertice;
				}
				center1 /= vertices1.Length;

				mouseDetectors[0].Init(0, vertices0, triangles0, uvs0);
				mouseDetectors[1].Init(1, vertices1, triangles1, uvs1);

				_mouseDetectors.AddRange(mouseDetectors);

				return new[] {
					center0,
					center1
				};
			}

			return null;
		}

		private void Update() {
			if (_mouseDetectors.Count == 0) return;

			if (GameManager.I.Stage.CardManager.State != CardState.Select) {
				foreach(var m in _mouseDetectors) {
					m.RendererEnable(false);
				}
			}

			// 보드 레이캐스트
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layerMask = 1 << LayerMask.NameToLayer("MouseDetector");
			if (Physics.Raycast(ray, out hit, 200f, layerMask)) {
				_isMouseHover = true;
				MouseDetector mouseDetector = hit.collider.gameObject.GetComponent<MouseDetector>();
				int newIdx = mouseDetector.Idx;
				if (_hoveredIdx != newIdx) {
					foreach(var m in _mouseDetectors) {
						m.RendererEnable(false);
					}

					if (newIdx >= 0 && GameManager.I.Stage.CardManager.State == CardState.Select) {
						mouseDetector.RendererEnable(true);
					}

					_hoveredIdx = newIdx;
				}
			} else {
				_isMouseHover = false;

				foreach(var m in _mouseDetectors) {
					m.RendererEnable(false);
				}
			}

			// UI 레이캐스트
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = Input.mousePosition;

			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, results);

			bool isMouseHoverUI = false;
			foreach (var result in results) {
				UIMouseDetector mouseDetector = result.gameObject.GetComponent<UIMouseDetector>();
				if (mouseDetector == null) {
					continue;
				} else {
					_hoveredMouseDetectorType = mouseDetector.MouseDetectorType;
					isMouseHoverUI = true;
				}
			}
			_isMouseHoverUI = isMouseHoverUI;
		}

		private void CreateGroundDetector() {
			GameObject mouseDetectorObj = new GameObject("GroundMouseDetector");
			mouseDetectorObj.transform.SetParent(transform);
			mouseDetectorObj.layer = LayerMask.NameToLayer("MouseDetector");

			MouseDetector mouseDetector = mouseDetectorObj.AddComponent<MouseDetector>();

			Vector3[] vertices = new[] {
				_groundLeftTopPos + new Vector3(0, -_convexOffset, 0),
				_groundRightTopPos,
				_groundLeftBottomPos,
				_groundRightBottomPos + new Vector3(0, -_convexOffset, 0),
				_leftTopPos + new Vector3(0, -_convexOffset / 2, 0),
				_rightTopPos,
				_leftBottomPos,
				_rightBottomPos + new Vector3(0, -_convexOffset / 2, 0)
			};

			int[] triangles = new[] {
				0, 4, 2,
				4, 6, 2,
				6, 7, 2,
				2, 7, 3,
				7, 1, 3,
				5, 1, 7,
				0, 1, 4,
				4, 1, 5
			};

			Vector2[] uvs = new[] {
				new Vector2(0.5f, 0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(0, 0),
				new Vector2(1, 0),
				new Vector2(0, 1),
				new Vector2(1, 1)
			};

			mouseDetector.Init(-1, vertices, triangles, uvs);

			_mouseDetectors.Add(mouseDetector);
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
