using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cardinals.Buff;
using Cardinals.Enums;
using Cardinals.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace Cardinals.Board {

	public class CircleBoardInputHandler: MonoBehaviour, IBoardInputHandler {
		public bool IsMouseHover => _isMouseHover;
		public bool IsMouseHoverUI => _isMouseHoverUI;
		public int HoveredIdx => _hoveredIdx;
		public UIMouseDetectorType HoveredMouseDetectorType => _hoveredMouseDetectorType;

		private float _convexOffset = 0f;

		private CircleBoardBuilder _builder;

		private bool _isMouseHover = false;
		private bool _isMouseHoverUI = false;
		private int _hoveredIdx = int.MaxValue;
		private UIMouseDetectorType _hoveredMouseDetectorType = default;
		private List<MouseDetector> _mouseDetectors = new List<MouseDetector>();

		// 정점 데이터
		private List<Vector3> _verticeList = new List<Vector3>();
		private List<Vector2> _uvList = new List<Vector2>();
		private Vector3 _groundLeftTopPos;
		private Vector3 _groundRightTopPos;
		private Vector3 _groundLeftBottomPos;
		private Vector3 _groundRightBottomPos;

		public void Init(IBoardBuilder builder) {
			_builder = builder as CircleBoardBuilder;

			SetMeshData();
		}

		public Vector3[] CreateMouseDetectors(int count) {
			foreach (var mouseDetector in _mouseDetectors) {
				Destroy(mouseDetector.gameObject);
			}
			_mouseDetectors.Clear();

			CreateGroundDetector();

			if (count == 0) return null;
			else if (count < _builder.TileCount) {
				int tileNum = _builder.TileCount;
				int verticeNum = _builder.TileCount * 3;
				int verticesPerDetector = verticeNum / count;

				List<Vector3> centerList = new List<Vector3>();

				for (int i = 0; i < count; i++) {
					GameObject mouseDetectorObj = new GameObject("MouseDetector");
					mouseDetectorObj.transform.SetParent(transform);
					mouseDetectorObj.layer = LayerMask.NameToLayer("MouseDetector");

					MouseDetector mouseDetector = mouseDetectorObj.AddComponent<MouseDetector>();

					List<Vector3> targetVerticeList = new List<Vector3> {
                        _verticeList[0]
                    };
					List<int> triangleList = new();

					int initialIdx = verticesPerDetector * i + 1;
					for (int j = initialIdx; j < initialIdx + verticesPerDetector; j++) {
						triangleList.Add(j);
						targetVerticeList.Add(_verticeList[j]);
						if (j + 1 >= _verticeList.Count) {
							triangleList.Add(1);
							targetVerticeList.Add(_verticeList[1]);
						} else {
							triangleList.Add(j + 1);
						}
						triangleList.Add(0);
					}

					mouseDetector.Init(i, _verticeList.ToArray(), triangleList.ToArray(), _uvList.ToArray());

					_mouseDetectors.Add(mouseDetector);

					// 몬스터 생성위치 게산
					Vector3 center = Vector3.zero;
					foreach (var vertice in targetVerticeList) {
						center += vertice;
					}
					center /= targetVerticeList.Count;

					centerList.Add(center);
				}

				return centerList.ToArray();
			} else {
				return null;
			}
		}

		private void Update() {
			if (_mouseDetectors.Count == 0) return;

			if (GameManager.I.Stage.CardManager.State != CardState.Select) {
				foreach(var m in _mouseDetectors) {
					m.RendererEnable(false);
				}
				
				GameManager.I.Player.UpdateAction(PlayerActionType.None); // 플레이어 행동 설정
			}
					
			// 보드 레이캐스트
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layerMask = 1 << LayerMask.NameToLayer("MouseDetector");
			if (Physics.Raycast(ray, out hit, 200f, layerMask)) {
				_isMouseHover = true;
				MouseDetector mouseDetector = hit.collider.gameObject.GetComponent<MouseDetector>();
				int newIdx = mouseDetector.Idx;

				PlayerActionType playerActionType = default;
				if (_hoveredIdx != newIdx)
				{
					foreach (var m in _mouseDetectors)
					{
						m.RendererEnable(false);
					}

					if (newIdx >= 0 && GameManager.I.Stage.CardManager.State == CardState.Select)
					{
						mouseDetector.RendererEnable(true);

						// 플레이어 행동 설정
						if (GameManager.I.Stage.CardManager.CheckUseCardOnAction())
						{
							if (GameManager.I.Player.OnTile.Type == TileType.Attack)
							{
								playerActionType = PlayerActionType.Attack;
							}
							else if (GameManager.I.Player.OnTile.Type == TileType.Defence)
							{
								playerActionType = PlayerActionType.Defense;
							}
						}
						else
						{
							playerActionType = PlayerActionType.CantUsed;
						}
					}

					_hoveredIdx = newIdx;
				}
				else playerActionType = GameManager.I.Player.CurActionType;

				if (IsSelectState() && 
				    !(playerActionType == PlayerActionType.Attack || 
				      playerActionType == PlayerActionType.Defense ||
				      playerActionType == PlayerActionType.CantUsed)) 
				{
					playerActionType = PlayerActionType.Move;
				}
				
				GameManager.I.Player.UpdateAction(playerActionType);
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

			if(results.Any(result => result.gameObject.GetComponent<UIMouseDetector>()?.MouseDetectorType  == UIMouseDetectorType.CardEvent))
			{
				_isMouseHoverUI = true;
				_hoveredMouseDetectorType = UIMouseDetectorType.CardEvent;
				return;
			}
			
			bool isMouseHoverUI = false;
			
			foreach (var result in results) {
				UIMouseDetector mouseDetector = result.gameObject.GetComponent<UIMouseDetector>();
				if (mouseDetector == null) {
					continue;
				} else {
					_hoveredMouseDetectorType = mouseDetector.MouseDetectorType;
					isMouseHoverUI = true;
					if(IsSelectState())
						 GameManager.I.Player.UpdateAction(PlayerActionType.Cancel);
				}
			}
			_isMouseHoverUI = isMouseHoverUI;
		}

		private bool IsSelectState()
		{
			return GameManager.I.Stage.CardManager.State == CardState.Select;
		}

		private void CreateGroundDetector() {
			GameObject mouseDetectorObj = new GameObject("GroundMouseDetector");
			mouseDetectorObj.transform.SetParent(transform);
			mouseDetectorObj.layer = LayerMask.NameToLayer("MouseDetector");

			MouseDetector mouseDetector = mouseDetectorObj.AddComponent<MouseDetector>();
			
			List<Vector3> targetVerticeList = new List<Vector3> {
				_groundLeftTopPos,
				_groundRightTopPos,
				_groundRightBottomPos,
				_groundLeftBottomPos
			};
			targetVerticeList.AddRange(_verticeList);
			Vector3[] vertices = targetVerticeList.ToArray();
			
			int intuitionGap = (_verticeList.Count - 1) / 4;

			List<int> triangles = new List<int>();

			for (int i = 5; i < vertices.Length; i++) {
				if (i == 5) {
					triangles.Add(0);
					triangles.Add(1);
					triangles.Add(i);
				} else if (i == 5 + intuitionGap) {
					triangles.Add(1);
					triangles.Add(2);
					triangles.Add(i);
				} else if (i == 5 + intuitionGap * 2) {
					triangles.Add(2);
					triangles.Add(3);
					triangles.Add(i);
				} else if (i == 5 + intuitionGap * 3) {
					triangles.Add(3);
					triangles.Add(0);
					triangles.Add(i);
				}

				if (i >= 5 && i < 5 + intuitionGap) {
					triangles.Add(i);
					triangles.Add(1);
					triangles.Add(i + 1);
				} else if (i >= 5 + intuitionGap && i < 5 + intuitionGap * 2) {
					triangles.Add(i);
					triangles.Add(2);
					triangles.Add(i + 1);
				} else if (i >= 5 + intuitionGap * 2 && i < 5 + intuitionGap * 3) {
					triangles.Add(i);
					triangles.Add(3);
					triangles.Add(i + 1);
				} else if (i >= 5 + intuitionGap * 3 && i < 5 + intuitionGap * 4) {
					triangles.Add(i);
					triangles.Add(0);
					if (i + 1 >= vertices.Length) {
						triangles.Add(5);
					} else {
						triangles.Add(i + 1);
					}
				}
			}

			List<Vector2> uvList = new List<Vector2>();
			for (int i = 0; i < vertices.Length; i++) {
				if (i % 3 == 0) {
					uvList.Add(new Vector2(0.5f, 0.5f));
				} else if (i % 3 == 1) {
					uvList.Add(new Vector2(0.5f, 0.6f));
				} else if (i % 3 == 2) {
					uvList.Add(new Vector2(0.6f, 0.5f));
				}
			}

			mouseDetector.Init(-1, vertices, triangles.ToArray(), uvList.ToArray());

			_mouseDetectors.Add(mouseDetector);
		}

		private void SetMeshData() {
			transform.position = new Vector3(0, _convexOffset, 0);

			// 정점 데이터 초기화 및 생성

			_verticeList = new();
			int t = 0;
			Vector3? lastVertice = null;

			_verticeList.Add(Vector3.zero);
			foreach(Tile tile in _builder.Board) {
				var tilePos = tile.TilePositionOnGround + new Vector3(0, 0, 0);
				tilePos.y = 0;

				var tileCenterVertice = tilePos.normalized * (_builder.BoardRadius - Constants.GameSetting.Board.TileHeight / 2);
				var temp = Vector3.Cross(tileCenterVertice, Vector3.up);
				var tileLeftTopVertice = tileCenterVertice - temp.normalized * Constants.GameSetting.Board.TileWidth / 2;
				var tileRightTopVertice = tileCenterVertice + temp.normalized * Constants.GameSetting.Board.TileWidth / 2;
				
				if (t == 0) {
					lastVertice = tileRightTopVertice;

					_verticeList.Add(tileCenterVertice);
					_verticeList.Add(tileLeftTopVertice);
				} else {
					_verticeList.Add(tileRightTopVertice);
					_verticeList.Add(tileCenterVertice);
					_verticeList.Add(tileLeftTopVertice);
				}
				
				t++;
			}

			if (lastVertice != null) {
				_verticeList.Add(lastVertice.Value);
			}

			// UV 맵 좌표 보드 반지름 기준 계산
			float uvLongBasis = (_verticeList[3] - _verticeList[2]).magnitude / _builder.BoardRadius;
			float uvShortBasis = (_verticeList[2] - _verticeList[1]).magnitude / _builder.BoardRadius;

			for (int i = 0; i < _verticeList.Count; i++) {
				if (i == 0) {
					_uvList.Add(new Vector2(0.5f, 0.5f));
				} else if (i % 6 == 0) {
					_uvList.Add(new Vector2(uvShortBasis, 1));
				} else if (i % 6 == 1) {
					_uvList.Add(new Vector2(0, 1));
				} else if (i % 6 == 2) {
					_uvList.Add(new Vector2(uvShortBasis, 1));
				} else if (i % 6 == 3) {
					_uvList.Add(new Vector2(uvLongBasis + uvShortBasis, 1));
				} else if (i % 6 == 4) {
					_uvList.Add(new Vector2(uvLongBasis, 1));
				} else if (i % 6 == 5) {
					_uvList.Add(new Vector2(uvLongBasis + uvShortBasis, 1));
				}
			}

			// 보드 외부 정점 데이터 생성

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
