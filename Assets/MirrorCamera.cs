using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using RenderPipeline = UnityEngine.Rendering.RenderPipelineManager;



public class MirrorCamera : MonoBehaviour
{
    [SerializeField] private Mirror[] l_mirror;

    [SerializeField] private int iterations = 7;

    [SerializeField] private Camera mainCamera;

    public void Awake()
    {
        foreach (Mirror mirror in l_mirror) {
            mirror.tempTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        }
    }

    private void Start() {
        
    }

    private void OnEnable() {
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }
    
    private void OnDisable() {
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }
    
    void UpdateCamera(ScriptableRenderContext context, Camera camera) {
        foreach (Mirror mirror in l_mirror){
            //if (mirror.Renderer.isVisible) {

            //}
        }
    }
    
}
