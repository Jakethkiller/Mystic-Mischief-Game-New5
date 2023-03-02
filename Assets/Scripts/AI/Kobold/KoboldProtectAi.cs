using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoboldProtectAi : BaseEnemyAI
{
    float attackCooldown = 1f;
    float currentAttack = 1f;

    [SerializeField]
    bool attackedPlayer = false;

    PlayerController playerController;

    GameObject player;

    [SerializeField]
    GameObject Item;

    [SerializeField]
    Transform ObjectNewLocation;


    public bool Protect;

    public bool holdingItem { get; private set; }

    public GameObject HeldItem;
    public Animator anim;

    [SerializeField]
    private Transform fleeLocation;

    [SerializeField]
    private float knockbackForce;

    private bool flee;

    new void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        Protect = false;
        holdingItem = false;
        flee = false;
    }

    new void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        base.Update();
        if (attackedPlayer)
        {
            LostPlayer();
            currentAttack -= Time.deltaTime;
            if (currentAttack <= 0)
            {
                currentAttack = attackCooldown;
                attackedPlayer = false;
            }
        }
        if (Item != null)
        {
            if (Protect)
            {
                anim.SetBool("HasItem", true);
                ProtectObject(Item);
            }
            if(flee)
            {
                Flee();
            }
            else
            {
                Patrol();
                // anim.SetBool("HasItem", false);
            }
            
        }

        if(base.stunned == true)
        {
            anim.SetTrigger("Hurt");
        }
        
                if (base.spottedPlayer == true)
        {
            anim.SetBool("FoundPlayer", true);
        }
        else {anim.SetBool("FoundPlayer", false);}
        if (base.ai.speed > 0.1)
        {
            anim.SetFloat("RunSpeed", 1f);
        }
        if (dist <= 2f)
        {
            anim.SetTrigger("Bite");
        }

    }
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (Protect && collision.gameObject.tag == "PickUp")
        {
            anim.SetTrigger("Bite");
            Protect = false;
            HeldItem = collision.gameObject;
            HeldItem.transform.SetParent(this.transform, true);
            HeldItem.transform.position = ObjectNewLocation.position;
            holdingItem = true;
            flee = true;
        }
        if (!attackedPlayer && collision.gameObject.tag == "Player")
        {
            attackedPlayer = true;
            playerController.currentHealth--;
            //Knockback
            collision.transform.position += transform.forward * Time.deltaTime * knockbackForce;

        }
        if (collision.gameObject.tag == "Whip")
        {
            stunned = true;
            // base.ai_Rb.AddForce(Player.transform.position * ai.speed, ForceMode.Impulse);
        }
           
        if (collision.gameObject.tag == "Poop")
        {
            stunned = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Whip")
        {
            stunned = true;
            base.ai_Rb.AddForce(Player.transform.position * ai.speed, ForceMode.Impulse);
        }
    }
   
    private void ProtectObject(GameObject obj)
    {
        Vector3 itemDirection = obj.transform.position;
        UpdateDestination(itemDirection);

    }

    private void Flee()
    {
        if (holdingItem)
        {
            UpdateDestination(fleeLocation.position);
            HeldItem.transform.position = ObjectNewLocation.position;
            flee = false;
        }
        ContinuePatrol();
    }

    private void ContinuePatrol()
    {
        if(transform.position == fleeLocation.transform.position)
        {
            LostPlayer();
        }
    }
}
