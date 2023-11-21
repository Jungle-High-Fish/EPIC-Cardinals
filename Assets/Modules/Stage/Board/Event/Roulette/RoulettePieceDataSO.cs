using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.BoardEvent.Roulette
{
	[CreateAssetMenu(fileName = "RouletteData", menuName = "Cardinals/BoardEvent/Roulette Data", order = 0)]
	public class RoulettePieceDataSO : ScriptableObject
	{
		public BoardEventRoulette type;
		public Sprite icon; // ������ �̹��� ����
		public string description; // �̸�, �Ӽ�, �ɷ�ġ ���� ����

		// 3���� ������ ����Ȯ��(chance)�� 100, 60, 40�̸�
		// ����Ȯ���� ���� 200. 100/200 = 50%, 60/200 = 30%, 40/200 = 20%
		[Range(1, 100)] public int chance = 100; // ����Ȯ��

		[HideInInspector] public int index; // ������ ����
		[HideInInspector] public int weight; // ����ġ
	}
}