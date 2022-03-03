using UnityEngine;
using TMPro;

namespace Platformer.Inputs
{
    public class DamagePopUp : MonoBehaviour
    {
        public float fadeTime = 1f;
        public float moveSpeed = 3f;
        public float fadeSpeed = 1f;
        public Vector3 Offset = Vector3.zero;
        private static Vector3 offset;
        
        private Color textColor;
        public static DamagePopUp Create(Vector3 pos, float damage, float lifetime = 2f)
        {
            pos += offset;
            Transform trans = Instantiate(  GameAssets.instance.damageCounter, 
                                            pos,
                                            Quaternion.identity);
            DamagePopUp damagePopUp = trans.GetComponent<DamagePopUp>();
            TextMeshPro textMeshPro = trans.GetComponent<TextMeshPro>();
            damagePopUp.PopUp(damage);
            return damagePopUp;
        }

        private TextMeshPro textMeshPro;

        void Awake()
        {
            offset = Offset;
            textMeshPro = GetComponent<TextMeshPro>();
        }
        public void PopUp(float damage)
        {
            Color clr;
            if(damage < 0)
                clr = Color.red;
            else
                clr = Color.green;
            textMeshPro.SetText(Mathf.Abs(damage).ToString());
            textMeshPro.faceColor = clr;
            textColor = textMeshPro.color;
        }

        private void Update()
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            fadeTime -= Time.deltaTime;
            if(fadeTime < 0)
            {
                textColor.a -= fadeSpeed * Time.deltaTime;
                textMeshPro.color = textColor;
                if(textColor.a < 0 )
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}