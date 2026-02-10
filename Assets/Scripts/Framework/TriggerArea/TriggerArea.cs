using System;
using UnityEngine;
using UnityEngine.Events;

using Framework.Attributes;
using Framework.Extensions;

namespace Framework.TriggerArea
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(MeshFilter))]
    public sealed class TriggerArea : MonoBehaviour
    {
        private const string FBX_SUFFIX = ".fbx";
        
        [SerializeField] private StandardMeshes shapeToUse;
        [SerializeField, Tag] private string tagToTriggerWith = "Player";
        [SerializeField] private TriggerBehaviour behaviour;
        [SerializeField] private bool isOneTimeUse;
        
        [Space(20)]
        [SerializeField] private UnityEvent<GameObject> onEnter = new();
        [SerializeField] private UnityEvent<GameObject> onExit = new();

        private MeshFilter _meshFilter;
        private BoxCollider _boxCollider;
        private SphereCollider _sphereCollider;
        private CapsuleCollider _capsuleCollider;

        private bool _isTriggered;
        
#if UNITY_EDITOR
        private bool _needsMeshUpdate;
#endif

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider>();
            _sphereCollider = GetComponent<SphereCollider>();
            _capsuleCollider = GetComponent<CapsuleCollider>();

            _boxCollider.isTrigger = true;
            _sphereCollider.isTrigger = true;
            _capsuleCollider.isTrigger = true;
            
            if (!_meshFilter)
                _meshFilter = GetComponent<MeshFilter>();

            _meshFilter.mesh = null;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying
                && _needsMeshUpdate)
            {
                _needsMeshUpdate = false;
                UpdateMesh();
            }
#endif
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (behaviour == TriggerBehaviour.EXIT_ONLY
                || CheckOneTimeUse()
                || !other.CompareTag(tagToTriggerWith))
                return;

            _isTriggered = true;
            onEnter?.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (behaviour == TriggerBehaviour.ENTER_ONLY
                || CheckOneTimeUse()
                || !other.CompareTag(tagToTriggerWith))
                return;
            
            _isTriggered = true;
            onExit?.Invoke(other.gameObject);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying
                && UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
            {
                UnityEditor.EditorApplication.update -= DeferredMeshUpdate;
                UnityEditor.EditorApplication.update += DeferredMeshUpdate;
            }
        }

        private void DeferredMeshUpdate()
        {
            UnityEditor.EditorApplication.update -= DeferredMeshUpdate;

            if (this != null)
                UpdateMesh();
        }
#endif
        
        public void TestTrigger() => Debug.Log(shapeToUse);

        private void UpdateMesh()
        {
            if (_meshFilter == null)
                _meshFilter = GetComponent<MeshFilter>();

            Mesh mesh = Resources.GetBuiltinResource<Mesh>(shapeToUse.GetStringValue() + FBX_SUFFIX);
            
            if (mesh != null)
                _meshFilter.sharedMesh = mesh;

            _boxCollider = GetComponent<BoxCollider>();
            _sphereCollider = GetComponent<SphereCollider>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            
            switch (shapeToUse)
            {
                case StandardMeshes.CUBE:
                    _boxCollider.enabled = true;
                    _sphereCollider.enabled = false;
                    _capsuleCollider.enabled = false;
                    break;
                
                case StandardMeshes.SPHERE:
                    _boxCollider.enabled = false;
                    _sphereCollider.enabled = true;
                    _capsuleCollider.enabled = false;
                    break;
                
                case StandardMeshes.CAPSULE:
                    _boxCollider.enabled = false;
                    _sphereCollider.enabled = false;
                    _capsuleCollider.enabled = true;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool CheckOneTimeUse() => isOneTimeUse && _isTriggered;
    }
}