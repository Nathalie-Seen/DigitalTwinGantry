using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BlackOut : MonoBehaviour
{
    [SerializeField] private string[] m_ignoreTags;
    [SerializeField] private Shader m_effect;
    [SerializeField, Range(0, 1)] private float m_completeBlack;
    [SerializeField] private float m_fadeMultiplier;

    private Material m_material;
    private SphereCollider m_collider;
    private float m_fadeDistance;

    private List<Collider> m_inCollider;

    private void Start() {
        m_inCollider = new List<Collider>();

        m_material = new Material(m_effect);
        m_collider = GetComponent<SphereCollider>();
        m_fadeDistance = m_collider.radius;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetType() == typeof(MeshCollider)) {
            return;
        }

        for (int i = 0; i < m_ignoreTags.Length; i++) {
            if (other.tag == m_ignoreTags[i]) {
                return;
            }
        }

        if (!m_inCollider.Contains(other)) {
            m_inCollider.Add(other);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (m_inCollider.Contains(other)) {
            m_inCollider.Remove(other);
        }
    }

    private void Update() {
        float minDistance = 1000;
        for (int i = 0; i < m_inCollider.Count; i++) {
            Vector3 point = m_inCollider[i].ClosestPoint(transform.position);

            float distance = Vector3.Distance(transform.position, point);
            minDistance = Mathf.Min(minDistance, distance);
        }

        float blackness = Mathf.Max(1 - (minDistance / (m_fadeDistance * m_fadeMultiplier)), 0);
        // float blackness = Mathf.Max(1 - Mathf.Lerp(Mathf.Lerp(minDistance / m_fadeDistance, 1, minDistance / (m_fadeDistance - m_fadeMultiplier)), 1, minDistance / (m_fadeDistance - m_fadeMultiplier)), 0);

        if (1 - blackness < m_completeBlack) {
            blackness = 1;
        }

        m_material.SetFloat("_Blackness", blackness);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, m_material);
    }
}