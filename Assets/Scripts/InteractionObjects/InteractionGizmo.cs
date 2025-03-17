using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionGizmo : MonoBehaviour
{
    public float radius = 5f;         // ��ä���� ������
    public float angle = 45f;         // ��ä���� ���� (�߾ӿ��� �������� �������� ����)
    public float height = 1f;         // ��ä���� ����, ������ ���̸� ���� (3D �������� ���̸� ����)
    public int numSteps = 30;         // ��ä���� �׸� �� ���Ǵ� ���� �� (�� �׸� ���� ����)

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;  // ����� ���� ����

        // ��ä�� �׸���
        DrawFan(transform.position, transform.forward, radius, angle, numSteps);
    }

    void DrawFan(Vector3 position, Vector3 direction, float radius, float angle, int numSteps)
    {
        // ��ä���� ���� ������ �� ���� ���
        float startAngle = -angle / 2;
        float endAngle = angle / 2;

        // ��ä���� �ٴ� ���� �׸��� ���� ���� ���
        Vector3[] points = new Vector3[numSteps + 1];

        // ��ä���� �� ���� ��� (������ ���� ���� ���)
        for (int i = 0; i <= numSteps; i++)
        {
            float stepAngle = Mathf.Lerp(startAngle, endAngle, i / (float)numSteps) * Mathf.Deg2Rad;
            float x = Mathf.Cos(stepAngle) * radius;
            float z = Mathf.Sin(stepAngle) * radius;
            points[i] = position + new Vector3(x, 0f, z);
        }

        // ��ä���� �ٴ� ���� �׸��� ���� ���� �����Ͽ� ��ä�� ���� �׸���
        for (int i = 0; i < numSteps; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }

        // ��ä���� �߽ɿ��� �ٴ� ���� �� ������ �����Ͽ� ��ä���� ���� �׸���
        foreach (var point in points)
        {
            Gizmos.DrawLine(position, point);
        }

        // ���̸� �߰��Ͽ� 3D ����ó�� �׸��� (���� ����)
        if (height > 0f)
        {
            Vector3 top = position + direction * height;

            foreach (var point in points)
            {
                Gizmos.DrawLine(point, point + (top - position).normalized * height);
                Gizmos.DrawLine(top, point + (top - position).normalized * height);
            }
        }
    }
}
