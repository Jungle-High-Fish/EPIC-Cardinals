using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cardinals.Enums;

namespace Cardinals
{
    public class Dice
    {
        private List<int> _diceNumbers;
        private DiceType _diceType;
        private int _rollResultNumber;
        private int _rollResultIndex;
        public Dice(List<int> numbers, DiceType type)
        {
            _diceNumbers = numbers.ToList();
            _diceType = type;
        }

        public List<int> DiceNumbers => _diceNumbers;
        public DiceType DiceType => _diceType;

        public int RollResultNumber { get; set; }
        public int RollResultIndex { get; set; }

    }
}
