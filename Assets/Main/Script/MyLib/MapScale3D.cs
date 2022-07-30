using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hsiang
{
    public class MapScale3D : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        public GameObject plane;

        private void Start()
        {
            this.DrawMap();
        }

        void DrawMap()
        {
            int pointCount = 0;
            Vector3 planePos = this.plane.transform.position;
            planePos.y = 1;
            this.lineRenderer.positionCount = pointCount;

            int sX = -5;
            int eX = 5;
            for (int i = 0; i < 10; i++)
            {
                pointCount += 2;
                this.lineRenderer.positionCount = pointCount;
                Vector3 sPos = new Vector3(planePos.x + sX, planePos.y, planePos.z + 5 - i);
                Vector3 ePos = new Vector3(planePos.x + eX, planePos.y, planePos.z + 5 - i);
                this.lineRenderer.SetPosition(pointCount - 2, sPos);
                this.lineRenderer.SetPosition(pointCount - 1, ePos);

                eX = -(eX);
                sX = -(sX);
            }

            int sZ = -5;
            int eZ = 5;

            for (int i = 0; i < 10; i++)
            {
                pointCount += 2;

                this.lineRenderer.positionCount = pointCount;
                Vector3 sPos = new Vector3(planePos.x + -5 + i, planePos.y, planePos.z + sZ);
                Vector3 ePos = new Vector3(planePos.x + -5 + i, planePos.y, planePos.z + eZ);
                this.lineRenderer.SetPosition(pointCount - 2, sPos);
                this.lineRenderer.SetPosition(pointCount - 1, ePos);

                eZ = -(eZ);
                sZ = -(sZ);
            }

        }

    }

}
