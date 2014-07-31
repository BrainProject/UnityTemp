using UnityEngine;
using System.Collections;

#if UNITY_STANDALONE
namespace Kinect {
	public class GrabDropScript : MonoBehaviour 
	{
		public GameObject[] draggableObjects;
		public float dragSpeed = 3.0f;
		public Material selectedObjectMaterial;
		
		private InteractionManager manager;
		private bool isLeftHandDrag;

		private GameObject draggedObject;
		private float draggedObjectDepth;
		private Vector3 draggedObjectOffset;
		private Material draggedObjectMaterial;
		
		GameObject infoGUI;
		
		
		void Awake() 
		{
			manager = Camera.mainCamera.GetComponent<InteractionManager>();
			infoGUI = GameObject.Find("HandGuiText");
		}
		
		
		void Update() 
		{
			if(manager != null && manager.IsInteractionInited())
			{
				Vector3 screenNormalPos = Vector3.zero;
				Vector3 screenPixelPos = Vector3.zero;
				
				if(draggedObject == null)
				{
					// no object is currently selected or dragged.
					// if there is a hand grip, try to select the underlying object and start dragging it.
					if(manager.IsLeftHandPrimary())
					{
						// if the left hand is primary, check for left hand grip
						if(manager.GetLastLeftHandEvent() == InteractionWrapper.InteractionHandEventType.Grip)
						{
							isLeftHandDrag = true;
							screenNormalPos = manager.GetLeftHandScreenPos();
						}
					}
					else if(manager.IsRightHandPrimary())
					{
						// if the right hand is primary, check for right hand grip
						if(manager.GetLastRightHandEvent() == InteractionWrapper.InteractionHandEventType.Grip)
						{
							isLeftHandDrag = false;
							screenNormalPos = manager.GetRightHandScreenPos();
						}
					}
					
					// check if there is an underlying object to be selected
					if(screenNormalPos != Vector3.zero)
					{
						// convert the normalized screen pos to pixel pos
						screenPixelPos.x = (int)(screenNormalPos.x * Camera.mainCamera.pixelWidth);
						screenPixelPos.y = (int)(screenNormalPos.y * Camera.mainCamera.pixelHeight);
						Ray ray = Camera.mainCamera.ScreenPointToRay(screenPixelPos);
						
						// check for underlying objects
						RaycastHit hit;
						if(Physics.Raycast(ray, out hit))
						{
							foreach(GameObject obj in draggableObjects)
							{
								if(hit.collider.gameObject == obj)
								{
									// an object was hit by the ray. select it and start drgging
									draggedObject = obj;
									draggedObjectDepth = draggedObject.transform.position.z - Camera.main.transform.position.z;
									draggedObjectOffset = hit.point - draggedObject.transform.position;
									
									// set selection material
									draggedObjectMaterial = draggedObject.renderer.material;
									draggedObject.renderer.material = selectedObjectMaterial;
									break;
								}
							}
						}
					}
					
				}
				else
				{
					// continue dragging the object
					screenNormalPos = isLeftHandDrag ? manager.GetLeftHandScreenPos() : manager.GetRightHandScreenPos();
					
					// check if there is pull-gesture
					bool isPulled = isLeftHandDrag ? manager.IsLeftHandPull(true) : manager.IsRightHandPull(true);
					if(isPulled)
					{
						// set object depth to its original depth
						draggedObjectDepth = -Camera.main.transform.position.z;
					}
					
					// convert the normalized screen pos to 3D-world pos
					screenPixelPos.x = (int)(screenNormalPos.x * Camera.mainCamera.pixelWidth);
					screenPixelPos.y = (int)(screenNormalPos.y * Camera.mainCamera.pixelHeight);
					screenPixelPos.z = screenNormalPos.z + draggedObjectDepth;
					
					Vector3 newObjectPos = Camera.mainCamera.ScreenToWorldPoint(screenPixelPos) - draggedObjectOffset;
					draggedObject.transform.position = Vector3.Lerp(draggedObject.transform.position, newObjectPos, dragSpeed * Time.deltaTime);
					
					// check if the object (hand grip) was released
					bool isReleased = isLeftHandDrag ? (manager.GetLastLeftHandEvent() == InteractionWrapper.InteractionHandEventType.Release) :
						(manager.GetLastRightHandEvent() == InteractionWrapper.InteractionHandEventType.Release);
					
					if(isReleased)
					{
						// restore the object's material and stop dragging the object
						draggedObject.renderer.material = draggedObjectMaterial;
						draggedObject = null;
					}
				}
			}
		}
		
		void OnGUI()
		{
			if(infoGUI != null && manager != null && manager.IsInteractionInited())
			{
				string sInfo = string.Empty;
				
				uint userID = manager.GetUserID();
				if(userID != 0)
				{
					if(draggedObject != null)
						sInfo = "Dragging the " + draggedObject.name + " around.";
					else
						sInfo = "Please grab and drag an object around.";
				}
				else
				{
					sInfo = "Waiting for Users...";
				}
				
				infoGUI.guiText.text = sInfo;
			}
		}
	}
}
#endif