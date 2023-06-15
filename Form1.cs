using SpaceRacer.Properties;
using System.Drawing.Drawing2D;

namespace SpaceRacer
{
    public partial class SpaceRacer : Form
    {
        readonly System.Windows.Forms.Timer timer = new();
        readonly Random rand = new();
        readonly List<Size> sizes = new() { new Size(600,800), new Size(30,70), new Size(50,50), new Size(350,350), new Size(100,100),
            new Size(100,100), new Size(100,100), new Size(5,5) };
        readonly List<Image> images = new() { Resources.space, Resources.spaceship, Resources.saucer, Resources.star, Resources.planet_blue_green,
            Resources.planet_green, Resources.planet_brown, Resources.shot };
        Size gamesize = new();
        Size arenasize = new(600, 800);
        readonly Bitmap arenabmp = new(600, 800);
        readonly Bitmap infobmp = new(400, 800);
        readonly SpObj ship;
        readonly List<SpObj> spobjs = new();
        readonly List<SpObj> enemies = new();
        readonly List<SpObj> shots = new();
        int level = 0, score = 0, shotdelay = 5;
        float life = 100;
        bool paused = false, died = false, autofire = false;
        public SpaceRacer()
        {
            InitializeComponent();
            ship = new(arenasize.Width / 2, arenasize.Height - sizes[1].Height, sizes[1].Width, sizes[1].Height, images[1]);
        }
        private void HandleLoad(object sender, EventArgs e)
        {
            gamesize = ClientSize;
            timer.Interval = 100 - level;
            timer.Tick += TimedGame;
            StartGame();
        }
        private void StartGame()
        {
            timer.Stop();
            level = 0;
            score = 0;
            life = 100;
            shotdelay = 5;
            shots.Clear();
            died = false;
            LoadLevel();
            timer.Start();
            paused = false;
        }
        private void TimedGame(object? sender, EventArgs e)
        {
            if (died)
            {
                if (GetYesNo("Play again?", "You DIED!"))
                    StartGame();
                else
                    Application.Exit();
            }
            ship.Move();
            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].MoveBy(0, -5);
                for (int j = 0; j < spobjs.Count; j++)
                {
                    if (shots[i].IsHit(spobjs[j]))
                    {
                        spobjs[j].health -= shots[i].health;
                        shots[i].health -= spobjs[j].health;
                        if (spobjs[j].health < 0)
                        {
                            score += 500;
                            spobjs.RemoveAt(j);
                            j--;
                        }
                    }
                }
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (shots[i].IsHit(enemies[j]))
                    {
                        enemies[j].health -= shots[i].health;
                        shots[i].health -= enemies[j].health;
                        if (enemies[j].health < 0)
                        {
                            score += 100;
                            enemies.RemoveAt(j);
                            j--;
                        }
                    }
                }
                if ((shots[i].posy < 4) || (shots[i].health < 0))
                {
                    shots.RemoveAt(i);
                    i--;
                }
            }
            foreach (SpObj s in spobjs)
            {
                s.MoveBy(0, 10);
            }
            foreach (SpObj s in enemies)
            {
                s.MoveBy(0, 10); //dropping by 10
                s.MoveToward(ship, 0.25 * (level + 1)); //then move toward ship
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if ((enemies[i].posy + enemies[i].height > arenasize.Height) && (enemies[i].fuel <= 0))
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
            LoadLine();
            for (int i = 0; i < spobjs.Count; i++)
            {
                if (spobjs[i].posy > arenasize.Height)
                {
                    spobjs.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].posy > arenasize.Height)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
            foreach (SpObj s in spobjs)
                if (s.IsHit(ship))
                    life -= 5;
            foreach (SpObj s in enemies)
                if (s.IsHit(ship))
                    life--;
            if (life < 0)
            {
                died = true;
            }
            score++;
            if (score > 1000 * (level + 1))
            {
                level++;
                life = 100;
            }
            PaintIt();
        }
        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            Keys k = e.KeyData;
            switch (k)
            {
                case Keys.A:
                    if (autofire)
                    {
                        autofire = false;
                    }
                    else
                    {
                        if (level > 5)
                            autofire = true;
                    }
                    break;
                case Keys.P:
                    paused = !paused;
                    if (paused)
                        timer.Stop();
                    else
                        timer.Start();
                    PaintIt();
                    break;
                case Keys.Q:
                case Keys.Escape:
                    if (GetYesNo("Are you sure?", "Quit?"))
                        Close();
                    break;
                case Keys.Left:
                    ship.Move(-1, 0);
                    break;
                case Keys.Right:
                    ship.Move(1, 0);
                    break;
                case Keys.Up:
                    ship.Move(0, -1);
                    break;
                case Keys.Down:
                    ship.Move(0, 1);
                    break;
                case Keys.Space:
                    LoadShot();
                    break;
            }
            e.Handled = true;
        }
        private void LoadShot()
        {
            shotdelay--;
            if (shotdelay == 0)
            {
                shots.Add(new(ship.posx, ship.posy, 5, 5, images[7]));
                LastOne(shots).health = Rand(50, 150);
                shotdelay = 5;
            }
        }
        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            ;
        }
        private void HandleResize(object sender, EventArgs e) => ClientSize = gamesize; //refuse resize
        private void PaintIt()
        {
            Rectangle src = new(0, 0, images[0].Width, images[0].Height);
            Rectangle dst = new(0, 0, arenabmp.Width, arenabmp.Height);
            Graphics arena = Graphics.FromImage(arenabmp);
            arena.DrawImage(images[0], dst, src, GraphicsUnit.Pixel);
            for (int si = 0; si < spobjs.Count; si++)
                spobjs[si].Paint(arena);
            for (int ei = 0; ei < enemies.Count; ei++)
                enemies[ei].Paint(arena);
            for (int hi = 0; hi < shots.Count; hi++)
                shots[hi].Paint(arena);
            ship.Paint(arena);
            if (paused)
                Utils.DrawOutlinedString(arena, "Paused", "Comic Sans MS", 50, dst, Color.DimGray, Color.White, 20);
            if (died)
                Utils.DrawOutlinedString(arena, "You DIED!", "Comic Sans MS", 50, dst, Color.Yellow, Color.Red, 20);
            arena.Dispose();
            arenaPB.Image = arenabmp;
            arenaPB.Invalidate();
            Graphics info = Graphics.FromImage(infobmp);
            info.Clear(Color.Black);
            Font font = new("Comic Sans MS", 20, FontStyle.Bold);
            SolidBrush brush = new(Color.White);
            int stars = 0, planets = 0;
            foreach (SpObj s in spobjs)
            {
                if (s.name == "planet")
                    planets++;
                if (s.name == "star")
                    stars++;
            }
            Point p = Utils.DrawStringR(info, planets.ToString() + " planets, " + stars.ToString() + " stars", font, Color.White, 10, 10);
            p = Utils.DrawStringR(info, enemies.Count.ToString() + " saucers", p, font, Color.White);
            p = Utils.DrawStringR(info, " ", p, font, Color.White); //blank line
            p = Utils.DrawStringR(info, " ", p, font, Color.White); //blank line
            p = Utils.DrawStringR(info, "Life: " + life.ToString(), p, font, Color.White);
            p = Utils.DrawBarGraph(info, "Life", p, new Size(infobmp.Width, 30), (int)life, 100);
            p = Utils.DrawStringR(info, " ", p, font, Color.White); //blank line
            p = Utils.DrawStringR(info, "Score: " + score.ToString(), p, font, Color.White);
            p = Utils.DrawBarGraph(info, "Score/Level", p, new Size(infobmp.Width, 30), score, (1000 * (level + 1)));
            p = Utils.DrawStringR(info, " ", p, font, Color.White); //blank line
            p = Utils.DrawStringR(info, "Level: " + level.ToString(), p, font, Color.White);
            info.Dispose();
            infoPB.Image = infobmp;
            infoPB.Invalidate();
        }
        private bool GetYesNo(string message, string title)
        {
            timer.Stop();
            bool yn = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
            timer.Start();
            return yn;
        }
        private void LoadLevel()
        {
            spobjs.Clear();
            enemies.Clear();
            for (int i = 0; i < arenasize.Height; i += 10)
            {
                LoadLine();
                foreach (SpObj s in spobjs)
                    s.MoveBy(0, 10);
                foreach (SpObj s in enemies)
                    s.MoveBy(0, 10);
            }
            PaintIt();
        }
        private void LoadLine() //a "line" is 10 pixels high
        {
            int x = 0, y = 0, w, r = 0, numtries = 1000;
            if (autofire) LoadShot(); //if autofire, load a shot
            if (Rand1000() <= (level + 1) * 30) //3% chance / level of planet or star
            {
                if (Rand100() > 2) //98% are gonna be planets
                {
                    int tryno = 0; //we haven't tried yet
                    bool istrue = true; //variable for keep trying
                    while (istrue) //if we want to keep trying (haven't found anything yet)
                    {
                        tryno++; //this is another try
                        x = Rand(arenasize.Width); //place within boundaries
                        y = Rand(10); //from 0 to 9
                        r = sizes[4].Height + Rand(-50, 51); //make radius a bit random
                        w = arenasize.Width; //width full for keeping only one planet at a time
                        istrue = false; //if this works, we won't keep trying
                        for (int j = 0; j < spobjs.Count; j++) //for each planet or star
                            if (spobjs[j].IsHit(w / 2, y, w, r)) //if it hits our vertical position
                                istrue = true; //keep trying
                        for (int j = 0; j < enemies.Count; j++) //for each enemy
                            if (enemies[j].IsHit(x, y, r, r)) //if it hits our computed position
                                istrue = true; //keep trying
                        if (tryno > numtries) //if we've tried enough
                            break; //give up
                    }
                    int i = Rand(4, 6); //which planet to use
                    if (!istrue) //if we didn't give up
                    {
                        spobjs.Add(new(x, y, r, r, images[i])); //add new planet as we computed
                        LastOne(spobjs).health = Rand(1500, 5500); //set the health of that planet to 1500 ~ 5500
                        LastOne(spobjs).name = "planet"; //mark it as a planet
                    }
                }
                else //otherwise (we'll make a star)
                {
                    if (Rand100() > 50) //half the time, we'll go left
                        x = Rand(-10, 10); //compute position
                    else //the rest, we'll go right
                        x = arenasize.Width + Rand(-10, 10); //compute position
                    r = sizes[3].Width + Rand(-50, 50); //compute radius
                    y = Rand(10); //get vertical position
                    spobjs.Add(new(x, y, r, r, images[3])); //add new star as we computed
                    LastOne(spobjs).health = Rand(7500, 55000); //set the health of that star to 7500 ~ 55000
                    LastOne(spobjs).name = "star"; //mark it as a star
                }
            }
            if (Rand1000() <= (level + 1) * 5) //.5% chance / level of enemy
            {
                int tryno = 0; //we haven't tried yet
                bool istrue = true; //set variable for keep trying
                while (istrue) //if keep trying
                {
                    tryno++; //another try
                    x = Rand(arenasize.Width); //place within boundaries
                    y = Rand(10); //from 0 to 9
                    r = sizes[2].Width; //radius
                    istrue = false; //figure to accept our calculations
                    for (int i = 0; i < spobjs.Count; i++)  //for each planet and star
                        if (spobjs[i].IsHit(x, y, r, r)) //if it hits us
                            istrue = true; //keep trying
                    for (int j = 0; j < enemies.Count; j++) //for each enemy
                        if (enemies[j].IsHit(x, y, r, r)) //if it hits us
                            istrue = true; //keep trying
                    if (tryno > numtries) //if we've tried more than numtries
                        break; //give up
                }
                if (!istrue) //if we didn't give up
                {
                    enemies.Add(new(x, y, r, r, images[2])); //add enemy
                    LastOne(enemies).health = Rand(50, 150); //set health randomly to 50 ~ 150
                    LastOne(enemies).fuel = Rand(75, 125) * (level + 1) / 2; //set enemy fuel
                    LastOne(enemies).actmaxy = 3; //set to ignore lower part of screen
                    LastOne(enemies).name = "saucer"; //mark it as a saucer
                }
            }
        }
        private int Rand1000() => Rand(1000) + 1; //makes syntax clearer
        private int Rand100() => Rand(100) + 1; //makes syntax clearer
        private int Rand(int amt) => rand.Next(0, amt); //makes syntax clearer
        private int Rand(int min, int max) => rand.Next(min, max); //makes syntax clearer
        private void HandleMouse(object sender, MouseEventArgs e)
        {
            ship.posx = e.X; //move to mouse X position
            ship.posy = e.Y; //move to mouse Y position
            if ((e.Button == MouseButtons.Left) && (paused == true)) //if left mouse button pressed while paused
            {
                paused = false; //set flag to not paused
                timer.Start(); //restart timer
            }
            if ((e.Button == MouseButtons.Left) || (autofire)) //if left mouse button pressed or autofire
                LoadShot(); //load up a shot
        }
        private static SpObj LastOne(List<SpObj> list) => list[^1]; //makes syntax clearer for accessing newly added SpObj's
        private void FocusLost(object sender, EventArgs e)
        {
            timer.Stop();
            paused = true;
            PaintIt();
        }
    }
}
