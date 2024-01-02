using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Board {

	public class TileEffect: MonoBehaviour {
		public TileEffectData Data => _data;
		public bool IsActive => _data != null;
		
		private TileEffectData _data;
		
		private int _passedTurn = 0;
		
		public void Init(TileEffectData data=null) {
			_data = data;
		}

		public void SetEffect(TileEffectData data) {
			_data = data;
			_passedTurn = 0;
		}

		public void StepOnAction(IBoardPiece piece) {
			_data?.StepOnAction(piece);
		}

		public void ArriveAction(IBoardPiece piece) {
			_data?.ArriveAction(piece);
		}

		public bool OnTurnEnd() {
			if (_data == null) {
				return true;
			}

			_passedTurn++;
			if (_passedTurn >= _data.TargetTurn) {
				_data.EffectEndAction();
				_data = null;
				return true;
			}

			return false;
		}

		public void ClearEffect() {
			_data = null;
		}
	}

}
