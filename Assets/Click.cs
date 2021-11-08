using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Click : MonoBehaviour
{
    Texture2D texture;
    Camera mainCamera;
    float time = 0.0f;
    void Start()
    {
      texture = GetComponent<SpriteRenderer>().sprite.texture;
      // Debug.Log(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
      time += Time.deltaTime;
      if(Input.GetMouseButtonDown(0)){
        var myColor = new Color();
        GetSpritePixelColorUnderMousePointer(GetComponent<SpriteRenderer>(),out myColor);
        if(myColor[0] == 1 && myColor[1] == 1 && myColor[2] == 1){
          StartButton.levelNumber += 1;
          Log(SceneManager.GetActiveScene().name,texture.name,time.ToString("0.00") + " seconds");
          if(StartButton.levelNumber < 18){
            SceneManager.LoadScene(StartButton.levels[StartButton.levelNumber]);
          }
          else{
            SceneManager.LoadScene("End");
          }
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




     private void Log(params object[] args)
     {
         string line = string.Join(", ", args);
         line = DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss") + ", " + line;
         Debug.Log(line);

         if (StartButton.logWriter != null)
         {
             StartButton.logWriter.WriteLine(line);
             StartButton.logWriter.Flush();
         }
     }

     private void CloseLog()
     {
         if (StartButton.logWriter != null)
         {
             StartButton.logWriter.Flush();
             StartButton.logWriter.Close();
             StartButton.logWriter = null;
         }
     }


}
