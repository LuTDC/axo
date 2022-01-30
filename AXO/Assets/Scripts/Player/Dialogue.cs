using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    private bool isSnake = false;
    private bool isCarp = false;
    private bool isMouse = false;
    private bool isSnail = false;
    private bool isFrog = false;

    private Player player;

    private GameObject npc = null;

    [SerializeField]
    private TextMeshProUGUI dialogue;
    [SerializeField]
    private GameObject textBox;

    private string[] sentences = new string[2];
    private int index = 0;

    private bool isDialogue = false;
    private bool isInitial = true;
    private bool canPass = true;

    [SerializeField]
    private GameObject[] spheres = new GameObject[4];

    private bool isSkillPopup = false;

    private int skillIndex = 0;

    [SerializeField]
    private GameObject waterDialogue, darkDialogue;

    [SerializeField]
    private Animator fade;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        dialogue.gameObject.SetActive(true);
        textBox.SetActive(true);

        player.SetDialogue(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Talk")){
            if(isSnake) SnakeDialogue();
            else if(isCarp) CarpDialogue();
            else if(isMouse) MouseDialogue();
            else if(isSnail) SnailDialogue();
            else if(isFrog) FrogDialogue();
        }

        if(isInitial && Input.GetButton("Talk")){
            EndDialogue();
            isInitial = false;
        }

        if(isDialogue) DialogueController();

        if(isSkillPopup && Input.GetButton("Talk") && canPass) CloseSkillPopup();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Snake") isSnake = true;
        if(other.tag == "Carp") isCarp = true;
        if(other.tag == "Mouse") isMouse = true;
        if(other.tag == "Snail") isSnail = true;
        if(other.tag == "Frog") isFrog = true;

        if(isCarp || isFrog || isMouse || isSnail || isSnake) npc = other.gameObject;

        if(other.tag == "WaterEdge" && !player.GetSkills()[0]) WaterBox();
        if(other.tag == "DarkEdge" && !player.GetSkills()[2]) DarkBox();
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Snake") isSnake = false;
        if(other.tag == "Carp") isCarp = false;
        if(other.tag == "Mouse") isMouse = false;
        if(other.tag == "Snail") isSnail = false;
        if(other.tag == "Frog") isFrog = false;

        npc = null;
    }

    private void StartDialogue(){
        StartCoroutine(DialogueWaiter());

        dialogue.text = sentences[0];

        isDialogue = true;

        player.SetDialogue(true);
        npc.GetComponent<Collider2D>().enabled = false;

        dialogue.gameObject.SetActive(true);
        textBox.SetActive(true);

        index = 0;
    }

    private void EndDialogue(){
        if(skillIndex >= 4) EndGame();
        else{
            player.SetDialogue(false);

            isDialogue = false;

            dialogue.gameObject.SetActive(false);
            textBox.SetActive(false);

            if(!isInitial && skillIndex < 4) GetSkill();
            StartCoroutine(DialogueWaiter());
        }
    }

    private void SnakeDialogue(){
        sentences[0] = "Hello, my friend Snake! Why do you like to live on land?";
        sentences[1] = "Friend Axolotl, only on land can you enjoy the sunshine and feel the warmth on your skin. Why would I ever want to live under the water?";

        StartDialogue();
    }
    
    private void CarpDialogue(){
        sentences[0] = "Hello, my friend Carp! Why do you like to live under water?";
        sentences[1] = "Friend Axolotl, only under water can you enjoy the fresh flow on your skin in the most hot days. Why would I ever want to live on land?";

        StartDialogue();
    }

    private void MouseDialogue(){
        sentences[0] = "Hello, my friend Mouse! Why do you like to live on land?";
        sentences[1] = "Friend Axolotl, only on land can you breath the breeze that blows through the air. Why would I ever want to live under the water?";

        StartDialogue();
    }

    private void SnailDialogue(){
        sentences[0] = "Hello, my friend Snail! Why do you like to live under water?";
        sentences[1] = "Friend Axolotl, only under water can you be your own in the most dark deepness and have some space for yourself. Why would I ever want to live on land?";

        StartDialogue();
    }

    private void FrogDialogue(){
        sentences[0] = "Hello, my friend Frog! I don't know what to do anymore! How can you be so happy and wise being a two worlds animal?";
        sentences[1] = "Friend Axolotl, because of my nature I own friends on both worlds. I enjoy the sun and the breeze, as the water waves and the deep darkness. Why wouldn't I want to be who I am?";

        StartDialogue();
    }

    private void DialogueController(){
        if(Input.GetButton("Talk") && canPass){
            index++;

            if(index >= 2) EndDialogue();
            else{
                dialogue.text = sentences[index];
                StartCoroutine(DialogueWaiter());
            }
        }
    }

    private IEnumerator DialogueWaiter(){
        canPass = false;

        yield return new WaitForSeconds(1);

        canPass = true;
    }

    private void GetSkill(){
        spheres[skillIndex].SetActive(true);
        player.SetNewSkill(skillIndex);

        skillIndex++;

        player.SetDialogue(true);
        isSkillPopup = true;
    }

    private void CloseSkillPopup(){
        isSkillPopup = false;

        player.SetDialogue(false);

        for(int i = 0; i < 4; i++) spheres[i].SetActive(false);
    }

    public void WaterBox(){
        StartCoroutine(ActivateWaterBox());   
    }

    public void DarkBox(){
        StartCoroutine(ActivateDarkBox());   
    }

    private IEnumerator ActivateWaterBox(){
        waterDialogue.SetActive(true);
        player.ReturnHome();
        player.SetDialogue(true);

        yield return new WaitForSeconds(3);

        player.SetDialogue(false);
        waterDialogue.SetActive(false);
    }

    private IEnumerator ActivateDarkBox(){
        darkDialogue.SetActive(true);
        player.ReturnHome();
        player.SetDialogue(true);

        yield return new WaitForSeconds(3);

        player.SetDialogue(false);
        darkDialogue.SetActive(false);
    }

    private void EndGame(){
        fade.SetTrigger("Fade");
    }
}
