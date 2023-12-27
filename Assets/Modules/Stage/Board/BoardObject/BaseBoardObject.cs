using System.Collections;
using UnityEngine;
using Cardinals.Board;
using Cardinals.BoardEvent;
using Cardinals.Enums;
using Cardinals.UI;
using Cardinals.UI.Description;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using Util;

namespace Cardinals.BoardObject
{
    /// <summary>
    /// 보드 위에 존재하는 소환수
    /// </summary>
    public class BaseBoardObject : MonoBehaviour, IMovingBoardObject
    {
        protected NewBoardObjectDataSO _data;
        public Tile OnTile => _onTile;
        
        [Header("Information")]
        protected Tile _onTile;
        [SerializeField] private int _lifeCount;
        [SerializeField] private float _moveDuration = .2f;
        
        [Header("Component")]
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected Animator _animator;

        [Header("UI Component")]
        private ObjectGetter _moveValueObject = new(TypeOfGetter.ChildByName, "Renderer/Canvas/Grid/MoveValue");
        private ComponentGetter<TextMeshProUGUI> _moveValueTMP = new(TypeOfGetter.ChildByName, "Renderer/Canvas/Grid/MoveValue/TMP");
        private ObjectGetter _turnCountObject = new(TypeOfGetter.ChildByName, "Renderer/Canvas/Grid/TurnCount");
        private ComponentGetter<TextMeshProUGUI> _turnCountTMP = new(TypeOfGetter.ChildByName, "Renderer/Canvas/Grid/TurnCount/TMP");

        /// <summary>
        /// 보드에 생성되는 이벤트 초기화
        /// </summary>
        public virtual void Init(Tile tile, string type)
        {
            // 기본 설정
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            
            GameManager.I.Stage.BoardObjects.Add(this);
            _onTile = tile;
            transform.position = GetPosition(_onTile.gameObject.transform.position);

            // 데이타 설정            
            _data = ResourceLoader.LoadSO<NewBoardObjectDataSO>(Constants.FilePath.Resources.SO_BoardObjectData + type);
            _lifeCount = _data.keepTurnCount;
            _renderer.sprite = _data.sprite;
            
            // UI 설정
            if (_data.onBoardType == NewBoardEventOnType.Move)
                _moveValueTMP.Get(gameObject).text = $"{_data.moveCount}";
            else 
                _moveValueObject.Get(gameObject).SetActive(false);

            if (_data.keepTurnCount > 0)
                _turnCountTMP.Get(gameObject).text = $"{_data.keepTurnCount}";
            else 
                _turnCountObject.Get(gameObject).SetActive(false);
            GetComponent<GridSizeUpdator>().Resizing();


            // 설명창 설정
            var des = transform.AddComponent<BoardObjectDescription>();
            des.Init(_data);
            GetComponent<DescriptionConnector>().Init();
        }
        protected virtual Vector3 GetPosition(Vector3 vector) => vector + new Vector3(0, Random.Range(1, 1.3f), 0);

        public virtual IEnumerator OnTurn()
        {
            if (_data.onBoardType == NewBoardEventOnType.Move)
            {
                var playerTile = GameManager.I.Player.OnTile;
                Tile tile = _onTile;

                for (int i = 0; i < _data.moveCount; i++)
                {
                    tile = tile.Next;
                    transform.DOMove(GetPosition(tile.transform.position), _moveDuration);
                    yield return new WaitForSeconds(_moveDuration);

                    if (playerTile == tile) // 이동했는데 플레이어 타일
                    {
                        yield return OnCollisionPlayer();
                        break;
                    }
                }

                _onTile = tile;
            }

            _lifeCount--;
            _turnCountTMP.Get(gameObject).text = $"{_lifeCount}";
            if (_lifeCount == 0)
            {
                _turnCountTMP.Get(gameObject).DOColor(Color.red, .5f).SetEase(Ease.InBounce);
                transform.DOLocalMoveY(-1, .5f).SetEase(Ease.InBounce)
                    .OnComplete(Destroy);
            }
        }
        
        public virtual IEnumerator OnCollisionPlayer()
        {
            if (_data.executeType == NewBoardEventExecuteType.Touch)
            {
                yield return Execute();
                Destroy();
            }
        }
        
        public virtual IEnumerator ArrivePlayer()
        {
            if (_data.executeType == NewBoardEventExecuteType.Arrive)
            {
                yield return Execute();
                Destroy();
            }
        }

        public void Destroy()
        {
            GameManager.I.Stage.BoardObjects.Remove(this);
            Destroy(gameObject);
        }

        protected virtual IEnumerator Execute()
        {
            yield return null;
        }
    }
}