using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerMouse : MonoBehaviour
{
    [SerializeField] private int playerNum;
    [SerializeField] private GameManager manager;
    [SerializeField] private GraphicRaycaster raycaster;

    [SerializeField] private float mouseSpeed;
    [SerializeField] private Image mouseSprite;
    [SerializeField] private PlayerDisplay display;

    [SerializeField] private PlayerControl controls;
    [SerializeField] private bool activated;

    [SerializeField] private EventSystem eventSystem;
    private PointerEventData pointer;
    private Camera cam;
    private RectTransform rectPos;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        mouseSprite.enabled = false;
        cam = Camera.main;
        rectPos = GetComponent<RectTransform>();

        if (manager.roundData.activePlayers[playerNum])
            display.SelectCharacter(manager.roundData.character[playerNum]);
        else
        {
            display.NoCharacter(playerNum);
        }
            
        

        //raycaster = new GraphicRaycaster();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated && (Input.GetKeyDown(controls.abilityPrimary) || manager.roundData.activePlayers[playerNum]))
        {
            ActivateMouse();
        }
        else if (activated)
        {
            if (!manager.roundData.activePlayers[playerNum])
            {
                DisableMouse();
            }

            Vector3 inp = new Vector2((Input.GetKey(controls.right) ? 1 : 0) + (Input.GetKey(controls.left) ? -1 : 0), (Input.GetKey(controls.up) ? 1 : 0) + (Input.GetKey(controls.down) ? -1 : 0));
            rectPos.position += inp * Time.deltaTime * mouseSpeed;

            Vector2 bound = CameraManager.cameraBounds;
            rectPos.position = new Vector2(Mathf.Clamp(transform.position.x, -bound.x, bound.x), Mathf.Clamp(transform.position.y, -bound.y, bound.y));
            
            pointer = new PointerEventData(eventSystem);
            pointer.position = cam.WorldToScreenPoint(transform.position);
                //Input.mousePosition;

            List<RaycastResult> result = new List<RaycastResult>();
            raycaster.Raycast(pointer, result); 

            if (result.Count > 0)
            {
                //Debug.Log("Working");
                //result[0].gameObject.GetComponent<Image>().color = Color.black;
                AbilityMenuDisplay ability = result[0].gameObject.GetComponent<AbilityMenuDisplay>();
                if (ability)
                {
                    Ability data = ability.GetAbility();

                    display.InspectAbility(data);
                    if (Input.GetKeyDown(controls.abilityPrimary))
                    {
                        manager.SelectAbility(playerNum, data);
                        display.SelectAbility(data);
                    }
                }
                else
                {
                    display.StopInspection();
                }

                CharacterMenuDisplay character = result[0].gameObject.GetComponent<CharacterMenuDisplay>();
                if (character)
                {
                    CharacterData data = character.GetCharacter();

                    if (display.currentCharacter != data)
                    {
                        display.InspectCharacter(data);
                    }

                    if (Input.GetKeyDown(controls.abilityPrimary))
                    {
                        manager.SelectCharacter(playerNum, data);
                        display.SelectCharacter(data);
                    }

                    
                    //display.InspectAbility(ability.GetAbility());
                }

                Button button = result[0].gameObject.GetComponent<Button>();
                if (button && Input.GetKeyDown(controls.abilityPrimary))
                {
                    button.onClick.Invoke();
                }
            }
            else
            {
                display.StopInspection();
            }
        }
    }

    public void ActivateMouse()
    {
        transform.DOKill();
        mouseSprite.DOKill();

        activated = true;
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 2f).SetEase(Ease.OutSine);
        mouseSprite.enabled = true;
        mouseSprite.transform.localScale = Vector3.one * 10f;
        mouseSprite.transform.DOScale(Vector3.one, 2.5f).SetEase(Ease.OutBounce);

        display.SelectCharacter(manager.roundData.character[playerNum]);
        display.SelectAbility(manager.roundData.playerAbilityPrimary[playerNum]);
        manager.roundData.activePlayers[playerNum] = true;
    }

    public void DisableMouse()
    {
        transform.DOKill();
        mouseSprite.DOKill();
        activated = false;
        manager.roundData.activePlayers[playerNum] = false;
        transform.DOScale(0, .5f).SetEase(Ease.OutSine);

        display.NoCharacter(playerNum);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        AbilityMenuDisplay ability = collision.GetComponent<AbilityMenuDisplay>();
        if (ability)
        {
            //display.InspectAbility();
        }
    }
}
