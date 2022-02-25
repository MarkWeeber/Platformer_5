using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField] private Transform prefabTransform = null;
    [SerializeField] private LayerMask arrowMask;
    private Transform prefabClone = null;
    private void Start() {
        //Invoke("Explode", 2f);
    }
    public void Explode()
    {
        prefabClone = Instantiate(prefabTransform, this.transform.position, Quaternion.identity);
        Destroy(this.transform.gameObject);
        Destroy(prefabClone.gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if ((arrowMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Destroy(other.transform.gameObject);
            Explode();
        }
    }
}
