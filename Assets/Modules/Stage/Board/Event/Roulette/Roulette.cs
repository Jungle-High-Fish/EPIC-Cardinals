using System.Collections;
using Unity.Mathematics;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Serialization;
using Util;
using Random = UnityEngine.Random;

namespace Cardinals.BoardEvent.Roulette
{
	public class Roulette : MonoBehaviour
	{
		[SerializeField] private Transform _piecePrefab; // 룰렛에 표시되는 정보 프리팹
		[SerializeField] private Transform _linePrefab; // 정보들을 구분하는 선 프리팹
		[SerializeField] private Transform _pieceParent; // 정보들이 배치되는 부모 Transform
		[SerializeField] private Transform _lineParent; // 선들이 배치되는 부모 Transform
		[SerializeField] private RoulettePieceDataSO[] _roulettePieceDatas; // 룰렛에 표시되는 정보 배열

		[SerializeField] private int _spinDuration; // 회전 시간
		[SerializeField] private Transform _spinningRoulette; // 실제 회전하는 회전판 Transfrom
		[SerializeField] private AnimationCurve _spinningCurve; // 회전 속도 제어를 위한 그래프

		private float _pieceAngle; // 정보 하나가 배치되는 각도
		private float _halfPieceAngle; // 정보 하나가 배치되는 각도의 절반 크기
		private float _halfPieceAngleWithPaddings; // 선의 굵기를 고려한 Padding이 포함된 절반 크기

		private int _accumulatedWeight; // 가중치 계산을 위한 변수
		private bool _isSpinning = false; // 현재 회전중인지
		private int _selectedIndex = 0; // 룰렛에서 선택된 아이템

		private void Awake()
		{
			_pieceAngle = 360 / _roulettePieceDatas.Length;
			_halfPieceAngle = _pieceAngle * 0.5f;
			_halfPieceAngleWithPaddings = _halfPieceAngle - (_halfPieceAngle * 0.25f);

			SpawnPiecesAndLines();
			CalculateWeightsAndIndices();

			// Debug..
			//Debug.Log($"Index : {GetRandomIndex()}");
		}

		public void Init()
		{
			_isSpinning = false;
			_spinningRoulette.rotation = quaternion.identity;
		}
		
		private void SpawnPiecesAndLines()
		{
			for (int i = 0; i < _roulettePieceDatas.Length; ++i)
			{
				Transform piece = Instantiate(_piecePrefab, _pieceParent.position, Quaternion.identity, _pieceParent);
				// 생성한 룰렛 조각의 정보 설정 (아이콘, 설명)
				piece.GetComponent<RoulettePiece>().Setup(_roulettePieceDatas[i]);
				// 생성한 룰렛 조각 회전
				piece.RotateAround(_pieceParent.position, Vector3.back, (_pieceAngle * i));

				Transform line = Instantiate(_linePrefab, _lineParent.position, Quaternion.identity, _lineParent);
				// 생성한 선 회전 (룰렛 조각 사이를 구분하는 용도)
				line.RotateAround(_lineParent.position, Vector3.back, (_pieceAngle * i) + _halfPieceAngle);
			}
		}

		private void CalculateWeightsAndIndices()
		{
			for (int i = 0; i < _roulettePieceDatas.Length; ++i)
			{
				_roulettePieceDatas[i].index = i;

				// 예외처리. 혹시라도 chance값이 0 이하이면 1로 설정
				if (_roulettePieceDatas[i].chance <= 0)
				{
					_roulettePieceDatas[i].chance = 1;
				}

				_accumulatedWeight += _roulettePieceDatas[i].chance;
				_roulettePieceDatas[i].weight = _accumulatedWeight;

				// Debug.Log(
				// 	$"({_roulettePieceDatas[i].index}){_roulettePieceDatas[i].description}:{_roulettePieceDatas[i].weight}");
			}
		}

		private int GetRandomIndex()
		{
			int weight = Random.Range(0, _accumulatedWeight);

			for (int i = 0; i < _roulettePieceDatas.Length; ++i)
			{
				if (_roulettePieceDatas[i].weight > weight)
				{
					return i;
				}
			}

			return 0;
		}

		public void Spin(UnityAction<RoulettePieceDataSO> action = null)
		{
			if (_isSpinning == true) return;

			// 룰렛의 결과 값 선택
			_selectedIndex = GetRandomIndex();
			// 선택된 결과의 중심 각도
			float angle = _pieceAngle * _selectedIndex;
			// 정확히 중심이 아닌 결과 값 범위 안의 임의의 각도 선택
			float leftOffset = (angle - _halfPieceAngleWithPaddings) % 360;
			float rightOffset = (angle + _halfPieceAngleWithPaddings) % 360;
			float randomAngle = Random.Range(leftOffset, rightOffset);

			// 목표 각도(targetAngle) = 결과 각도 + 360 * 회전 시간 * 회전 속도
			int rotateSpeed = 2;
			float targetAngle = (randomAngle + 360 * _spinDuration * rotateSpeed);

			// Debug.Log($"SelectedIndex:{_selectedIndex}, Angle:{angle}");
			// Debug.Log($"left/right/random:{leftOffset}/{rightOffset}/{randomAngle}");
			// Debug.Log($"targetAngle:{targetAngle}");

			_isSpinning = true;
			StartCoroutine(OnSpin(targetAngle, action));
		}

		private IEnumerator OnSpin(float end, UnityAction<RoulettePieceDataSO> action)
		{
			float current = 0;
			float percent = 0;

			while (percent < 1)
			{
				current += Time.deltaTime;
				percent = current / _spinDuration;

				float z = Mathf.Lerp(0, end, _spinningCurve.Evaluate(percent));
				_spinningRoulette.rotation = Quaternion.Euler(0, 0, z);

				yield return null;
			}

			// _isSpinning = false; // Init()에서 초기화 해줄 것

			if (action != null) action.Invoke(_roulettePieceDatas[_selectedIndex]);
		}
	}
}