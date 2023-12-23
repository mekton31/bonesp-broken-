using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace bonesp
{
    public static class Calculate
    {
        public static Vector2 WorldToScreen(ViewMatrix matrix, Vector3 pos, int width, int height)
        {
            Vector2 screenCoordinates = new Vector2();

            // get scrrenW
            float screenW = (matrix.m41 * pos.X) + (matrix.m42 * pos.Y) + (matrix.m43 * pos.Z) + matrix.m44;
                
                if (screenW > 0.001f)
                 {
                //calcuta x adn y
                float screenX = (matrix.m11 * pos.X) + (matrix.m12 * pos.Y) + (matrix.m13 * pos.Z) + matrix.m14;
                float screenY = (matrix.m21 * pos.X) + (matrix.m22 * pos.Y) + (matrix.m23 * pos.Z) + matrix.m24;;

                // camera center
                float camX = width / 2;
                float camY = height / 2;

                //perform perspectve division
                float X = camX + (camX * screenX / screenW);
                float Y = camY - (camY * screenY / screenW);

                //return coords
                screenCoordinates.X = X;
                screenCoordinates.Y = Y;
                return screenCoordinates;
                } //if out range
                else
               {
                return new Vector2(-99, -99);
               }

        }




    }
}
