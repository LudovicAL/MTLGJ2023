using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.InputSystem.iOS;
#endif

namespace AI.ZombieStateMachine
{
   
    public class ZombieMaterialController : MonoBehaviour
    {
        public UnityEvent onDissolveDone;
        
        public Material material;
        public SkinnedMeshRenderer[] skinnedMeshRenderers;

        private float dissolveDelay = 10.0f;
        
        private Material _materialProxy;
        private float _timeElapsed;
        private readonly int _shaderProperty = Shader.PropertyToID("_Clipping");
        
        private void Awake()
        {
            _materialProxy = new Material(material);
            foreach (var skinnedMeshRenderer in skinnedMeshRenderers)
            {
                skinnedMeshRenderer.materials = new[] {_materialProxy};
            }
        }

        public void StartDissolve()
        {
            StartCoroutine(nameof(AsyncDissolve));
        }

        IEnumerator AsyncDissolve()
        {
            while (_timeElapsed < dissolveDelay)
            {
                float t = Mathf.Lerp(0.0f, 1.0f, _timeElapsed / dissolveDelay);
                _timeElapsed += Time.deltaTime;
                _materialProxy.SetFloat(_shaderProperty, t);
                yield return null;
            }
            
            onDissolveDone.Invoke();

        }
        
        public void Reset()
        {
            _timeElapsed = 0.0f;
            _materialProxy.SetFloat(_shaderProperty, _timeElapsed);
        }
    }
    
#if UNITY_EDITOR

    [CustomEditor(typeof(ZombieMaterialController))]
    public class ZombieMaterialControllerEditor : Editor
    {
        private ZombieMaterialController _zombieMaterialController;
        
        private void OnEnable()
        {
            _zombieMaterialController = (ZombieMaterialController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("StartDissolve"))
            {
                _zombieMaterialController.StartDissolve();
            }
            
            if (GUILayout.Button("Reset"))
            {
                _zombieMaterialController.Reset();
            }
        }
    }



#endif
}


