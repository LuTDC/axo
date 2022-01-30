using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;

    [SerializeField]
    private ParticleSystem bubbles;

    private float speed = 5f;

    private float size;

    private bool isWater = false;
    private bool isWaterSurface = false;
    private bool isGround = true;

    private bool[] spheres = new bool[4];

    private bool isDialogue = false;

    private Vector2 checkpoint;

    [SerializeField]
    private UnityEngine.Experimental.Rendering.Universal.Light2D globalLight;
    [SerializeField]
    private UnityEngine.Experimental.Rendering.Universal.Light2D localLight;

    [SerializeField]
    private GameObject cloud;

    private bool isWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        size = transform.localScale.x;

        checkpoint = transform.position;

        for(int i = 0; i < 4; i++) spheres[i] = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDialogue){
            if(!isWater) Walk();
            else Swim();

            AllowJump();

            SoundController();
        }
        else rigidBody.velocity = new Vector2(0, 0);

        ParticleController();
    }

    private void Walk(){
        animator.SetBool("Swimming", false);

        float move = Input.GetAxis("Horizontal");

        rigidBody.velocity = new Vector2(speed*move, rigidBody.velocity.y);

        if(speed*move == 0){
            animator.SetBool("Walking", false);
            isWalking = false;
        }
        else{
            if(speed*move > 0) transform.localScale = new Vector2(size, size);
            else transform.localScale = new Vector2(size*-1, size);

            isWalking = true;
            animator.SetBool("Walking", true);
        }
    }

    private void Swim(){
        animator.SetBool("Walking", false);
        isWalking = false;

        float movex = Input.GetAxis("Horizontal");
        float movey = Input.GetAxis("Vertical");

        rigidBody.velocity = new Vector2(speed*movex, speed*movey);

        if(speed*movex == 0 && speed*movey == 0) animator.SetBool("Swimming", false);
        else{
            if(speed*movex >= 0) transform.localScale = new Vector2(size, size);
            else transform.localScale = new Vector2(size*-1, size);

            animator.SetBool("Swimming", true);
        }
    }

    private void AllowJump(){
        if((isWaterSurface && Input.GetAxis("Vertical") > 0) || (spheres[1] && Input.GetButton("Jump") && isGround)) Jump();
    }

    private void Jump(){
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);

        if(isWater){
            rigidBody.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
            isWater = false;

            speed = 5f;
        }
        else{
            rigidBody.AddForce(new Vector2(0, 7), ForceMode2D.Impulse);
            isGround = false;
        }

        animator.SetTrigger("Jump");
    }

    private void ParticleController(){
        if(!isWater) bubbles.Stop();
        else{
            if(!bubbles.isPlaying) bubbles.Play();

            if(rigidBody.velocity.x == 0) bubbles.transform.localRotation = Quaternion.Euler(0, 0, 0);
            else if(rigidBody.velocity.x > 0) bubbles.transform.localRotation = Quaternion.Euler(0, 0, 90);
            else if(rigidBody.velocity.x < 0) bubbles.transform.localRotation = Quaternion.Euler(0, 0, 79);
        }
    }

    public void SetDialogue(bool status){
        isDialogue = status;
    }

    public void SetNewSkill(int sphere){
        spheres[sphere] = true;

        if(sphere == 3) cloud.SetActive(false);
    }

    public bool[] GetSkills(){
        return spheres;
    }

    public void ReturnHome(){
        transform.position = checkpoint;
        isWater = false;
        localLight.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().enabled = false;
    }

    private IEnumerator TurnOffLight(){
        if(spheres[2]) localLight.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().enabled = true;
        while(globalLight.intensity > 0){
            yield return new WaitForSeconds(0.1f);
            globalLight.intensity -= 0.1f;
        }
    }

    private IEnumerator TurnOnLight(){
        if(spheres[2]) localLight.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().enabled = false;
        while(globalLight.intensity < 1){
            yield return new WaitForSeconds(0.1f);
            globalLight.intensity += 0.1f;
        }
    }

    private void SoundController(){
        if(!GetComponent<AudioSource>().isPlaying && isWalking) GetComponent<AudioSource>().Play();
        else if(GetComponent<AudioSource>().isPlaying && !isWalking) GetComponent<AudioSource>().Stop();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Water"){
            isWater = true;
            isWaterSurface = true;

            speed = 3f;
        }
        if(other.tag == "Ground") isGround = true;
        if(other.tag == "Dark") StartCoroutine(TurnOffLight());
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.tag == "Water"){
            isWater = true;
            isWaterSurface = true;

            speed = 3f;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Water") isWaterSurface = false;
        if(other.tag == "Dark") StartCoroutine(TurnOnLight());
    }
}
