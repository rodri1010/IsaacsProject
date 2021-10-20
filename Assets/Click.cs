using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Click : MonoBehaviour
{
    Texture2D texture;
    Camera mainCamera;
    Dictionary<string, string> levels;
    float time = 0.0f;
    void Start()
    {
      texture = GetComponent<SpriteRenderer>().sprite.texture;
      levels = new Dictionary<string, string>();
      initializeDictionary();
    }

    // Update is called once per frame
    void Update()
    {
      time += Time.deltaTime;
      if(Input.GetMouseButtonDown(0)){
        var myColor = new Color();
        GetSpritePixelColorUnderMousePointer(GetComponent<SpriteRenderer>(),out myColor);
        if(myColor[0] == 1 && myColor[1] == 1 && myColor[2] == 1){
          Debug.Log(time);
          SceneManager.LoadScene(levels[GetComponent<SpriteRenderer>().sprite.name]);
        }
      }

    }
    public bool GetSpritePixelColorUnderMousePointer(SpriteRenderer spriteRenderer, out Color color) {
         color = new Color();
         Camera cam = Camera.main;
         Vector2 mousePos = Input.mousePosition;
         Vector2 viewportPos = cam.ScreenToViewportPoint(mousePos);
         if(viewportPos.x < 0.0f || viewportPos.x > 1.0f || viewportPos.y < 0.0f || viewportPos.y > 1.0f) return false; // out of viewport bounds
         Ray ray = cam.ViewportPointToRay(viewportPos);

         return IntersectsSprite(spriteRenderer, ray, out color);
     }
     private bool IntersectsSprite(SpriteRenderer spriteRenderer, Ray ray, out Color color) {
         color = new Color();
         if(spriteRenderer == null) return false;
         Sprite sprite = spriteRenderer.sprite;
         if(sprite == null) return false;
         Texture2D texture = sprite.texture;
         if(texture == null) return false;
         if(sprite.packed && sprite.packingMode == SpritePackingMode.Tight) {
             Debug.LogError("SpritePackingMode.Tight atlas packing is not supported!");
             return false;
         }
         Plane plane = new Plane(transform.forward, transform.position);
         float rayIntersectDist; 
         if(!plane.Raycast(ray, out rayIntersectDist)) return false; 
         Vector3 spritePos = spriteRenderer.worldToLocalMatrix.MultiplyPoint3x4(ray.origin + (ray.direction * rayIntersectDist));
         Rect textureRect = sprite.textureRect;
         float pixelsPerUnit = sprite.pixelsPerUnit;
         float halfRealTexWidth = texture.width * 0.5f; 
         float halfRealTexHeight = texture.height * 0.5f;
         int texPosX = (int)(spritePos.x * pixelsPerUnit + halfRealTexWidth);
         int texPosY = (int)(spritePos.y * pixelsPerUnit + halfRealTexHeight);
         if(texPosX < 0 || texPosX < textureRect.x || texPosX >= Mathf.FloorToInt(textureRect.xMax)) return false; // out of bounds
         if(texPosY < 0 || texPosY < textureRect.y || texPosY >= Mathf.FloorToInt(textureRect.yMax)) return false; // out of bounds
         color = texture.GetPixel(texPosX, texPosY);
         return true;
     }

    public void initializeDictionary()
    {
      levels.Add("ispy1", "Level2");
      levels.Add("ispy2", "Level3");
      levels.Add("ispy3", "Level4");
      levels.Add("ispy4", "Level5");
      levels.Add("ispy5", "Level6");
      levels.Add("ispy6", "Level7");
      levels.Add("ispy7", "Level8");
      levels.Add("ispy8", "Level9");
      levels.Add("ispy9", "Level10");
      levels.Add("ispy10", "Level11");
      levels.Add("ispy11", "Level12");
      levels.Add("ispy12", "Level13");
      levels.Add("ispy13", "Level14");
      levels.Add("mistery1", "Level15");
      levels.Add("mistery2", "Level16");
      levels.Add("mistery3", "Level17");
      levels.Add("mistery4", "Level18");
      levels.Add("mistery5", "Level19");
      levels.Add("mistery6", "Level20");
      levels.Add("mistery8", "Level21");
      levels.Add("mistery9", "Level22");
      levels.Add("mistery11", "Level23");
    }
}
