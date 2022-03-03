using UnityEngine;
using TMPro;

namespace Platformer.Inputs
{// https://www.youtube.com/watch?v=iD1_JczQcFY
    public class DamagePopUp : MonoBehaviour
    {
        public static DamagePopUp Create(Vector3 pos, float damage, float lifetime = 2f)
        {
            Transform trans = Instantiate(GameAssets.instance.damageCounter, pos, Quaternion.identity);
            DamagePopUp damagePopUp = trans.GetComponent<DamagePopUp>();
            TextMeshPro textMeshPro = trans.GetComponent<TextMeshPro>();
            damagePopUp.PopUp(damage);
            return damagePopUp;
        }

        private TextMeshPro textMeshPro;

        void Awake()
        {
            textMeshPro = GetComponent<TextMeshPro>();
        }
        public void PopUp(float damage)
        {
            textMeshPro.SetText(damage.ToString());
        }
    }
}