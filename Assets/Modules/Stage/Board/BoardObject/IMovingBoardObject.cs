using System.Collections;
using Cardinals.Board;
using UnityEngine;

namespace Cardinals.BoardEvent
{
    /// <summary>
    /// 보드 위에 존재하는 움직이는 소환수들
    /// </summary>
    public interface IMovingBoardObject
    {
        /// <summary>
        /// 현재 존재하는 타일 정보
        /// </summary>
        public Board.Tile OnTile { get; }
        
        /// <summary>
        /// 턴 처리
        /// </summary>
        /// <returns></returns>
        public IEnumerator OnTurn();

        /// <summary>
        /// 플레이어와 충돌 처리
        /// </summary>
        public IEnumerator OnCollisionPlayer();

        /// <summary>
        /// 도착 이벤트 일 때
        /// </summary>
        public IEnumerator ArrivePlayer();
        
        /// <summary>
        /// 제거 로직
        /// </summary>
        public void Destroy();
    }
}