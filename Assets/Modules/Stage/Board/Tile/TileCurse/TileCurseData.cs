using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cardinals.Enums;

namespace Cardinals.Board {

	public class TileCurseData {
		public int TargetTurn => _targetTurn;
		public Action Action => _curseAction;

		private TileCurseType _curseType;
		private int _targetTurn;
		private Action _curseAction;

		public TileCurseData(TileCurseType _curseType, int targetTurn, Action curseAction) {
			_targetTurn = targetTurn;
			_curseAction = curseAction;
		}
	}

}
