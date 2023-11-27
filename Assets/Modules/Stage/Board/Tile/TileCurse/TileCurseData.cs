using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cardinals.Enums;
using Util;

namespace Cardinals.Board {

	public abstract class TileCurseData
	{
		private Tile _baseTile;
		protected Tile BaseTile => _baseTile;
		public int TargetTurn
		{
			get => _targetTurn;
			protected set => _targetTurn = value;
		}

		public Action Action
		{
			get => _curseAction;
			protected set => _curseAction = value;
		}

		private TileCurseType _curseType;
		private int _targetTurn;
		private Action _curseAction;
		
		protected TileCurseData(TileCurseType _curseType, int targetTurn = 0, Action curseAction = null) {
			_targetTurn = targetTurn;
			_curseAction = curseAction;
		}

		public void Init(Tile tile, int turn)
		{
			_baseTile = tile;
			TargetTurn = turn;
		}

		public static TileCurseUIDataSO Data(TileCurseType curseType) {
			return ResourceLoader.LoadSO<TileCurseUIDataSO>(
                Constants.FilePath.Resources.SO_TileCurseUIData + curseType
            );
		}
	}

}
