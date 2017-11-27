using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Improbable.Player;
using Improbable.Core;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using UnityEngine.UI;

namespace Assets.Gamelogic.Player
{
    [WorkerType(WorkerPlatform.UnityClient)]
    public class HealtScript : MonoBehaviour
    {
        [Require] private Health.Reader healthReader;
        [Require] private ClientAuthorityCheck.Writer ClientAuthorityCheckWriter;

        private Slider healtSlider;

        private void Awake()
        {
            healtSlider = GameObject.Find("Canvas/Slider").GetComponent<Slider>();

        }


        private void OnEnable()
        {
            // Register callback for when components change
            healthReader.CurrentHealthUpdated.Add(OnHealtUpdated);
        }

        private void OnDisable()
        {
            // Deregister callback for when components change
            healthReader.CurrentHealthUpdated.Add(OnHealtUpdated);
        }

        private void OnHealtUpdated(int currentHealt)
        {
            updateGUI(currentHealt);
        }

        void updateGUI(int currentHealt)
        {
            healtSlider.value = currentHealt;

        }
    }
}