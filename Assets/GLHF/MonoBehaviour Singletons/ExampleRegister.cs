using UnityEngine;

namespace GLHF.Singletons
{
    public class ExampleRegister : MonoBehaviour
    {
        [SerializeField]
        private int myInt = 0;

        private void Awake()
        {
            MonoBehaviourSingletons.Register(this);
        }

        public void UpdateMyInt(int i)
        {
            myInt += i;
            Debug.Log("Singleton has been updated, new number is: " + myInt);
        }
    }
}