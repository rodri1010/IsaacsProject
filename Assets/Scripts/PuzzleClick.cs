using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleClick : MonoBehaviour, IPointerClickHandler
{

    private Image image;
    private RectTransform rectTransform;
    public GameManager GameManager;
    public GameObject TheGameController;
    public LevelScene LevelScene;
    public GameObject LevelController;

    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        TheGameController = GameObject.Find("GameManager");
        GameManager = TheGameController.GetComponent<GameManager>();
        if(!GameManager.transitionSceneOn)
        {
            LevelController = GameObject.Find("LevelScene");
            LevelScene = LevelController.GetComponent<LevelScene>();
        }
        

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 rectCoords;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out rectCoords))
        {
            // Normalize to (0,1) coordinate space
            Vector2 normCoords = new Vector2(rectCoords.x / rectTransform.rect.width, rectCoords.y / rectTransform.rect.height);
            normCoords += rectTransform.pivot; // Correct for pivot location (in same coordinate space)
            //Debug.Log($"Norm: {normCoords}");

            // Get pixel color under click
            Color pixel = image.sprite.texture.GetPixelBilinear(normCoords.x, normCoords.y);
            /* not needed, use UV (0,1) coordinates with GetPixelBilinear
            // Get actual texture coordinates
            Vector2 texCoords = new Vector2(normCoords.x * texture.width, normCoords.y * texture.height);
            Debug.Log($"Tex: {texCoords}");
            Debug.Log(texture.GetPixel((int)texCoords.x, (int)texCoords.y));
            */

            // Check if clicked on the hidden object
            if (pixel.grayscale > 0)
            {
               if(GameManager.transitionSceneOn)
               {
                   GameManager.TransitionToGame();
               }
               else
               {   
                   LevelScene.CorrectClick();
               }
            }
            else
            {
                if(GameManager.transitionSceneOn)
                {
                    
                }
                else
                {
                    LevelScene.WrongClick();
                }
            }
        }
    }
}
