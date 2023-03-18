using UnityEngine;

public class TestigHeight : MonoBehaviour
{
	public Terrain terrain;
	public float speed = 6f;
	public float baseOffset	= 0.5f;
	private Vector3 vecpor = new Vector3(0f, 0f, 100f);

    // Update is called once per frame
    void Update()
    {
		if(this.transform.position != vecpor)
			MoveTo(vecpor);
    }
	
	private void MoveTo(Vector3 goal)
	{
		float movementAmount = speed * Time.deltaTime;	// how much it will move on XZ plane
        Vector3 movementDirection = (goal - this.transform.position).normalized;	//the direction towards which will the unit move
		Vector3 step = this.transform.position + movementDirection * movementAmount;	// the coordinate of 1xUpdate()
		step.y = terrain.SampleHeight(step) + baseOffset;	//the Y coordinate of one step
		
		this.transform.position = step;
	}
}
