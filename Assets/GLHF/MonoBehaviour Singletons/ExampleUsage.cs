using UnityEngine;

namespace GLHF.Singletons
{
    public class ExampleUsage : MonoBehaviour
    {
        [SerializeField]
        private int newInt = 0;

        [SerializeField]
        private bool updateSingletonIntButton = false;

        private void Update()
        {
            var mySingleton = MonoBehaviourSingletons.Get<ExampleRegister>();

            if (mySingleton == null)
                return;

            if (updateSingletonIntButton)
            {
                updateSingletonIntButton = false;
                mySingleton.UpdateMyInt(newInt);
            }

            Debug.Log("Singleton has been found!");
        }
    }
}