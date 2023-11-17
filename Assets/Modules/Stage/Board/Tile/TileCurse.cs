using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Board {

	public class TileCurse: MonoBehaviour {
		public TileCurseData Data => _data;
		public bool IsActive => _data != null;
		
		private TileCurseData _data;

		private int _passedTurn = 0;
		
		public void Init(TileCurseData data=null) {
			_data = data;
		}

		public void SetCurse(TileCurseData data) {
			_data = data;
			_passedTurn = 0;
		}

		public bool OnTurnEnd() {
			if (_data == null) {
				return true;
			}

			_passedTurn++;
			if (_passedTurn >= _data.TargetTurn) {
				_data.Action();
				_data = null;
				return true;
			}

			return false;
		}

		public void ClearCurse() {
			_data = null;
		}
	}

}
