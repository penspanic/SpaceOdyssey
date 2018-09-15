using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace DefaultNamespace
{
	public class dasd : MonoBehaviour
	{
#if UNITY_EDITOR
		[MenuItem("Tools/PlayerPrefs/DeleteAll")]
		public static void PlayerPrefsDeleteAll() {
			PlayerPrefs.DeleteAll();
		}
#endif	
	}
	
}