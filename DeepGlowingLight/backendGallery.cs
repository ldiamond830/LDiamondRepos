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
