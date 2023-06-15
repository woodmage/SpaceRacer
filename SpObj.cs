using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceRacer
{
    internal class SpObj
    {
        public float posx, posy;
        public float width, height;
        public float radiusx, radiusy;
        public float movex, movey;
        public float minx, miny, maxx, maxy;
        public int actminx, actminy, actmaxx, actmaxy;
        public Image image;
        public float health;
        public float fuel;
        public string name;

        public SpObj(float posx, float posy, float width, float height, Image image)
        {
            this.posx = posx;
            this.posy = posy;
            this.width = width;
            this.height = height;
            radiusx = width / 2;
            radiusy = height / 2;
            movex = movey = 0;
            this.image = image;
            minx = miny = 0;
            maxx = 599;
            maxy = 799;
            actminx = actminy = actmaxx = actmaxy = 0;
            health = 100;
            fuel = 100;
            name = "space object";
        }
        public float GetLeft() => (posx - radiusx);
        public float GetRight() => (posx + radiusx);
        public float GetTop() => (posy - radiusy);
        public float GetBottom() => (posy + radiusy);
        public void SetMove(float x, float y)
        {
            movex = x;
            movey = y;
        }
        public void AddMove(float x, float y)
        {
            movex += x;
            movey += y;
        }
        public void MoveBy(float x, float y) => MoveBy(x, y, false);
        private void MoveBy(float x, float y, bool bound)
        {
            posx += x;
            posy += y;
            if (bound)
            { 
                if (GetLeft() < minx)
                {
                    if (actminx == 0) //stop
                    {
                        posx = minx + radiusx;
                        movex = 0;
                    }
                    if (actminx == 1) //bounce
                    {
                        posx = minx + (minx - GetLeft()) + radiusx;
                        movex = -movex;
                    }
                    if (actminx == 2) //wrap
                    {
                        posx = maxx - (minx - GetLeft()) - radiusx;
                    }
                }
                if (GetRight() > maxx)
                {
                    if (actmaxx == 0)
                    {
                        posx = maxx - radiusx;
                        movex = 0;
                    }
                    if (actminx == 1)
                    {
                        posx = maxx - (GetRight() - maxx) - radiusx;
                        movex = -movex;
                    }
                    if (actmaxx == 2)
                    {
                        posx = minx + (GetRight() - maxx) + radiusx;
                    }
                }
                if (GetTop() < miny)
                {
                    if (actminy == 0)
                    {
                        posy = miny + radiusy;
                        movey = 0;
                    }
                    if (actminy == 1)
                    {
                        posy = miny + (miny - GetTop()) + radiusy;
                        movey = -movey;
                    }
                    if (actminy == 2)
                    {
                        posy = maxy - (miny - GetTop()) - radiusx;
                    }
                }
                if (GetBottom() > maxy)
                {
                    if (actmaxy == 0)
                    {
                        posy = maxy - radiusy;
                        movey = 0;
                    }
                    if (actmaxy == 1)
                    {
                        posy = maxy - (GetBottom() - maxy) - radiusy;
                        movey = -movey;
                    }
                    if (actmaxy == 2)
                    {
                        posy = miny + (GetBottom() - maxy) + radiusy;
                    }
                }
            }
        }
        public void Move() => MoveBy(movex, movey, true);
        public void Move(float x, float y)
        {
            AddMove(x, y);
            Move();
        }
        public void MoveTo(float x, float y)
        {
            posx = x;
            posy = y;
        }
        public void MoveToward(float x, float y, float max, bool dofuel)
        {
            float r = GetRadius(x, y); //get radius (distance)
            float t = GetAngle(x, y); //get theta (angle)
            if (r > max) r = max; //if radius more than maximum, set radius to maximum
            if (dofuel) //if we're using fuel
                if (r > fuel) r = fuel; //if radius more than available fuel, set radius to fuel
            Move(GetX(r, t), GetY(r, t)); //move by x and y calculated from radius and theta
            if (dofuel) //if we're using fuel
                fuel -= r; //use the calculated amount of fuel
        }
        public void MoveToward(SpObj s, float max, bool dofuel) => MoveToward(s.posx, s.posy, max, dofuel);
        public void MoveToward(SpObj s, double max, bool dofuel) => MoveToward(s, (float)max, dofuel);
        public void MoveToward(float x, float y, float max) => MoveToward(x, y, max, true);
        public void MoveToward(SpObj s, float max) => MoveToward(s.posx, s.posy, max);
        public void MoveToward(SpObj s, double max) => MoveToward(s, (float)max);
        public void MoveTowardFree(float x, float y, float max) => MoveToward(x, y, max, false);
        public void MoveTowardFree(SpObj s, float max) => MoveTowardFree(s.posx, s.posy, max);
        public void MoveTowardFree(SpObj s, double max) => MoveTowardFree(s, (float)max);
        private double GetRadius(double x, double y) => Math.Sqrt(Math.Pow((x - posx), 2) + Math.Pow((y - posy), 2));
        private double GetAngle(double x, double y) => Math.Atan2((y - posy), (x - posx));
        private float GetRadius(float x, float y) => (float)GetRadius((double)x, (double)y);
        private float GetAngle(float x, float y) => (float)GetAngle((double)x, (double)y);
        private static double GetX(double radius, double theta) => radius * Math.Cos(theta);
        private static double GetY(double radius, double theta) => radius * Math.Sin(theta);
        private static float GetX(float radius, float theta) => (float)GetX((double)radius, (double)theta);
        private static float GetY(float radius, float theta) => (float)GetY((double)radius, (double)theta);
        public bool IsHit(float x, float y, float w, float h)
        {
            if (posx - radiusx > x + w / 2) return false;
            if (posx + radiusx < x - w / 2) return false;
            if (posy - radiusy > y + h / 2) return false;
            if (posy + radiusy < y - h / 2) return false;
            return true;
        }
        public bool IsHit(SpObj s) => IsHit(s.posx, s.posy, s.width, s.height);
        public void Paint(Graphics g)
        {
            Rectangle src = new(0, 0, image.Width, image.Height);
            Rectangle dst = new((int)(posx - radiusx), (int)(posy - radiusy), (int)width, (int)height);
            g.DrawImage(image, dst, src, GraphicsUnit.Pixel);
        }
    }
}
