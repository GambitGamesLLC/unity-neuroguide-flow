using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Floater : MonoBehaviour
{
    //Rigidbody component of floating object
    public Rigidbody rb;
    //Depth at which object starts to experience buoyancy
    public float depthBeforeSub;
    //Amount of buoyant force applied
    public float displaceAmount;
    //Number of points applying buoyant force
    public int floaters;

    //Drag coefficient in water
    public float waterDrag;
    //Angular drag coefficient in water
    public float waterAngularDrag;
    //Reference to the water surface management component
    public WaterSurface water;

    //Holds parameters for searching the water surface
    WaterSearchParameters Search;
    //Stores result of water surface search
    WaterSearchResult SearchResult;

    // Adjust for stronger or weaker gravity
    public float gravityMultiplier = 1.0f; 



    private void FixedUpdate()
    {
        //Apply a distributed gravitational force
        rb.AddForceAtPosition(Physics.gravity / floaters, transform.position, ForceMode.Acceleration);
        //rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, 0.1f);

        //Set up search parameters for projecting on water surface
        Search.startPositionWS = SearchResult.candidateLocationWS;
        Search.targetPositionWS = gameObject.transform.position;
        Search.error = 0.9f;
        Search.maxIterations = 4;
        

        //Project point onto water surface and get result
        water.ProjectPointOnWaterSurface(Search, out SearchResult);

        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        //If object is below the water surface
        if(transform.position.y < SearchResult.projectedPositionWS.y)
        {
            //Calculate displacement multiplier based on submersion depth
            float displacementMulti = Mathf.Clamp01((SearchResult.projectedPositionWS.y - transform.position.y) / depthBeforeSub) * displaceAmount;
            //Apply buoyant force upwards
            rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMulti, 0f), transform.position, ForceMode.Acceleration);
            //Apply water drag force against velocity
            rb.AddForce(displacementMulti * -rb.linearVelocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            //Apply water angular drag torque against angular velocity
            rb.AddTorque(displacementMulti * -rb.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
