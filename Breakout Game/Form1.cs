using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Breakout_Game
{
    public partial class Form1 : Form
    {

        bool goLeft;
        bool goRight;
        bool isGameOver;

        int score;
        int ballx;
        int bally;
        int playerSpeed;

        Random rnd = new Random();

        PictureBox[] blockArray;
        public Form1()
        {
            InitializeComponent();

            PlaceBlocks();
        }
 
        //postavljanje default vrednosti za promenljive
        private void setupGame()
        {
            isGameOver = false;
            score = 0;
            ballx = 5;
            bally = 5;
            playerSpeed = 12;
            txtScore.Text = "Score " + score;

            ball.Left = 460;
            ball.Top = 375;
            player.Left = 425;
            
            gameTimer.Start();

            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "blocks")
                {
                    x.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                }
            }
        }
        //salje poruku da je igra gotova
        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = "Score: " + score + " " + message;
        }
        //postavlja blokove na svoje mesto automatski nakon svakog pokretanja igre
        //potrebno je obrisati postojece blokove koji su napravljeni ranije da bi funkcija radila
        private void PlaceBlocks()
        {
            blockArray = new PictureBox[15];

            int a = 0;//broj blokova u jednom redu, ide do 5
            int top = 50;
            int left = 100;

            for(int i = 0; i < blockArray.Length; i++)//postavljanje blokova na mesto
            {
                blockArray[i] = new PictureBox();
                blockArray[i].Height = 32;
                blockArray[i].Width = 100;
                blockArray[i].Tag = "blocks";
                blockArray[i].BackColor = Color.White;

                if(a == 5)//kada broj blokova u redu bude 5, prelazi se u novi red
                {
                    top = top + 50;
                    left = 100;
                    a = 0;
                }
                if(a < 5)
                {
                    a++;
                    blockArray[i].Left = left;
                    blockArray[i].Top = top;
                    this.Controls.Add(blockArray[i]);
                    left = left + 110;
                }
            }
            setupGame();
        }
        //uklanjanje svih blokova u slucaju da izgubimo i postavljanje novih
        private void removeBlocks()
        {
            foreach(PictureBox x in blockArray)
            {
                this.Controls.Remove(x);
            }
        }
        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;

            //uslov da igrac ne moze da predje levu granicu forme
            if (goLeft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }
            //uslov da igrac ne moze da predje desnu granicu forme
            if (goRight == true && player.Left < 650)
            {
                player.Left += playerSpeed;
            }
            //pomeranje lopte horiozontalno i vertikalno po formi
            ball.Left += ballx;
            ball.Top += bally;
            //skakutanje lopte po formi
            if(ball.Left < 0 || ball.Left > 700)
            {
                ballx = -ballx;
            }
            if(ball.Top < 0)
            {
                bally = -bally;
            }
            //menjanje brzine i smera lopte prilikom dodira sa igracem
            if(ball.Bounds.IntersectsWith(player.Bounds))
            {
                ball.Top = 363;

                bally = rnd.Next(5, 12) * -1; //lopta ide gore ili dole

                if(ballx < 0)
                {
                    ballx = rnd.Next(5, 12) * -1; //lopta ide desno
                }
                else
                {
                    ballx = rnd.Next(5, 12); //lopta ide levo
                }
            }
            
            foreach(Control x in this.Controls)
            {
                if(x is PictureBox && (string)x.Tag == "blocks")
                {
                    if(ball.Bounds.IntersectsWith(x.Bounds))//provera da li lopta dolazi u dodir sa blokovima
                    {
                        score += 1; //ako dolazi, rezultat se povecava za jedan i blok nestaje
                        bally = -bally;

                        this.Controls.Remove(x);
                    }
                }
            }

            if(score == 15) //rezultat je 15 jer ima samo 15 blokova
            {
                //prikaz poruke o pobedi
                gameOver("Pobedili ste!!! Pritisnite Enter da biste igrali ponovo");
            }

            if(ball.Top > 610)
            {
                //prikaz poruke o gubutku u igri
                gameOver("Izgubili ste!!! Pritisnite Enter da biste igrali ponovo");
            }
        }

        private void keyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if(e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
        }

        private void keyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if(e.KeyCode == Keys.Enter && isGameOver == true)//ponovni pocetak igre
            {
                removeBlocks();
                PlaceBlocks();
            }
        }
    }
}
