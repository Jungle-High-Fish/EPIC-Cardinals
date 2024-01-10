using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Board {

	public class TileCurse: MonoBehaviour {
		public TileCurseData Data => _data;
		public bool IsActive => _data != null;
		public int PassedTurn => _passedTurn;
		
		private TileCurseData _data;

		private int _passedTurn = 0;
		
		public void Init(TileCurseData data=null) {
			_data = data;
		}

		public void SetCurse(TileCurseData data) {
			_data = data;
			_passedTurn = 0;
		}

        private bool _turnEndResult;
        public bool TurnEndResult => _turnEndResult;
		public IEnumerator OnTurnEnd()
        {
            _turnEndResult = false;
			if (_data == null)
            {
                _turnEndResult = true;
            }
            else
            {
                _passedTurn++;
                if (_passedTurn >= _data.TargetTurn) {
                    yield return _data.Action;
                
                    _data = null;
                    _turnEndResult = true;
                }
            }

            yield return null;
        }

		public void ClearCurse() {
			_data = null;
		}
	}

}
