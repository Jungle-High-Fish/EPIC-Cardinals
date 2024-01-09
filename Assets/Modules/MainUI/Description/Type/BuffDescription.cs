using System.Collections;
using System.Collections.Generic;
using Cardinals.Board;
using Cardinals.Buff;
using Cardinals.Enums;
using Cardinals.Game;
using UnityEngine;
using Util;

namespace Cardinals.UI.Description
{
    public class BuffDescription : BaseDescription, IDescription
    {
        private Growth _growth;
        private BuffDataSO _data;
 
        public void Init(BuffType type, Growth growth = null)
        {
            _growth = growth;
            _data = ResourceLoader.LoadSO<BuffDataSO>(Constants.FilePath.Resources.SO_BuffData + type);
        }

        public string Name => TMPUtils.CustomParse(_data.buffName,true);

        public string Description
        {
            get
            {
                if (_growth != null)
                {
                    var increaseText = GameManager.I.Localization.Get(LocalizationEnum.BUFF_GROWTH_EXPLAIN2)
                        .Replace("$1", _growth.IncreaseValue.ToString());
                    return _data.Description + increaseText;
                }
                else
                {
                    return _data.Description;
                }
            }
        }

        public Sprite IconSprite { get; }
        public Color Color { get; }

        public string Key
        {
            get
            {
                if (_growth != null)
                {
                    var key = $"{_data.type}_{_growth.IncreaseValue}";
                    Debug.Log(key);
                    return key;
                }
                else
                {
                    return _data.type.ToString();
                }
            }
        }
    }
}
