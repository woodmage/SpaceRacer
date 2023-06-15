using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceRacer
{
    internal class Utils
    {
        public static void DrawOutlinedString(Graphics g, string text, string fontfamily, int fontsize, Rectangle rect, Color insides, Color outline, int size) =>
            DrawOutlinedString(g, text, fontfamily, fontsize, rect.Left, rect.Top, rect.Width, rect.Height, insides, outline, size);
        public static void DrawOutlinedString(Graphics g, string text, string fontfamily, int fontsize, int posx, int posy, int sizex, int sizey, Color insides, Color outline, int size)
        {
            Font font = new(fontfamily, fontsize, FontStyle.Bold); //make font
            SizeF textsize = g.MeasureString(text, font); //measure text
            int px = posx + (sizex - (int)textsize.Width) / 2; //compute starting horizontal position
            int py = posy + (sizey - (int)textsize.Height) / 2; //compute starting vertical position
            FontFamily family = new(fontfamily); //make font family
            Pen p = new(insides, size); //make pen for outline
            SolidBrush b = new(outline); //make brush for insides
            GraphicsPath gp = new(); //make new graphics path
            gp.AddString(text, family, (int)FontStyle.Bold, g.DpiY * fontsize / 72, new Point(px, py), new StringFormat()); //add string
            g.DrawPath(p, gp); //draw outline
            g.FillPath(b, gp); //draw insides
        }
        public static void DrawCentered(Graphics g, string s, Size size)
        {
            Font font = new("Arial Black", 50, FontStyle.Bold);
            DrawStringCentered(g, s, 0, 0, size.Width, size.Height, true, font, Color.White);
        }
        public static void DrawStringCentered(Graphics g, string text, int x0, int y0, int wide, int high, bool clear, Font font, Color color)
        {
            SolidBrush brush = new(color); //set brush to color
            SolidBrush backbrush = new(Color.Black); //set brush for background
            SizeF size = g.MeasureString(text, font); //get size of text
            float px = (float)(x0 + ((wide - size.Width) / 2)); //calculate starting x position
            float py = (float)(y0 + ((high - size.Height) / 2)); //calculate starting y position
            if (clear) //if clear
                g.FillRectangle(backbrush, px, py, size.Width, size.Height); //then erase background
            g.DrawString(text, font, brush, px, py); //draw the text
        }
        public static Point DrawStringC(Graphics g, string text, Font font, Color color, int x, int y) =>
            DrawStringC(g, text, new Point(x, y), font, color);
        public static Point DrawStringR(Graphics g, string text, Font font, Color color, int x, int y) => 
            DrawStringR(g, text, new Point(x, y), font, color);
        public static Point DrawStringR(Graphics g, string text, Point p, Font font, Color color)
        {
            SolidBrush brush = new(color);
            SizeF size = g.MeasureString(text, font);
            g.DrawString(text, font, brush, p);
            p.Y += (int)size.Height;
            return p;
        }
        public static Point DrawStringC(Graphics g, string text, Point p, Font font, Color color)
        {
            SolidBrush brush = new(color);
            SizeF size = g.MeasureString(text, font);
            g.DrawString(text, font, brush, p);
            p.X += (int)size.Width;
            return p;
        }
        public static Point DrawBarGraph(Graphics g, string text, Point p, Size s, int var1, int var2)
        {
            if (var2 > 0) //avoid divide by zero error for variable when not available
            {
                while (var2 > 100000) { var1 /= 1000; var2 /= 1000; } //reduce size of variables as needed
                SolidBrush backbrush = new(Color.DarkRed); //make background brush dark red
                SolidBrush frontbrush = new(Color.DarkGreen); //make foreground brush dark green
                g.FillRectangle(backbrush, p.X, p.Y, s.Width - 1, s.Height); //apply background
                int newwidth = s.Width * var1 / var2; //compute portion for foreground
                g.FillRectangle(frontbrush, p.X, p.Y, newwidth, s.Height); //apply foreground
                g.DrawRectangle(new Pen(Color.White), p.X, p.Y, s.Width - 1, s.Height); //outline in white
                Font font = new("Arial Black", 20, FontStyle.Bold);
                DrawStringCentered(g, text, p.X, p.Y, s.Width, s.Height, false, font, Color.Black); //draw text centered
                p.Y += s.Height; //add height to point
            }
            return p; //return point
        }

    }
}
