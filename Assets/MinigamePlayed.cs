using UnityEngine;
using System.Collections;

/// <summary>
/// Minigame played set status of current minigame to "played".
/// If "played", minigame sphere turns green with glow.
/// Simply put it in scene (there is a prefab with this script).
/// </summary>
/// 
/// \author Milan Doležal
/// 
/// \date 07-2014
public class MinigamePlayed : MonoBehaviour {
	void Start () {
		MGC.Instance.minigameStates.SetPlayed (Application.loadedLevelName);
		Destroy (this.gameObject);
	}
}
