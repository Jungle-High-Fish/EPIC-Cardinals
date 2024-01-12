using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Util {
    public static class ResourceLoader {

		private static Dictionary<string, GameObject> _prefabList = new Dictionary<string, GameObject>();

		private static Dictionary<string, Material> _materialList = new Dictionary<string, Material>();
		
		private static Dictionary<string, Sprite> _spriteList = new Dictionary<string, Sprite>();
		private static Dictionary<string, ScriptableObject> _SOList = new ();
		private static Dictionary<string, TMP_FontAsset> _fontList = new Dictionary<string, TMP_FontAsset>();
		private static HashSet<string> _loadedSpriteDirectory = new HashSet<string>();

		private static Dictionary<string, RuntimeAnimatorController> _animatorControllerList = new Dictionary<string, RuntimeAnimatorController>();

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

		public static Material LoadMaterial(string materialName)
		{
			string targetPath = Cardinals.Constants.FilePath.Resources.Materials + materialName;

			if (_materialList.ContainsKey(materialName))
			{
				return _materialList[materialName];
			}
			_materialList.Add(materialName, Resources.Load<Material>(targetPath));
			return _materialList[materialName];
		}

		public static TMP_FontAsset LoadFont(string fontName)
		{
			string targetPath = Cardinals.Constants.FilePath.Resources.Fonts + fontName;

			if (_fontList.ContainsKey(fontName))
			{
				return _fontList[fontName];
			}
			_fontList.Add(fontName, Resources.Load<TMP_FontAsset>(targetPath));
			return _fontList[fontName];
		}

		public static T LoadSO<T>(string soName) where T : ScriptableObject
		{
			string targetPath = Cardinals.Constants.FilePath.Resources.SO + soName;

			if (!_SOList.ContainsKey(soName))
			{
				_SOList.Add(soName, Resources.Load<T>(targetPath));
			}
			return _SOList[soName] as T;
		}

		public static Dictionary<string, Sprite> LoadSpritesInDirectory(string directoryName) {
			string targetPath = Cardinals.Constants.FilePath.Resources.Sprites + directoryName;

			if (_loadedSpriteDirectory.Contains(directoryName)) {
				var targetDict = 
					_spriteList.Where(x => x.Key.Contains(directoryName)).ToDictionary(x => x.Key, x => x.Value);

				return targetDict;
			}

			var sprites = Resources.LoadAll<Sprite>(targetPath);
			//var spriteDict = new Dictionary<string, Sprite>();
			foreach (var sprite in sprites) {
				string combinedPath = Path.Combine(directoryName, sprite.name);
				//spriteDict.Add(sprite.name, sprite);
				_spriteList.Add(combinedPath, sprite);
			}

			_loadedSpriteDirectory.Add(directoryName);
			return _spriteList;
		}

		public static RuntimeAnimatorController LoadAnimatorController(string controllerName) {
			string targetPath = Cardinals.Constants.FilePath.Resources.AnimatorControllers + controllerName;

			if (_animatorControllerList.ContainsKey(controllerName)) {
				return _animatorControllerList[controllerName];
			}

			_animatorControllerList.Add(controllerName, Resources.Load<RuntimeAnimatorController>(targetPath));
			return _animatorControllerList[controllerName];
		}
    }
}