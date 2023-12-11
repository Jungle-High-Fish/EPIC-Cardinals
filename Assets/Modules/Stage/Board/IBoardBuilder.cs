using System.Collections;

namespace Cardinals.Board
{
    public interface IBoardBuilder
    {
        Tile StartTile { get; }

        void Load(BoardData boardData);
        IEnumerator LoadWithAnimation(BoardData boardData, float animationDelay=0.1f);

        void Clear();
    }
}