//player dash
private IEnumerator Dash()
    {
      isDashing = true;
      canDash = false;

      float originalGravity = rigidbody.gravityScale;
      
      //keeps the player's vertical height constant while dashing
      rigidbody.gravityScale = 0.0f;

      //if the player has no horizontal direction dashes them to the right
      if (direction.x == 0)
      {
          rigidbody.AddForce(new Vector2(dashSpeed, 0));
      }
      else
      { 
         rigidbody.AddForce(new Vector2((direction.x * dashSpeed), 0.0f));
      }
 
      yield return new WaitForSeconds(dashLength);
      rigidbody.gravityScale = originalGravity;
      isDashing = false;

      //cooldown
      yield return new WaitForSeconds(dashCooldown);
      canDash = true;
    }

//player update
private void FixedUpdate()
    {
        //vertical is handled in the separate jump method
        rigidbody.velocity = new Vector2((direction.x * speed * Time.deltaTime), rigidbody.velocity.y);
    }

private void OnJump(InputValue value)
    {
        if (canJump)
        {
            rigidbody.AddForce(new Vector2(0, jumpHeight));
        }
    }


//platform collision controller 
private void OnCollisionEnter2D(Collision2D collision)
{
        if(player == null)
        {
            player = collision.gameObject.GetComponent<PlayerController>();
        }
        if (player.CanJump == false)
        {
            player.CanJump = true;
        }
    }
}

//code for controlling light level of the overall stage, taken from DarkController.cs which aside from this was written by another member of the team
globalLight.intensity = lightMeterUI.fillAmount;

//checkpoint behavior
private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerHasReached)
        {
            player.RespawnPoint = this.transform.position;
        }
    }

