using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MetroidvaniaTools
{
    public class GameManager : MonoBehaviour
    {
        protected GameObject player;
        protected PlayerStateManager character;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            player = FindObjectOfType<PlayerStateManager>().gameObject;
            character = player.GetComponent<PlayerStateManager>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
