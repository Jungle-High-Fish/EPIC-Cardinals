using System;
using UnityEngine;

namespace Util {

public class ObjectGetter {
	private GameObject _gameObject;
	private string _path;
	private TypeOfGetter _TypeOfGetter;

	public ObjectGetter(TypeOfGetter TypeOfGetter, string path="") {
		_path = path;
		_TypeOfGetter = TypeOfGetter;
	}

	public GameObject Get(GameObject gameObject = null) {
		if (_gameObject != null) {
			return _gameObject;
		}

		switch (_TypeOfGetter) {
			case TypeOfGetter.This:
				_gameObject = gameObject;
				break;
			case TypeOfGetter.Child:
				_gameObject = gameObject.transform.GetChild(0).gameObject;
				break;
			case TypeOfGetter.Parent:
				_gameObject = gameObject.transform.parent.gameObject;
				break;
			case TypeOfGetter.Global:
				_gameObject = GameObject.Find(_path);
				break;
			case TypeOfGetter.ChildByName:
				_gameObject = gameObject.transform.Find(_path).gameObject;
				break;
			case TypeOfGetter.GlobalByName:
				_gameObject = GameObject.Find(_path);
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		return _gameObject;
	}
}

public class ComponentGetter<T> where T : UnityEngine.Object {
    #region PublicVariables
	#endregion

	#region PrivateVariables
	private T _object;
	private string _path;
	private TypeOfGetter _TypeOfGetter;
	#endregion

	#region PublicMethod
	public ComponentGetter(TypeOfGetter TypeOfGetter, string path="") {
		_path = path;
		_TypeOfGetter = TypeOfGetter;
	}

	public T Get(GameObject gameObject = null) {
		if (_object != null) {
			return _object;
		}

		switch (_TypeOfGetter) {
			case TypeOfGetter.This:
				_object = gameObject.GetComponent<T>();
				break;
			case TypeOfGetter.Child:
				_object = gameObject.GetComponentInChildren<T>();
				break;
			case TypeOfGetter.Parent:
				_object = gameObject.GetComponentInParent<T>();
				break;
			case TypeOfGetter.Global:
				_object = GameObject.FindObjectOfType<T>();
				break;
			case TypeOfGetter.ChildByName:
				_object = gameObject.transform.Find(_path).GetComponent<T>();
				break;
			case TypeOfGetter.GlobalByName:
				_object = GameObject.Find(_path).GetComponent<T>();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		return _object;
	}

	public void Clear()
    {
		_object = null;
    }
	#endregion
    
	#region PrivateMethod
	#endregion
}

public enum TypeOfGetter {
	This,
	Child,
	Parent,
	Global,
	ChildByName,
	GlobalByName
}

}