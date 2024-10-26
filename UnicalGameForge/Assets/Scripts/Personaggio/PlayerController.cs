using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed; 
    public float groundDist;

    public LayerMask groundLayer; 
    public Rigidbody rb;
    public SpriteRenderer sr; 

    public Animator animator; 

    public Camera camera; 

    public Transform target_sprite; 

    public float dashDistance = 1.5f; // La distanza che il player coprirà durante il dash
    public float dashDuration = 0.5f; // Durata del dash (puoi adattarla alla durata dell'animazione)
    private bool isDashing = false; // Stato che verifica se il player sta facendo il dash

    private Vector3 target_sprite_position_real; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target_sprite_position_real = target_sprite.position; 
        Debug.Log("CICCIO: " + target_sprite_position_real );
        
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit; 
        Vector3 castPos = transform.position; 
        castPos.y += 1 ;
        animator.ResetTrigger("Dash"); 

        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, groundLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position; 
                movePos.y = hit.point.y + groundDist; 
                transform.position = movePos; 
             
            }
        }

        float x = Input.GetAxis("Horizontal"); 
        float y = Input.GetAxis("Vertical");
        // Se muovo il personaggio avvio l'animazione di movimento
        Debug.Log("x: " + x + " y: " + y); 
         
        if (!isDashing) // Se non sta dashing
        {
            Vector3 moveDir = new Vector3(x, 0, y);  
            rb.velocity = moveDir * speed;

            if (x != 0 && x < 0)
            {
                sr.flipX = true; 
            }
            else if (x != 0 && x > 0)
            {
                sr.flipX = false; 
            }
        }


        // Se clicco shift avvio il trigger dash
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // avvio l'animazione del dash chiamata 
            Debug.Log("Dash"); 
            // avvio il trigger Dash
            animator.SetTrigger("Dash"); 
            // Avvio la couritine per il dash 
            StartCoroutine(PerformDash()); 
           
             
        } 
        else if (x != 0 || y != 0)
        {
            // Stoppo l'animazione in corso
            animator.SetBool("isMoving", true); 
        }
        else
        {
            animator.SetBool("isMoving", false); 
        }
        


        
    }

    // Coroutine per eseguire il dash
        // Coroutine per eseguire il dash
    IEnumerator PerformDash()
    {
        isDashing = true;

        float dashStartTime = Time.time;
        float dashEndTime = dashStartTime + dashDuration;

        float direction = sr.flipX ? -1 : 1; // Determina la direzione del dash (sinistra o destra)
        Vector3 startPosition = transform.position; // Posizione iniziale
        Vector3 targetPosition = startPosition + new Vector3(0.50f * direction, 0, 0); // Posizione finale

        Vector3 startTarget = target_sprite.position; 
        Vector3 targetTarget = startTarget + new Vector3(-0.50f * direction, 0, 0); // Posizione finale del target sprite (direzione opposta)

        // Movimento durante il dash
        while (Time.time < dashEndTime)
        {
            float t = (Time.time - dashStartTime) / dashDuration; // Intervallo temporale (da 0 a 1)
            transform.position = Vector3.Lerp(startPosition, targetPosition, t); // Interpolazione tra start e target
            //target_sprite.position = Vector3.Lerp(startTarget, targetTarget, t); 
            yield return null; // Aspetta il prossimo frame
        }
        

        // Assicurati che il movimento termini esattamente sulla posizione finale
        transform.position = targetPosition;

       // target_sprite.position = target_sprite_position_real; // La variabile target_sprite_position_real è la posizione originale

    
        isDashing = false;
    }
}


    
    

