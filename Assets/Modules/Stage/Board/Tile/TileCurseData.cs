using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Board {

	public class TileCurseData {
		public int TargetTurn => _targetTurn;
		public Action Action => _curseAction;

		private int _targetTurn;
		private Action _curseAction;

		public TileCurseData(int targetTurn, Action curseAction) {
			_targetTurn = targetTurn;
			_curseAction = curseAction;
		}
	}

}
