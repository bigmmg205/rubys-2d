using System;
using UnityEngine;

/// <summary>
/// This class handle Enemy behaviour. It make them walk back & forth as long as they aren't fixed, and then just idle
/// without being able to interact with the player anymore once fixed.
/// </summary>
public class EnemyController : MonoBehaviour
{
	// ====== ENEMY MOVEMENT ========
	public float speed;
	public float timeToChange;
	public bool horizontal;

	
	Rigidbody2D rigidbody2d;
	float remainingTimeToChange;
	Vector2 direction = Vector2.right;


	bool broken=true;
	
	// ===== ANIMATION ========
	Animator animator;
	
	// ================= SOUNDS =======================

	void Start ()
	{
		rigidbody2d = GetComponent<Rigidbody2D>();
		remainingTimeToChange = timeToChange;

		animator = GetComponent<Animator>();

		direction = horizontal ? Vector2.right : Vector2.down;
	}
	
	void Update()
	{
		if(!broken)
		return;
		remainingTimeToChange -= Time.deltaTime;

		if (remainingTimeToChange <= 0)
		{
			remainingTimeToChange += timeToChange;
			direction *= -1;
		}

		animator.SetFloat("Move X", direction.x);
		animator.SetFloat("Move Y", direction.y);
	}

	void FixedUpdate()
	{
		rigidbody2d.MovePosition(rigidbody2d.position + direction * speed * Time.deltaTime);
	}

	void OnCollisionStay2D(Collision2D other)
	{
		RubyController controller = other.collider.GetComponent<RubyController>();
		
		if(controller != null)
		{
			controller.ChangeHealth(-1);
		}
			
	}

	//Public because we want to call it from elsewhere like the projectile script
		public void Fix()
		{
			broken = false;
			rigidbody2d.simulated = false;
		}


}
