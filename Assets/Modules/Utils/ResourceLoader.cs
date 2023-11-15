using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util {
    public static class ResourceLoader {

		private static Dictionary<string, GameObject> _prefabList = new Dictionary<string, GameObject>();
		private static Dictionary<string, Sprite> _spriteList = new Dictionary<string, Sprite>();

		public static GameObject LoadPrefab(string prefabName)
		{
			string targetPath = Cardinals.Constants.FilePath.Resources.Prefabs + prefabName;

			if (_prefabList.ContainsKey(prefabName))
			{
				return _prefabList[prefabName];
			}
			_prefabList.Add(prefabName, Resources.Load<GameObject>(targetPath));
			return _prefabList[prefabName];
		}

		
		public static Sprite LoadSprite(string spriteName)
		{
			string targetPath = Cardinals.Constants.FilePath.Resources.Sprites + spriteName;

			if (_spriteList.ContainsKey(spriteName))
			{
				return _spriteList[spriteName];
			}
			_spriteList.Add(spriteName, Resources.Load<Sprite>(targetPath));
			return _spriteList[spriteName];
		}
    }
}