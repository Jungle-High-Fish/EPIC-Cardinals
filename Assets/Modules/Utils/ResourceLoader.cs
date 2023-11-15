using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Util {
    public static class ResourceLoader {

		private static Dictionary<string, GameObject> _prefabList = new Dictionary<string, GameObject>();
		private static Dictionary<string, Sprite> _spriteList = new Dictionary<string, Sprite>();
		private static HashSet<string> _loadedSpriteDirectory = new HashSet<string>();

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

		public static Dictionary<string, Sprite> LoadSpritesInDirectory(string directoryName) {
			string targetPath = Cardinals.Constants.FilePath.Resources.Sprites + directoryName;

			if (_loadedSpriteDirectory.Contains(directoryName)) {
				var targetDict = 
					_spriteList.Where(x => x.Key.Contains(directoryName)).ToDictionary(x => x.Key, x => x.Value);

				return targetDict;
			}

			var sprites = Resources.LoadAll<Sprite>(targetPath);
			var spriteDict = new Dictionary<string, Sprite>();
			foreach (var sprite in sprites) {
				string combinedPath = Path.Combine(directoryName, sprite.name);
				spriteDict.Add(sprite.name, sprite);
				_spriteList.Add(combinedPath, sprite);
			}
			return spriteDict;
		}
    }
}