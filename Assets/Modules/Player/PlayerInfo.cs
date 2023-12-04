using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Cardinals.Enums;
using Sirenix.OdinInspector;
using Cardinals.Game;

namespace Cardinals
{
    public class PlayerInfo 
    {
        public int Gold { get; private set; }
        
        private List<Potion> _potions;
        public Action<int, Potion> AddPotionEvent { get; set; }
        public Action<int, Potion> DeletePotionEvent { get; set; }
        public Action<int> UpdateGoldEvent { get; set; }
        public List<BlessType> BlessList => _blessList;
        
        public Dictionary<BlessType, Action> BlessEventDict = new();
        public List<Artifact> ArtifactList => _artifacts;
        public List<Potion> PotionList => _potions;

        private List<Artifact> _artifacts;
        public Action<Artifact> AddArtifactEvent { get; set; }
        public Action<BlessType> AddBlessEvent { get; set; }
        public PlayerInfo()
        {
            _potions = new();
            for(int i=0; i < Constants.GameSetting.Player.MaxPotionCapacity; i++)
            {
                _potions.Add(null);
            }

            _artifacts = new();
            for(int i=0; i < Constants.GameSetting.Player.MaxArtifactCapacity; i++)
            {
                _artifacts.Add(null);
            }

            foreach (var type in Enum.GetValues(typeof(BlessType)).Cast<BlessType>())
            {
                BlessEventDict.Add(type, new Action(() => { }));
            }
        }

        private List<BlessType> _blessList = new();
        
        public bool CheckBlessExist(BlessType blessType)
        {
            return _blessList.Any(bless => bless == blessType);
        }
        
        [Button]
        public void GetBless(BlessType blessType)
        {
            _blessList.Add(blessType);
            AddBlessEvent?.Invoke(blessType);
        }
       
        public void AddPotion(PotionType potionType)
        {
            for (int i = 0; i < Constants.GameSetting.Player.MaxPotionCapacity; i++)
            {
                if(_potions[i] == null)
                {
                    Potion potion = EnumHelper.GetPotion(potionType);
                    _potions[i] = potion;
                    AddPotionEvent?.Invoke(i, potion);
                    break;
                }
            }
        }

        public void DeletePotion(int index)
        {
            Potion potion = _potions[index];
            _potions[index] = null;
            DeletePotionEvent?.Invoke(index, potion);
        }

        public bool UsePotion(int index)
        {
            if (_potions[index].UsePotion())
            {
                DeletePotion(index);
                return true;
            }

            return false;
        }

        public void AddArtifact(ArtifactType artifactType)
        {
            for (int i = 0; i < Constants.GameSetting.Player.MaxArtifactCapacity; i++)
            {
                if (_artifacts[i] == null&&!CheckArtifactExist(artifactType))
                {
                    Artifact artifact = EnumHelper.GetArtifact(artifactType);
                    _artifacts[i] = artifact;
                    AddArtifactEvent?.Invoke(artifact);
                    break;
                }
            }
        }

        public bool CheckArtifactExist(ArtifactType artifactType)
        {
            foreach(Artifact artifact in _artifacts)
            {
                if (artifact == null)
                {
                    continue;
                }
                if(artifact.Type == artifactType)
                {
                    return true;
                }
            }
            return false;
        }

        public void DebugArtifactList()
        {
            foreach(Artifact artifact in _artifacts)
            {
                if (artifact == null)
                    continue;
                Debug.Log(artifact.Data().artifactName);
            }
        }
    

        public void AddGold(int value)
        {
            Gold += value;
            UpdateGoldEvent?.Invoke(Gold);
        }

        public void UseGold(int value)
        {
            Gold -= value;
            UpdateGoldEvent?.Invoke(Gold);
        }
    }
}
