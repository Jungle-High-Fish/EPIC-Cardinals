using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinals.Board {

	public class TileEffectData {
		public int TargetTurn => _targetTurn;
		public Action<IBoardPiece> ArriveAction => _arriveAction;
		public Action<IBoardPiece> StepOnAction => _stepOnAction;
		public Action EffectEndAction => _effectEndAction;

		private int _targetTurn;
		private Action<IBoardPiece> _arriveAction;
		private Action<IBoardPiece> _stepOnAction;
		private Action _effectEndAction;

		public TileEffectData(
			int targetTurn, 
			Action<IBoardPiece> arriveAction, 
			Action<IBoardPiece> stepOnAction, 
			Action effectEndAction
		) {
			_targetTurn = targetTurn;
			_arriveAction = arriveAction;
			_stepOnAction = stepOnAction;
			_effectEndAction = effectEndAction;
		}
	}

}
